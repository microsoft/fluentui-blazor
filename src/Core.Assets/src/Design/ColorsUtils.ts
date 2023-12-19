class ColorsUtils {

  public static isSystemDark(): boolean {
    return (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches);
  }

  /**
   * See https://github.com/microsoft/fast -> packages/utilities/fast-colors/src/parse-color.ts
   */
  public static parseColorHexRGB(raw: string | null): ColorRGB | null {
    // Matches #RGB and #RRGGBB, where R, G, and B are [0-9] or [A-F]
    const hexRGBRegex: RegExp = /^#((?:[0-9a-f]{6}|[0-9a-f]{3}))$/i;
    const result: string[] | null = hexRGBRegex.exec(raw ?? ColorsUtils.DEFAULT_COLOR);

    if (result === null) {
      return null;
    }

    let digits: string = result[1];

    if (digits.length === 3) {
      const r: string = digits.charAt(0);
      const g: string = digits.charAt(1);
      const b: string = digits.charAt(2);

      digits = r.concat(r, g, g, b, b);
    }

    const rawInt: number = parseInt(digits, 16);

    if (isNaN(rawInt)) {
      return null;
    }

    return new ColorRGB(
      this.normalized((rawInt & 0xff0000) >>> 16, 0, 255),
      this.normalized((rawInt & 0x00ff00) >>> 8, 0, 255),
      this.normalized(rawInt & 0x0000ff, 0, 255),
    );
  }

  /**
   * Scales an input to a number between 0 and 1
   */
  public static normalized(i: number, min: number, max: number): number {
    if (isNaN(i) || i <= min) {
      return 0.0;
    } else if (i >= max) {
      return 1.0;
    }
    return i / (max - min);
  }

  /**
   * Convert to named color to an equivalent Hex color
   * @param name Office color name
   * @returns Hexadecimal color
   */
  public static getHexColor(name: string | null): string {
    return this.NAMED_COLORS.find(item => item.App.toLowerCase() === name?.toLowerCase())?.Color ?? ColorsUtils.DEFAULT_COLOR;
  }

  static DEFAULT_COLOR = "#0078D4";

  /**
   * List of Office colors
  */
  static NAMED_COLORS = [
    { App: "Access", Color: "#a4373a" },
    { App: "Booking", Color: "#00a99d" },
    { App: "Exchange", Color: "#0078d4" },
    { App: "Excel", Color: "#217346" },
    { App: "GroupMe", Color: "#00bcf2" },
    { App: "Office", Color: "#d83b01" },
    { App: "OneDrive", Color: "#0078d4" },
    { App: "OneNote", Color: "#7719aa" },
    { App: "Outlook", Color: "#0f6cbd" },
    { App: "Planner", Color: "#31752f" },
    { App: "PowerApps", Color: "#742774" },
    { App: "PowerBI", Color: "#f2c811" },
    { App: "PowerPoint", Color: "#b7472a" },
    { App: "Project", Color: "#31752f" },
    { App: "Publisher", Color: "#077568" },
    { App: "SharePoint", Color: "#0078d4" },
    { App: "Skype", Color: "#0078d4" },
    { App: "Stream", Color: "#bc1948" },
    { App: "Sway", Color: "#008272" },
    { App: "Teams", Color: "#6264a7" },
    { App: "Visio", Color: "#3955a3" },
    { App: "Windows", Color: "#0078d4" },
    { App: "Word", Color: "#2b579a" },
    { App: "Yamme", Color: "#106ebe" },
    { App: "Word", Color: "" },
  ];
}

class ColorRGB {
  constructor(red: number, green: number, blue: number) {
    this.r = red;
    this.g = green;
    this.b = blue;
  }

  public readonly r: number;
  public readonly g: number;
  public readonly b: number;
}

export { ColorsUtils };
