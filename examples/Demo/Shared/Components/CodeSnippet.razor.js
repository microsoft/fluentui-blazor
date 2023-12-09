function addCopyButton() {
    var snippets = document.querySelectorAll('.snippet');
    var numberOfSnippets = snippets.length;
    for (var i = 0; i < numberOfSnippets; i++) {
        let code = snippets[i].getElementsByTagName('code')[0].innerText;

        //snippets[i].classList.add('hljs'); // append copy button to pre tag

        snippets[i].innerHTML = snippets[i].innerHTML + '<button class="hljs-copy">Copy</button>'; // append copy button

        snippets[i].getElementsByClassName('hljs-copy')[0].addEventListener("click", function () {
            //this.innerText = 'Copying..';
            
                navigator.clipboard.writeText(code);
            
            this.innerText = 'Copied!';
            let button = this;
            setTimeout(function () {
                button.innerText = 'Copy';
            }, 1000)
        });
    }
}
addCopyButton();