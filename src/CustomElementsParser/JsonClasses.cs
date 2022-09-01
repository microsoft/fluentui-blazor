namespace CustomElementsParser;

#nullable disable

public class CustomElements
{
    public string schemaVersion { get; set; }
    public string readme { get; set; }
    public Module[] modules { get; set; }
}

public class Module
{
    public string kind { get; set; }
    public string path { get; set; }
    public Declaration[] declarations { get; set; }
    public Export[] exports { get; set; }
}

public class Declaration
{
    public string kind { get; set; }
    public string description { get; set; }
    public string name { get; set; }
    public Csspart[] cssParts { get; set; }
    public Slot[] slots { get; set; }
    public Member[] members { get; set; }
    public Attribute[] attributes { get; set; }
    public Superclass superclass { get; set; }
    public bool customElement { get; set; }
    public Type type { get; set; }
    public string _default { get; set; }
    public string privacy { get; set; }
    public Event[] events { get; set; }
    public Mixin[] mixins { get; set; }
    public Return _return { get; set; }
    public Parameter1[] parameters { get; set; }
}

public class Superclass
{
    public string name { get; set; }
    public string package { get; set; }
    public string module { get; set; }
}

public class Type
{
    public string text { get; set; }
}

public class Return
{
    public Type1 type { get; set; }
}

public class Type1
{
    public string text { get; set; }
}

public class Csspart
{
    public string description { get; set; }
    public string name { get; set; }
}

public class Slot
{
    public string description { get; set; }
    public string name { get; set; }
}

public class Member
{
    public string kind { get; set; }
    public string name { get; set; }
    public Type2 type { get; set; }
    public string privacy { get; set; }
    public string description { get; set; }
    public string _default { get; set; }
    public Return1 _return { get; set; }
    public Parameter[] parameters { get; set; }
    public bool _static { get; set; }
    public Inheritedfrom inheritedFrom { get; set; }
}

public class Type2
{
    public string text { get; set; }
}

public class Return1
{
    public Type3 type { get; set; }
}

public class Type3
{
    public string text { get; set; }
}

public class Inheritedfrom
{
    public string name { get; set; }
    public string module { get; set; }
}

public class Parameter
{
    public string name { get; set; }
    public Type4 type { get; set; }
    public string _default { get; set; }
    public string description { get; set; }
    public bool optional { get; set; }
}

public class Type4
{
    public string text { get; set; }
}

public class Attribute
{
    public string name { get; set; }
    public Type5 type { get; set; }
    public string description { get; set; }
    public string fieldName { get; set; }
    public string _default { get; set; }
    public Inheritedfrom1 inheritedFrom { get; set; }
}

public class Type5
{
    public string text { get; set; }
}

public class Inheritedfrom1
{
    public string name { get; set; }
    public string module { get; set; }
}

public class Event
{
    public string description { get; set; }
    public string name { get; set; }
}

public class Mixin
{
    public string name { get; set; }
    public string module { get; set; }
}

public class Parameter1
{
    public string name { get; set; }
    public Type6 type { get; set; }
    public string description { get; set; }
    public bool optional { get; set; }
}

public class Type6
{
    public string text { get; set; }
}

public class Export
{
    public string kind { get; set; }
    public string name { get; set; }
    public Declaration1 declaration { get; set; }
}

public class Declaration1
{
    public string name { get; set; }
    public string module { get; set; }
}
