export function getScrollDistToTop() {
    const scrollTop = document.documentElement.scrollTop || document.body.scrollTop || 0;
    return Math.round(scrollTop);
}

export function getScrollDistToBottom() {
    const dist = document.documentElement.scrollHeight -
        document.documentElement.scrollTop -
        document.documentElement.clientHeight;
    return Math.round(dist);
}
