namespace Kapa.Core.States;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class TraitAttribute : Attribute
{
    public TraitAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
