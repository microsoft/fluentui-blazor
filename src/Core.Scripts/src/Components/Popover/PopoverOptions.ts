export interface PopoverOptions {

  /**
   * ID of the popover
   */
  id: string;

  /**
   * ID of the element that attaches the popover
   */
  anchorId: string,

  /**
   * ID of the button that opens the popover.
   * If null, the anchorId will be used as the trigger.
   */
  triggerId: string | null,

  /**
   * ID of the dialog element
   */
  dialogId: string,

  /**
   * Offset from the target element
   */
  offsetVertical: number,

  //placement: 'top', // 'top', 'bottom', 'left', 'right'
  //closeOnClickOutside: true, // Close on click outside
  //closeOnEsc: true // Close on Escape key
}
