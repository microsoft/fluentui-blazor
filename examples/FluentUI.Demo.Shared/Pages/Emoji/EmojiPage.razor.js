export function copyText(text) {
    navigator.clipboard.writeText(text).then(function () {
        alert("FluentEmoji component declaration copied to clipboard:\n\n" + text);
    })
        .catch(function (error) {
            alert(error);
        });
}