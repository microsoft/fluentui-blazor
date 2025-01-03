import * as caretPos from 'caret-pos';

export function scrollTextAreaDownToCaretIfNeeded(textArea: HTMLTextAreaElement | HTMLInputElement) {
  // Note that this only scrolls *down*, because that's the only scenario after a suggestion is accepted
  const pos = caretPos.position(textArea);
  const lineHeightInPixels = parseFloat(window.getComputedStyle(textArea).lineHeight);
  if (pos.top > textArea.clientHeight + textArea.scrollTop - lineHeightInPixels) {
    textArea.scrollTop = pos.top - textArea.clientHeight + lineHeightInPixels;
  }
}

export function getCaretOffsetFromOffsetParent(elem: HTMLTextAreaElement | HTMLInputElement): { top: number, left: number, height: number, elemStyle: CSSStyleDeclaration } {
  const elemStyle = window.getComputedStyle(elem);
  const pos = caretPos.position(elem);

  return {
    top: pos.top + parseFloat(elemStyle.borderTopWidth) + elem.offsetTop - elem.scrollTop,
    left: pos.left + parseFloat(elemStyle.borderLeftWidth) + elem.offsetLeft - elem.scrollLeft - 0.25,
    height: pos.height,
    elemStyle: elemStyle,
  }
}

export function insertTextAtCaretPosition(textArea: HTMLTextAreaElement | HTMLInputElement, text: string) {
  // Even though document.execCommand is deprecated, it's still the best way to insert text, because it's
  // the only way that interacts correctly with the undo buffer. If we have to fall back on mutating
  // the .value property directly, it works but erases the undo buffer.
  if (document.execCommand) {
    document.execCommand('insertText', false, text);
  } else {
    let caretPos = textArea.selectionStart ?? 0;
    textArea.value = textArea.value.substring(0, caretPos)
      + text
      + textArea.value.substring(textArea.selectionEnd ?? 0);
    caretPos += text.length;
    textArea.setSelectionRange(caretPos, caretPos);
  }
}
