using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Extensions;

namespace Kapa.Core.Capabilities;

[AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.GenericParameter,
    AllowMultiple = false
)]
public sealed class KapaParamAttribute : Attribute
{
    public KapaParamAttribute(string description, params Type[] rules)
    {
        Description = description;
        Rules = rules;
    }

    public string Description { get; }

    public IReadOnlyCollection<Type> Rules { get; }

    public IKapaParam ToKapaParam(ParameterInfo parameterInfo)
    {
        ArgumentNullException.ThrowIfNull(parameterInfo);
        ArgumentNullException.ThrowIfNull(parameterInfo.Name);

        var paramType = parameterInfo.ParameterType.InferKapaParamType();
        var rules = Rules.ToRules();
        return new KapaParam(parameterInfo.Name, Description, paramType, [.. rules]);
    }
}
