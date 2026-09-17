export namespace Microsoft.FluentUI.Blazor.Utilities.Logger {

  /**
   * Logs a message to the console
   * @param message Message to log
   * @param args Additional arguments to log
   */
  export function log(message: string, ...args: any[]): void {
    if (!isLocalhost()) {
      return;
    }

    console.log(`[${new Date().toISOString()}] FluentUI Blazor: ${message}`, ...args);
  }

  /**
   * Logs a message to the console, only if the current page is running on localhost
   * @param message Message to log
   * @param args Additional arguments to log
   */
  export function debug(message: string, ...args: any[]): void {
    if (isLocalhost()) {
      log(message, ...args);
    }
  }

  /**
   * Returns True if the current page is running on localhost
   * @returns
   */
  function isLocalhost(): boolean {
    try {
      const parsedUrl = new URL(window.location.href);
      return parsedUrl.hostname === 'localhost' || parsedUrl.hostname === '127.0.0.1';
    } catch (e) {
      return false;
    }
  }
}
