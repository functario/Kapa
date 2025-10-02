using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class MethodInfoExtensions
{
    public static ICollection<IKapaParam> ExtractKapaParams(this MethodInfo method)
    {
        ArgumentNullException.ThrowIfNull(method);
        var kapaParams = new List<IKapaParam>();
        var parameters = method.GetParameters();
        foreach (var param in parameters)
        {
            var paramAttr = param
                .GetCustomAttributes(typeof(KapaParamAttribute), inherit: true)
                .Cast<KapaParamAttribute>()
                .FirstOrDefault();

            if (paramAttr is not null)
            {
                kapaParams.Add(paramAttr.ToKapaParam(param));
            }
        }
        return kapaParams;
    }
}
