namespace Kapa.Abstractions.Actors;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1040:Avoid empty interfaces",
    Justification = $"Discriminant to indentify {nameof(IActor)} that are created from attribute."
)]
public interface IGeneratedActor { }
