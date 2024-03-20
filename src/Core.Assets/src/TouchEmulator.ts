/**
 * create an touch point
 * @constructor
 * @param target
 * @param identifier
 * @param pos
 * @param deltaX
 * @param deltaY
 * @returns {Object} touchPoint
 */
class Touch {
  public clientX: number;
  public clientY: number;
  public pageX: number;
  public pageY: number;
  public screenX: number;
  public screenY: number;

  constructor(
    public target: EventTarget | null,
    public identifier: number,
    pos: { pageX: any; pageY: any; screenX: any; screenY: any; clientX: any; clientY: any; } | null | undefined,
    deltaX: number = 0,
    deltaY: number = 0
  ) {
    deltaX = deltaX || 0;
    deltaY = deltaY || 0;

    this.identifier = identifier;
    this.target = target;
    this.clientX = pos?.clientX + deltaX;
    this.clientY = pos?.clientY + deltaY;
    this.screenX = pos?.screenX + deltaX;
    this.screenY = pos?.screenY + deltaY;
    this.pageX = pos?.pageX + deltaX;
    this.pageY = pos?.pageY + deltaY;
  }
}

/*
class TouchList extends Array<any> {
  /**
   * create empty touchlist with the methods
   * @constructor
   * @returns touchList
   */
/*
  constructor() {
    super([]);
  }

  public item(index) {
    return this[index] || null;
  }

  // specified by Mozilla
  public identifiedTouch(id) {
    return this[id + 1] || null;
  }

}
*/

/**
 * create empty touchlist with the methods
 * @constructor
 * @returns touchList
 */

function createEmptyTouchList(): TouchList | Array<Touch> {
  var touchList: Array<Touch> = [];

  (touchList as any).item = function (index: string | number) {
    return this[index] || null;
  };

  // specified by Mozilla
  (touchList as any).identifiedTouch = function (id: number) {
    return this[id + 1] || null;
  };
  return touchList;

}

export class TouchEmulator {
  private isMultiTouch = false;
  private multiTouchStartPos: { pageX: any; pageY: any; clientX: any; clientY: any; screenX: any; screenY: any; } | null | undefined;
  private touchElements: any = {};
  private lastAlt: number | undefined;
  public static active = false;
  private eventTarget: EventTarget | null = null;

  // start distance when entering the multitouch mode
  private multiTouchOffset: number = 75;

  constructor() {
    // polyfills
    if (!(document as any).createTouch) {
      (document as any).createTouch = function (
        view: any,
        target: EventTarget | null,
        identifier: number,
        pageX: number,
        pageY: number,
        screenX: any,
        screenY: any,
        clientX: number | undefined,
        clientY: number | undefined
      ) {
        // auto set
        if (clientX == undefined || clientY == undefined) {
          clientX = pageX - globalThis.pageXOffset;
          clientY = pageY - globalThis.pageYOffset;
        }

        return new Touch(target, identifier, {
          pageX: pageX,
          pageY: pageY,
          screenX: screenX,
          screenY: screenY,
          clientX: clientX,
          clientY: clientY,
        });
      };
    }

    if (!(document as any).createTouchList) {
      (document as any).createTouchList = function () {
        var touchList = createEmptyTouchList();
        for (var i = 0; i < arguments.length; i++) {
          (touchList as Array<Touch>).push(arguments[i]);
        }
        return touchList;
      };
    }
    if (this.hasTouchSupport()) {
      return;
    }

    this.fakeTouchSupport();

    globalThis.addEventListener('keydown', this.toggleTouch, true);

    globalThis.addEventListener('mousedown', (event) => this.onMouse('touchstart')(event), true);
    globalThis.addEventListener('mousemove', (event) => this.onMouse('touchmove')(event), true);
    globalThis.addEventListener('mouseup', (event) => this.onMouse('touchend')(event), true);

    globalThis.addEventListener('mouseenter', (event) => this.preventMouseEvents(event), true);
    globalThis.addEventListener('mouseleave', (event) => this.preventMouseEvents(event), true);
    globalThis.addEventListener('mouseout', (event) => this.preventMouseEvents(event), true);
    globalThis.addEventListener('mouseover', (event) => this.preventMouseEvents(event), true);

    // it uses itself!
    globalThis.addEventListener('touchstart', (event) => this.showTouches(event), true);
    globalThis.addEventListener('touchmove', (event) => this.showTouches(event), true);
    globalThis.addEventListener('touchend', (event) => this.showTouches(event), true);
    globalThis.addEventListener('touchcancel', (event) => this.showTouches(event), true);
    console.log('Touch gesture emulation has been enabled.');
    console.log(
      'Hit the ALT or OPTION key twice in a second to active touch gestures. Caveat: this disables your mouse.'
    );
    console.log(
      'Hitting the ALT or OPTION key twice again deactivates touch gestures again and allow you to use your mouse again.'
    );
  }

  /**
   * Simple trick to fake touch event support
   * this is enough for most libraries like Modernizr and Hammer
   */
  private fakeTouchSupport() {
    var objs: { [index: string]: any } = [window, document.documentElement];
    var props = ['ontouchstart', 'ontouchmove', 'ontouchcancel', 'ontouchend'];

    for (var o = 0; o < objs.length; o++) {
      for (var p = 0; p < props.length; p++) {
        if (objs[o] && objs[o][props[p]] == undefined) {
          objs[o][props[p]] = null;
        }
      }
    }
  }

  /**
   * we don't have to emulate on a touch device
   * @returns {boolean}
   */
  private hasTouchSupport() {
    if (typeof window === 'undefined') {
      // server-side rendering
      return false;
    }
    return (
      'ontouchstart' in window || // touch events
      // (globalThis.Modernizr && globalThis.Modernizr.touch) || // modernizr
      navigator?.maxTouchPoints > 2
    ); // pointer events
  }

  /**
   * disable mouseevents on the page
   * @param ev
   */
  private preventMouseEvents(ev: MouseEvent): void {
    ev.preventDefault();
    ev.stopPropagation();
  }

  private toggleTouch(ev: { altKey: any; }) {
    if (ev.altKey) {
      const currentAlt = new Date().getTime();
      if (this.lastAlt) {
        if (currentAlt - this.lastAlt < 1000) {
          TouchEmulator.active = !TouchEmulator.active;
          if (TouchEmulator.active) {
            console.log('simulating touch events');
          } else {
            console.log('Back to mouse events');
          }
        }
      }
      this.lastAlt = currentAlt;
    }
  }

  /**
   * only trigger touches when the left mousebutton has been pressed
   * @param touchType
   * @returns {Function}
   */
  private onMouse(touchType: string) {
    return (ev: MouseEvent) => {
      if (!TouchEmulator.active) return;

      // prevent mouse events
      this.preventMouseEvents(ev);

      if (ev.which !== 1) {
        return;
      }

      // The EventTarget on which the touch point started when it was first placed on the surface,
      // even if the touch point has since moved outside the interactive area of that element.
      // also, when the target doesnt exist anymore, we update it
      if (
        ev.type == 'mousedown' ||
        !this.eventTarget ||
        (this.eventTarget && !this.eventTarget.dispatchEvent)
      ) {
        this.eventTarget = ev.target;
      }

      // shiftKey has been lost, so trigger a touchend
      if (this.isMultiTouch && !ev.shiftKey) {
        this.triggerTouch('touchend', ev);
        this.isMultiTouch = false;
      }

      this.triggerTouch(touchType, ev);

      // we're entering the multi-touch mode!
      if (!this.isMultiTouch && ev.shiftKey) {
        this.isMultiTouch = true;
        this.multiTouchStartPos = {
          pageX: ev.pageX,
          pageY: ev.pageY,
          clientX: ev.clientX,
          clientY: ev.clientY,
          screenX: ev.screenX,
          screenY: ev.screenY,
        };
        this.triggerTouch('touchstart', ev);
      }

      // reset
      if (ev.type == 'mouseup') {
        this.multiTouchStartPos = null;
        this.isMultiTouch = false;
        this.eventTarget = null;
      }
    };
  }

  /**
   * trigger a touch event
   * @param eventName
   * @param mouseEv
   */
  private triggerTouch(eventName: string, mouseEv: MouseEvent) {
    var touchEvent = document.createEvent('Event');
    touchEvent.initEvent(eventName, true, true);

    (touchEvent as any).altKey = mouseEv.altKey;
    (touchEvent as any).ctrlKey = mouseEv.ctrlKey;
    (touchEvent as any).metaKey = mouseEv.metaKey;
    (touchEvent as any).shiftKey = mouseEv.shiftKey;

    (touchEvent as any).touches = this.getActiveTouches(mouseEv, eventName);
    (touchEvent as any).targetTouches = this.getActiveTouches(
      mouseEv,
      eventName
    );
    (touchEvent as any).changedTouches = this.getChangedTouches(
      mouseEv,
      eventName
    );

    this.eventTarget?.dispatchEvent(touchEvent);
  }

  /**
   * create a touchList based on the mouse event
   * @param mouseEv
   * @returns {TouchList}
   */
  private createTouchList(mouseEv: { pageX: any; pageY: any; screenX: any; screenY: any; clientX: any; clientY: any; }) {
    var touchList = createEmptyTouchList();

    if (this.isMultiTouch) {
      var f = this.multiTouchOffset;
      var deltaX = this.multiTouchStartPos?.pageX - mouseEv.pageX;
      var deltaY = this.multiTouchStartPos?.pageY - mouseEv.pageY;

      (touchList as Array<Touch>).push(
        new Touch(
          this.eventTarget,
          1,
          this.multiTouchStartPos,
          deltaX * -1 - f,
          deltaY * -1 + f
        )
      );
      (touchList as Array<Touch>).push(
        new Touch(
          this.eventTarget,
          2,
          this.multiTouchStartPos,
          deltaX + f,
          deltaY - f
        )
      );
    } else {
      (touchList as Array<Touch>).push(new Touch(this.eventTarget, 1, mouseEv, 0, 0));
    }

    return touchList;
  }

  /**
   * receive all active touches
   * @param mouseEv
   * @returns {TouchList}
   */
  private getActiveTouches(mouseEv: MouseEvent, eventName: string) {
    // empty list
    if (mouseEv.type == 'mouseup') {
      return createEmptyTouchList();
    }

    var touchList = this.createTouchList(mouseEv);
    if (
      this.isMultiTouch &&
      mouseEv.type != 'mouseup' &&
      eventName == 'touchend'
    ) {
      (touchList as Array<Touch>).splice(1, 1);
    }
    return touchList;
  }

  /**
   * receive a filtered set of touches with only the changed pointers
   * @param mouseEv
   * @param eventName
   * @returns {TouchList}
   */
  private getChangedTouches(mouseEv: MouseEvent, eventName: string) {
    var touchList = this.createTouchList(mouseEv);

    // we only want to return the added/removed item on multitouch
    // which is the second pointer, so remove the first pointer from the touchList
    //
    // but when the mouseEv.type is mouseup, we want to send all touches because then
    // no new input will be possible
    if (
      this.isMultiTouch &&
      mouseEv.type != 'mouseup' &&
      (eventName == 'touchstart' || eventName == 'touchend')
    ) {
      (touchList as Array<Touch>).splice(0, 1);
    }

    return touchList;
  }

  /**
   * show the touchpoints on the screen
   */
  private showTouches(ev: TouchEvent) {
    var touch, i, el, styles: {[index:string]:any} = {};

    // first all visible touches
    for (i = 0; i < ev.touches.length; i++) {
      touch = ev.touches[i];
      el = this.touchElements[touch.identifier];
      if (!el) {
        el = this.touchElements[touch.identifier] =
          document.createElement('div');
        document.body.appendChild(el);
      }

      styles = this.template(touch);
      for (var prop in styles) {
        el.style[prop] = styles[prop];
      }
    }

    // remove all ended touches
    if (ev.type == 'touchend' || ev.type == 'touchcancel') {
      for (i = 0; i < ev.changedTouches.length; i++) {
        touch = ev.changedTouches[i];
        el = this.touchElements[touch.identifier];
        if (el) {
          el.parentNode.removeChild(el);
          delete this.touchElements[touch.identifier];
        }
      }
    }
  }

  /**
   * TouchEmulator initializer
   */

  /**
   * css template for the touch rendering
   * @param touch
   * @returns object
   */
  private template(touch: globalThis.Touch) {
    var size = 30;
    var transform =
      'translate(' +
      (touch.clientX - size / 2) +
      'px, ' +
      (touch.clientY - size / 2) +
      'px)';
    return {
      position: 'fixed',
      left: 0,
      top: 0,
      background: '#fff',
      border: 'solid 1px #999',
      opacity: 0.6,
      borderRadius: '100%',
      height: size + 'px',
      width: size + 'px',
      padding: 0,
      margin: 0,
      display: 'block',
      overflow: 'hidden',
      pointerEvents: 'none',
      webkitUserSelect: 'none',
      mozUserSelect: 'none',
      userSelect: 'none',
      webkitTransform: transform,
      mozTransform: transform,
      transform: transform,
      zIndex: 100,
    };
  }
}
