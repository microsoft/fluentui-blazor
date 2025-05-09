import { getCaretOffsetFromOffsetParent, scrollTextAreaDownToCaretIfNeeded } from "./CaretUtil";
import { FluentTextSuggestion } from "./FluentTextSuggestion";
import { SuggestionDisplay } from "./SuggestionDisplay";

export class InlineSuggestionDisplay implements SuggestionDisplay {
  private latestSuggestionText: string = '';
  private suggestionStartPos: number | null = null;
  private suggestionEndPos: number | null = null;
  private fakeCaret: FakeCaret | null = null;
  private originalValueProperty: PropertyDescriptor;

  public static SUGGESTION_VISIBLE_ATTRIBUTE: string = 'fluent-suggestion-visible';

  constructor(private owner: FluentTextSuggestion, private textArea: HTMLTextAreaElement | HTMLInputElement) {
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
    this.suggestionStartPos = this.textArea.selectionStart ?? 0;
    this.suggestionEndPos = this.suggestionStartPos + suggestion.length;

    this.textArea.setAttribute(InlineSuggestionDisplay.SUGGESTION_VISIBLE_ATTRIBUTE, '');
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
    this.textArea.removeAttribute(InlineSuggestionDisplay.SUGGESTION_VISIBLE_ATTRIBUTE);

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
    this.textArea.removeAttribute(InlineSuggestionDisplay.SUGGESTION_VISIBLE_ATTRIBUTE);
    this.fakeCaret?.hide();
  }
}

class FakeCaret {
  readonly caretDiv: HTMLDivElement;

  constructor(private owner: FluentTextSuggestion, private textArea: HTMLTextAreaElement | HTMLInputElement) {
    this.caretDiv = document.createElement('div');
    owner.appendChild(this.caretDiv);

    const caretStyle = document.createElement('style');
    caretStyle.innerHTML = `
            @keyframes caret-blink {
              from, to {
                opacity: 100%;
              }
              50% {
                opacity: 0%;
              }
            }

            [${InlineSuggestionDisplay.SUGGESTION_VISIBLE_ATTRIBUTE}]::selection {
              color: #999;
              background-color: transparent;
            }
        `;
    owner.appendChild(caretStyle);

    const shadowRoot = findFirstShadowRoot(textArea);
    if (shadowRoot !== null && shadowRoot instanceof ShadowRoot) {
      const style = document.createElement('style');
      style.textContent = `
        input[${InlineSuggestionDisplay.SUGGESTION_VISIBLE_ATTRIBUTE}]::selection,
        textarea[${InlineSuggestionDisplay.SUGGESTION_VISIBLE_ATTRIBUTE}]::selection {
          color: #999;
          background-color: transparent;
        }
      `;
      shadowRoot.appendChild(style);
    }
  }

  addExtraTop(): number {
    // This is a hack to make the caret appear in the right place in the FluentTextInput.
    // TODO: how to find these 6px by code?
    if (this.owner.shadowQuerySelector !== null && this.owner.shadowQuerySelector.includes("input")) {
      return 6;
    }

    return 0;
  }

  addExtraLeft(): number {
    return 0;
  }

  show() {
    const caretOffset = getCaretOffsetFromOffsetParent(this.textArea);
    const style = this.caretDiv.style;
    style.position = 'absolute';
    style.display = 'block';
    style.top = (caretOffset.top + this.addExtraTop()) + 'px';
    style.left = (caretOffset.left + this.addExtraLeft()) + 'px';
    style.height = caretOffset.height + 'px';
    style.width = '1.0px';
    style.zIndex = this.textArea.style.zIndex;
    style.backgroundColor = caretOffset.elemStyle.caretColor;
    style.animation = 'caret-blink 1.025s step-end infinite';
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

function findFirstShadowRoot(element: Element) {
  let currentNode: Node | null = element;

  while (currentNode) {
    // Check if the current node is a shadow root
    const rootNode = currentNode.getRootNode();
    if (rootNode instanceof ShadowRoot) {
      return rootNode; // Return the first ShadowRoot found
    }

    // Move to the next parent node
    currentNode = currentNode.parentNode;
  }

  return null; // No ShadowRoot found
}
