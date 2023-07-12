export function copyText(text) {
    navigator.clipboard.writeText(text).then(function () {
        alert("FluentIcon component declaration copied to clipboard:\n\n" + text);
    })
        .catch(function (error) {
            alert(error);
        });
}