// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

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
