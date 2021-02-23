#tool nuget:?package=vswhere
#tool "nuget:?package=NUnit.Runners&version=2.6.2"

#addin "Cake.FileHelpers"
#addin "Cake.Git"

var target = Argument("target", "Default");
var nugetKey = Argument("nugetKey", "");

// Directories
var root = Directory(System.IO.Path.GetFullPath("./"));

var justCliSlnPath = root + File("src/JustCli.sln");
var justCliNuspecPath = root + File("src/core/JustCli/JustCli.nuspec");

var releaseFolder = root + Directory("src/core/JustCli/bin/Release");

var nugetOutput = root + Directory("nuget");
var nugetSource = nugetOutput + Directory("source");

// variables
ReleaseNotes releaseNotes;
string version;

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

Task("ReadReleaseNotes")
  .Does(() => 
{
  releaseNotes = ParseReleaseNotes("RELEASE_NOTES.md");
  version = releaseNotes.Version.ToString();
});

Task("SetVersion")
  .IsDependentOn("ReadReleaseNotes")
  .Does(() => 
{
  if (releaseNotes == null)
  {
    throw new Exception("Release notes is empty. Call ReleaseNotes task first.");
  }

  Information("Version: " + version);

  ReplaceRegexInFiles("./src/core/JustCli/JustCli.csproj", 
    "<AssemblyVersion>(.+?)</AssemblyVersion>", 
    "<AssemblyVersion>" + version + "</AssemblyVersion>");

  ReplaceRegexInFiles("./src/core/JustCli/JustCli.csproj", 
    "<Version>(.+?)</Version>", 
    "<Version>" + version + "</Version>");
});

Task("Compile")
  .Description("Builds the solution")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() =>
{
  // find Microsoft.Component.MSBuild in all products and get latest
  var buildToolsInstallation =  VSWhereProducts(
      "*",
      new VSWhereProductSettings
      {
        ReturnProperty = "",
        Requires = "Microsoft.Component.MSBuild",
        ArgumentCustomization = args => args.Append("-latest").Append("-find MSBuild/**/Bin/MSBuild.exe")
      })
    .FirstOrDefault();

  if (buildToolsInstallation == null)
  {
    Error("Cannot find MSBuild.");
    throw new Exception("Cannot find MSBuild.");
  }

  var msbuildPath = File(buildToolsInstallation.ToString());
  Information($"MSBuild: {msbuildPath}");

  var settings = new MSBuildSettings {
    Configuration = "Release",
    ToolPath = msbuildPath
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
  .IsDependentOn("SetVersion")
  .IsDependentOn("Compile")
  .IsDependentOn("RunTests")
  .Does(() =>
{
  CreateDirectory(nugetSource);
  CopyFile(File("readme.txt"), nugetSource + File("readme.txt"));
  CopyDirectory(releaseFolder + Directory("net40"), nugetSource + Directory("lib/net40"));
  CopyDirectory(releaseFolder + Directory("net45"), nugetSource + Directory("lib/net45"));
  CopyDirectory(releaseFolder + Directory("netstandard2.0"), nugetSource + Directory("lib/netstandard2.0"));
});

Task("CreateNuget")
  .Description("Creates Nuget")
  .IsDependentOn("ReadReleaseNotes")
  .IsDependentOn("CreateNugetFolder")
  .Does(() =>
{
  if (releaseNotes == null)
  {
    throw new Exception("Release notes is empty. Call ReleaseNotes task first.");
  }

  var nuGetPackSettings = new NuGetPackSettings {
    Version = version,
    ReleaseNotes = releaseNotes.Notes.ToList(),
    OutputDirectory = nugetOutput + Directory("nuget"),
    BasePath = nugetSource,
    WorkingDirectory = nugetSource + Directory("lib")
  };

  NuGetPack(justCliNuspecPath, nuGetPackSettings);
});

Task("PublishNuget")
  .Description("Publish nuget")
  .IsDependentOn("ReadReleaseNotes")
  .IsDependentOn("CreateNuget")
  .Does(() => 
{
  var nugetPackagePath = new DirectoryInfo(nugetOutput + Directory("nuget")).GetFiles("*.nupkg").LastOrDefault().FullName;
  NuGetPush(nugetPackagePath, new NuGetPushSettings {
    Source = "https://www.nuget.org/api/v2/package",
    ApiKey = nugetKey
  });

  var versionTag = "v." + version;
  GitTag(".", versionTag);
});

RunTarget(target);