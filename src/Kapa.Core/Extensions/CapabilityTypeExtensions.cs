using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Exceptions;
using Kapa.Core.Capabilities;

namespace Kapa.Core.Extensions;

public static class CapabilityTypeExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="capabilityType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotCapabilityException"></exception>
    public static ICapabilityType ToCapabilityType(this Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);

        ThrowIfNotCapabilityType(capabilityType);

        var capability = GetCapabilities(capabilityType);

        return new CapabilityType(capability);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="capabilityType"></param>
    /// <returns></returns>
    /// <exception cref="MissingCapabilityException"></exception>
    /// <exception cref="TypeIsNotCapabilityException"></exception>
    public static ICollection<ICapability> GetCapabilities(this Type capabilityType)
    {
        ArgumentNullException.ThrowIfNull(capabilityType);

        ThrowIfNotCapabilityType(capabilityType);

        var capabilities = new List<ICapability>();
        var methods = capabilityType.GetMethods(
            BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.Public
                | BindingFlags.NonPublic
        );

        foreach (var method in methods)
        {
            if (method.ToCapability() is ICapability capability)
            {
                capabilities.Add(capability);
            }
        }

        ThrowIfMissingCapabilityException(capabilityType, capabilities.Count);

        ThrowIfDuplicatedDescriptions(capabilities, capabilityType);

        return capabilities;
    }

    public static bool IsCapabilityType(this Type type) =>
        type?.IsDefined(typeof(CapabilityTypeAttribute), inherit: true) ?? false;

    private static void ThrowIfMissingCapabilityException(Type capabilityType, int count)
    {
        if (count == 0)
        {
            throw new MissingCapabilityException(capabilityType);
        }
    }

    private static void ThrowIfDuplicatedDescriptions(
        ICollection<ICapability> capabilities,
        Type capabilityType
    )
    {
        var duplicateStrings = capabilities
            .GroupBy(x => x.Description) // Group elements by their value
            .Where(g => g.Count() > 1) // Filter groups that have more than one element (duplicates)
            .Select(g => g.Key) // Select the key (the string itself) from these groups
            .ToArray(); // Convert the result to a List<string>

        if (duplicateStrings.Length > 0)
        {
            throw new DuplicateDescriptionsException(
                capabilityType,
                typeof(ICapabilityType),
                duplicateStrings
            );
        }
    }

    private static void ThrowIfNotCapabilityType(this Type type)
    {
        if (!type.IsCapabilityType())
        {
            throw new TypeIsNotCapabilityException(
                type,
                $"The attribute '{nameof(CapabilityTypeAttribute)}' does not decorate the class."
            );
        }
    }
}
