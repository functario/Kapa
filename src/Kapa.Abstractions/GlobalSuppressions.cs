// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Naming",
    "CA1720:Identifier contains type name",
    Justification = "Type names in this enum represent JSON types"
        + " and including type names improves clarity and interoperability.",
    Scope = "type",
    Target = "~T:Kapa.Abstractions.Capabilities.ParameterTypes"
)]
