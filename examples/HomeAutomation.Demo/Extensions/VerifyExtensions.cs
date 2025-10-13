using System.Reflection;
using System.Runtime.CompilerServices;

namespace HomeAutomation.Demo.Extensions;

public static class VerifyExtensions
{
    [ModuleInitializer]
    public static void Initialize()
    {
        DerivePathInfo(
            (_, projectDirectory, type, method) =>
            {
                // Resolve path based on namespace because default sourcefile is based on
                // VerifySettings constructor called emplacement.
                var root = Path.GetFileName(projectDirectory.TrimEnd(Path.DirectorySeparatorChar));
                var typePath = type
                    .FullName?.Replace(root, "", StringComparison.OrdinalIgnoreCase)
                    .Replace(".", "/", StringComparison.OrdinalIgnoreCase)
                    .TrimStart('/')!;

                var directory = Path.Combine(projectDirectory, $"{typePath}_Snapshots");
                var filePrefix = method.Name;
                var fileName = GetMethodDisplayName(method);

                return new PathInfo(directory, filePrefix, fileName);
            }
        );
    }

    public static SettingsTask VerifyMermaidAsync(this string mermaid)
    {
        var verifySettings = GetVerifySettings();
        return Verify(mermaid, "mmd", verifySettings);
    }

    private static string GetMethodDisplayName(MethodInfo method)
    {
        string displayName;

        var testMethodAttribute = method
            .GetCustomAttributes()
            .FirstOrDefault(x =>
                x.GetType().Name == nameof(FactAttribute)
                || x.GetType().Name == nameof(TheoryAttribute)
            );

        if (testMethodAttribute is null)
        {
            displayName = method.Name;
        }
        else
        {
            var value =
                testMethodAttribute
                    .GetType()
                    .GetProperty("DisplayName", BindingFlags.Public | BindingFlags.Instance)
                    ?.GetValue(testMethodAttribute)
                    ?.ToString()
                ?? method.Name;

            displayName = value
                .Replace("'", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(",", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(" ", "_", StringComparison.OrdinalIgnoreCase);
        }

        return displayName;
    }

    private static VerifySettings GetVerifySettings()
    {
        var verifySettings = new VerifySettings();
        verifySettings.AddScrubber(x => x.Replace("\r\n", "\n"));
        verifySettings.UseStrictJson();
        return verifySettings;
    }
}
