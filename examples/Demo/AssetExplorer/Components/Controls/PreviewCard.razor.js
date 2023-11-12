export function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        return "";
    }).catch(function (error) {
        return error;
    });
}