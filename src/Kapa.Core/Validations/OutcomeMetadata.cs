using Kapa.Abstractions;
using Kapa.Abstractions.Validations;

namespace Kapa.Core.Validations;

public sealed record OutcomeMetadata(string Source, IValueInfo ValueInfo, OutcomeTypes OutcomeType)
    : IOutcomeMetadata
{ }
