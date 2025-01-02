import { getCaretOffsetFromOffsetParent, insertTextAtCaretPosition, scrollTextAreaDownToCaretIfNeeded } from "./CaretUtil";
import { SmartTextArea } from "./SmartTextArea";
import { SuggestionDisplay } from "./SuggestionDisplay";

export class OverlaySuggestionDisplay implements SuggestionDisplay {
  latestSuggestionText: string = '';
  suggestionElement: HTMLDivElement;
  suggestionPrefixElement: HTMLSpanElement;
  suggestionTextElement: HTMLSpanElement;
  showing: boolean;

  constructor(owner: SmartTextArea, private textArea: HTMLTextAreaElement) {
    this.showing = false;
    this.suggestionElement = document.createElement('div');
    this.suggestionElement.classList.add('smart-textarea-suggestion-overlay');
    this.suggestionElement.addEventListener('mousedown', e => this.handleSuggestionClicked(e));
    this.suggestionElement.addEventListener('touchend', e => this.handleSuggestionClicked(e));

    this.suggestionPrefixElement = document.createElement('span');
    this.suggestionTextElement = document.createElement('span');
    this.suggestionElement.appendChild(this.suggestionPrefixElement);
    this.suggestionElement.appendChild(this.suggestionTextElement);

    this.suggestionPrefixElement.style.opacity = '0.3';

    const computedStyle = window.getComputedStyle(this.textArea);
    this.suggestionElement.style.font = computedStyle.font;
    this.suggestionElement.style.marginTop = (parseFloat(computedStyle.fontSize) * 1.4) + 'px';

    owner.appendChild(this.suggestionElement);
  }

  get currentSuggestion() {
    return this.latestSuggestionText;
  }

  show(suggestion: string): void {
    this.latestSuggestionText = suggestion;

    this.suggestionPrefixElement.textContent = suggestion[0] != ' ' ? getCurrentIncompleteWord(this.textArea, 20) : '';
    this.suggestionTextElement.textContent = suggestion;

    const caretOffset = getCaretOffsetFromOffsetParent(this.textArea);
    const style = this.suggestionElement.style;
    style.minWidth = '';
    this.suggestionElement.classList.add('smart-textarea-suggestion-overlay-visible');
    style.zIndex = this.textArea.style.zIndex;
    style.top = caretOffset.top + 'px';

    // If the horizontal position is already close enough, leave it alone. Otherwise it
    // can jiggle annoyingly due to inaccuracies in measuring the caret position.
    const newLeftPos = caretOffset.left - this.suggestionPrefixElement.offsetWidth;
    if (!style.left || Math.abs(parseFloat(style.left) - newLeftPos) > 10) {
      style.left = newLeftPos + 'px';
    }

    this.showing = true;


    // Normally we're happy for the overlay to take up as much width as it can up to the edge of the page.
    // However, if it's too narrow (because the edge of the page is already too close), it will wrap onto
    // many lines. In this case we'll force it to get wider, and then we have to move it further left to
    // avoid spilling off the screen.
    const suggestionComputedStyle = window.getComputedStyle(this.suggestionElement);
    const numLinesOfText = Math.round((this.suggestionElement.offsetHeight - parseFloat(suggestionComputedStyle.paddingTop) - parseFloat(suggestionComputedStyle.paddingBottom))
      / parseFloat(suggestionComputedStyle.lineHeight));
    if (numLinesOfText > 2) {
      const oldWidth = this.suggestionElement.offsetWidth;
      style.minWidth = `calc(min(70vw, ${(numLinesOfText * oldWidth / 2)}px))`; // Aim for 2 lines, but don't get wider than 70% of the screen
    }

    // If the suggestion is too far to the right, move it left so it's not off the screen
    const suggestionClientRect = this.suggestionElement.getBoundingClientRect();
    if (suggestionClientRect.right > document.body.clientWidth - 20) {
      style.left = `calc(${parseFloat(style.left) - (suggestionClientRect.right - document.body.clientWidth)}px - 2rem)`;
    }
  }

  accept(): void {
    if (!this.showing) {
      return;
    }

    insertTextAtCaretPosition(this.textArea, this.currentSuggestion);

    // The newly-inserted text could be so long that the new caret position is off the bottom of the textarea.
    // It won't scroll to the new caret position by default
    scrollTextAreaDownToCaretIfNeeded(this.textArea);

    this.hide();
  }

  reject(): void {
    this.hide();
  }

  hide(): void {
    if (this.showing) {
      this.showing = false;
      this.suggestionElement.classList.remove('smart-textarea-suggestion-overlay-visible');
    }
  }

  isShowing(): boolean {
    return this.showing;
  }

  handleSuggestionClicked(event: Event) {
    event.preventDefault();
    event.stopImmediatePropagation();
    this.accept();
  }
}

function getCurrentIncompleteWord(textArea: HTMLTextAreaElement, maxLength: number) {
  const text = textArea.value;
  const caretPos = textArea.selectionStart;

  // Not all languages have words separated by spaces. Imposing the maxlength rule
  // means we'll not show the prefix for those languages if you're in the middle
  // of longer text (and ensures we don't search through a long block), which is ideal.
  for (let i = caretPos - 1; i > caretPos - maxLength; i--) {
    if (i < 0 || text[i].match(/\s/)) {
      return text.substring(i + 1, caretPos);
    }
  }

  return '';
}
