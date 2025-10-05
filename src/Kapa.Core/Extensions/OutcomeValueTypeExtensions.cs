using Kapa.Abstractions;
using Kapa.Abstractions.Extensions;

namespace Kapa.Core.Extensions;

public static class OutcomeValueTypeExtensions
{
    public static IValueInfo GetValueInfo(this Type valueType)
    {
        ArgumentNullException.ThrowIfNull(valueType);
        var fullName = valueType.FullName ?? "";
        var kind = valueType.InferKind();
        if (valueType.IsGenericType)
        {
            var genericArguments = valueType.GetGenericArguments();
            List<IValueInfo> argumentsValueInfos = [];

            foreach (var arg in genericArguments)
            {
                var typeName = arg.Name;
                var innerTypeArguments = arg.IsGenericType
                    ? arg.GetGenericArguments()
                    : Type.EmptyTypes;

                // Add exception to stop before stackoverflow
                foreach (var innerType in innerTypeArguments)
                {
                    var innerTypeValueInfo = innerType.GetValueInfo();
                    argumentsValueInfos.Add(innerTypeValueInfo);
                }
            }

            return new ValueInfo(kind, fullName, true, argumentsValueInfos);
        }

        return new ValueInfo(kind, fullName, false, []);
    }
}
