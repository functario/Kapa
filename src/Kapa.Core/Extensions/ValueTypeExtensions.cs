using Kapa.Abstractions;
using Kapa.Abstractions.Extensions;

namespace Kapa.Core.Extensions;

public static class ValueTypeExtensions
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
                var innerTypeArguments = arg.IsGenericType
                    ? arg.GetGenericArguments()
                    : Type.EmptyTypes;

                List<IValueInfo> innerArgumentsValueInfos = [];

                // TODO:  To improve since as any recursive call, this could stack overflow
                foreach (var innerType in innerTypeArguments)
                {
                    var innerTypeValueInfo = innerType.GetValueInfo();
                    innerArgumentsValueInfos.Add(innerTypeValueInfo);
                }

                var argValueInfo = arg.GetValueInfo();
                argumentsValueInfos.Add(argValueInfo);
            }

            return new ValueInfo(kind, fullName, true, argumentsValueInfos);
        }

        return new ValueInfo(kind, fullName, false, []);
    }
}
