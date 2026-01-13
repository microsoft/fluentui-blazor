// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static FluentUI.Demo.SampleData.Olympics2024;

namespace FluentUI.Demo.SampleData;

/// <summary>
/// A class representing a starship with various properties and validation attributes.
/// </summary>
[RequiresUnreferencedCode("Necessary because of RangeAttribute usage")]
public class Starship
{
    /// <summary>
    /// The unique identifier for the starship.
    /// </summary>
    [Required]
    [MinLength(3, ErrorMessage = "Identifier is too short")]
    [StringLength(16, ErrorMessage = "Identifier too long (16 character limit)")]
    public string? Identifier { get; set; }

    /// <summary>
    /// The description of the starship.
    /// </summary>
    [Required(ErrorMessage = "Description is required")]
    [MinLength(10, ErrorMessage = "Description is too short")]
    public string? Description { get; set; }

    /// <summary>
    /// Countries where the starship is registered.
    /// </summary>
    [Required(ErrorMessage = "Countries are required")]
    [MinLength(1, ErrorMessage = "Countries are required")]
    public IEnumerable<Country>? Countries { get; set; }

    /// <summary>
    /// Classification of the starship.
    /// </summary>
    [Required(ErrorMessage = "A classification is required")]
    public string? Classification { get; set; }

    /// <summary>
    /// Maximum accommodation capacity of the starship.
    /// </summary>
    ///[Range(1, 100000, ErrorMessage = "Accommodation invalid (1-100000)")]
    public string? MaximumAccommodation { get; set; }

    /// <summary>
    /// Indicates whether the starship design has been validated.
    /// </summary>
    [Required]
    [Range(typeof(bool), "true", "true",
        ErrorMessage = "This form disallows unapproved ships")]
    public bool IsValidatedDesign { get; set; }

    /// <summary>
    /// The production date of the starship.
    /// </summary>
    [Required]
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the starship is equipped with a teleporter.
    /// </summary>
    public bool HasTeleporter { get; set; }
}
