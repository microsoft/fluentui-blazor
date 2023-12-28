import { CSSDesignToken } from '@microsoft/fast-foundation';
import { Swatch } from "@fluentui/web-components";
import { ColorsUtils } from './ColorsUtils';

class Converters {
  /**
   * Apply the string to the item (if not null).
   * @param item
   * @param value
   * @returns
   */
  public static applyDefaultString(item: CSSDesignToken<string>, value: string | null): void {
    value && item.withDefault(value);
  }

  /**
   * Apply the number to the item (if not null).
   * @param item
   * @param value
   * @returns
   */
  public static applyDefaultNumber(item: CSSDesignToken<number>, value: number | null): void {
    value && item.withDefault(value);
  }

  /**
   * Apply the color to the item (if not null).
   * @param item
   * @param color
   * @returns
   */
  public static applyDefaultColors(item: CSSDesignToken<Swatch>, color: string | null): void {

    if (color == null) {
      return;
    }

    const swatchRGB = ColorsUtils.swatchHexColor(color);
    if (swatchRGB == null) {
      return
    }

    item.withDefault(swatchRGB);
  }
}

export { Converters };
