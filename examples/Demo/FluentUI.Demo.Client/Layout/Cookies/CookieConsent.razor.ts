type ClarityFunction = ((...args: unknown[]) => void) & { q?: unknown[][] };

interface CookiePolicy {
  acceptAnalytics?: boolean | null;
  acceptSocialMedia?: boolean | null;
  acceptAdvertising?: boolean | null;
}

declare global {
  interface Window {
    dataLayer: IArguments[];
    clarity: ClarityFunction;
  }
}

window.dataLayer = window.dataLayer || [];

function deleteCookie(name: string, domain: string): void {
  document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/;domain=' + domain;
}

function gtag(..._args: unknown[]): void {
  window.dataLayer.push(arguments);
}

function eraseGoogleAnalyticsCookies(measurementId: string): void {
  deleteCookie('_ga', '.fluentui-blazor.net');
  deleteCookie('_gid', '.fluentui-blazor.net');
  deleteCookie('_gat', '.fluentui-blazor.net');
  deleteCookie('_ga_' + measurementId.split('-')[1], '.fluentui-blazor.net');


  console.log('Google Analytics cookies erased');
}

const injectGAScript = (measurementId: string): void => {
  // Load the Google tag manager script dynamically
  const script = document.createElement('script');
  script.async = true;
  script.src = `https://www.googletagmanager.com/gtag/js?id=${measurementId}`;
  document.head.appendChild(script);

  // Initialize GA4 once the script loads
  script.onload = () => {
    gtag('js', new Date());
    gtag('config', measurementId);
    console.log('Google Analytics 4 initialized successfully');
  };

  script.onerror = () => {
    console.error('Failed to load Google Analytics 4');
  };
};

function injectMCScript(projectId: string): void {
  try {
    if (document.getElementById('clarity-script')) {
      return;
    }

    window.clarity = window.clarity || ((...args: unknown[]) => {
      (window.clarity.q ??= []).push(args);
    });

    const script = document.createElement('script');
    script.async = true;
    script.src = 'https://www.clarity.ms/tag/' + projectId;
    script.id = 'clarity-script';

    const firstScript = document.getElementsByTagName('script')[0];
    firstScript?.parentNode?.insertBefore(script, firstScript);
    console.log('Microsoft Clarity initialized successfully');

  } catch (error) {
    console.error('Failed to load Microsoft Clarity', error);
  }
}

const Clarity = {
  init(projectId: string): void {
    injectMCScript(projectId);
  },

  setTag(key: string, value: string): void {
    window.clarity('set', key, value);
  },

  identify(customerId: string, customSessionId?: string, customPageId?: string, friendlyName?: string): void {
    window.clarity('identify', customerId, customSessionId, customPageId, friendlyName);
  },

  consent(acceptAnalytics: boolean | null | undefined, acceptAdvertising: boolean | null | undefined): void {
    const analyticsStorage = acceptAnalytics === null ? 'denied' : acceptAnalytics ? 'granted' : 'denied';
    const adStorage = acceptAdvertising === null ? 'denied' : acceptAdvertising ? 'granted' : 'denied';

    window.clarity('consentv2', {
      analytics_Storage: analyticsStorage,
      ad_Storage: adStorage
    });

    console.log(`Microsoft Clarity initialized and consent set: analytics_Storage=${analyticsStorage}, ad_Storage=${adStorage}`);
  },

  upgrade(reason: string): void {
    window.clarity('upgrade', reason);
  },

  event(eventName: string): void {
    window.clarity('event', eventName);
  },

  erase(): void {
    window.clarity('consent', false);
  }
};

export async function getCookiePolicy(): Promise<CookiePolicy | null> {
  const cookiePolicy = localStorage.getItem('cookie-policy');

  return cookiePolicy === null ? null : JSON.parse(cookiePolicy) as CookiePolicy;
}

export async function setCookiePolicy(state: CookiePolicy): Promise<void> {
  localStorage.setItem('cookie-policy', JSON.stringify(state));
}

export async function initAnalytics( GAmeasurementId: string, MCprojectId: string, cookiePolicy: CookiePolicy | null): Promise<void> {

  // Microsoft Clarity initialization
  Clarity.init(MCprojectId);

  if (cookiePolicy === null || cookiePolicy.acceptAnalytics === null) {
    eraseGoogleAnalyticsCookies(GAmeasurementId);
    console.log('Google Analytics cookies erased');

    Clarity.erase();
    console.log('Microsoft Clarity cookies erased');

    return;
  }

  if (cookiePolicy?.acceptAnalytics) {
    injectGAScript(GAmeasurementId);
  }

  if (cookiePolicy?.acceptAnalytics || cookiePolicy?.acceptAdvertising) {
    Clarity.consent(cookiePolicy?.acceptAnalytics, cookiePolicy?.acceptAdvertising);
  }
}
