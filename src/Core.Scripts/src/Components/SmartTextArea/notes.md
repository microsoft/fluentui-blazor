 `<textarea>` doesn't natively have any way to render an inline suggestion. It doesn't even have a way
 to render text in more than one color. There are several ways we could approach doing it:

 * Selection
   Append the suggestion to the actual textarea value, and use JS to mark the suggested range as "selected"
   e.g., setting selectionStart/selectionEnd. Use the ::selection pseudoselector to style the suggestion,
   e.g., in gray. This CSS needs to apply only when a suggestion is being shown, and not if the user performs
   some other interaction such as clicking or moving the caret.
   Example implementation: https://www.w3.org/WAI/ARIA/apg/patterns/combobox/examples/combobox-autocomplete-both/
   Pro: Takes care of almost all styling issues automatically. No weirdness in the DOM.
   Pro: *Might* be what assistive technologies expect when you set aria-autocomplete="inline". It's the closest
        thing to the example at the w3.org link above.
        (although that example has broken UX in other ways)
   Pro: Possible to insert suggestions arbitrarily in the middle of the text, not just at the end, and the subsequent
        text will move along with it.
   Con: The caret will not be rendered by default while there is a selection, so you'd need to render a custom one.
   Con: Since it's literally editing the textarea.value, it may trigger unrelated behaviors such as confusing a
        SPA library's renderer that also writes to textarea.value, or making the browser show spell-check highlights
        on the suggested text.
   Con: Since it modifies the textarea value programmatically, it erases the undo buffer. To some extent this can be
        mitigated by using document.execCommand to mutate the value, but [1] execCommand is deprecated and [2] it
        still leaves edits like "add suggestion" and "remove suggestion" on the undo stack, so ctrl+z behaves weirdly
        by re-showing earlier suggestions - you may have to ctrl+z many times to get back to the earlier state you want.
   Con: Like above, since it modifies the textarea value programmatically, it loses track of which column you want the
        caret to be in when you use arrows to move up and down lines (the native behavior is to track which column you
        were in before you moved up or down, and to try to keep the caret in the same column).
        Possible mitigation: don't show suggestions in response to the user pressing an arrow key or otherwise moving
        the caret, but only in response to typing.
   Con: A long tail of code needed to handle all the editing gestures and make sure they still work correctly given
        how we're mutating the value (e.g., when you arrow left, we don't just want to deselect the suggestion, which
        is what that key does normally when you have a selection). Hard to even guess what edge cases might need to
        be accounted for on touch devices.
   Con: On iOS at least, selection is displayed in a much more intrusive manner, with different styling than you've set
        in CSS, and with big drag handles around it. This is not an acceptable appearance because it doesn't look like
        it's a suggestion at all. I don't see evidence that you can suppress this.
   Con: Having made a version of this that works moderately well on desktop, I tried on iOS and found it to behave almost
        entirely wrongly. For example, when you reject a selection it moves the cursor to the end for some reason, and
        the hold-to-move-caret gesture leaves the suggestion behind. This would be a *long* tail of bugs, with no guarantee
        that new ones won't appear with each iOS update.

   Overall, this is a good option for non-touch devices as it allows for inline suggestions in the middle of text while
   retaining most native `<textarea>` behaviors. The impact on 'undo' is unfortunate but will only occur when a suggestion
   actually appears, so a partial mitigation is not showing suggestions except when the user actually types new content.

 * Contenteditable
   Instead of a `<textarea>`, render <div contenteditable="true">. You can then arbitrarily style the text. This is
   more like how other inline autocompletions typically work, e.g., the "mention" features in Twitter/Slack/etc.
   Pro: Again, *might* be what aria-autocomplete="inline" has in mind, though it's unclear how it would know which part
        of the text to announce as being a suggestion.
   Con: Since it's not actually a `<textarea>`, many native behaviors would be lost (e.g., maxlength, spellcheck,
        drag to resize). Also SPA renderers would not know how to read and write the value without further integration.
   Con: The app's CSS rules won't automatically style this to look like a textarea. Developers would need to write
        special-case CSS for this, which would be extremely hard if you use something like Tailwind and don't even know
        the actual underlying CSS rules that you need to apply.
   Con: Presumably, mutating the value programmatially will cause the same loss of native text editing behaviors as
        in the selection approach (trashing the undo buffer, losing track of the column when moving up and down lines, etc).

   Overall, the loss of native behaviors and styling makes this too problematic. It's a good option if you were building this
   just for one particular web app where you know what styling and other behaviors are required. It's not good as a general
   solution for all web apps that needs to integrate with arbitrary styling and needs to retain other `<textarea>` features.

 * Overlaying another element
   Render some other element (e.g., another `<textarea>`) behind the real textarea, using a combination of CSS and JS
   to keep the position and style equivalent. Force the real textarea to have a transparent background so we
   see the one behind it.
   Example implementation: https://complete-ly.appspot.com/
   Another example: https://kyuch4n.github.io/react-inline-autocomplete/
   Pro: The real textarea remains a real `<textarea>` and nothing about its behavior changes, so it will naturally
        integrate with anything that a textarea normally integrates with.
   Con: Some CSS rules would break it, e.g., if the developer explicitly puts an ID on the real element and then
        matches with #id or selectors like :first-child, since they would apply styles only to the real element and
        not to the suggestions.
   Con: Requires effort from the framework to position the suggestion element correctly.
   Con: I don't think this would make sense to aria-autocomplete="inline", and am unsure what other mechanism would
        exist to make the suggestion known to assistive technologies.
   Con: No way to insert suggestions arbitrarily in the middle of the text (even restricted to end-of-line) because
        if the suggestion requires inserting another line, the text in the real textarea would not be in sync.
        This is a fundamental issue: you can't solve it by mutating the text in the real textarea to make room, because
        that reintroduces all the same problems (destruction of native behaviors) as the previous approaches.

   Overall, the inability to insert suggestions anywhere except at the very end makes this approach too limited. Which
   is a shame because beside that, it's not bad.

 * Floating suggestion box
   i.e., something more like the traditional code completion UI. This is the only way I've actually seen autocomplete
   done on the web for real (excluding hyper-sophisticated solutions like the Monaco text editor).
   Example implementation: https://primer.style/react/drafts/InlineAutocomplete
   Pro: Can insert suggestions anywhere, not just at the end.
   Pro: No interference with the native textarea behaviors.
   Pro: Can style it arbitrarily, not limited like in the "selection" approach.
   Con: Doesn't look as good.

   Overall, while this option is less cool, it's the only option that works on touch devices while also allowing insertions
   in the middle of text.
