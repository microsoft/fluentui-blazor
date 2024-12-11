// See https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/Rendering/Events/EventTypes.ts

interface EventTypeOptions {
  browserEventName?: string;
  createEventArgs?: (event: EventType) => unknown;
}
