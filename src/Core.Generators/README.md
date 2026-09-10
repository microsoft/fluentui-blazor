# Enum Attribute Values

Enum attribute generation is opt-in. Register each enum on an internal static
partial context, using `EnumAttributeValuesAttribute` supplied by the generator:

```csharp
using Microsoft.FluentUI.AspNetCore.Components.Generators;

namespace Microsoft.FluentUI.AspNetCore.Components.Extensions;

[EnumAttributeValues(typeof(Color))]
[EnumAttributeValues(typeof(Orientation))]
internal static partial class GeneratedEnumExtensions
{
}
```

Core and Charts each maintain their registrations in
`Extensions/GeneratedEnumExtensions.cs`. Types from referenced assemblies must
also be registered explicitly. The generator never scans assemblies or
automatically registers enum declarations.

For each registration, the generator adds concrete `GetDescription` and
`ToAttributeValue` extension overloads to the context, including nullable,
`isNull`, and `returnEmptyAsNull` variants. They read `DescriptionAttribute` at
build time and use typed switch expressions at runtime. There is no generic
dispatcher, boxing, reflection-based attribute lookup, runtime name lookup,
or runtime dictionary.

Import the context's namespace at the call site (including Razor imports).
Existing calls such as `Color.Primary.ToAttributeValue()` then select the
concrete overload instead of converting to `Enum`. Diagnostic `FUIBGEN001`
reports typed calls that still select the reflection API, so missing
registrations or imports fail the library build. Calls whose receiver is only
known as `Enum` retain the public reflection-based fallback.

Descriptions are emitted verbatim, including empty strings. Fields without a
description use their invariant lowercase name. Undefined values and unnamed
flags combinations return an empty string, matching `EnumExtensions`. Named
flags combinations use the matching field's description; individual flag
descriptions are not combined.

Limitation: aliases sharing a numeric value use the first declared member's
description (or its lowercase name when no description is present). If aliases
have different descriptions, the result may differ from the reflection-based
implementation because .NET does not guarantee which alias its name lookup
selects. Aliases with identical descriptions are unaffected. Generated overloads
do not call `ToString()` or `Enum.GetName`. `GetDisplay` and `IsObsolete` are not
changed.

The generator is a build-only project reference; it is not shipped as a runtime
dependency of the component libraries. Tests cover explicit opt-in, imported
enum types, concrete overloads, constants, description literals, and the
registration diagnostic. Malformed registrations are skipped so they do not
prevent generation for valid registrations. Core and Charts tests also compare
all registered enum values against the reflection implementation. Runtime tests
cover all four flags overloads, including aliases with identical descriptions
and named/unnamed combinations. Separate tests cover the conflicting-alias
limitation.