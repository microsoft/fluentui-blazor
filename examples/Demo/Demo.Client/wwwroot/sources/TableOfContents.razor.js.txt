export function queryDomForTocEntries() {
    const article = document.getElementById('article');
    const headings = article.querySelectorAll("h2, h3, h4");

    let tocArray = new Array()
    let chapter = null;
    let subchapter = null;

    for (let element of headings) {
        if (!element.id) {
            let anchorText = element.innerText;
            let elementId = anchorText.replaceAll(" ", "-", "/", "\\", "#", "$", "@", ":", ",").toLowerCase();
            element.id = elementId;
        }
        if (element.innerText) {
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
            } else if ("H4" === element.nodeName) {
                if (subchapter) {
                    subchapter.anchors.push(anchor);
                }
            }
            else {
                chapter = anchor;
                tocArray.push(chapter);

            }
        }
    }
    return tocArray;
}

let backToTopButton = document.getElementById("backtotop");

// When the user scrolls down 20px from the top of the document, show the button
let bodycontent = document.getElementById('body-content');
if (!bodycontent) {
    bodycontent = document.body;
}

bodycontent.onscroll = function ()
{ 
    scrollFunction()
};

function scrollFunction() {
    if (document.body.scrollTop > 20 ||document.getElementById('body-content').scrollTop > 20 || document.documentElement.scrollTop > 20) {
        backToTopButton.style.display = "flex";
    } else {
        backToTopButton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
export function backToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    document.getElementById('body-content').scrollTop = 0;
}

// Very simple check to see if mobile or tablet is being used 
export function isDevice() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}