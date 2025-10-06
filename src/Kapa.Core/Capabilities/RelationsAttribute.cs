using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class RelationsAttribute<TPrototypeRelations> : Attribute
    where TPrototypeRelations : IPrototypeRelations<IGeneratedPrototype>, new()
{
    public RelationsAttribute()
    {
        var type = typeof(TPrototypeRelations);
        if (Activator.CreateInstance(type) is TPrototypeRelations instance)
        {
            Relations = instance;
        }
        else
        {
            throw new InvalidCastException();
        }
    }

    public TPrototypeRelations Relations { get; }
}
