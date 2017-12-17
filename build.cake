#tool "nuget:?package=NUnit.Runners&version=2.6.2"

var target = Argument("target", "Default");
var nugetKey = Argument("nugetKey", "");

// Directories
var root = Directory(System.IO.Path.GetFullPath("./"));

var justCliSlnPath = root + File("src/JustCli.sln");
var justCliNuspecPath = root + File("src/core/JustCli/JustCli.nuspec");

var releaseFolder = root + Directory("src/core/JustCli/bin/Release");

var nugetOutput = root + Directory("nuget");
var nugetSource = nugetOutput + Directory("source");

Task("Default")
  .Does(() =>
{
  RunTarget("CreateNuget");
});

Task("Clean")
  .Does(() =>
{
  CleanDirectory(nugetOutput);
  CleanDirectory(releaseFolder);
});

Task("Restore-NuGet-Packages")
  .Does(() =>
{
  NuGetRestore(justCliSlnPath);
});

Task("Compile")
  .Description("Builds the solution")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() =>
{
  var settings = new MSBuildSettings {
    Configuration = "Release",
    ToolPath = "D:/Program Files (x86)/Microsoft Visual Studio/2017/Community/MSBuild/15.0/Bin/MSBuild.exe"
  };

  MSBuild(justCliSlnPath, settings);
});

Task("RunTests")
  .Description("Runs tests.")
  .IsDependentOn("Compile")
  .Does(() =>
{
  var assemblies = GetFiles("./src/core/JustCli.Tests/bin/Release/JustCli.Tests.dll");
  NUnit(assemblies, new NUnitSettings() { NoResults = true });
});

Task("CreateNugetFolder")
  .Description("Creates Nuget folder")
  .IsDependentOn("Compile")
  .IsDependentOn("RunTests")
  .Does(() =>
{
  CreateDirectory(nugetSource);
  CopyFile(File("readme.txt"), nugetSource + File("readme.txt"));
  CopyDirectory(releaseFolder + Directory("net4"), nugetSource + Directory("lib/net40"));
  CopyDirectory(releaseFolder + Directory("netstandard2.0"), nugetSource + Directory("lib/netstandard2.0"));
});

Task("CreateNuget")
  .Description("Creates Nuget")
  .IsDependentOn("CreateNugetFolder")
  .Does(() =>
{
  var releaseNotes = ParseReleaseNotes("RELEASE_NOTES.md");

  var nuGetPackSettings = new NuGetPackSettings {
    Version = releaseNotes.Version.ToString(),
    ReleaseNotes = releaseNotes.Notes.ToList(),
    OutputDirectory = nugetOutput + Directory("nuget"),
    BasePath = nugetSource,
    WorkingDirectory = nugetSource + Directory("lib")
  };

  NuGetPack(justCliNuspecPath, nuGetPackSettings);
});

Task("PublishNuget")
  .Description("Publish nuget")
  .IsDependentOn("CreateNuget")
  .Does(() => {
    var nugetPackagePath = new DirectoryInfo(nugetOutput + Directory("nuget")).GetFiles("*.nupkg").LastOrDefault().FullName;
    NuGetPush(nugetPackagePath, new NuGetPushSettings {
      Source = "https://www.nuget.org/api/v2/package",
      ApiKey = nugetKey
    });
  });

RunTarget(target);