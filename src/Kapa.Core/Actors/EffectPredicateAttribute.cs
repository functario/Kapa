namespace Kapa.Core.Actors;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class EffectPredicateAttribute : Attribute
{
    public EffectPredicateAttribute(string id, string description)
    {
        Id = id;
        Description = description;
    }

    public string Id { get; }
    public string Description { get; }
}
