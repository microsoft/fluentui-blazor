using System.ComponentModel.DataAnnotations;

namespace FluentUI.Demo.Shared.Pages.FormDialog.Examples;

public class FormModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}