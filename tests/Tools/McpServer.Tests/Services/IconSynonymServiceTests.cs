// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Tests for the <see cref="IconSynonymService"/> class.
/// </summary>
public class IconSynonymServiceTests
{
    private readonly IconSynonymService _service = new();

    #region Count

    [Fact]
    public void Count_ReturnsPositiveNumber()
    {
        Assert.True(_service.Count > 0);
    }

    #endregion

    #region TryGetTargets

    [Theory]
    [InlineData("trash", new[] { "Delete", "Bin" })]
    [InlineData("bell", new[] { "Alert" })]
    [InlineData("gear", new[] { "Settings" })]
    [InlineData("home", new[] { "Home" })]
    [InlineData("heart", new[] { "Heart" })]
    public void TryGetTargets_KnownSynonym_ReturnsTrueWithTargets(string term, string[] expectedTargets)
    {
        var found = _service.TryGetTargets(term, out var targets);

        Assert.True(found);
        Assert.Equal(expectedTargets, targets);
    }

    [Fact]
    public void TryGetTargets_UnknownTerm_ReturnsFalse()
    {
        var found = _service.TryGetTargets("xyznonexistent", out _);

        Assert.False(found);
    }

    [Fact]
    public void TryGetTargets_CaseInsensitive()
    {
        var found = _service.TryGetTargets("TRASH", out var targets);

        Assert.True(found);
        Assert.Contains("Delete", targets, StringComparer.Ordinal);
    }

    #endregion

    #region GetPartialMatches

    [Fact]
    public void GetPartialMatches_PartialKey_ReturnsMatches()
    {
        // "notif" should partially match "notification"
        var matches = _service.GetPartialMatches("notif").ToList();

        Assert.NotEmpty(matches);
        Assert.Contains(matches, targets => targets.Contains("Alert", StringComparer.Ordinal));
    }

    [Fact]
    public void GetPartialMatches_NoMatch_ReturnsEmpty()
    {
        var matches = _service.GetPartialMatches("qqqwwwnomatch").ToList();

        Assert.Empty(matches);
    }

    [Fact]
    public void GetPartialMatches_TermContainsKey_ReturnsMatches()
    {
        // "mytrashcan" contains the key "trash"
        var matches = _service.GetPartialMatches("mytrashcan").ToList();

        Assert.NotEmpty(matches);
        Assert.Contains(matches, targets => targets.Contains("Delete", StringComparer.Ordinal));
    }

    #endregion

    #region GetAllKeys

    [Fact]
    public void GetAllKeys_ReturnsAllSynonymKeys()
    {
        var keys = _service.GetAllKeys().ToList();

        Assert.True(keys.Count > 50, "Expected at least 50 synonym entries");
        Assert.Contains(keys, static k => string.Equals(k, "trash", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(keys, static k => string.Equals(k, "bell", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(keys, static k => string.Equals(k, "gear", StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region HasSynonym

    [Theory]
    [InlineData("trash", true)]
    [InlineData("bell", true)]
    [InlineData("nonexistent_word_xyz", false)]
    public void HasSynonym_ReturnsExpected(string term, bool expected)
    {
        Assert.Equal(expected, _service.HasSynonym(term));
    }

    [Fact]
    public void HasSynonym_CaseInsensitive()
    {
        Assert.True(_service.HasSynonym("TRASH"));
        Assert.True(_service.HasSynonym("Trash"));
    }

    #endregion

    #region Category Coverage

    [Theory]
    [InlineData("save")]
    [InlineData("undo")]
    [InlineData("refresh")]
    [InlineData("back")]
    [InlineData("hamburger")]
    [InlineData("notification")]
    [InlineData("email")]
    [InlineData("user")]
    [InlineData("file")]
    [InlineData("settings")]
    [InlineData("warning")]
    [InlineData("edit")]
    [InlineData("database")]
    [InlineData("download")]
    [InlineData("heart")]
    [InlineData("lock")]
    [InlineData("calendar")]
    [InlineData("add")]
    [InlineData("ellipsis")]
    public void TryGetTargets_AllCategories_HaveEntries(string term)
    {
        var found = _service.TryGetTargets(term, out var targets);

        Assert.True(found, $"Synonym '{term}' should exist");
        Assert.NotEmpty(targets);
    }

    #endregion
}
