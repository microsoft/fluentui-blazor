export function setSpacing(margin, padding) {
  // Create a div element
  const div = document.createElement('div');

  // Set the margin of the div element
  div.style.margin = margin;
  div.style.padding = padding;
  div.className = margin + ' ' + padding;

  // Append the div to the document body
  document.body.appendChild(div);

  // Retrieve the computed margin values
  const computedStyle = window.getComputedStyle(div);
  const minSize = "18px";

  document.querySelector(".margin-top").style.height = computedStyle.marginTop;
  document.querySelector(".margin-right").style.width = computedStyle.marginRight;
  document.querySelector(".margin-bottom").style.height = computedStyle.marginBottom;
  document.querySelector(".margin-left").style.width = computedStyle.marginLeft;

  document.querySelector(".padding-top").style.height = computedStyle.paddingTop;
  document.querySelector(".padding-right").style.width = computedStyle.paddingRight;
  document.querySelector(".padding-bottom").style.height = computedStyle.paddingBottom;
  document.querySelector(".padding-left").style.width = computedStyle.paddingLeft;

  document.querySelector(".container").style.width =
    " calc(100px + 3px " +
    "+ max(" + computedStyle.marginLeft + ", " + minSize + ") " +
    "+ max(" + computedStyle.marginRight + ", " + minSize + ") " +
    "+ max(" + computedStyle.paddingLeft + ", " + minSize + ") " +
    "+ max(" + computedStyle.paddingRight + ", " + minSize + ") )";

  document.querySelector(".container").style.height =
    " calc(32px + 3px " +
    "+ max(" + computedStyle.marginTop + ", " + minSize + ") " +
    "+ max(" + computedStyle.marginBottom + ", " + minSize + ") " +
    "+ max(" + computedStyle.paddingTop + ", " + minSize + ") " +
    "+ max(" + computedStyle.paddingBottom + ", " + minSize + ") )";

  // Dimension values
  document.querySelector(".margin-top").innerHTML = parseFloat(computedStyle.marginTop);
  document.querySelector(".margin-right").innerHTML = parseFloat(computedStyle.marginRight);
  document.querySelector(".margin-bottom").innerHTML = parseFloat(computedStyle.marginBottom);
  document.querySelector(".margin-left").innerHTML = parseFloat(computedStyle.marginLeft);

  document.querySelector(".padding-top").innerHTML = parseFloat(computedStyle.paddingTop);
  document.querySelector(".padding-right").innerHTML = parseFloat(computedStyle.paddingRight);
  document.querySelector(".padding-bottom").innerHTML = parseFloat(computedStyle.paddingBottom);
  document.querySelector(".padding-left").innerHTML = parseFloat(computedStyle.paddingLeft);

  // Optionally, remove the div from the document
  document.body.removeChild(div);
}
