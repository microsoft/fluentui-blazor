export function scrollToLastConsoleItem() {

  const itemList = document.getElementById('console-items');

  // Callback function to execute when mutations are observed
  const observerCallback = (mutationsList, observer) => {
    for (let mutation of mutationsList) {
      if (mutation.type === 'childList') {
        // Scroll to the bottom when a new item is added
        itemList.scrollTop = itemList.scrollHeight;
      }
    }
  };

  // Create an instance of MutationObserver and pass in the callback function
  const observer = new MutationObserver(observerCallback);

  // Start observing the itemList for changes
  observer.observe(itemList, { childList: true });
}
