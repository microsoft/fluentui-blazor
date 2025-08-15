export namespace Microsoft.FluentUI.Blazor.Calendar {

  /**
   * Set accessibility keyboard navigation for the calendar.
   * @param calendar - The calendar element to enhance accessibility.
  */
  export function SetAccessibilityKeyboard(calendar: HTMLElement, defaultFocusSelector: string | null = null) {

    if (!calendar) {
      return;
    }

    // Add keydown event listeners
    AddKeyAcceptListener(calendar, `.title div`, `div[part='months'] div[tabindex='0']`);
    AddKeyAcceptListener(calendar, `.previous`, `.previous`);
    AddKeyAcceptListener(calendar, `.next`, `.next`);

    AddKeyAcceptListener(calendar, `.day`);
    AddKeyAcceptListener(calendar, `.month`);
    AddKeyAcceptListener(calendar, `.year`);

    // Add navigation event listeners
    AddNavigateListener(calendar, `.day`);
    AddNavigateListener(calendar, `.month`);
    AddNavigateListener(calendar, `.year`);

    // Default focus selector
    if (defaultFocusSelector) {
      SetFocus(calendar, defaultFocusSelector);
    }
  }


  function AddNavigateListener(calendar: HTMLElement | null, itemSelector: string) {
    if (calendar) {
      const items = calendar.querySelectorAll(itemSelector) as NodeListOf<HTMLElement>;

      for (const item of items) {
        if (item && !(item as any).__navigationRegistered) {

          // Add keydown event listener to the item
          item.addEventListener("keydown", (event: KeyboardEvent) => {
            // Check if the pressed key is ArrowLeft, ArrowRight, ArrowUp, or ArrowDown
            if (event.code === "ArrowLeft" || event.code === "ArrowRight" ||
              event.code === "ArrowUp" || event.code === "ArrowDown") {
              event.preventDefault();
              event.stopPropagation();

              const nextItem = GetNextItem(items, item, event.code);

              // If a next item is found, set focus on it
              if (nextItem) {
                SetFocus(calendar, nextItem);
              }
              else {
                // Go to the next month/year
                if (event.code === "ArrowRight" || event.code === "ArrowDown") {
                  const nextButton = calendar.querySelector(".next") as HTMLElement;
                  nextButton?.click();
                }

                // Go to the previous month/year
                if (event.code === "ArrowLeft" || event.code === "ArrowUp") {
                  const nextButton = calendar.querySelector(".previous") as HTMLElement;
                  nextButton?.click();
                }
              }
            }
          });
        }

        // Mark the item as having the keydown listener registered
        (item as any).__navigationRegistered = "true";
      }
    }
  }

  /**
   * Determines the next focusable item based on the current item and the pressed key (Left, Right, ...).
   */
  function GetNextItem(items: NodeListOf<HTMLElement>, item: HTMLElement, keyCode: string): HTMLElement | null {
    const itemArray = Array.from(items);
    const currentIndex = itemArray.indexOf(item);
    const nextLineIncrement: number = item.classList.contains("day") ? 7 : item.classList.contains("month") ? 4 : 4;

    switch (keyCode) {
      // Right
      case "ArrowRight":
        for (let i = currentIndex + 1; i < itemArray.length; i++) {
          if (isEnableItem(itemArray[i])) {
            return itemArray[i];
          }
        }
        break;

      // Left
      case "ArrowLeft":
        for (let i = currentIndex - 1; i >= 0; i--) {
          if (isEnableItem(itemArray[i])) {
            return itemArray[i];
          }
        }
        break;

      // Down
      case "ArrowDown":
        for (let i = currentIndex + nextLineIncrement; i < itemArray.length; i += nextLineIncrement) {
          if (isEnableItem(itemArray[i])) {
            return itemArray[i];
          }
        }
        break;

      // Up
      case "ArrowUp":
        for (let i = currentIndex - nextLineIncrement; i >= 0; i -= nextLineIncrement) {
          if (isEnableItem(itemArray[i])) {
            return itemArray[i];
          }
        }
        break;
    }

    // Not found
    return null;

    // Returns True if the item is enabled (not disabled or inactive)
    function isEnableItem(element: HTMLElement): boolean {
      return !element.hasAttribute("disabled") && !element.hasAttribute("inactive");
    }
  }


  /**
   * Adds a keydown event listener to a specified element within a calendar.
   * This function enhances accessibility by allowing keyboard navigation.
   * When the Space or Enter key is pressed on the specified element, it triggers a click event
   * and moves the focus to the next focusable element within the calendar.
   * @param calendar - The calendar element to search within.
   * @param itemSelector - The CSS selector to find the item to attach the keydown listener to.
   * @param focusSelector - The CSS selector to find the next focusable element after the item is clicked. If null, the current item will be used.
   */
  function AddKeyAcceptListener(calendar: HTMLElement | null, itemSelector: string, focusSelector: string | null = null) {
    if (calendar) {
      const items = calendar.querySelectorAll(itemSelector) as NodeListOf<HTMLElement>;

      for (const item of items) {
        if (item && !(item as any).__keyAcceptRegistered) {

          // Add keydown event listener to the item
          item.addEventListener("keydown", (event: KeyboardEvent) => {

            // Check if the pressed key is Space or Enter
            if (event.code === "Space" || event.code === "Enter") {
              event.preventDefault();
              event.stopPropagation();
              item.click();

              // Find the first focusable element within the specified selector
              SetFocus(calendar, focusSelector ?? item);
            }
          });

          // Mark the item as having the keydown listener registered
          (item as any).__keyAcceptRegistered = "true";
        }
      }
    }
  }

  /**
   * Sets focus on the first element matching the provided query selector.
   * If the element is not found, it will retry every 20 milliseconds until the timeout is reached.
   * @param calendar - The calendar element to search within.
   * @param querySelectorOrItem - The CSS selector to find the element or an HTMLElement to focus on.
   * @param timeOut - The maximum time in milliseconds to wait for the element to appear (default is 400 ms).
   */
  function SetFocus(calendar: HTMLElement, querySelectorOrItem: string | HTMLElement, timeOut: number = 500) {
    const intervalTime = 20; // Interval time between attempts in milliseconds
    let elapsedTime = 0; // Elapsed time in milliseconds

    const intervalId = setInterval(() => {
      elapsedTime += intervalTime;

      // Select the first element matching the query selector
      const element = (querySelectorOrItem instanceof HTMLElement)
        ? querySelectorOrItem
        : calendar.querySelector(querySelectorOrItem) as HTMLElement;

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
