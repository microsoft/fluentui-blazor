import {
    Swatch,
  SwatchRGB,
  bodyFont,
  controlCornerRadius,
  fillColor,
  layerCornerRadius,
  neutralBaseColor
} from "@fluentui/web-components/dist/web-components";
import { CSSDesignToken } from '@microsoft/fast-foundation';
import { ColorsUtils } from "./ColorsUtils";

class Tokens {
  public apply(token: ThemeToken) {
    console.log(token);

    token.common.bodyFont && bodyFont.withDefault(token.common.bodyFont);

    this.applyColor(fillColor, token.common.fillColor);
    this.applyColor(neutralBaseColor, token.common.neutralBaseColor);

    token.common.controlCornerRadius && controlCornerRadius.withDefault(token.common.controlCornerRadius);
    token.common.layerCornerRadius && layerCornerRadius.withDefault(token.common.layerCornerRadius);
  }

  applyColor(item: CSSDesignToken<Swatch>, color: string | null) {

    if (color == null) {
      return;
    }

    const swatchRGB: SwatchRGB = this.swatchColor(color);
    if (swatchRGB == null) {
        return
    }

    item.withDefault(swatchRGB);
  }

  /**
   * Convert the RGB string to a SwatchRGB
   * @param color
   * @returns
   */
  swatchColor(color: string): SwatchRGB {
    const rgb = ColorsUtils.parseColorHexRGB(color);
    if (rgb != null) {
      return SwatchRGB.from(rgb);
    }
    else {
      throw new Error(`This is not a valid HEX Color: "${color}".`);
    }
  }
}

/* https://csharptotypescript.azurewebsites.net/ */

interface ThemeToken {
  common: CommonProps;
  colors: ColorsProps;
  others: OthersProps;
}

interface CommonProps {
  fillColor: string | null;
  neutralBaseColor: string | null;
  bodyFont: string | null;
  controlCornerRadius: number | null;
  layerCornerRadius: number | null;
  density: string | null;
}

interface ColorsProps {
  accentFill: RecipeStateful;
  accentForeground: RecipeStateful;
  foregroundOnAccent: RecipeStateful;
}

interface OthersProps {
  typeRamps: TypeRamps;
  baseHeightMultiplier: string | null;
  baseHorizontalSpacingMultiplier: string | null;
  designUnit: string | null;
  strokeWidth: string | null;
  focusStrokeWidth: string | null;
  disabledOpacity: string | null;
}

interface TypeRamps {
  minus1: TypeRamp;
  minus2: TypeRamp;
  base: TypeRamp;
  plus1: TypeRamp;
  plus2: TypeRamp;
  plus3: TypeRamp;
  plus4: TypeRamp;
  plus5: TypeRamp;
  plus6: TypeRamp;
}

interface TypeRamp {
  fontSize: string | null;
  lineHeight: string | null;
}

interface RecipeStateful {
  active: string | null;
  focus: string | null;
  hover: string | null;
  recipe: string | null;
  rest: string | null;
  activeDelta: string | null;
  focusDelta: string | null;
  hoverDelta: string | null;
  restDelta: string | null;
}

export { Tokens };
