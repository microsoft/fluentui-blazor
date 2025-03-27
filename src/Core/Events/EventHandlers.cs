// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/* Blazor supports custom event arguments, which enable you to pass arbitrary data to .NET event handlers with custom events.
 * https://learn.microsoft.com/aspnet/core/blazor/components/event-handling#custom-event-arguments
 * 
 *  In the Components C# project
 *  ----------------------------
 *    1. In this `Events` folder, create a class that derives from `EventArgs`.                            Ex. DialogToggleEventArgs.cs
 *    2. Add the `EventHandler` attribute (prefixed by "on") to the `EventHandlers` class in this file.    Ex. "ondialogtoggle"
 *  
 *  In the Components.Scripts project
 *  ---------------------------------
 *    3. Define a registering method the Custom Event Type in the `FluentUICustomEvents.ts` file.          Ex. "dialogtoggle"
 *    4. Call the Registering method in the `Startup.ts` file.
 *    
 *  In the C# component
 *  -------------------
 *    5. Use this new event in the component: `@ondialogtoggle="@(e => ...)"`
 */

/// <summary>
/// List of custom events to associate an event argument type with an event attribute name.
/// </summary>
[EventHandler("ondialogbeforetoggle", typeof(DialogToggleEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ondialogtoggle", typeof(DialogToggleEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("ondropdownchange", typeof(DropdownEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
}
