import {
  createTabster,
  getGroupper,
  getModalizer,
  getMover,
  getRestorer,
  setTabsterAttribute,
  Types as TabsterTypes
} from 'tabster';

import { KEYBORG_FOCUSIN, KeyborgFocusInEvent } from 'keyborg';

const FOCUS_VISIBLE_ATTR_NAME = 'data-fui-focus-visible';
const FOCUS_WITHIN_ATTR_NAME = 'data-fui-focus-within';

interface ArrowNavigationGroupModes {
  Both: 0;
  Vertical: 1;
  Horizontal: 2;
  Grid: 3;
  GridLinear: 4
};
const ArrowNavigationGroupModes: ArrowNavigationGroupModes = {
  Both: 0,
  Vertical: 1,
  Horizontal: 2,
  Grid: 3,
  GridLinear: 4
};
type ArrowNavigationGroupMode = ArrowNavigationGroupModes[keyof ArrowNavigationGroupModes];

interface FocusGroupModes {
  Off: 0;
  NoTab: 1;
  TabExit: 2;
  TabOnly: 3;
};
const FocusGroupModes: FocusGroupModes = {
  Off: 0,
  NoTab: 1,
  TabExit: 2,
  TabOnly: 3
};
type FocusGroupMode = FocusGroupModes[keyof FocusGroupModes];

interface FocusableElementVisibilities {
  Invisible: 0;
  PartiallyVisible: 1;
  Visible: 2;
}
const FocusableElementVisibilities: FocusableElementVisibilities = {
  Invisible: 0,
  PartiallyVisible: 1,
  Visible: 2
}
type FocusableElementVisibility = FocusableElementVisibilities[keyof FocusableElementVisibilities];

interface FocusRestoreRoles {
  Source: 0;
  Target: 1;
}
const FocusRestoreRoles: FocusRestoreRoles = {
  Source: 0,
  Target: 1
};
type FocusRestoreRole = FocusRestoreRoles[keyof FocusRestoreRoles];

interface ArrowNavigationGroupParameters {
  mode: ArrowNavigationGroupMode;
  firstFocusedMinimumVisibility: FocusableElementVisibility;
  rememberLastFocused: boolean;
  navigationWithTab: boolean;
  circular: boolean;
}

interface FocusableElementParameters {
  default: boolean;
  ignoreAriaDisabled: boolean;
  excludeFromArrowKeyNavigation: boolean;
  ignoredKeydownEvents?: {
    tab: boolean;
    escape: boolean;
    enter: boolean;
    arrowUp: boolean;
    arrowDown: boolean;
    arrowLeft: boolean;
    arrowRight: boolean;
    pageUp: boolean;
    pageDown: boolean;
    home: boolean;
    end: boolean;
  }
}

interface FocusableGroupParameters {
  mode?: FocusGroupMode;
}

interface FocusRestoreParameters {
  role: FocusRestoreRole;
}

interface ModalParameters {
  groupName: string;
  alwaysAccessible: boolean;
  trapFocus: boolean;
}

interface FocusTrackingParameters {
  focusVisible: boolean;
  focusWithin: boolean;
}

interface FocusManagementParameters {
  arrowNavigationGroup?: ArrowNavigationGroupParameters;
  focusableElement?: FocusableElementParameters;
  focusableGroup?: FocusableGroupParameters;
  focusRestore?: FocusRestoreParameters;
  modal?: ModalParameters;
  tracking?: FocusTrackingParameters;
  update: boolean;
}

interface FocusVisibleState {
  current?: HTMLElement;
}

interface ElementFocusVisibleTrackingState {
  dispose: () => void;
}
interface ElementFocusWithinTrackingState {
  dispose: () => void;
}

type HTMLElementCallback = (e: HTMLElement) => void;

type FocusTrackingHTMLElement = HTMLElement & {
  __fuiFocusVisibleTracking?: ElementFocusVisibleTrackingState,
  __fuiFocusWithinTracking?: ElementFocusWithinTrackingState,
  __fuiOnRemovedCallbacks?: HTMLElementCallback[],
  __fuiOnRemoved?: HTMLElementCallback
}

const DEFAULT_MODAL_FOCUS_RESTORE_PARAMETERS: FocusManagementParameters = {
  focusRestore: {
    role: FocusRestoreRoles.Source
  },
  update: true
};

const TABSTER_TABBABILITY_MAP = {
  [FocusGroupModes.Off]: undefined,
  [FocusGroupModes.NoTab]: TabsterTypes.GroupperTabbabilities.LimitedTrapFocus,
  [FocusGroupModes.TabExit]: TabsterTypes.GroupperTabbabilities.Limited,
  [FocusGroupModes.TabOnly]: TabsterTypes.GroupperTabbabilities.Unlimited
};

class TabsterFocusManager {
  private _tabsterCore: TabsterTypes.Tabster | undefined;

  public setParameters(element: HTMLElement, parameters: FocusManagementParameters): void {
    this.ensureTabsterInitialized();

    const tabsterProps = this.getTabsterProps(parameters);

    this.adjustElementAttributes(element, parameters);
    this.setFocusTracking(element, parameters);
    setTabsterAttribute(element, tabsterProps, parameters?.update);
  }

  public findFocusable(container: HTMLElement, acceptCondition?: (el: HTMLElement) => boolean): HTMLElement[] {
    this.ensureTabsterInitialized();

    return this._tabsterCore?.focusable.findAll({
      container,
      acceptCondition
    }) || [];
  }

  private ensureTabsterInitialized() {
    if (!!this._tabsterCore) {
      return;
    }

    this._tabsterCore = createTabster(window, { autoRoot: {}, controlTab: false });

    if (!this._tabsterCore) {
      return;
    }

    getGroupper(this._tabsterCore);
    getModalizer(this._tabsterCore);
    getMover(this._tabsterCore);
    getRestorer(this._tabsterCore);
  }

  private getTabsterProps(parameters: FocusManagementParameters): TabsterTypes.TabsterAttributeProps {
    if (!parameters) {
      return {};
    }

    const tabsterProps = {};

    this.setFocusableProps(parameters, tabsterProps);
    this.setGroupperProps(parameters, tabsterProps);
    this.setModalizerProps(parameters, tabsterProps);
    this.setMoverProps(parameters, tabsterProps);
    this.setRestorerProps(parameters, tabsterProps);

    return tabsterProps;
  }

  private setFocusableProps(parameters: FocusManagementParameters, tabsterProps: TabsterTypes.TabsterAttributeProps) : void {
    const params = parameters.focusableElement;
    if (!params) {
      return;
    }

    const props : TabsterTypes.FocusableProps = {
      isDefault: params.default,
      ignoreAriaDisabled: params.ignoreAriaDisabled,
      excludeFromMover: params.excludeFromArrowKeyNavigation
    };

    if (params.ignoredKeydownEvents) {
      const ignoredEvents = params.ignoredKeydownEvents;

      props.ignoreKeydown = {
        Tab: ignoredEvents.tab,
        Escape: ignoredEvents.escape,
        Enter: ignoredEvents.enter,
        ArrowUp: ignoredEvents.arrowUp,
        ArrowDown: ignoredEvents.arrowDown,
        ArrowLeft: ignoredEvents.arrowLeft,
        ArrowRight: ignoredEvents.arrowRight,
        PageUp: ignoredEvents.pageUp,
        PageDown: ignoredEvents.pageDown,
        Home: ignoredEvents.home,
        End: ignoredEvents.end
      };
    }
    tabsterProps.focusable = props;
  }

  private setGroupperProps(parameters: FocusManagementParameters, tabsterProps: TabsterTypes.TabsterAttributeProps) : void {
    const params = parameters.focusableGroup;
    if (!params) {
      return;
    }

    const tabbability = TABSTER_TABBABILITY_MAP[params.mode || FocusGroupModes.Off];
    tabsterProps.groupper = {
      tabbability: tabbability
    };
  }

  private setModalizerProps(parameters: FocusManagementParameters, tabsterProps: TabsterTypes.TabsterAttributeProps): void {
    const params = parameters.modal;
    if (!params) {
      return;
    }

    tabsterProps.modalizer = {
      id: params.groupName,
      isAlwaysAccessible: params.alwaysAccessible,
      isOthersAccessible: !params.trapFocus,
      isTrapped: params.trapFocus
    }

    this.setRestorerProps(DEFAULT_MODAL_FOCUS_RESTORE_PARAMETERS, tabsterProps);
  }

  private setMoverProps(parameters: FocusManagementParameters, tabsterProps: TabsterTypes.TabsterAttributeProps): void {
    const params = parameters.arrowNavigationGroup;
    if (!params) {
      return;
    }

    tabsterProps.mover = {
      direction: params.mode,
      visibilityAware: params.firstFocusedMinimumVisibility,
      memorizeCurrent: params.rememberLastFocused,
      tabbable: params.navigationWithTab,
      cyclic: params.circular
    };
  }

  private setRestorerProps(parameters: FocusManagementParameters, tabsterProps: TabsterTypes.TabsterAttributeProps): void {
    const params = parameters.focusRestore;
    if (!params) {
      return;
    }

    tabsterProps.restorer = {
      type: params.role
    };
  }

  private adjustElementAttributes(element: HTMLElement, parameters: FocusManagementParameters): void {
    if ((parameters?.focusableGroup?.mode || FocusGroupModes.Off) !== FocusGroupModes.Off &&
      element.getAttribute('tabindex') === null) {
      element.setAttribute('tabindex', '0');
    }
  }

  private setFocusTracking(element: HTMLElement, parameters: FocusManagementParameters): void {
    const focusVisible = parameters?.tracking?.focusVisible || false;
    const focusWithin = parameters?.tracking?.focusWithin || false;

    if (focusVisible) {
      this.startFocusVisibleTracking(element);
    } else {
      this.stopFocusVisibleTracking(element);
    }

    if (focusWithin) {
      this.startFocusWithinTracking(element);
    } else {
      this.stopFocusWithinTracking(element);
    }
  }

  private startFocusVisibleTracking(element: FocusTrackingHTMLElement) {
    if (!this._tabsterCore ||
      !!element['__fuiFocusVisibleTracking'] ||
      this.getIsInFocusVisibleTrackingScope(element)) {
      return;
    }

    const focusVisibleState: FocusVisibleState = {
      current: undefined
    };

    const keyboardNavigationChangedCallback = (isNavigatingWithKeyboard: boolean) => {
      if (!isNavigatingWithKeyboard && !!focusVisibleState.current) {
        this.removeFocusVisibleAttribute(focusVisibleState.current);
        focusVisibleState.current = undefined;
      }
    };

    this._tabsterCore.keyboardNavigation.subscribe(keyboardNavigationChangedCallback);

    const focusInListener = (e: KeyborgFocusInEvent) => {
      if (focusVisibleState.current) {
        this.removeFocusVisibleAttribute(focusVisibleState.current);
        focusVisibleState.current = undefined;
      }

      if (this._tabsterCore?.keyboardNavigation?.isNavigatingWithKeyboard() &&
        isHTMLElement(e.target) &&
        e.target) {
        focusVisibleState.current = e.target;
        TabsterFocusManager.setFocusVisibleAttribute(focusVisibleState.current);
      }
    };

    const focusOutListener = (e: FocusEvent) => {
      if (!e.relatedTarget || (isHTMLElement(e.relatedTarget) && !element.contains(e.relatedTarget))) {
        this.removeFocusVisibleAttribute(element);
      }
    };

    element.addEventListener(KEYBORG_FOCUSIN, focusInListener as (e: Event) => void);
    element.addEventListener('focusout', focusOutListener);

    const tabsterCore = this._tabsterCore;

    const dispose = () => {
      element.removeEventListener(KEYBORG_FOCUSIN, focusInListener as (e: Event) => void);
      element.removeEventListener('focusout', focusOutListener);
      tabsterCore.keyboardNavigation.unsubscribe(keyboardNavigationChangedCallback);
    };

    const focusVisibleTracking = {
      dispose
    };
    element['__fuiFocusVisibleTracking'] = focusVisibleTracking;
    this.addOnRemovedCallback(element, focusVisibleTracking.dispose);
  }

  private stopFocusVisibleTracking(element: FocusTrackingHTMLElement) : void {
    if (!this._tabsterCore || !element['__fuiFocusVisibleTracking']) {
      return;
    }

    const dispose = element['__fuiFocusVisibleTracking'].dispose;
    this.removeOnRemovedCallback(element, dispose);
    dispose();
    delete element['__fuiFocusVisibleTracking'];
  }

  private startFocusWithinTracking(element: FocusTrackingHTMLElement) : void {
    if (!this._tabsterCore || !!element['__fuiFocusWithinTracking']) {
      return;
    }

    const keyboardNavigationChangedCallback = (isNavigatingWithKeyboard: boolean) => {
      if (!isNavigatingWithKeyboard) {
        TabsterFocusManager.removeFocusWithinAttribute(element);
      }
    }
    const focusInListener = (e: KeyborgFocusInEvent) => {
      if (this._tabsterCore?.keyboardNavigation?.isNavigatingWithKeyboard() &&
        isHTMLElement(e.target)) {
        TabsterFocusManager.setFocusWithinAttribute(element);
      }
    }
    const focusOutListener = (e: FocusEvent) => {
      if (!e.relatedTarget || (isHTMLElement(e.relatedTarget) && !element.contains(e.relatedTarget))) {
        TabsterFocusManager.removeFocusWithinAttribute(element);
      }
    }

    this._tabsterCore.keyboardNavigation.subscribe(keyboardNavigationChangedCallback);
    element.addEventListener(KEYBORG_FOCUSIN, focusInListener as (e: Event) => void);
    element.addEventListener('focusout', focusOutListener);

    const tabster = this._tabsterCore;
    const dispose = () => {
      tabster.keyboardNavigation.unsubscribe(keyboardNavigationChangedCallback);
      element.removeEventListener(KEYBORG_FOCUSIN, focusInListener as (e: Event) => void);
      element.removeEventListener('focusout', focusOutListener);
    };
    const focusWithinTrackingState : ElementFocusWithinTrackingState = {
      dispose
    };
    element['__fuiFocusWithinTracking'] = focusWithinTrackingState;
    this.addOnRemovedCallback(element, focusWithinTrackingState.dispose);
  }

  private stopFocusWithinTracking(element: FocusTrackingHTMLElement) : void {
    if (!this._tabsterCore || !element['__fuiFocusWithinTracking']) {
      return;
    }

    const dispose = element['__fuiFocusWithinTracking'].dispose;
    this.removeOnRemovedCallback(element, dispose);
    dispose();

    delete element['__fuiFocusWithinTracking'];
  }

  private getIsInFocusVisibleTrackingScope(element: HTMLElement | null | undefined) : boolean {
    if (!element) {
      return false;
    }

    if (!!(element as FocusTrackingHTMLElement)['__fuiFocusVisibleTracking']) {
      return true;
    }

    return this.getIsInFocusVisibleTrackingScope(element?.parentElement);
  }

  private static setFocusVisibleAttribute(element: HTMLElement) : void {
    element.setAttribute(FOCUS_VISIBLE_ATTR_NAME, '');
  }

  private removeFocusVisibleAttribute(element: HTMLElement) : void {
    element.removeAttribute(FOCUS_VISIBLE_ATTR_NAME);
  }

  private static setFocusWithinAttribute(element: HTMLElement): void {
    element.setAttribute(FOCUS_WITHIN_ATTR_NAME, '');
  }

  private static removeFocusWithinAttribute(element: HTMLElement): void {
    element.removeAttribute(FOCUS_WITHIN_ATTR_NAME);
  }

  private addOnRemovedCallback(element: FocusTrackingHTMLElement, handler: HTMLElementCallback) : void {
    let onRemovedCallbacks = element['__fuiOnRemovedCallbacks'];

    if (!onRemovedCallbacks) {
      onRemovedCallbacks = [];
      element['__fuiOnRemovedCallbacks'] = onRemovedCallbacks;

      const onRemoved = (el: FocusTrackingHTMLElement) => {
        const callbacks = element['__fuiOnRemovedCallbacks'];
        if (callbacks?.length) {
          for (let i = 0; i < callbacks.length; i++) {
            try {
              callbacks[i](el);
            } catch { }
          }
        }
      }

      element['__fuiOnRemoved'] = onRemoved;
      elementRemovalObserver.subscribe(element, onRemoved);
    }

    onRemovedCallbacks.push(handler);
  }

  private removeOnRemovedCallback(element: FocusTrackingHTMLElement, handler: HTMLElementCallback) : void {
    const onRemovedCallbacks = element['__fuiOnRemovedCallbacks'];

    if (!onRemovedCallbacks) {
      return;
    }

    const handlerIndex = onRemovedCallbacks.indexOf(handler);
    if (handlerIndex === -1) {
      return;
    }

    onRemovedCallbacks.splice(handlerIndex, 1);

    if (!onRemovedCallbacks.length) {
      const onRemoved = element['__fuiOnRemoved'];
      if (onRemoved) {
        elementRemovalObserver.unsubscribe(element, onRemoved);
        delete element['__fuiOnRemoved'];
      }

      delete element['__fuiOnRemovedCallbacks']
    }
  }
}

function isHTMLElement(target: EventTarget | null): target is HTMLElement {
  if (!target) {
    return false;
  }

  return Boolean(target && typeof target === 'object' && 'classList' in target && 'contains' in target);
}

class ElementRemovalObserver {
  private _observed: Map<HTMLElement, HTMLElementCallback[]>;
  private _observer?: MutationObserver;

  constructor() {
    this._observed = new Map<HTMLElement, HTMLElementCallback[]>();
    this._observer = undefined;
  }

  subscribe(element: HTMLElement, callback: HTMLElementCallback): void {
    if (this._observed.has(element)) {
      const callbacks = this._observed.get(element);
      if (callbacks!.indexOf(callback) === -1) {
        callbacks?.push(callback);
      }
      return;
    }

    const callbacks: HTMLElementCallback[] = [];
    callbacks.push(callback);
    this._observed.set(element, callbacks);

    if (this._observer === undefined) {
      const removalObserver = this;
      this._observer =
        new MutationObserver(
          mutations => removalObserver.mutationCallback(mutations));
      this._observer.observe(
        document.body,
        {
          subtree: true,
          childList: true
        });
    }
  }

  unsubscribe(element: HTMLElement, callback: HTMLElementCallback): void {
    if (!this._observed.has(element)) {
      return;
    }

    const callbacks = this._observed.get(element);
    const callbackIndex = callbacks!.indexOf(callback);
    if (callbackIndex === -1) {
      return;
    }

    callbacks!.splice(callbackIndex, 1);
    if (!callbacks!.length) {
      this._observed.delete(element);
    }

    if (!this._observed.size && !!this._observer) {
      this._observer.disconnect();
      this._observer = undefined;
    }
  }

  mutationCallback(mutations: MutationRecord[]) {
    const removedNodes : Node[] = [];
    mutations
      .filter(mutation => !!mutation.removedNodes.length)
      .map(mutation => mutation.removedNodes)
      .reduce(
        (allRemoved, removedInMutation) => {
          allRemoved.push(...removedInMutation);
          return allRemoved;
        },
        removedNodes);

    const removedObserved: HTMLElement[] = [];

    for (const element of this._observed.keys()) {
      if (removedNodes.indexOf(element) > -1 ||
        removedNodes.some(parentElement => parentElement.contains(element))) {
        removedObserved.push(element);
        }
    }

    if (removedObserved.length) {
      for (let i = 0; i < removedObserved.length; i++) {
        const removedElement = removedObserved[i];
        var callbacks = this._observed.get(removedElement)

        for (let j = 0; j < callbacks!.length; j++) {
          try {
            callbacks![j](removedElement);
          } catch { }
        }

        this._observed.delete(removedElement);
      }

      if (!this._observed.size && !!this._observer) {
        this._observer.disconnect();
        this._observer = undefined;
      }
    }
  }
}

const elementRemovalObserver = new ElementRemovalObserver();
const focusManager = new TabsterFocusManager();

/**
 * Set focus management parameters.
 * @param element
 * @param parameters
 */
export function setFocusManagementParameters(element: HTMLElement, parameters: FocusManagementParameters) {
  focusManager.setParameters(element, parameters);
}

export function findFocusableElements(container: HTMLElement, acceptCondition?: (el: HTMLElement) => boolean) : HTMLElement[] {
  return focusManager.findFocusable(container, acceptCondition);
}
