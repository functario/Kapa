using Kapa.Abstractions.Actors;

namespace Kapa.Core.Capabilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class RelationsAttribute<TRelations> : Attribute
    where TRelations : IRelations<IGeneratedActor>, new()
{
    public RelationsAttribute()
    {
        var type = typeof(TRelations);
        if (Activator.CreateInstance(type) is TRelations instance)
        {
            Relations = instance;
        }
        else
        {
            throw new InvalidCastException();
        }
    }

    public TRelations Relations { get; }
}
