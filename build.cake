#addin "Newtonsoft.Json"
#tool "nuget:?package=GitVersion.CommandLine"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var solutionDir = Directory("./");
var solutionFile = solutionDir + File("YesSensu.sln");
var globalFile = solutionDir + File("global.json");
var sensuCoreProjDir = "./src/YesSensu.Core/";
var sensuCoreBuildDir = Directory(sensuProjDir + "bin") + Directory(configuration);
var sensuProjDir = "./src/YesSensu/";
var sensuBuildDir = Directory(sensuProjDir + "bin") + Directory(configuration);
var sensuEnrichersProjDir = "./src/YesSensu.Enrichers/";
var sensuEnrichersBuildDir = Directory(sensuProjDir + "bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(sensuBuildDir);
});


Task("Restore")
    .Does(() =>
{    
    DotNetCoreRestore();
});


Task("Update-Version")
    .Does(() => 
{
    var jsonFile = sensuProjDir + "project.json";
    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true});
    string version = GitVersion().FullSemVer;
    Console.WriteLine(version);

    var project = Newtonsoft.Json.Linq.JObject.Parse(
        System.IO.File.ReadAllText(jsonFile, Encoding.UTF8));

    project["version"].Replace(version);

    System.IO.File.WriteAllText(jsonFile, project.ToString(), Encoding.UTF8);
}).OnError(exception =>
{
    Console.WriteLine(exception.ToString());
});


Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Update-Version")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration));
    }
});


Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .Does(() =>
{ });


Task("Package")
    .Does(() => 
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./artifacts/"
    };

    DotNetCorePack(sensuCoreProjDir + "project.json", settings);
    DotNetCorePack(sensuProjDir + "project.json", settings);
    DotNetCorePack(sensuEnrichersProjDir + "project.json", settings);
});


Task("Test")
    .Does(() =>
{
    var testProjects = GetFiles("./test/**/project.json");
    Console.WriteLine("Test Projects: " + testProjects.Count());
    foreach(var testProj in testProjects)
    {
        Console.WriteLine(testProj.FullPath);
        DotNetCoreTest(testProj.FullPath);
    }
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Rebuild")
    .IsDependentOn("Test")
    .IsDependentOn("Package");
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);