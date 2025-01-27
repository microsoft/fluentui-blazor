// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bunit;
using static Microsoft.FluentUI.AspNetCore.Components.Tests.Samples.Icons;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

internal static class BaseHelpers
{
    /// <summary>
    /// Gets the derived types from the specified base type.
    /// </summary>
    public static IEnumerable<Type> GetDerivedTypes<T>(IEnumerable<Type> except) => GetDerivedTypes(typeof(T), except);

    /// <summary>
    /// Gets the derived types from the specified base type.
    /// </summary>
    public static IEnumerable<Type> GetDerivedTypes(Type baseType, IEnumerable<Type> except)
    {
        var assembly = typeof(AspNetCore.Components._Imports).Assembly;

        return assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && IsSubclassOfGenericType(t, baseType) && !except.Contains(t));
    }

    public static bool ContainsAttribute(this string markup, string htmlName, object value)
    {
        // The pattern to match the "attribute"="value" in the markup.
        var pattern = @$"\b{htmlName}\s*=\s*[""'][^""']*\b{value}\b[^""']*[""']";

        // For boolean attributes, the value is not required.
        if (value is bool && (bool)value == true)
        {
            pattern = @$"\b{htmlName}\b";
        }

        return Regex.IsMatch(markup, pattern);

    }

    /// <summary>
    /// Check if the type is a subclass of the generic type (direct or indirect)
    /// </summary>
    private static bool IsSubclassOfGenericType(Type? type, Type genericType)
    {
        while (type != null && type != typeof(object))
        {
            // Interface
            if (genericType.IsInterface)
            {
                foreach (var item in type.GetInterfaces())
                {
                    if (item == genericType)
                    {
                        return true;
                    }
                }
            }

            // Class
            var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (cur == genericType)
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;
    }
}
