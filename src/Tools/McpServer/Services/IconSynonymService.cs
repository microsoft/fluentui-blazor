// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

/// <summary>
/// Service that manages synonym mappings for Fluent UI icon search.
/// Maps common keywords and alternative terms to icon name fragments for improved discoverability.
/// </summary>
public sealed class IconSynonymService
{
    /// <summary>
    /// The synonym map: keys are common keywords, values are arrays of icon name fragments to search.
    /// </summary>
    private static readonly Dictionary<string, string[]> SynonymMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Actions
        ["trash"] = ["Delete", "Bin"],
        ["remove"] = ["Delete", "Dismiss", "Subtract"],
        ["close"] = ["Dismiss"],
        ["cancel"] = ["Dismiss"],
        ["x"] = ["Dismiss"],
        ["save"] = ["Save", "Checkmark"],
        ["floppy"] = ["Save"],
        ["confirm"] = ["Checkmark"],
        ["ok"] = ["Checkmark"],
        ["yes"] = ["Checkmark"],
        ["validate"] = ["Checkmark"],
        ["undo"] = ["ArrowUndo"],
        ["redo"] = ["ArrowRedo"],
        ["refresh"] = ["ArrowSync", "ArrowClockwise"],
        ["reload"] = ["ArrowSync", "ArrowClockwise"],
        ["retry"] = ["ArrowCounterclockwise"],

        // Navigation
        ["back"] = ["ArrowLeft", "ChevronLeft", "ArrowReply"],
        ["forward"] = ["ArrowRight", "ChevronRight", "ArrowForward"],
        ["up"] = ["ArrowUp", "ChevronUp"],
        ["down"] = ["ArrowDown", "ChevronDown"],
        ["menu"] = ["Navigation", "LineHorizontal3"],
        ["hamburger"] = ["Navigation", "LineHorizontal3"],
        ["expand"] = ["ChevronDown", "ArrowExpand", "ChevronRight"],
        ["collapse"] = ["ChevronUp", "ArrowCollapseAll", "ChevronLeft"],

        // Communication
        ["notification"] = ["Alert"],
        ["bell"] = ["Alert"],
        ["alarm"] = ["Alert"],
        ["email"] = ["Mail"],
        ["envelope"] = ["Mail"],
        ["message"] = ["Chat", "Mail"],
        ["send"] = ["Send"],
        ["phone"] = ["Call"],
        ["call"] = ["Call"],

        // People
        ["user"] = ["Person"],
        ["profile"] = ["Person"],
        ["account"] = ["Person"],
        ["people"] = ["People", "Person"],
        ["team"] = ["People", "PersonBoard"],
        ["group"] = ["People", "Group"],

        // Content
        ["file"] = ["Document"],
        ["document"] = ["Document"],
        ["folder"] = ["Folder"],
        ["directory"] = ["Folder"],
        ["image"] = ["Image"],
        ["photo"] = ["Image"],
        ["picture"] = ["Image"],
        ["video"] = ["Video"],
        ["music"] = ["MusicNote"],
        ["audio"] = ["Speaker", "MusicNote"],

        // UI / Layout
        ["settings"] = ["Settings"],
        ["gear"] = ["Settings"],
        ["cog"] = ["Settings"],
        ["config"] = ["Settings"],
        ["configuration"] = ["Settings"],
        ["home"] = ["Home"],
        ["house"] = ["Home"],
        ["search"] = ["Search"],
        ["magnifier"] = ["Search"],
        ["loupe"] = ["Search"],
        ["filter"] = ["Filter"],
        ["sort"] = ["ArrowSort"],
        ["grid"] = ["Grid"],
        ["list"] = ["List"],
        ["table"] = ["Table"],

        // Status
        ["warning"] = ["Warning"],
        ["caution"] = ["Warning"],
        ["exclamation"] = ["Warning", "Alert"],
        ["error"] = ["Error", "DismissCircle"],
        ["danger"] = ["Error", "Warning"],
        ["success"] = ["CheckmarkCircle", "Checkmark"],
        ["info"] = ["Info"],
        ["information"] = ["Info"],
        ["help"] = ["QuestionCircle", "Question"],
        ["question"] = ["QuestionCircle", "Question"],

        // Editing
        ["edit"] = ["Edit", "Pen"],
        ["pencil"] = ["Edit"],
        ["pen"] = ["Edit", "Pen"],
        ["write"] = ["Edit", "Compose"],
        ["compose"] = ["Compose"],
        ["copy"] = ["Copy"],
        ["clipboard"] = ["Clipboard"],
        ["paste"] = ["ClipboardPaste"],
        ["cut"] = ["Cut"],

        // Data
        ["database"] = ["Database"],
        ["storage"] = ["Database", "Storage"],
        ["cloud"] = ["Cloud"],
        ["server"] = ["Server"],
        ["download"] = ["ArrowDownload"],
        ["upload"] = ["ArrowUpload"],

        // Favorites
        ["heart"] = ["Heart"],
        ["love"] = ["Heart"],
        ["like"] = ["ThumbLike"],
        ["dislike"] = ["ThumbDislike"],
        ["favorite"] = ["Star", "Heart"],
        ["star"] = ["Star"],
        ["bookmark"] = ["Bookmark"],

        // Security
        ["lock"] = ["Lock"],
        ["unlock"] = ["LockOpen"],
        ["key"] = ["Key"],
        ["password"] = ["Password", "Key"],
        ["shield"] = ["Shield"],
        ["security"] = ["Shield", "Lock"],

        // Misc
        ["calendar"] = ["Calendar"],
        ["date"] = ["Calendar"],
        ["time"] = ["Clock"],
        ["clock"] = ["Clock"],
        ["map"] = ["Map"],
        ["location"] = ["Location"],
        ["pin"] = ["Pin", "Location"],
        ["link"] = ["Link"],
        ["share"] = ["Share"],
        ["print"] = ["Print"],
        ["attach"] = ["Attach"],
        ["attachment"] = ["Attach"],
        ["bug"] = ["Bug"],
        ["code"] = ["Code"],
        ["terminal"] = ["Terminal"],
        ["eye"] = ["Eye"],
        ["visible"] = ["Eye"],
        ["hidden"] = ["EyeOff"],
        ["invisible"] = ["EyeOff"],
        ["add"] = ["Add"],
        ["plus"] = ["Add"],
        ["new"] = ["Add"],
        ["create"] = ["Add"],
        ["minus"] = ["Subtract"],
        ["more"] = ["MoreHorizontal", "MoreVertical"],
        ["dots"] = ["MoreHorizontal", "MoreVertical"],
        ["ellipsis"] = ["MoreHorizontal", "MoreVertical"],
        ["checkmark"] = ["Checkmark"],
        ["tick"] = ["Checkmark"],
        ["check"] = ["Checkmark", "Checkbox"],
    };

    /// <summary>
    /// Gets the number of synonym entries.
    /// </summary>
    public int Count => SynonymMap.Count;

    /// <summary>
    /// Tries to get the exact targets for a synonym term.
    /// </summary>
    /// <param name="term">The search term.</param>
    /// <param name="targets">The icon name fragments to match.</param>
    /// <returns><see langword="true"/> if an exact match was found.</returns>
    public bool TryGetTargets(string term, out string[] targets)
    {
        return SynonymMap.TryGetValue(term, out targets!);
    }

    /// <summary>
    /// Gets all synonym entries where the key partially matches the given term
    /// (the key contains the term, or the term contains the key).
    /// </summary>
    /// <param name="term">The search term.</param>
    /// <returns>An enumerable of matching target arrays.</returns>
    public IEnumerable<string[]> GetPartialMatches(string term)
    {
        foreach (var kvp in SynonymMap)
        {
            if (kvp.Key.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                term.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
            {
                yield return kvp.Value;
            }
        }
    }

    /// <summary>
    /// Gets all synonym keys (the searchable terms).
    /// </summary>
    public IEnumerable<string> GetAllKeys() => SynonymMap.Keys;

    /// <summary>
    /// Checks whether a given term has a synonym mapping (exact, case-insensitive).
    /// </summary>
    /// <param name="term">The term to check.</param>
    /// <returns><see langword="true"/> if the term exists as a synonym key.</returns>
    public bool HasSynonym(string term) => SynonymMap.ContainsKey(term);
}
