using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public record KapaStep(string Name, string Description) : IKapaStep { }
