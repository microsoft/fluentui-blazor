export function stopKeyPropagation() {
    //stop propagation of keypress
    document.body.addEventListener('keydown', setupListner);
}

export function enableKeyPropagation() {
    //enable propagation of keypress
    document.body.removeEventListener('keydown', setupListner);
}

function setupListner(event) {
    if (event.altKey || event.ctrlKey || event.metaKey) {
        event.preventDefault(); // Prevent the default action
        event.stopPropagation(); // Stop the event from propagating up the DOM tree
        console.log('keydown prevented!');
    }
} 
