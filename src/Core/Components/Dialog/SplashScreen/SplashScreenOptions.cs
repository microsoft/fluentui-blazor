// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a splash screen.
/// </summary>
public class SplashScreenOptions : DialogOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SplashScreenOptions"/> class.
    /// </summary>
    public SplashScreenOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SplashScreenOptions"/> class
    /// using the specified implementation factory.
    /// </summary>
    /// <param name="implementationFactory">Action used to configure the splash screen options.</param>
    public SplashScreenOptions(Action<SplashScreenOptions> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SplashScreenOptions"/> class
    /// using the specified implementation factory for the splash screen content.
    /// </summary>
    /// <param name="implementationFactory">Action used to configure the splash screen content.</param>
    public SplashScreenOptions(Action<SplashScreenContent> implementationFactory)
    {
        implementationFactory.Invoke(this.SplashScreen);
    }

    /// <summary>
    /// Gets the content displayed by the splash screen.
    /// </summary>
    public SplashScreenContent SplashScreen { get; } = new();
}
