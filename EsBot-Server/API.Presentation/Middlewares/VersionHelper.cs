using System.Reflection;
using System.Text.RegularExpressions;

namespace API.Presentation.Middlewares;

public class VersionHelper
{
    public static (string Major, string Minor, string Patch, string CommitHash) GetAppVersion()
    {
        var version = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        if (string.IsNullOrWhiteSpace(version))
            return ("unknown", "unknown", "unknown", "unknown");

        // Extract major.minor.patch
        var versionMatch = Regex.Match(version, @"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)");
        string major = versionMatch.Success ? versionMatch.Groups["major"].Value : "unknown";
        string minor = versionMatch.Success ? versionMatch.Groups["minor"].Value : "unknown";
        string patch = versionMatch.Success ? versionMatch.Groups["patch"].Value : "unknown";

        // Extract commit hash after '+'
        var commitMatch = Regex.Match(version, @"\+([a-f0-9]+)", RegexOptions.IgnoreCase);
        string commitHash = commitMatch.Success ? commitMatch.Groups[1].Value : "unknown";

        return (major, minor, patch, commitHash);
    }
}
