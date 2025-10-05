namespace Kapa.Abstractions.Validations;

public interface IOutcomes : IOutcome { }

public interface IOutcomes<TOutcome1, TOutcome2> : IOutcomes
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
{ }

public interface IOutcomes<TOutcome1, TOutcome2, TOutcome3> : IOutcomes
    where TOutcome1 : IOutcome
    where TOutcome2 : IOutcome
    where TOutcome3 : IOutcome
{ }
