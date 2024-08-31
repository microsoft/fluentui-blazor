// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.SampleData;

/// <summary>
/// Miscellaneous data for the sample app.
/// </summary>
public partial class Miscellaneous
{
    /// <summary>
    /// Gets a list of Beatles hits.
    /// </summary>
    public static IEnumerable<string> BeatlesHits =>
    [
        "Hey Jude",
        "Let It Be",
        "Yesterday",
        "Come Together",
        "Here Comes the Sun",
        "Something",
        "A Hard Day’s Night",
        "Help!",
        "I Want to Hold Your Hand",
        "All You Need Is Love",
        "Penny Lane",
        "Strawberry Fields Forever",
        "Eleanor Rigby",
        "Lucy in the Sky with Diamonds",
        "Twist and Shout"
    ];

    /// <summary>
    /// Gets a list of sizes.
    /// </summary>
    public IEnumerable<string> Sizes =>
    [
        "Extra small",
        "Small",
        "Medium",
        "Large",
        "Extra large"
    ];
}
