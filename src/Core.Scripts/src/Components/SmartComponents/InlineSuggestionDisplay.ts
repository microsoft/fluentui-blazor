import { getCaretOffsetFromOffsetParent } from "./CaretUtil";
import { SmartTextArea } from "./SmartTextArea";
import { SuggestionDisplay } from "./SuggestionDisplay";

export class InlineSuggestionDisplay implements SuggestionDisplay {
  latestSuggestionText: string = '';
  suggestionStartPos: number | null = null;
  suggestionEndPos: number | null = null;
  fakeCaret: FakeCaret | null = null;
  originalValueProperty: PropertyDescriptor;

  constructor(private owner: SmartTextArea, private textArea: HTMLTextAreaElement) {
    // When any other JS code asks for the value of the textarea, we want to return the value
    // without any pending suggestion, otherwise it will break things like bindings
    this.originalValueProperty = findPropertyRecursive(textArea, 'value');
    const self: any = this;
    Object.defineProperty(textArea, 'value', {
      get() {
        const trueValue = self.originalValueProperty.get.call(textArea);
        return self.isShowing()
          ? trueValue.substring(0, self.suggestionStartPos) + trueValue.substring(self.suggestionEndPos)
          : trueValue;
      },
      set(v) {
        self.originalValueProperty.set.call(textArea, v);
      }
    });
  }

  get valueIncludingSuggestion() {
    return (this as any).originalValueProperty.get.call(this.textArea);
  }

  set valueIncludingSuggestion(val: string) {
    (this as any).originalValueProperty.set.call(this.textArea, val);
  }

  isShowing(): boolean {
    return this.suggestionStartPos !== null;
  }

  show(suggestion: string): void {
    this.latestSuggestionText = suggestion;
    this.suggestionStartPos = this.textArea.selectionStart;
    this.suggestionEndPos = this.suggestionStartPos + suggestion.length;

    this.textArea.setAttribute('data-suggestion-visible', '');
    this.valueIncludingSuggestion = this.valueIncludingSuggestion.substring(0, this.suggestionStartPos) + suggestion + this.valueIncludingSuggestion.substring(this.suggestionStartPos);
    this.textArea.setSelectionRange(this.suggestionStartPos, this.suggestionEndPos);

    this.fakeCaret ??= new FakeCaret(this.owner, this.textArea);
    this.fakeCaret.show();
  }

  get currentSuggestion() {
    return this.latestSuggestionText;
  }

  accept(): void {
    this.textArea.setSelectionRange(this.suggestionEndPos, this.suggestionEndPos);
    this.suggestionStartPos = null;
    this.suggestionEndPos = null;
    this.fakeCaret?.hide();
    this.textArea.removeAttribute('data-suggestion-visible');

    // The newly-inserted text could be so long that the new caret position is off the bottom of the textarea.
    // It won't scroll to the new caret position by default
    scrollTextAreaDownToCaretIfNeeded(this.textArea);
  }

  reject(): void {
    if (!this.isShowing()) {
      return; // No suggestion is shown
    }

    const prevSelectionStart = this.textArea.selectionStart;
    const prevSelectionEnd = this.textArea.selectionEnd;
    this.valueIncludingSuggestion = this.valueIncludingSuggestion.substring(0, (this as any).suggestionStartPos) + this.valueIncludingSuggestion.substring((this as any).suggestionEndPos);

    if (this.suggestionStartPos === prevSelectionStart && this.suggestionEndPos === prevSelectionEnd) {
      // For most interactions we don't need to do anything to preserve the cursor position, but for
      // 'scroll' events we do (because the interaction isn't going to set a cursor position naturally)
      this.textArea.setSelectionRange(prevSelectionStart, prevSelectionStart /* not 'end' because we removed the suggestion */);
    }

    this.suggestionStartPos = null;
    this.suggestionEndPos = null;
    this.textArea.removeAttribute('data-suggestion-visible');
    this.fakeCaret?.hide();
  }
}

class FakeCaret {
  readonly caretDiv: HTMLDivElement;

  constructor(owner: SmartTextArea, private textArea: HTMLTextAreaElement) {
    this.caretDiv = document.createElement('div');
    this.caretDiv.classList.add('smart-textarea-caret');
    owner.appendChild(this.caretDiv);
  }

  show() {
    const caretOffset = getCaretOffsetFromOffsetParent(this.textArea);
    const style = this.caretDiv.style;
    style.display = 'block';
    style.top = caretOffset.top + 'px';
    style.left = caretOffset.left + 'px';
    style.height = caretOffset.height + 'px';
    style.zIndex = this.textArea.style.zIndex;
    style.backgroundColor = caretOffset.elemStyle.caretColor;
  }

  hide() {
    this.caretDiv.style.display = 'none';
  }
}

function findPropertyRecursive(obj: any, propName: string): PropertyDescriptor {
  while (obj) {
    const descriptor = Object.getOwnPropertyDescriptor(obj, propName);
    if (descriptor) {
      return descriptor;
    }
    obj = Object.getPrototypeOf(obj);
  }

  throw new Error(`Property ${propName} not found on object or its prototype chain`);
}

function scrollTextAreaDownToCaretIfNeeded(textArea: HTMLTextAreaElement) {
    throw new Error("Function not implemented.");
}
