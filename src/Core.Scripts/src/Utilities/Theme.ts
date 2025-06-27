import { webLightTheme, webDarkTheme, BrandVariants, Theme, createDarkTheme, createLightTheme } from '@fluentui/tokens';
import { setTheme } from '@fluentui/web-components';

export namespace Microsoft.FluentUI.Blazor.Utilities.Theme {

  const themeColorVariants: BrandVariants = {
    10: "#050205",
    20: "#231121",
    30: "#3C183A",
    40: "#511C4E",
    50: "#661F63",
    60: "#7D2279",
    70: "#94248F",
    80: "#AA28A5",
    90: "#B443AE",
    100: "#BD59B6",
    110: "#C66EBF",
    120: "#CF82C7",
    130: "#D795D0",
    140: "#DFA8D9",
    150: "#E7BBE1",
    160: "#EECEEA"
  };

  const lightTheme: Theme = {
    ...createLightTheme(themeColorVariants),
  };

  const darkTheme: Theme = {
    ...createDarkTheme(themeColorVariants),
  };

  /**
   * Returns true if the browser is in dark mode
   * @returns
   */
  export function isSystemDark(): boolean {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
  }

  /**
   * Returns true if the current FluentUI theme is dark mode
   * @returns
   */
  export function isDarkMode(): boolean {
    const luminance: string = getComputedStyle(document.documentElement).getPropertyValue('--base-layer-luminance');
    return parseFloat(luminance) < 0.5;
  }

  /**
   * Sets the FluentUI theme to light mode
   */
  export function setLightTheme(): void {
    setTheme(webLightTheme);
    updateBodyTag(false);
  }

  /**
   * Sets the FluentUI theme to dark mode
   */
  export function setDarkTheme(): void {
    setTheme(webDarkTheme);
    updateBodyTag(true);
  }

  /**
  * Adds a listener for media queries to the window object
  * to update the body `data-media` attribute with the current media query
  */
  export function addMediaQueriesListener(): void {

    // Check if the media queries listener is already added
    if ((window as any).__fluentuiBlazorMediaQueriesListener) {
      return;
    }
    (window as any).__fluentuiBlazorMediaQueriesListener = true;

    // List of media queries to listen to
    const getMediaQueries = (): { id: string, query: string }[] => {
      return [
        { id: 'xs', query: '(max-width: 599.98px)' },
        { id: 'sm', query: '(min-width: 600px) and (max-width: 959.98px)' },
        { id: 'md', query: '(min-width: 960px) and (max-width: 1279.98px)' },
        { id: 'lg', query: '(min-width: 1280px) and (max-width: 1919.98px)' },
        { id: 'xl', query: '(min-width: 1920px) and (max-width: 2559.98px)' },
        { id: 'xxl', query: '(min-width: 2560px)' },
      ];
    }

    const dispatchMediaChanged = (bodyTag: HTMLElement) => {
      // Dispatch a custom event when the media query changes
      if (bodyTag) {
        const event = new CustomEvent('mediaChanged', {
          detail: {
            media: bodyTag.getAttribute('data-media')
          }
        });
        bodyTag.dispatchEvent(event);
      }
    }

    // Set the initial data-media attribute based on the current matching media query
    const bodyTag: HTMLElement = document?.body;
    if (bodyTag) {
      const matched = getMediaQueries().find(mq => window.matchMedia(mq.query).matches);
      if (matched) {
        bodyTag.setAttribute('data-media', matched.id);
        dispatchMediaChanged(bodyTag);
      }
    }

    // Add event listeners for each media query
    getMediaQueries().forEach((mediaQuery) => {
      window.matchMedia(mediaQuery.query).addEventListener('change', media => {
          if (media.matches) {
            const bodyTag: HTMLElement = document?.body;
            if (bodyTag && bodyTag.getAttribute('data-media') !== mediaQuery.id) {
              bodyTag.setAttribute('data-media', mediaQuery.id);
              dispatchMediaChanged(bodyTag);
            }
          }
        });
    });
  }

  // Update the body tag to set the data-theme attribute
  function updateBodyTag(isDark: boolean): void {
    const bodyTag: HTMLElement = document?.body;

    if (bodyTag) {
      if (isDark) {
        bodyTag.setAttribute('data-theme', 'dark');
      } else {
        bodyTag.removeAttribute('data-theme');
      }

      // Dispatch the custom event
      const event = new CustomEvent('themeChanged', {
        detail: {
          isDark
        }
      });
      bodyTag.dispatchEvent(event);
    }
  }
}
