export namespace Microsoft.FluentUI.Blazor.Calendar {

  /**
   * Set accessibility keyboard navigation for the calendar.
   * @param id - The ID of the calendar element to enhance accessibility.
   */
  export function SetAccessibilityKeyboard(id: string) {
    const calendar = document.getElementById(id);

    // Add keydown event listeners
    AddKeydownListener(calendar, `.title div`, `div[part='months'] div[tabindex='0']`);
    AddKeydownListener(calendar, `.previous`);
    AddKeydownListener(calendar, `.next`);
  }







  /**
   * Adds a keydown event listener to a specified element within a calendar.
   * This function enhances accessibility by allowing keyboard navigation.
   * When the Space or Enter key is pressed on the specified element, it triggers a click event
   * and moves the focus to the next focusable element within the calendar.
   * @param calendar - The calendar element to search within.
   * @param itemSelector - The CSS selector to find the item to attach the keydown listener to.
   * @param focusSelector - The CSS selector to find the next focusable element after the item is clicked. If null, the itemSelector will be used.
   */
  function AddKeydownListener(calendar: HTMLElement | null, itemSelector: string, focusSelector: string | null = null) {
    if (calendar) {
      const item = calendar.querySelector(itemSelector) as HTMLElement;

      if (item && !item.dataset.keydownRegistered) {

        // Add keydown event listener to the item
        item.addEventListener("keydown", (event: KeyboardEvent) => {

          // Check if the pressed key is Space or Enter
          if (event.code === "Space" || event.code === "Enter") {
            event.preventDefault();
            event.stopPropagation();
            item.click();

            // Find the first focusable element within the specified selector
            SetFocus(calendar, focusSelector ?? itemSelector);
          }
        });
        item.dataset.keydownRegistered = "true";
      }

    }
  }

  /**
   * Sets focus on the first element matching the provided query selector.
   * If the element is not found, it will retry every 20 milliseconds until the timeout is reached.
   * @param calendar - The calendar element to search within.
   * @param querySelector - The CSS selector to find the element.
   * @param timeOut - The maximum time in milliseconds to wait for the element to appear (default is 400 ms).
   */
  function SetFocus(calendar: HTMLElement, querySelector: string, timeOut: number = 400) {
    const intervalTime = 20; // Interval time between attempts in milliseconds
    let elapsedTime = 0; // Elapsed time in milliseconds

    const intervalId = setInterval(() => {
      elapsedTime += intervalTime;

      // Select the first element matching the query selector
      const element = calendar.querySelector(querySelector) as HTMLElement;

      // If the element is found, move focus to it and clear the interval
      if (element) {
        element.focus();
        clearInterval(intervalId);
        return;
      }

      // If the maximum time has elapsed, clear the interval
      if (elapsedTime >= timeOut) {
        clearInterval(intervalId);
      }
    }, intervalTime);
  }
}
