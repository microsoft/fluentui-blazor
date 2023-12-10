export function addCopyButton() {
    var snippets = document.querySelectorAll('.snippet');
    var numberOfSnippets = snippets.length;
    for (var i = 0; i < numberOfSnippets; i++) {
        let copyButton = snippets[i].getElementsByClassName("hljs-copy")
        if (copyButton.length === 0) {
            let code = snippets[i].getElementsByTagName('code')[0].innerText;
            snippets[i].innerHTML = snippets[i].innerHTML + '<button class="hljs-copy">Copy</button>'; // append copy button

            copyButton[0].addEventListener("click", function () {
                navigator.clipboard.writeText(code);

                this.innerText = 'Copied!';
                let button = this;
                setTimeout(function () {
                    button.innerText = 'Copy';
                }, 1000)
            });
        }
    }
}
