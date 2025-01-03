import { StartedMode } from "../../d-ts/StartedMode";
import { insertTextAtCaretPosition, scrollTextAreaDownToCaretIfNeeded } from "./CaretUtil";
import { InlineSuggestionDisplay } from "./InlineSuggestionDisplay";
import { SuggestionDisplay } from "./SuggestionDisplay";

export class FluentTextSuggestion extends HTMLElement {

  private typingDebounceTimeout: number | null = null;
  private textArea!: HTMLTextAreaElement;
  private suggestionDisplay!: SuggestionDisplay;
  private pendingSuggestionAbortController?: AbortController;
  private isInitialized: boolean = false;

  /**
   * Register the FluentPageScript component
   * @param blazor
   * @param mode
   */
  public static registerComponent = (blazor: Blazor, mode: StartedMode): void => {
    customElements.define('fluent-text-suggestion', FluentTextSuggestion);
  };

  constructor() {
    super();
  }

  /**
   * Gets the value of the suggestion attribute.
   */
  get value(): string | null {
    return this.getAttribute("value");
  }

  /**
   * Sets the value of the suggestion attribute.
   */
  set value(value: string | null) {
    this.updateAttribute("value", value);
  }

  /**
  * Gets the identifier of the textarea element.
  */
  get anchor(): string | null {
    return this.getAttribute("anchor");
  }

  /**
   * Sets the identifier of the textarea element.
   */
  set anchor(value: string | null) {
    this.updateAttribute("anchor", value);
  }

  /**
  * Gets the minimum length of the text field that the user must enter for the suggestion to be displayed.
  */
  get minlength(): number | null {
    return parseInt(this.getAttribute("minlength") ?? '0');
  }

  /**
   * Sets the minimum length of the text field that the user must enter for the suggestion to be displayed.
   */
  set minlength(value: number | null) {
    this.updateAttribute("minlength", value);
  }

  /**
  * Gets the delay in milliseconds before the suggestion is displayed.
  */
  get delay(): number | null {
    return parseInt(this.getAttribute("delay") ?? '350');
  }

  /**
   * Sets the delay in milliseconds before the suggestion is displayed.
   */
  set delay(value: number | null) {
    this.updateAttribute("delay", value);
  }

  // Custom element added to page.
  connectedCallback() {
    this.initializeTextArea();
    this.isInitialized = true;
  }

  // Custom element removed from page.
  disconnectedCallback() {
    this.isInitialized = false;
  }

  // Initialize the textarea element, adding the KeyboardEvent listeners.
  private initializeTextArea(): void {

    if (this.anchor === null || !(document.getElementById(this.anchor) instanceof HTMLTextAreaElement)) {
      throw new Error(`Impossible to find a textarea element, with the id: '${this.anchor}'.`);
    }

    this.textArea = document.getElementById(this.anchor) as HTMLTextAreaElement;

    //this.suggestionDisplay = this.shouldUseInlineSuggestions(this.textArea)
    //  ? new InlineSuggestionDisplay(this, this.textArea)
    //  : new OverlaySuggestionDisplay(this, this.textArea);

    this.suggestionDisplay = new InlineSuggestionDisplay(this, this.textArea);

    this.textArea.addEventListener('keydown', e => this.handleKeyDown(e));
    this.textArea.addEventListener('keyup', e => this.handleKeyUp(e));
    this.textArea.addEventListener('mousedown', () => this.removeExistingOrPendingSuggestion());
    this.textArea.addEventListener('focusout', () => this.removeExistingOrPendingSuggestion());

    // If you scroll, we don't need to kill any pending suggestion request, but we do need to hide
    // any suggestion that's already visible because the fake cursor will now be in the wrong place
    this.textArea.addEventListener('scroll', () => this.suggestionDisplay.reject(), { passive: true });
  }

  // Handle the keydown event.
  private handleKeyDown(event: KeyboardEvent): void {
    switch (event.key) {
      case 'Tab':
        if (this.suggestionDisplay.isShowing()) {
          this.suggestionDisplay.accept();
          this.value = null;
          event.preventDefault();
        }
        break;

      case 'Alt':
      case 'Control':
      case 'Shift':
      case 'Command':
        this.removeExistingOrPendingSuggestion();
        break;

      default:
        const keyMatchesExistingSuggestion = this.suggestionDisplay.isShowing()
          && this.suggestionDisplay.currentSuggestion.startsWith(event.key);

        if (keyMatchesExistingSuggestion) {
          // Let the typing happen, but without side-effects like removing the existing selection
          insertTextAtCaretPosition(this.textArea, event.key);
          event.preventDefault();

          // Update the existing suggestion to match the new text
          this.suggestionDisplay.show(this.suggestionDisplay.currentSuggestion.substring(event.key.length));
          scrollTextAreaDownToCaretIfNeeded(this.textArea);

        } else {
          this.removeExistingOrPendingSuggestion();
        }
        break;
    }
  }

  // If this was changed to a 'keypress' event instead, we'd only initiate suggestions after
  // the user types a visible character, not pressing another key (e.g., arrows, or ctrl+c).
  // However for now I think it is desirable to show suggestions after cursor movement.
  private handleKeyUp(event: KeyboardEvent) {
    // If a suggestion is already visible, it must match the current keystroke or it would
    // already have been removed during keydown. So we only start the timeout process if
    // there's no visible suggestion.
    if (!this.suggestionDisplay.isShowing()) {
      clearTimeout(this.typingDebounceTimeout ?? undefined);
      this.typingDebounceTimeout = setTimeout(() => this.handleTypingPaused(), this.delay ?? 350);
    }
  }

  // If the user has paused typing, we should show a suggestion.
  private handleTypingPaused() {
    if (document.activeElement !== this.textArea) {
      return;
    }

    // We only show a suggestion if the cursor is at the end of the current line. Inserting suggestions in
    // the middle of a line is confusing (things move around in unusual ways).
    // TODO: You could also allow the case where all remaining text on the current line is whitespace
    const isAtEndOfCurrentLine =
      this.textArea.selectionStart === this.textArea.selectionEnd
      && (this.textArea.selectionStart === this.textArea.value.length ||
        this.textArea.value[this.textArea.selectionStart] === '\n');

    if (!isAtEndOfCurrentLine) {
      return;
    }

    this.showSuggestion(this.value);
  }

  // Remove any existing suggestion.
  private removeExistingOrPendingSuggestion() {
    clearTimeout(this.typingDebounceTimeout ?? undefined);

    this.pendingSuggestionAbortController?.abort();
    this.pendingSuggestionAbortController = undefined;

    this.suggestionDisplay.reject();
  }

  // Show the suggestion, included in the `value` attribute.
  private showSuggestion(suggestionText: string | null): void {

    // Cancel any pending suggestion
    this.suggestionDisplay.reject();
    this.pendingSuggestionAbortController?.abort();
    this.pendingSuggestionAbortController = new AbortController();

    // If the text is too short, don't show a suggestion
    if (this.textArea.value.length < (this.minlength ?? 0) || this.isNullOrEmpty(suggestionText)) {
      return;
    }

    // Show the suggestion
    const snapshot = {
      abortSignal: this.pendingSuggestionAbortController.signal,
      textAreaValue: this.textArea.value,
      cursorPosition: this.textArea.selectionStart,
    };

    if (suggestionText !== null && this.isNotNullOrEmpty(suggestionText)
      && snapshot.textAreaValue === this.textArea.value
      && snapshot.cursorPosition === this.textArea.selectionStart) {
      if (!suggestionText.endsWith(' ')) {
        suggestionText += ' ';
      }

      this.suggestionDisplay.show(suggestionText);
    }
  }

  // Show or update the suggestion text.
  private showOrUpdateSuggestion(suggestionText: string | null = ''): void {
    this.pendingSuggestionAbortController?.abort();
    this.pendingSuggestionAbortController = new AbortController();

    const snapshot = {
      abortSignal: this.pendingSuggestionAbortController.signal,
      textAreaValue: this.textArea.value,
      cursorPosition: this.textArea.selectionStart,
    };

    if (suggestionText != null && suggestionText !== ''
      && snapshot.textAreaValue === this.textArea.value
      && snapshot.cursorPosition === this.textArea.selectionStart) {
      if (!suggestionText.endsWith(' ')) {
        suggestionText += ' ';
      }

      this.suggestionDisplay.show(suggestionText);
    }
  }

  /**
   * Update the attribute value.
   * @param name
   * @param value
   */
  private updateAttribute(name: string, value: string | number | null): void {

    if (this.getAttribute(name) != value) {
      if (value) {
        this.setAttribute(name, '' + value);
      } else {
        this.removeAttribute(name);
      }
    }

    // anchor attribute changed
    if (name === "anchor") {
      this.initializeTextArea();
    }

    // value attribute changed
    if (name === "value" && typeof value === 'string' && this.isNotNullOrEmpty(value) && this.isNotNullOrEmpty(this.textArea.value) && this.suggestionDisplay.isShowing()) {
      this.showSuggestion(value);
    }

  }

  /**
   * Determines if the inline suggestions should be used.
   * @param textArea
   * @returns
   */
  private shouldUseInlineSuggestions(textArea: HTMLTextAreaElement): boolean {
    // Allow the developer to specify this explicitly if they want
    const explicitConfig = textArea.getAttribute('data-inline-suggestions');
    if (explicitConfig) {
      return explicitConfig.toLowerCase() === 'true';
    }

    // ... but by default, we use overlay on touch devices, inline on non-touch devices
    // That's because:
    //  - Mobile devices will be touch, and most mobile users don't have a "tab" key by which to accept inline suggestions
    //  - Mobile devices such as iOS will display all kinds of extra UI around selected text (e.g., selection handles),
    //    which would look completely wrong
    // In general, the overlay approach is the risk-averse one that works everywhere, even though it's not as attractive.
    const isTouch = 'ontouchstart' in window; // True for any mobile. Usually not true for desktop.
    return !isTouch;
  }

  private isNullOrEmpty(value: string | null): boolean {
    return !this.isNotNullOrEmpty(value);
  }

  private isNotNullOrEmpty(value: string | null): boolean {
    return value !== null && value !== undefined && value !== '';
  }
}
