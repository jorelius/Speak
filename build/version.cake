#tool nuget:?package=GitVersion.CommandLine&version=3.6.5

Task("Version")
    .Does(() =>
{
    var version = GitVersion();
    Information($"Calculated semantic version {version.SemVer}");

    packageVersion = version.NuGetVersion;
    Information($"Corresponding package version {packageVersion}");

    if(!BuildSystem.IsLocalBuild)
    {
        GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.BuildServer,
            UpdateAssemblyInfo = true
        });
    }
});
