using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Extensions;

namespace Kapa.Core.Capabilities;

[AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.GenericParameter,
    AllowMultiple = false
)]
public sealed class ParameterAttribute : Attribute
{
    public ParameterAttribute(string description, params Type[] rules)
    {
        Description = description;
        Rules = rules;
    }

    public string Description { get; }

    public IReadOnlyCollection<Type> Rules { get; }

    public IParameter ToParameter(ParameterInfo parameterInfo)
    {
        ArgumentNullException.ThrowIfNull(parameterInfo);
        ArgumentNullException.ThrowIfNull(parameterInfo.Name);

        var paramType = parameterInfo.ParameterType.InferKind();
        var rules = Rules.ToRules();
        return new Parameter(parameterInfo.Name, Description, paramType, [.. rules]);
    }
}
