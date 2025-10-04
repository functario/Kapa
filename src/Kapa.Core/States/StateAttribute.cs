namespace Kapa.Core.States;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class StateAttribute : Attribute
{
    public StateAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}
