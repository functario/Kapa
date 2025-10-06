using System.Diagnostics;
using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Prototypes;
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
        var source = !string.IsNullOrWhiteSpace(capabilityAttribute.Source)
            ? capabilityAttribute.Source
            : method.InferSourceName();

        var relations = method.GetRelations();

        ThrowIfSourceNotFound(source, method);

        var outcomeMetadata = new OutcomeMetadata(source, valueInfo, outcomeTypes);

        if (parameters.Count > 0)
        {
            return new Capability(
                method.Name,
                capabilityAttribute.Description,
                outcomeMetadata,
                relations,
                [.. parameters]
            );
        }

        return new Capability(
            method.Name,
            capabilityAttribute.Description,
            outcomeMetadata,
            relations
        );
    }

    public static Attribute? GetRelationsAttribute(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        if (
            method
                .GetCustomAttributes(false)
                .FirstOrDefault(a =>
                    a.GetType().IsGenericType
                    && a.GetType().GetGenericTypeDefinition() == typeof(RelationsAttribute<>)
                )
            is not Attribute attr
        )
        {
            return null;
        }

        return attr;
    }

    public static IPrototypeRelations<IHasTrait>? GetRelations(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        var relationsAttributes = method.GetRelationsAttribute();

        // Trick to avoid using a constant that will not be attached to the RelationsAttribute.
        var relationString = nameof(RelationsAttribute<PrototypeRelations>.Relations);
        var relationsProperty = relationsAttributes?.GetType().GetProperty(relationString);

        if (relationsProperty is null)
        {
            return null;
        }

        if (
            relationsProperty.GetValue(relationsAttributes)
            is not IPrototypeRelations<IHasTrait> relations
        )
        {
            return null;
        }

        return relations;
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

    private static void ThrowIfSourceNotFound(string? source, MethodInfo method)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            // This should not happen since the source is actually resolve by reflection.
            // But the source is the discriminant to identify ICapability,
            // Therefor, the validation is kept in place to reduce risk of silent breaking change.
            throw new UnreachableException(
                $"{nameof(IOutcomeMetadata.Source)} not found for method '{method.Name}'"
                    + $" from type '{method.DeclaringType?.FullName}'."
            );
        }
    }
}
