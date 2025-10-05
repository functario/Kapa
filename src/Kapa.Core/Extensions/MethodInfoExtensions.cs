using System.Reflection;
using Kapa.Abstractions;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Validations;
using Kapa.Core.Capabilities;
using Kapa.Core.Validations;

namespace Kapa.Core.Extensions;

internal static class MethodInfoExtensions
{
    public static CapabilityAttribute? GetCapabilityAttribute(this MethodInfo method) =>
        method
            ?.GetCustomAttributes(typeof(CapabilityAttribute), inherit: true)
            .Cast<CapabilityAttribute>()
            .FirstOrDefault();

    public static ICapability? ToCapability(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);

        var capabilityAttribute = method.GetCapabilityAttribute();
        if (capabilityAttribute is null)
        {
            return null;
        }

        var parameters = GetParameters(method);
        var returnValueType = method.ReturnParameter.ParameterType;
        var valueInfo = returnValueType.GetValueInfo();
        var outcomeTypes = returnValueType.InferOutcomeTypes();
        var outcomeMetadata = new OutcomeMetadata(method.Name, valueInfo, outcomeTypes);

        if (parameters.Count > 0)
        {
            return new Capability(
                method.Name,
                capabilityAttribute.Description,
                outcomeMetadata,
                [.. parameters]
            );
        }

        return new Capability(method.Name, capabilityAttribute.Description, outcomeMetadata);
    }

    private static OutcomeTypes InferOutcomeTypes(this Type outcomeType)
    {
        // Handle non-generic types
        if (outcomeType == typeof(Ok))
            return OutcomeTypes.Ok;

        if (outcomeType == typeof(Fail))
            return OutcomeTypes.Fail;

        if (outcomeType == typeof(RulesFail))
            return OutcomeTypes.RulesFail;

        // Handle generic types
        if (outcomeType.IsGenericType)
        {
            var genericDef = outcomeType.GetGenericTypeDefinition();
            if (genericDef == typeof(Ok<>))
                return OutcomeTypes.Generic | OutcomeTypes.Ok;

            if (genericDef == typeof(Fail<>))
                return OutcomeTypes.Generic | OutcomeTypes.Fail;

            if (genericDef == typeof(RulesFail<>))
                return OutcomeTypes.Generic | OutcomeTypes.RulesFail;

            if (genericDef == typeof(Outcomes<,>) || genericDef == typeof(Outcomes<,,>))
            {
                var args = outcomeType.GetGenericArguments();
                var result = OutcomeTypes.None;
                foreach (var arg in args)
                {
                    if (typeof(IOutcome).IsAssignableFrom(arg))
                    {
                        // TODO:  To improve since as any recursive call, this could stack overflow
                        result |= arg.InferOutcomeTypes();
                    }
                }

                return result;
            }
        }

        return OutcomeTypes.None;
    }

    public static ICollection<IParameter> GetParameters(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        var capabilityParameters = new List<IParameter>();
        var methodParameters = method.GetParameters();
        foreach (var param in methodParameters)
        {
            var paramAttr = param
                .GetCustomAttributes(typeof(ParameterAttribute), inherit: true)
                .Cast<ParameterAttribute>()
                .FirstOrDefault();

            if (paramAttr is not null)
            {
                capabilityParameters.Add(paramAttr.ToParameter(param));
            }
        }

        return capabilityParameters;
    }
}
