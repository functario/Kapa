using System.Reflection;
using Kapa.Abstractions;
using Kapa.Abstractions.Extensions;

namespace Kapa.Core.Extensions;

public static class OutcomeValueTypeExtensions
{
    public static IValueInfo GetOutcomeValueInfo(this Type valueType)
    {
        throw new NotImplementedException();
    }

    // DOTO: A remettre dans MethodInfoExtensions
    public static Dictionary<string, Kinds[]> GetGenericTypes(MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        var returnType = method.ReturnParameter.ParameterType;
        Dictionary<string, Kinds[]> innerTypes = [];
        if (returnType.IsGenericType)
        {
            var genericArguments = returnType.GetGenericArguments();
            // genericArguments will contain Ok<string>, Fail<string>, RulesFail<string> types
            foreach (var arg in genericArguments)
            {
                // For each arg, you can inspect its generic type definition and its type arguments
                var typeName = arg.Name; // e.g., Ok`1
                var innerTypeArguments = arg.IsGenericType
                    ? arg.GetGenericArguments()
                    : Type.EmptyTypes;

                innerTypes.Add(typeName, [.. innerTypeArguments.Select(x => x.InferKind())]);
            }
        }

        return innerTypes;
    }
}
