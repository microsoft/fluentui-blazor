

import * as webcomponents from '@fluentui/web-components'

import { webLightTheme, webDarkTheme } from '@fluentui/tokens';
import { SplitPanels } from './SplitPanels'
import { FluentPageScript, onEnhancedLoad } from './FluentPageScript'
//import { DesignTheme } from './DesignTheme'



interface Blazor {
  registerCustomEventType: (
    name: string,
    options: CustomEventTypeOptions) => void;

  theme: {
    isSystemDark(): boolean,
    isDarkMode(): boolean
  }
  addEventListener: (name: string, callback: (event: any) => void) => void;
}

interface CustomEventTypeOptions {
  browserEventName: string;
  createEventArgs: (event: FluentUIEventType) => any;
}

interface FluentUIEventType {
  target: any;
  detail: any;
  _readOnly: any;
  type: string;
}


var styleSheet = new CSSStyleSheet();

const styles = `
body:has(.prevent-scroll) {
    overflow: hidden;
}
:root {
    --font-monospace: Consolas, "Courier New", "Liberation Mono", SFMono-Regular, Menlo, Monaco, monospace;
    --success: #0E700E;
    --warning: #E9835E;
    --error: #BC2F32;
    --info: #616161;
    --presence-available: #13a10e;
    --presence-away: #eaa300;
    --presence-busy: #d13438;
    --presence-dnd: #d13438;
    --presence-offline: #adadad;
    --presence-oof: #c239b3;
    --presence-unknown: #d13438;
    --highlight-bg: #fff3cd;

    --colorNeutralForeground1: #242424;
    --colorNeutralForeground1Hover: #242424;
    --colorNeutralForeground1Pressed: #242424;
    --colorNeutralForeground1Selected: #242424;
    --colorNeutralForeground2: #424242;
    --colorNeutralForeground2Hover: #242424;
    --colorNeutralForeground2Pressed: #242424;
    --colorNeutralForeground2Selected: #242424;
    --colorNeutralForeground2BrandHover: #0f6cbd;
    --colorNeutralForeground2BrandPressed: #115ea3;
    --colorNeutralForeground2BrandSelected: #0f6cbd;
    --colorNeutralForeground3: #616161;
    --colorNeutralForeground3Hover: #424242;
    --colorNeutralForeground3Pressed: #424242;
    --colorNeutralForeground3Selected: #424242;
    --colorNeutralForeground3BrandHover: #0f6cbd;
    --colorNeutralForeground3BrandPressed: #115ea3;
    --colorNeutralForeground3BrandSelected: #0f6cbd;
    --colorNeutralForeground4: #707070;
    --colorNeutralForegroundDisabled: #bdbdbd;
    --colorBrandForegroundLink: #115ea3;
    --colorBrandForegroundLinkHover: #0f548c;
    --colorBrandForegroundLinkPressed: #0c3b5e;
    --colorBrandForegroundLinkSelected: #115ea3;
    --colorNeutralForeground2Link: #424242;
    --colorNeutralForeground2LinkHover: #242424;
    --colorNeutralForeground2LinkPressed: #242424;
    --colorNeutralForeground2LinkSelected: #242424;
    --colorCompoundBrandForeground1: #0f6cbd;
    --colorCompoundBrandForeground1Hover: #115ea3;
    --colorCompoundBrandForeground1Pressed: #0f548c;
    --colorNeutralForegroundOnBrand: #ffffff;
    --colorNeutralForegroundInverted: #ffffff;
    --colorNeutralForegroundInvertedHover: #ffffff;
    --colorNeutralForegroundInvertedPressed: #ffffff;
    --colorNeutralForegroundInvertedSelected: #ffffff;
    --colorNeutralForegroundInverted2: #ffffff;
    --colorNeutralForegroundStaticInverted: #ffffff;
    --colorNeutralForegroundInvertedLink: #ffffff;
    --colorNeutralForegroundInvertedLinkHover: #ffffff;
    --colorNeutralForegroundInvertedLinkPressed: #ffffff;
    --colorNeutralForegroundInvertedLinkSelected: #ffffff;
    --colorNeutralForegroundInvertedDisabled: rgba(255, 255, 255, 0.4);
    --colorBrandForeground1: #0f6cbd;
    --colorBrandForeground2: #115ea3;
    --colorBrandForeground2Hover: #0f548c;
    --colorBrandForeground2Pressed: #0a2e4a;
    --colorNeutralForeground1Static: #242424;
    --colorBrandForegroundInverted: #479ef5;
    --colorBrandForegroundInvertedHover: #62abf5;
    --colorBrandForegroundInvertedPressed: #479ef5;
    --colorBrandForegroundOnLight: #0f6cbd;
    --colorBrandForegroundOnLightHover: #115ea3;
    --colorBrandForegroundOnLightPressed: #0e4775;
    --colorBrandForegroundOnLightSelected: #0f548c;
    --colorNeutralBackground1: #ffffff;
    --colorNeutralBackground1Hover: #f5f5f5;
    --colorNeutralBackground1Pressed: #e0e0e0;
    --colorNeutralBackground1Selected: #ebebeb;
    --colorNeutralBackground2: #fafafa;
    --colorNeutralBackground2Hover: #f0f0f0;
    --colorNeutralBackground2Pressed: #dbdbdb;
    --colorNeutralBackground2Selected: #e6e6e6;
    --colorNeutralBackground3: #f5f5f5;
    --colorNeutralBackground3Hover: #ebebeb;
    --colorNeutralBackground3Pressed: #d6d6d6;
    --colorNeutralBackground3Selected: #e0e0e0;
    --colorNeutralBackground4: #f0f0f0;
    --colorNeutralBackground4Hover: #fafafa;
    --colorNeutralBackground4Pressed: #f5f5f5;
    --colorNeutralBackground4Selected: #ffffff;
    --colorNeutralBackground5: #ebebeb;
    --colorNeutralBackground5Hover: #f5f5f5;
    --colorNeutralBackground5Pressed: #f0f0f0;
    --colorNeutralBackground5Selected: #fafafa;
    --colorNeutralBackground6: #e6e6e6;
    --colorNeutralBackgroundInverted: #292929;
    --colorNeutralBackgroundStatic: #333333;
    --colorNeutralBackgroundAlpha: rgba(255, 255, 255, 0.5);
    --colorNeutralBackgroundAlpha2: rgba(255, 255, 255, 0.8);
    --colorSubtleBackground: transparent;
    --colorSubtleBackgroundHover: #f5f5f5;
    --colorSubtleBackgroundPressed: #e0e0e0;
    --colorSubtleBackgroundSelected: #ebebeb;
    --colorSubtleBackgroundLightAlphaHover: rgba(255, 255, 255, 0.7);
    --colorSubtleBackgroundLightAlphaPressed: rgba(255, 255, 255, 0.5);
    --colorSubtleBackgroundLightAlphaSelected: transparent;
    --colorSubtleBackgroundInverted: transparent;
    --colorSubtleBackgroundInvertedHover: rgba(0, 0, 0, 0.1);
    --colorSubtleBackgroundInvertedPressed: rgba(0, 0, 0, 0.3);
    --colorSubtleBackgroundInvertedSelected: rgba(0, 0, 0, 0.2);
    --colorTransparentBackground: transparent;
    --colorTransparentBackgroundHover: transparent;
    --colorTransparentBackgroundPressed: transparent;
    --colorTransparentBackgroundSelected: transparent;
    --colorNeutralBackgroundDisabled: #f0f0f0;
    --colorNeutralBackgroundInvertedDisabled: rgba(255, 255, 255, 0.1);
    --colorNeutralStencil1: #e6e6e6;
    --colorNeutralStencil2: #fafafa;
    --colorNeutralStencil1Alpha: rgba(0, 0, 0, 0.1);
    --colorNeutralStencil2Alpha: rgba(0, 0, 0, 0.05);
    --colorBackgroundOverlay: rgba(0, 0, 0, 0.4);
    --colorScrollbarOverlay: rgba(0, 0, 0, 0.5);
    --colorBrandBackground: #0f6cbd;
    --colorBrandBackgroundHover: #115ea3;
    --colorBrandBackgroundPressed: #0c3b5e;
    --colorBrandBackgroundSelected: #0f548c;
    --colorCompoundBrandBackground: #0f6cbd;
    --colorCompoundBrandBackgroundHover: #115ea3;
    --colorCompoundBrandBackgroundPressed: #0f548c;
    --colorBrandBackgroundStatic: #0f6cbd;
    --colorBrandBackground2: #ebf3fc;
    --colorBrandBackground2Hover: #cfe4fa;
    --colorBrandBackground2Pressed: #96c6fa;
    --colorBrandBackground3Static: #0f548c;
    --colorBrandBackground4Static: #0c3b5e;
    --colorBrandBackgroundInverted: #ffffff;
    --colorBrandBackgroundInvertedHover: #ebf3fc;
    --colorBrandBackgroundInvertedPressed: #b4d6fa;
    --colorBrandBackgroundInvertedSelected: #cfe4fa;
    --colorNeutralCardBackground: #fafafa;
    --colorNeutralCardBackgroundHover: #ffffff;
    --colorNeutralCardBackgroundPressed: #f5f5f5;
    --colorNeutralCardBackgroundSelected: #ebebeb;
    --colorNeutralCardBackgroundDisabled: #f0f0f0;
    --colorNeutralStrokeAccessible: #616161;
    --colorNeutralStrokeAccessibleHover: #575757;
    --colorNeutralStrokeAccessiblePressed: #4d4d4d;
    --colorNeutralStrokeAccessibleSelected: #0f6cbd;
    --colorNeutralStroke1: #d1d1d1;
    --colorNeutralStroke1Hover: #c7c7c7;
    --colorNeutralStroke1Pressed: #b3b3b3;
    --colorNeutralStroke1Selected: #bdbdbd;
    --colorNeutralStroke2: #e0e0e0;
    --colorNeutralStroke3: #f0f0f0;
    --colorNeutralStrokeSubtle: #e0e0e0;
    --colorNeutralStrokeOnBrand: #ffffff;
    --colorNeutralStrokeOnBrand2: #ffffff;
    --colorNeutralStrokeOnBrand2Hover: #ffffff;
    --colorNeutralStrokeOnBrand2Pressed: #ffffff;
    --colorNeutralStrokeOnBrand2Selected: #ffffff;
    --colorBrandStroke1: #0f6cbd;
    --colorBrandStroke2: #b4d6fa;
    --colorBrandStroke2Hover: #77b7f7;
    --colorBrandStroke2Pressed: #0f6cbd;
    --colorBrandStroke2Contrast: #b4d6fa;
    --colorCompoundBrandStroke: #0f6cbd;
    --colorCompoundBrandStrokeHover: #115ea3;
    --colorCompoundBrandStrokePressed: #0f548c;
    --colorNeutralStrokeDisabled: #e0e0e0;
    --colorNeutralStrokeInvertedDisabled: rgba(255, 255, 255, 0.4);
    --colorTransparentStroke: transparent;
    --colorTransparentStrokeInteractive: transparent;
    --colorTransparentStrokeDisabled: transparent;
    --colorNeutralStrokeAlpha: rgba(0, 0, 0, 0.05);
    --colorNeutralStrokeAlpha2: rgba(255, 255, 255, 0.2);
    --colorStrokeFocus1: #ffffff;
    --colorStrokeFocus2: #000000;
    --colorNeutralShadowAmbient: rgba(0, 0, 0, 0.12);
    --colorNeutralShadowKey: rgba(0, 0, 0, 0.14);
    --colorNeutralShadowAmbientLighter: rgba(0, 0, 0, 0.06);
    --colorNeutralShadowKeyLighter: rgba(0, 0, 0, 0.07);
    --colorNeutralShadowAmbientDarker: rgba(0, 0, 0, 0.2);
    --colorNeutralShadowKeyDarker: rgba(0, 0, 0, 0.24);
    --colorBrandShadowAmbient: rgba(0, 0, 0, 0.3);
    --colorBrandShadowKey: rgba(0, 0, 0, 0.25);
    --colorPaletteRedBackground1: #fdf6f6;
    --colorPaletteRedBackground2: #f1bbbc;
    --colorPaletteRedBackground3: #d13438;
    --colorPaletteRedBorderActive: #d13438;
    --colorPaletteRedBorder1: #f1bbbc;
    --colorPaletteRedBorder2: #d13438;
    --colorPaletteRedForeground1: #bc2f32;
    --colorPaletteRedForeground2: #751d1f;
    --colorPaletteRedForeground3: #d13438;
    --colorPaletteRedForegroundInverted: #dc5e62;
    --colorPaletteGreenBackground1: #f1faf1;
    --colorPaletteGreenBackground2: #9fd89f;
    --colorPaletteGreenBackground3: #107c10;
    --colorPaletteGreenBorderActive: #107c10;
    --colorPaletteGreenBorder1: #9fd89f;
    --colorPaletteGreenBorder2: #107c10;
    --colorPaletteGreenForeground1: #0e700e;
    --colorPaletteGreenForeground2: #094509;
    --colorPaletteGreenForeground3: #107c10;
    --colorPaletteGreenForegroundInverted: #359b35;
    --colorPaletteDarkOrangeBackground1: #fdf6f3;
    --colorPaletteDarkOrangeBackground2: #f4bfab;
    --colorPaletteDarkOrangeBackground3: #da3b01;
    --colorPaletteDarkOrangeBorderActive: #da3b01;
    --colorPaletteDarkOrangeBorder1: #f4bfab;
    --colorPaletteDarkOrangeBorder2: #da3b01;
    --colorPaletteDarkOrangeForeground1: #c43501;
    --colorPaletteDarkOrangeForeground2: #7a2101;
    --colorPaletteDarkOrangeForeground3: #da3b01;
    --colorPaletteYellowBackground1: #fffef5;
    --colorPaletteYellowBackground2: #fef7b2;
    --colorPaletteYellowBackground3: #fde300;
    --colorPaletteYellowBorderActive: #fde300;
    --colorPaletteYellowBorder1: #fef7b2;
    --colorPaletteYellowBorder2: #fde300;
    --colorPaletteYellowForeground1: #817400;
    --colorPaletteYellowForeground2: #817400;
    --colorPaletteYellowForeground3: #fde300;
    --colorPaletteYellowForegroundInverted: #fef7b2;
    --colorPaletteBerryBackground1: #fdf5fc;
    --colorPaletteBerryBackground2: #edbbe7;
    --colorPaletteBerryBackground3: #c239b3;
    --colorPaletteBerryBorderActive: #c239b3;
    --colorPaletteBerryBorder1: #edbbe7;
    --colorPaletteBerryBorder2: #c239b3;
    --colorPaletteBerryForeground1: #af33a1;
    --colorPaletteBerryForeground2: #6d2064;
    --colorPaletteBerryForeground3: #c239b3;
    --colorPaletteMarigoldBackground1: #fefbf4;
    --colorPaletteMarigoldBackground2: #f9e2ae;
    --colorPaletteMarigoldBackground3: #eaa300;
    --colorPaletteMarigoldBorderActive: #eaa300;
    --colorPaletteMarigoldBorder1: #f9e2ae;
    --colorPaletteMarigoldBorder2: #eaa300;
    --colorPaletteMarigoldForeground1: #d39300;
    --colorPaletteMarigoldForeground2: #835b00;
    --colorPaletteMarigoldForeground3: #eaa300;
    --colorPaletteLightGreenBackground1: #f2fbf2;
    --colorPaletteLightGreenBackground2: #a7e3a5;
    --colorPaletteLightGreenBackground3: #13a10e;
    --colorPaletteLightGreenBorderActive: #13a10e;
    --colorPaletteLightGreenBorder1: #a7e3a5;
    --colorPaletteLightGreenBorder2: #13a10e;
    --colorPaletteLightGreenForeground1: #11910d;
    --colorPaletteLightGreenForeground2: #0b5a08;
    --colorPaletteLightGreenForeground3: #13a10e;
    --colorPaletteAnchorBackground2: #bcc3c7;
    --colorPaletteAnchorBorderActive: #394146;
    --colorPaletteAnchorForeground2: #202427;
    --colorPaletteBeigeBackground2: #d7d4d4;
    --colorPaletteBeigeBorderActive: #7a7574;
    --colorPaletteBeigeForeground2: #444241;
    --colorPaletteBlueBackground2: #a9d3f2;
    --colorPaletteBlueBorderActive: #0078d4;
    --colorPaletteBlueForeground2: #004377;
    --colorPaletteBrassBackground2: #e0cea2;
    --colorPaletteBrassBorderActive: #986f0b;
    --colorPaletteBrassForeground2: #553e06;
    --colorPaletteBrownBackground2: #ddc3b0;
    --colorPaletteBrownBorderActive: #8e562e;
    --colorPaletteBrownForeground2: #50301a;
    --colorPaletteCornflowerBackground2: #c8d1fa;
    --colorPaletteCornflowerBorderActive: #4f6bed;
    --colorPaletteCornflowerForeground2: #2c3c85;
    --colorPaletteCranberryBackground2: #eeacb2;
    --colorPaletteCranberryBorderActive: #c50f1f;
    --colorPaletteCranberryForeground2: #6e0811;
    --colorPaletteDarkGreenBackground2: #9ad29a;
    --colorPaletteDarkGreenBorderActive: #0b6a0b;
    --colorPaletteDarkGreenForeground2: #063b06;
    --colorPaletteDarkRedBackground2: #d69ca5;
    --colorPaletteDarkRedBorderActive: #750b1c;
    --colorPaletteDarkRedForeground2: #420610;
    --colorPaletteForestBackground2: #bdd99b;
    --colorPaletteForestBorderActive: #498205;
    --colorPaletteForestForeground2: #294903;
    --colorPaletteGoldBackground2: #ecdfa5;
    --colorPaletteGoldBorderActive: #c19c00;
    --colorPaletteGoldForeground2: #6c5700;
    --colorPaletteGrapeBackground2: #d9a7e0;
    --colorPaletteGrapeBorderActive: #881798;
    --colorPaletteGrapeForeground2: #4c0d55;
    --colorPaletteLavenderBackground2: #d2ccf8;
    --colorPaletteLavenderBorderActive: #7160e8;
    --colorPaletteLavenderForeground2: #3f3682;
    --colorPaletteLightTealBackground2: #a6e9ed;
    --colorPaletteLightTealBorderActive: #00b7c3;
    --colorPaletteLightTealForeground2: #00666d;
    --colorPaletteLilacBackground2: #e6bfed;
    --colorPaletteLilacBorderActive: #b146c2;
    --colorPaletteLilacForeground2: #63276d;
    --colorPaletteMagentaBackground2: #eca5d1;
    --colorPaletteMagentaBorderActive: #bf0077;
    --colorPaletteMagentaForeground2: #6b0043;
    --colorPaletteMinkBackground2: #cecccb;
    --colorPaletteMinkBorderActive: #5d5a58;
    --colorPaletteMinkForeground2: #343231;
    --colorPaletteNavyBackground2: #a3b2e8;
    --colorPaletteNavyBorderActive: #0027b4;
    --colorPaletteNavyForeground2: #001665;
    --colorPalettePeachBackground2: #ffddb3;
    --colorPalettePeachBorderActive: #ff8c00;
    --colorPalettePeachForeground2: #8f4e00;
    --colorPalettePinkBackground2: #f7c0e3;
    --colorPalettePinkBorderActive: #e43ba6;
    --colorPalettePinkForeground2: #80215d;
    --colorPalettePlatinumBackground2: #cdd6d8;
    --colorPalettePlatinumBorderActive: #69797e;
    --colorPalettePlatinumForeground2: #3b4447;
    --colorPalettePlumBackground2: #d696c0;
    --colorPalettePlumBorderActive: #77004d;
    --colorPalettePlumForeground2: #43002b;
    --colorPalettePumpkinBackground2: #efc4ad;
    --colorPalettePumpkinBorderActive: #ca5010;
    --colorPalettePumpkinForeground2: #712d09;
    --colorPalettePurpleBackground2: #c6b1de;
    --colorPalettePurpleBorderActive: #5c2e91;
    --colorPalettePurpleForeground2: #341a51;
    --colorPaletteRoyalBlueBackground2: #9abfdc;
    --colorPaletteRoyalBlueBorderActive: #004e8c;
    --colorPaletteRoyalBlueForeground2: #002c4e;
    --colorPaletteSeafoamBackground2: #a8f0cd;
    --colorPaletteSeafoamBorderActive: #00cc6a;
    --colorPaletteSeafoamForeground2: #00723b;
    --colorPaletteSteelBackground2: #94c8d4;
    --colorPaletteSteelBorderActive: #005b70;
    --colorPaletteSteelForeground2: #00333f;
    --colorPaletteTealBackground2: #9bd9db;
    --colorPaletteTealBorderActive: #038387;
    --colorPaletteTealForeground2: #02494c;
    --colorStatusSuccessBackground1: #f1faf1;
    --colorStatusSuccessBackground2: #9fd89f;
    --colorStatusSuccessBackground3: #107c10;
    --colorStatusSuccessForeground1: #0e700e;
    --colorStatusSuccessForeground2: #094509;
    --colorStatusSuccessForeground3: #107c10;
    --colorStatusSuccessForegroundInverted: #54b054;
    --colorStatusSuccessBorderActive: #107c10;
    --colorStatusSuccessBorder1: #9fd89f;
    --colorStatusSuccessBorder2: #107c10;
    --colorStatusWarningBackground1: #fff9f5;
    --colorStatusWarningBackground2: #fdcfb4;
    --colorStatusWarningBackground3: #f7630c;
    --colorStatusWarningForeground1: #bc4b09;
    --colorStatusWarningForeground2: #8a3707;
    --colorStatusWarningForeground3: #bc4b09;
    --colorStatusWarningForegroundInverted: #faa06b;
    --colorStatusWarningBorderActive: #f7630c;
    --colorStatusWarningBorder1: #fdcfb4;
    --colorStatusWarningBorder2: #bc4b09;
    --colorStatusDangerBackground1: #fdf3f4;
    --colorStatusDangerBackground2: #eeacb2;
    --colorStatusDangerBackground3: #c50f1f;
    --colorStatusDangerBackground3Hover: #b10e1c;
    --colorStatusDangerBackground3Pressed: #960b18;
    --colorStatusDangerForeground1: #b10e1c;
    --colorStatusDangerForeground2: #6e0811;
    --colorStatusDangerForeground3: #c50f1f;
    --colorStatusDangerForegroundInverted: #dc626d;
    --colorStatusDangerBorderActive: #c50f1f;
    --colorStatusDangerBorder1: #eeacb2;
    --colorStatusDangerBorder2: #c50f1f;
    --borderRadiusNone: 0;
    --borderRadiusSmall: 2px;
    --borderRadiusMedium: 4px;
    --borderRadiusLarge: 6px;
    --borderRadiusXLarge: 8px;
    --borderRadiusCircular: 10000px;
    --fontFamilyBase: "Segoe UI", "Segoe UI Web (West European)", -apple-system,
      BlinkMacSystemFont, Roboto, "Helvetica Neue", sans-serif;
    --fontFamilySerif: "Lora", serif;
    --fontFamilyMonospace: Consolas, "Courier New", Courier, monospace;
    --fontFamilyNumeric: Bahnschrift, "Segoe UI", "Segoe UI Web (West European)",
      -apple-system, BlinkMacSystemFont, Roboto, "Helvetica Neue", sans-serif;
    --fontSizeBase100: 10px;
    --fontSizeBase200: 12px;
    --fontSizeBase300: 14px;
    --fontSizeBase400: 16px;
    --fontSizeBase500: 20px;
    --fontSizeBase600: 24px;
    --fontSizeHero700: 28px;
    --fontSizeHero800: 32px;
    --fontSizeHero900: 40px;
    --fontSizeHero1000: 68px;
    --fontWeightRegular: 400;
    --fontWeightMedium: 500;
    --fontWeightSemibold: 600;
    --fontWeightBold: 700;
    --lineHeightBase100: 14px;
    --lineHeightBase200: 16px;
    --lineHeightBase300: 20px;
    --lineHeightBase400: 22px;
    --lineHeightBase500: 28px;
    --lineHeightBase600: 32px;
    --lineHeightHero700: 36px;
    --lineHeightHero800: 40px;
    --lineHeightHero900: 52px;
    --lineHeightHero1000: 92px;
    --shadow2: 0 0 2px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.14);
    --shadow4: 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.14);
    --shadow8: 0 0 2px rgba(0, 0, 0, 0.12), 0 4px 8px rgba(0, 0, 0, 0.14);
    --shadow16: 0 0 2px rgba(0, 0, 0, 0.12), 0 8px 16px rgba(0, 0, 0, 0.14);
    --shadow28: 0 0 8px rgba(0, 0, 0, 0.12), 0 14px 28px rgba(0, 0, 0, 0.14);
    --shadow64: 0 0 8px rgba(0, 0, 0, 0.12), 0 32px 64px rgba(0, 0, 0, 0.14);
    --shadow2Brand: 0 0 2px rgba(0, 0, 0, 0.3), 0 1px 2px rgba(0, 0, 0, 0.25);
    --shadow4Brand: 0 0 2px rgba(0, 0, 0, 0.3), 0 2px 4px rgba(0, 0, 0, 0.25);
    --shadow8Brand: 0 0 2px rgba(0, 0, 0, 0.3), 0 4px 8px rgba(0, 0, 0, 0.25);
    --shadow16Brand: 0 0 2px rgba(0, 0, 0, 0.3), 0 8px 16px rgba(0, 0, 0, 0.25);
    --shadow28Brand: 0 0 8px rgba(0, 0, 0, 0.3), 0 14px 28px rgba(0, 0, 0, 0.25);
    --shadow64Brand: 0 0 8px rgba(0, 0, 0, 0.3), 0 32px 64px rgba(0, 0, 0, 0.25);
    --strokeWidthThin: 1px;
    --strokeWidthThick: 2px;
    --strokeWidthThicker: 3px;
    --strokeWidthThickest: 4px;
    --spacingHorizontalNone: 0;
    --spacingHorizontalXXS: 2px;
    --spacingHorizontalXS: 4px;
    --spacingHorizontalSNudge: 6px;
    --spacingHorizontalS: 8px;
    --spacingHorizontalMNudge: 10px;
    --spacingHorizontalM: 12px;
    --spacingHorizontalL: 16px;
    --spacingHorizontalXL: 20px;
    --spacingHorizontalXXL: 24px;
    --spacingHorizontalXXXL: 32px;
    --spacingVerticalNone: 0;
    --spacingVerticalXXS: 2px;
    --spacingVerticalXS: 4px;
    --spacingVerticalSNudge: 6px;
    --spacingVerticalS: 8px;
    --spacingVerticalMNudge: 10px;
    --spacingVerticalM: 12px;
    --spacingVerticalL: 16px;
    --spacingVerticalXL: 20px;
    --spacingVerticalXXL: 24px;
    --spacingVerticalXXXL: 32px;
    --durationUltraFast: 50ms;
    --durationFaster: 100ms;
    --durationFast: 150ms;
    --durationNormal: 200ms;
    --durationGentle: 250ms;
    --durationSlow: 300ms;
    --durationSlower: 400ms;
    --durationUltraSlow: 500ms;
    --curveAccelerateMax: cubic-bezier(0.9, 0.1, 1, 0.2);
    --curveAccelerateMid: cubic-bezier(1, 0, 1, 1);
    --curveAccelerateMin: cubic-bezier(0.8, 0, 0.78, 1);
    --curveDecelerateMax: cubic-bezier(0.1, 0.9, 0.2, 1);
    --curveDecelerateMid: cubic-bezier(0, 0, 0, 1);
    --curveDecelerateMin: cubic-bezier(0.33, 0, 0.1, 1);
    --curveEasyEaseMax: cubic-bezier(0.8, 0, 0.2, 1);
    --curveEasyEase: cubic-bezier(0.33, 0, 0.67, 1);
    --curveLinear: cubic-bezier(0, 0, 1, 1);
    --specialColor: #edf6ff;
}


[role='checkbox'].invalid::part(control),
[role='combobox'].invalid::part(control),
fluent-combobox.invalid::part(control),
fluent-text-area.invalid::part(control),
fluent-text-field.invalid::part(root)
{
    outline: calc(var(--stroke-width) * 1px)  solid var(--error);
}

`;

styleSheet.replaceSync(styles);
// document.adoptedStyleSheets.push(styleSheet);
document.adoptedStyleSheets = [...document.adoptedStyleSheets, styleSheet];

var beforeStartCalled = false;
var afterStartedCalled = false;


export function afterWebStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor, 'web');
  }
}

export function beforeWebStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function beforeWebAssemblyStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function afterWebAssemblyStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor, 'wasm');
  }
}

export function beforeServerStart(options: any) {
  if (!beforeStartCalled) {
    beforeStart(options);
  }
}

export function afterServerStarted(blazor: any) {
  if (!afterStartedCalled) {
    afterStarted(blazor, 'server');
  }
}

export function afterStarted(blazor: Blazor, mode: string) {

  blazor.registerCustomEventType('radiogroupclick', {
    browserEventName: 'click',
    createEventArgs: event => {
      if (event.target!._readOnly || event.target!._disabled) {
        return null;
      }
      return {
        value: event.target!.value
      };
    }
  });

  blazor.registerCustomEventType('checkedchange', {
    browserEventName: 'change',
    createEventArgs: event => {

      // Hacking of a fake update
      if (event.target!.isUpdating) {
        return {
          checked: null,
          indeterminate: null
        }
      }

      return {
        checked: event.target!.currentChecked,
        indeterminate: event.target!.indeterminate
      };
    }
  });

  blazor.registerCustomEventType('switchcheckedchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      return {
        checked: event.target!.checked
      };
    }
  });

  blazor.registerCustomEventType('accordionchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-accordion-item') {
        return {
          activeId: event.target!.id,
          expanded: event.target!._expanded
        }
      };
      return null;
    }
  });

  blazor.registerCustomEventType('tabchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-tabs') {
        return {
          activeId: event.detail.id,
        }
      };
      return null;
    }
  });
  blazor.registerCustomEventType('selectedchange', {
    browserEventName: 'selected-change',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-tree-item') {
        return {
          affectedId: event.detail.attributes['id'].value,
          selected: event.detail._selected,
          expanded: event.detail._expanded
        }
      };
      return null;
    }
  });

  blazor.registerCustomEventType('expandedchange', {
    browserEventName: 'expanded-change',
    createEventArgs: event => {
      return {
        affectedId: event.detail.attributes['id'].value,
        selected: event.detail._selected,
        expanded: event.detail._expanded
      };
    }
  });
  blazor.registerCustomEventType('dateselected', {
    browserEventName: 'dateselected',
    createEventArgs: event => {
      return {
        calendarDateInfo: event.detail
      };
    }
  });

  blazor.registerCustomEventType('tooltipdismiss', {
    browserEventName: 'dismiss',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-tooltip') {
        return {
          reason: event.type
        };
      };
      return null;
    }
  });

  blazor.registerCustomEventType('dialogdismiss', {
    browserEventName: 'dismiss',
    createEventArgs: event => {
      if (event.target!.localName == 'fluent-dialog') {
        return {
          id: event.target!.id,
          reason: event.type
        };
      };
      return null;
    }
  });

  blazor.registerCustomEventType('menuchange', {
    browserEventName: 'change',
    createEventArgs: event => {
      return {
        id: event.target!.id,
        value: event.target!.innerText
      };
    }
  });
  blazor.registerCustomEventType('scrollstart', {
    browserEventName: 'scrollstart',
    createEventArgs: event => {
      return {
        scroll: event.detail
      };
    }
  });

  blazor.registerCustomEventType('scrollend', {
    browserEventName: 'scrollend',
    createEventArgs: event => {
      return {
        scroll: event.detail
      };
    }
  });

  blazor.registerCustomEventType('cellfocus', {
    browserEventName: 'cell-focused',
    createEventArgs: event => {
      return {
        cellId: event.detail.attributes['cell-id'].value
      };
    }
  });

  blazor.registerCustomEventType('rowfocus', {
    browserEventName: 'row-focused',
    createEventArgs: event => {
      return {
        rowId: event.detail.attributes['row-id'].value
      };
    }
  });

  blazor.registerCustomEventType('splitterresized', {
    browserEventName: 'splitterresized',
    createEventArgs: event => {
      return {
        panel1size: event.detail.panel1size,
        panel2size: event.detail.panel2size
      }
    }
  });
  blazor.registerCustomEventType('splittercollapsed', {
    browserEventName: 'splittercollapsed',
    createEventArgs: event => {
      return {
        collapsed: event.detail.collapsed
      }
    }
  });

  blazor.theme = {
    isSystemDark: () => {
      return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    },

    isDarkMode: () => {
      const luminance: string = getComputedStyle(document.documentElement).getPropertyValue('--base-layer-luminance');
      return parseFloat(luminance) < 0.5;
    }
  }


  if (typeof blazor.addEventListener === 'function' && mode === 'web') {
    customElements.define('fluent-page-script', FluentPageScript);
    blazor.addEventListener('enhancedload', onEnhancedLoad);
  }

  afterStartedCalled = true;
}

export function beforeStart(options: any) {

  webcomponents.setTheme(webDarkTheme);
  
  webcomponents.accordionItemDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.accordionDefinition
  webcomponents.AnchorButtonDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.AvatarDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.BadgeDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.ButtonDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.CheckboxDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.CompoundButtonDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.CounterBadgeDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.DialogDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.DialogBodyDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.DividerDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.FieldDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.ImageDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.LabelDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.MenuButtonDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.MenuItemDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.MenuListDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.MenuDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.ProgressBarDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.RadioGroupDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.RadioDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.SliderDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.SpinnerDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.SwitchDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.TabPanelDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.TabDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.TabsDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.TextInputDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.TextDefinition.define(webcomponents.FluentDesignSystem.registry);
  webcomponents.ToggleButtonDefinition.define(webcomponents.FluentDesignSystem.registry);
  
  //customElements.define("fluent-design-theme", DesignTheme);
  customElements.define("split-panels", SplitPanels);

  beforeStartCalled = true;
}
