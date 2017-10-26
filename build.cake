#tool nuget:?package=xunit.runner.console&version=2.2.0

#load build/paths.cake
#load build/version.cake

var target = Argument("Target", "Build"); 
var configuration = Argument("Configuration", "Release");
var packageOutputPath = Argument("PackageOutputPath", "packages");

var packageVersion = "0.1.0";

Task("Restore")
    .Does(() =>
{
    NuGetRestore(Paths.SolutionFile);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild(
        Paths.SolutionFile, 
        settings => 
            { 
                settings.SetConfiguration(configuration)
                            .WithTarget("Build")
                            .WithProperty("BuildInParallel", "true");
                
                settings.MaxCpuCount = 0;
                            
            });
});

Task("Clean")
    .IsDependentOn("Remove-Packages")
    .Does(() =>
{
    DotNetBuild(
        Paths.SolutionFile, 
        settings => settings.SetConfiguration("Release")
                            .WithTarget("Clean"));

    DotNetBuild(
        Paths.SolutionFile, 
        settings => settings.SetConfiguration("Debug")
                            .WithTarget("Clean"));
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2($"**/bin/{configuration}/*Tests.dll");
});

Task("Remove-Packages")
    .Does(() =>
{
    CleanDirectory(packageOutputPath); 
});

Task("Package-NuGet")
    .IsDependentOn("Test")
    .IsDependentOn("Version")
    .IsDependentOn("Remove-Packages")
    .Does(() =>
{
    EnsureDirectoryExists(packageOutputPath);
    
    NuGetPack(
        Paths.NuspecFile, 
        new NuGetPackSettings
        {
            Version = packageVersion,
            OutputDirectory = packageOutputPath
        }
    );
});

Task("Package-Chocolatey")
    .IsDependentOn("Test")
    .IsDependentOn("Version")
    .IsDependentOn("Remove-Packages")
    .Does(() =>
{
    EnsureDirectoryExists(packageOutputPath);
    
    ChocolateyPack(
        Paths.NuspecFile,
        new ChocolateyPackSettings
        {
            Version = packageVersion,
            OutputDirectory = packageOutputPath
        }
    );
});

RunTarget(target);