// https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web.JS/src/Platform/WebStartOptions.ts
interface WebStartOptions {
  enableClassicInitializers?: boolean;
  circuit: any; // CircuitStartOptions
  webAssembly: any; // WebAssemblyStartOptions
  logLevel?: LogLevel;
  ssr: any; // SsrStartOptions
}

enum LogLevel {
  /** Log level for very low severity diagnostic messages. */
  Trace = 0,
  /** Log level for low severity diagnostic messages. */
  Debug = 1,
  /** Log level for informational diagnostic messages. */
  Information = 2,
  /** Log level for diagnostic messages that indicate a non-fatal problem. */
  Warning = 3,
  /** Log level for diagnostic messages that indicate a failure in the current operation. */
  Error = 4,
  /** Log level for diagnostic messages that indicate a failure that will terminate the entire application. */
  Critical = 5,
  /** The highest possible log level. Used when configuring logging to indicate that no log messages should be emitted. */
  None = 6,
}
