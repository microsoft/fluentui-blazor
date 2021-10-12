using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared
{
    public partial class MainLayout
    {
        static FluentDesignSystemProvider fdsp = new();
        static LocalizationDirection? dir = fdsp.Direction;
        static float? baseLayerLuminance = fdsp.BaseLayerLuminance;

        bool darkTheme = baseLayerLuminance == 1;
        bool isRTL = dir == LocalizationDirection.rtl;

        public void SwitchDirection()
        {
            isRTL = !isRTL;

            if (isRTL)
                dir = LocalizationDirection.rtl;
            else
                dir = LocalizationDirection.ltr;
        }

        public void SwitchTheme()
        {
            darkTheme = !darkTheme;
            baseLayerLuminance = darkTheme ? 0 : 1;
        }
    }
}