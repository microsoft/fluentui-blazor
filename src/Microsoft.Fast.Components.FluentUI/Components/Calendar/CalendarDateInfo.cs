namespace Microsoft.Fast.Components.FluentUI
{
    public struct CalendarDateInfo
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public bool Selected { get; set; }
        public bool Disabled { get; set; }
    }
}