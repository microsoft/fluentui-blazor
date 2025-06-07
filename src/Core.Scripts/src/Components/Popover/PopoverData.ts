import { PopoverOptions } from "./PopoverOptions";

export interface PopoverData {
  Data: Map<string, PopoverDataItem>;
}

export interface PopoverDataItem {
  options: PopoverOptions;
  resizeHandler: (e: UIEvent) => void;
  scrollHandler: (e: Event) => void;
  clickHandler: (e: MouseEvent | TouchEvent) => void;
  outsideHandler: (e: MouseEvent | TouchEvent) => void;
  keydownHandler: (e: KeyboardEvent) => void;
}
