import type { BrandVariants } from '@fluentui/tokens';
import { Palette } from '../colors/types';
import { hexColorsFromPalette, hex_to_LCH } from '../colors/palettes';

type Options = {
  darkCp?: number;
  lightCp?: number;
  hueTorsion?: number;
};

/**
 * Port of:
 * https://github.com/microsoft/fluentui/blob/26229a953219cf4ec5b4c9f7aeb37070e2cf38db/packages/react-components/theme-designer/src/utils/getBrandTokensFromPalette.ts
 *
 * Only change: BrandVariants comes from @fluentui/tokens.
 */
export function getBrandTokensFromPalette(keyColor: string, options: Options = {}): BrandVariants {
  const { darkCp = 2 / 3, lightCp = 1 / 3, hueTorsion = 0 } = options;
  const brandPalette: Palette = {
    keyColor: hex_to_LCH(keyColor),
    darkCp,
    lightCp,
    hueTorsion,
  };
  const hexColors = hexColorsFromPalette(keyColor, brandPalette, 16, 1);
  return hexColors.reduce((acc: Record<string, string>, hexColor, h) => {
    acc[`${(h + 1) * 10}`] = hexColor;
    return acc;
  }, {}) as BrandVariants;
}
