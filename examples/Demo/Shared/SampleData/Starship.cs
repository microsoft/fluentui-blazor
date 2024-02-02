using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Demo.Shared.SampleData;

[RequiresUnreferencedCode("Necessary because of RangeAttribute usage")]
public class Starship
{
    [Required]
    [MinLength(3, ErrorMessage = "Identifier is too short!")]
    [StringLength(16, ErrorMessage = "Identifier too long (16 character limit).")]
    public string? Identifier { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "A classification is required")]
    public string? Classification { get; set; }

    [Range(1, 100000, ErrorMessage = "Accommodation invalid (1-100000).")]
    public int MaximumAccommodation { get; set; }

    [Required]
    [Range(typeof(bool), "true", "true",
        ErrorMessage = "This form disallows unapproved ships.")]
    public bool IsValidatedDesign { get; set; }

    [Required]
    public DateTime? ProductionDate { get; set; }

    public bool HasTeleporter { get; set; }
}
