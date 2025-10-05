using Kapa.Abstractions;
using Kapa.Abstractions.Validations;
using Kapa.Core.Validations;

namespace Kapa.Core.Extensions;

internal static class OutcomeTypeExtensions
{
    public static OutcomeTypes InferOutcomeTypes(this Type outcomeType)
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

            // Check if the type implements IOutcomes
            if (typeof(IOutcomes).IsAssignableFrom(outcomeType))
            {
                var args = outcomeType.GetGenericArguments();
                var result = OutcomeTypes.Generic | OutcomeTypes.Union;
                foreach (var arg in args)
                {
                    if (typeof(IOutcome).IsAssignableFrom(arg))
                    {
                        result |= arg.InferOutcomeTypes();
                    }
                }

                return result;
            }
        }

        return OutcomeTypes.None;
    }
}
