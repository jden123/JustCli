// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"
open Fake
open Fake.ReleaseNotesHelper

RestorePackages()

// Properties
let binDir = "bin"
let buildDir = binDir @@ "build"
let testDir  = binDir @@ "test"
let nugetDir  = binDir @@ "nuget"
let nugetPackageDir  = binDir @@ "nugetBuild"

let releaseNotes = "./RELEASE_NOTES.md"
let release = LoadReleaseNotes releaseNotes

// Targets

//--------------------------------------------------------------------------------
// Build
//--------------------------------------------------------------------------------
Target "CleanApp" (fun _ ->
    CleanDir buildDir
)

Target "BuildApp" (fun _ ->
    !! "src/core/JustCli/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

//--------------------------------------------------------------------------------
// Test
//--------------------------------------------------------------------------------
Target "CleanTests" (fun _ ->
    CleanDir testDir
)

Target "BuildTests" (fun _ ->
    !! "src/core/JustCli.Tests/*.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
    !! (testDir + "/JustCli.Tests.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testDir + "/TestResults.xml" })
)

//--------------------------------------------------------------------------------
// Nuget
//--------------------------------------------------------------------------------
// Clean nuget directory
Target "CleanNuget" <| fun _ ->
    CleanDir nugetDir

Target "CleanNugetPackage" (fun _ ->
    CleanDir nugetPackageDir
)

Target "BuildNugetPackage" (fun _ ->
    !! "src/core/JustCli/*.csproj"
      |> MSBuildRelease (nugetPackageDir @@ "lib") "Build"
      |> Log "NugetLib-Output: "
)

Target "CreateNuget" (fun _ ->
   let nuspec = "src/core/JustCli/JustCli.nuspec"

   let nugetAccessKey = getBuildParamOrDefault "nugetkey" ""
   let nugetDoPublish = nugetAccessKey.Equals "" |> not
   let nugetPublishUrl = getBuildParamOrDefault "nugetserver" "https://nuget.org"

   let tags = ["CLI";"commandline";"console";"utility";"command";"tool";"arguments";"args";"parser"]

   //  Create/Publish the nuget package
   NuGet (fun app ->
         {app with
            Authors = ["jden123";]
            Project = "JustCli"
            Version = release.NugetVersion
            ReleaseNotes = release.Notes |> toLines
            Tags = tags |> String.concat " "
            AccessKey = nugetAccessKey
            Publish = nugetDoPublish
            PublishUrl = nugetPublishUrl
            OutputPath = nugetDir
            WorkingDir = nugetPackageDir
            SymbolPackage = NugetSymbolPackage.Nuspec
         }) 
         nuspec
)

//--------------------------------------------------------------------------------
// Help 
//--------------------------------------------------------------------------------
Target "Help" <| fun _ ->
    List.iter printfn [
      "usage:"
      "build [target]"
      ""
      " Targets for building:"
      " * BuildApp   Builds"
      " * Test       Runs tests"
      " * CreateNuget       Creates nuget package."
      ""
      " Other Targets"
      " * Help       Display this help" 
      ""]


// Dependencies
// build app
"CleanApp" ==> "BuildApp"

// run tests
"CleanTests" ==> "BuildTests" ==> "Test"

// nuget
"CleanNugetPackage" ==> "BuildNugetPackage" ==> "CleanNuget" ==> "CreateNuget"

// start build
RunTargetOrDefault "Help"