using System.Reflection;
using DocXml.Reflection;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

/// <summary />
public partial class ApiDocumentation
{
    private string? _name { get; set; }
    private IEnumerable<MemberDescription>? _allMembers = null;

    private List<ColumnDefinition<MemberDescription>> _propertiesGrid = new();
    private List<ColumnDefinition<MemberDescription>> _eventsGrid = new();
    private List<ColumnDefinition<MemberDescription>> _methodsGrid = new();

    [Parameter]
    public Type Component { get; set; } = default!;




    private IEnumerable<MemberDescription> Properties => GetMembers(MemberTypes.Property);

    private IEnumerable<MemberDescription> Events => GetMembers(MemberTypes.Event);

    private IEnumerable<MemberDescription> Methods => GetMembers(MemberTypes.Method);


    protected override void OnInitialized()
    {
        base.OnInitialized();


        _propertiesGrid.Add(new ColumnDefinition<MemberDescription>("Name", x => x.Name));
        _propertiesGrid.Add(new ColumnDefinition<MemberDescription>("Type", x => x.Type));
        _propertiesGrid.Add(new ColumnDefinition<MemberDescription>("Default", x => x.Default));
        _propertiesGrid.Add(new ColumnDefinition<MemberDescription>("Description", x => x.Description));

        _eventsGrid.Add(new ColumnDefinition<MemberDescription>("Name", x => x.Name));
        _eventsGrid.Add(new ColumnDefinition<MemberDescription>("Type", x => x.Type));
        _eventsGrid.Add(new ColumnDefinition<MemberDescription>("Description", x => x.Description));

        _methodsGrid.Add(new ColumnDefinition<MemberDescription>("Name", x => x.Name));
        _methodsGrid.Add(new ColumnDefinition<MemberDescription>("Parameters", x => x.Parameters));
        //MethodsGrid.Add(new ColumnDefinition<MemberDescription>("Return", x => x.Return));
        _methodsGrid.Add(new ColumnDefinition<MemberDescription>("Description", x => x.Description));
    }

    private IEnumerable<MemberDescription> GetMembers(MemberTypes type)
    {
        string[] MEMBERS_TO_EXCLUDE = new[] { "AdditionalAttributes", "Equals", "GetHashCode", "GetType", "SetParametersAsync", "ToString" };

        if (_allMembers == null)
        {
            List<MemberDescription>? members = new List<MemberDescription>();
            object? obj = Activator.CreateInstance(Component);
            _name = obj!.GetType().Name;

            LoxSmoke.DocXml.DocXmlReader? docReader = new(a => a.Location.Replace(".dll", ".xml"));

            IEnumerable<MemberInfo>? allProperties = Component.GetProperties().Select(i => (MemberInfo)i);
            IEnumerable<MemberInfo>? allMethods = Component.GetMethods().Where(i => !i.IsSpecialName).Select(i => (MemberInfo)i);

            foreach (var memberInfo in allProperties.Union(allMethods))
            {
                if (!MEMBERS_TO_EXCLUDE.Contains(memberInfo.Name))
                {
                    var propertyInfo = memberInfo as PropertyInfo;
                    var methodInfo = memberInfo as MethodInfo;

                    if (propertyInfo != null)
                    {
                        bool isProperty = memberInfo.GetCustomAttribute<ParameterAttribute>() != null;

                        bool isEvent = isProperty &&
                                       propertyInfo.PropertyType.GetInterface("IEventCallback") != null;

                        // Properties
                        if (isProperty && !isEvent)
                        {
                            members.Add(new MemberDescription()
                            {
                                MemberType = MemberTypes.Property,
                                Name = propertyInfo.Name,
                                Type = propertyInfo.ToTypeNameString(),
                                EnumValues = GetEnumValues(propertyInfo),
                                Default = propertyInfo.GetValue(obj)?.ToString() ?? "null",
                                Description = RemoveExtraReferenceTags(docReader.GetMemberComment(propertyInfo))
                            });
                        }

                        // Events
                        if (isEvent)
                        {
                            string eventTypes = String.Join(", ", propertyInfo.PropertyType.GenericTypeArguments.Select(i => i.Name));
                            members.Add(new MemberDescription()
                            {
                                MemberType = MemberTypes.Event,
                                Name = propertyInfo.Name,
                                Type = propertyInfo.ToTypeNameString(),
                                Default = "",
                                Description = RemoveExtraReferenceTags(docReader.GetMemberComment(propertyInfo))
                            });
                        }
                    }

                    // Methods
                    if (methodInfo != null)
                    {
                        members.Add(new MemberDescription()
                        {
                            MemberType = MemberTypes.Method,
                            Name = methodInfo.Name,
                            Parameters = methodInfo.GetParameters().Select(i => $"{i.ToTypeNameString()} {i.Name}").ToArray(),
                            Type = methodInfo.ToTypeNameString(),
                            Description = RemoveExtraReferenceTags(docReader.GetMemberComment(methodInfo))
                        });
                    }
                }
            }

            _allMembers = members.OrderBy(i => i.Name).ToArray();
        }

        return _allMembers.Where(i => i.MemberType == type);
    }

    private string[] GetEnumValues(PropertyInfo? propertyInfo)
    {
        if (propertyInfo != null)
        {
            if (propertyInfo.PropertyType.IsEnum)
            {
                return propertyInfo.PropertyType.GetEnumNames();
            }
            else if (propertyInfo.PropertyType.GenericTypeArguments?.Length > 0 &&
                     propertyInfo.PropertyType.GenericTypeArguments[0].IsEnum)
            {
                return propertyInfo.PropertyType.GenericTypeArguments[0].GetEnumNames();
            }
        }

        return Array.Empty<string>();
    }

    private string RemoveExtraReferenceTags(string value)
    {
        return value.Replace("<see cref=\"", " ")
                    .Replace("<seealso cref=\"!:", " ")
                    .Replace("<seealso cref=\"P:", " ")
                    .Replace("<seealso cref=\"", " ")
                    .Replace("\"/>", " ")
                    .Replace("\" />", " ");
    }

    class MemberDescription
    {
        public MemberTypes MemberType { get; set; } = MemberTypes.Property;
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string[] EnumValues { get; set; } = Array.Empty<string>();
        public string[] Parameters { get; set; } = Array.Empty<string>();
        public string Default { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
