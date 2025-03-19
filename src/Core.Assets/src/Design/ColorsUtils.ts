class ColorsUtils {

  public static isSystemDark(): boolean {
    return (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches);
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

export { ColorsUtils };
