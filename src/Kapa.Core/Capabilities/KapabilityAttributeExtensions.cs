using Kapa.Abstractions.Capabilities;

namespace Kapa.Core.Capabilities;

public static class KapabilityAttributeExtensions
{
    public static IKapability ToKapability(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsDefined(typeof(KapabilityAttribute), inherit: true))
            throw new InvalidOperationException(
                $"Type '{type.FullName}' does not have '{nameof(KapabilityAttribute)}'."
            );

        var kapaSteps = new List<IKapaStep>();
        var methods = type.GetMethods(
            System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.NonPublic
        );

        foreach (var method in methods)
        {
            var stepAttr = method
                .GetCustomAttributes(typeof(KapaStepAttribute), inherit: true)
                .Cast<KapaStepAttribute>()
                .FirstOrDefault();

            if (stepAttr is not null)
            {
                kapaSteps.Add(stepAttr.ToKapaStep(method));
            }
        }

        return new Kapability(kapaSteps);
    }
}
