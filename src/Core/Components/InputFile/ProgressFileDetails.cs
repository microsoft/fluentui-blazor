// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the details of a file's progress in a process, including its index, name, and completion percentage.
/// </summary>
/// <param name="Index">The zero-based index of the file in the process.</param>
/// <param name="Name">The name of the file being processed.</param>
/// <param name="Percentage">The completion percentage of the file's progress. Must be a value between 0 and 100, inclusive.</param>
public record struct ProgressFileDetails(int Index, string Name, int Percentage);

