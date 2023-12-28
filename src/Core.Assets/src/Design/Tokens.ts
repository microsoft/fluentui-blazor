import {
  accentBaseColor,
  accentFillRecipe,
  baseLayerLuminance,
  bodyFont,
  controlCornerRadius,
  density,
  disabledOpacity,
  fillColor,
  layerCornerRadius,
  neutralBaseColor
} from "@fluentui/web-components/dist/web-components";
import { Converters } from "./Converters";

class Tokens {
  public apply(token: ThemeToken) {

    // Duplicate with DesignTheme attributes
    Converters.applyDefaultNumber(baseLayerLuminance, token.common.baseLayerLuminance)
    Converters.applyDefaultColors(accentBaseColor, token.common.accentBaseColor);

    // Common
    Converters.applyDefaultColors(fillColor, token.common.fillColor);
    Converters.applyDefaultColors(neutralBaseColor, token.common.neutralBaseColor);
    Converters.applyDefaultString(bodyFont, token.common.bodyFont);
    Converters.applyDefaultNumber(controlCornerRadius, token.common.controlCornerRadius);
    Converters.applyDefaultNumber(layerCornerRadius, token.common.layerCornerRadius);
    Converters.applyDefaultNumber(density, token.common.density);
  }
}



/* https://csharptotypescript.azurewebsites.net/ */

interface ThemeToken {
  common: CommonProps;
  colors: ColorsProps;
  others: OthersProps;
}

interface CommonProps {
  baseLayerLuminance: number | null;
  accentBaseColor: string | null;
  bodyFont: string | null;
  fillColor: string | null;
  neutralBaseColor: string | null;
  controlCornerRadius: number | null;
  layerCornerRadius: number | null;
  density: number | null;
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
