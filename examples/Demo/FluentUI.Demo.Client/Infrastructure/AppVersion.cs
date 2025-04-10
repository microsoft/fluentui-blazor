// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;

namespace FluentUI.Demo.Client;
internal static class AppVersion
{

    /// <summary>
    /// Gets the version of the application.
    /// Includes the (shortened) commit hash if available.
    /// </summary>
    public static string Version
    {
        get => GetVersionFromAssembly();
    }

    /// <summary>
    /// Retrieves a string that describes the .NET runtime framework version.
    /// This includes details about the framework being used.
    /// </summary>
    public static string FrameworkDescription
    {
        get => System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
    }

    /// <summary>
    /// Gets the current year as a string.
    /// The year is obtained from the system's current date and time.
    /// </summary>
    public static string CurrentYear
    {
        get => DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
    }

    private static string GetVersionFromAssembly()
    {
        string strVersion = default!;
        var versionAttribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (versionAttribute != null)
        {
            var version = versionAttribute.InformationalVersion;
            var plusIndex = version.IndexOf('+');
            if (plusIndex >= 0 && plusIndex + 9 < version.Length)
            {
                strVersion = version[..(plusIndex + 9)];
            }
            else
            {
                strVersion = version;
            }
        }

        return strVersion;
    }
}
