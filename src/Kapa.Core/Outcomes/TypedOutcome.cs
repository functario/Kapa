using System.Text.Json.Serialization;
using Kapa.Abstractions;
using Kapa.Abstractions.Outcomes;

namespace Kapa.Core.Outcomes;

public record TypedOutcome<TKind> : Outcome, ITypedOutcome<TKind>
    where TKind : IKinds
{
    public TypedOutcome(string source, OutcomeStatus status, TKind kind, object? value)
        : base(source, status)
    {
        Kind = kind;
        Value = value;
    }

    [JsonIgnore]
    public TKind Kind { get; init; }

    [JsonIgnore]
    public object? Value { get; init; }
}
