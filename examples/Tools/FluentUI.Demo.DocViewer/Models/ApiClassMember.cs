// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;

namespace FluentUI.Demo.DocViewer.Models;

internal class ApiClassMember
{
    public MemberTypes MemberType { get; set; } = MemberTypes.Property;
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string[] EnumValues { get; set; } = [];
    public string[] Parameters { get; set; } = [];
    public string? Default { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsParameter { get; set; }
}
