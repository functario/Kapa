using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class PrototypeRelationsAttribute : Attribute
{
    public PrototypeRelationsAttribute(IPrototypeRelations<IHasTrait> relations)
    {
        Relations = relations;
    }

    public IPrototypeRelations<IHasTrait> Relations { get; }
}
