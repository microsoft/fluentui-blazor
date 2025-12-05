// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Mcp.Server.Models;

namespace FluentUI.Mcp.Server.Services;

/// <summary>
/// Factory for creating enum information from types.
/// </summary>
internal sealed class EnumInfoFactory
{
    private readonly XmlDocumentationReader _xmlReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumInfoFactory"/> class.
    /// </summary>
    /// <param name="xmlReader">The XML documentation reader.</param>
    public EnumInfoFactory(XmlDocumentationReader xmlReader)
    {
        _xmlReader = xmlReader;
    }

    /// <summary>
    /// Creates an EnumInfo from a type.
    /// </summary>
    public EnumInfo CreateEnumInfo(Type type)
    {
        var values = new List<EnumValueInfo>();
        var names = Enum.GetNames(type);
        var enumValues = Enum.GetValues(type);

        for (var i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = Convert.ToInt32(enumValues.GetValue(i));
            var field = type.GetField(name);
            var description = field != null ? _xmlReader.GetMemberSummary(field) : string.Empty;

            values.Add(new EnumValueInfo
            {
                Name = name,
                Value = value,
                Description = description
            });
        }

        return new EnumInfo
        {
            Name = type.Name,
            FullName = type.FullName ?? type.Name,
            Description = _xmlReader.GetTypeSummary(type),
            Values = values
        };
    }
}
