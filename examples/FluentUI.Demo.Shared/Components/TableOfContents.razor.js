export function queryDomForTocEntries() {
    const article = document.getElementById('article');
    const headings = article.querySelectorAll("h2, h3");

    let tocArray = new Array()
    let chapter = null;
    let subchapter = null;

    for (let element of headings) {
        let anchor = {
            "level": element.nodeName,
            "text": element.innerText,
            "href": "#" + element.id,
            "anchors": new Array()
        };

        if ("H3" === element.nodeName) {
            if (chapter) {
                subchapter = anchor;
                chapter.anchors.push(subchapter);

            }
        } else {
            chapter = anchor;
            tocArray.push(chapter);

        }
    }
    return tocArray;
}

let backToTopButton = document.getElementById("backtotop");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function ()
{ 
    scrollFunction()
};

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        backToTopButton.style.display = "block";
    } else {
        backToTopButton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
export function backToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}