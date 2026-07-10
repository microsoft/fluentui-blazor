export namespace Microsoft {
  export namespace FluentUI {
    export namespace Blazor {
      export namespace TimePicker {

        const pendingScrolls = new Map<string, number>();
        const initializedDropdowns = new WeakMap<HTMLElement, boolean>();
        const cleanupCallbacks = new WeakMap<HTMLElement, () => void>();
        const defaultScrollDelayMs = 50;

        function scheduleScroll(id: string, value: string | null | undefined, delay: number = 0) {
          const key = `${id}:${value ?? ""}`;
          const existing = pendingScrolls.get(key);
          if (existing !== undefined) {
            window.clearTimeout(existing);
          }

          const timeoutId = window.setTimeout(() => {
            pendingScrolls.delete(key);
            ScrollToSelectedValue(id, value);
          }, delay);

          pendingScrolls.set(key, timeoutId);
        }

        export function Initialize(id: string) {
          const dropdown = document.getElementById(id) as HTMLElement | null;
          if (!dropdown || initializedDropdowns.has(dropdown)) {
            return;
          }

          initializedDropdowns.set(dropdown, true);

          // Use the DOM-selected option when the dropdown is opened or edited.
          const handleOpen = () => scheduleScroll(id, null, defaultScrollDelayMs);
          const handleInput = () => scheduleScroll(id, null);
          const handleKeyDown = (event: KeyboardEvent) => {
            if (["ArrowDown", "ArrowUp", "PageDown", "PageUp", "Home", "End"].includes(event.key)) {
              scheduleScroll(id, null);
            }
          };

          dropdown.addEventListener("click", handleOpen);
          dropdown.addEventListener("focusin", handleOpen);
          dropdown.addEventListener("input", handleInput);
          dropdown.addEventListener("change", handleInput);
          dropdown.addEventListener("keydown", handleKeyDown);

          cleanupCallbacks.set(dropdown, () => {
            dropdown.removeEventListener("click", handleOpen);
            dropdown.removeEventListener("focusin", handleOpen);
            dropdown.removeEventListener("input", handleInput);
            dropdown.removeEventListener("change", handleInput);
            dropdown.removeEventListener("keydown", handleKeyDown);
          });

          scheduleScroll(id, null, defaultScrollDelayMs);
        }

        export function Dispose(id: string) {
          const dropdown = document.getElementById(id) as HTMLElement | null;
          if (!dropdown) {
            return;
          }

          const cleanup = cleanupCallbacks.get(dropdown);
          cleanup?.();

          cleanupCallbacks.delete(dropdown);
          initializedDropdowns.delete(dropdown);
        }

        export function ScrollToSelectedValue(id: string, value: string | null | undefined) {
          const dropdown = document.getElementById(id) as HTMLElement | null;
          if (!dropdown) {
            return;
          }

          const listbox = dropdown.querySelector("fluent-listbox") as HTMLElement | null;
          if (!listbox) {
            return;
          }

          const options = Array.from(listbox.querySelectorAll("fluent-option")) as HTMLElement[];
          const selectedOption = findSelectedOption(options, value) ?? findSelectedOption(options, null);

          if (selectedOption) {
            selectedOption.scrollIntoView({ block: "start", inline: "nearest" });
          }
        }

        function findSelectedOption(options: HTMLElement[], value: string | null | undefined) {
          if (value) {
            const matchByValue = options.find(option => option.getAttribute("value") === value);
            if (matchByValue) {
              return matchByValue;
            }
          }

          return options.find(option => option.hasAttribute("selected") || option.getAttribute("aria-selected") === "true") ?? null;
        }
      }
    }
  }
}
