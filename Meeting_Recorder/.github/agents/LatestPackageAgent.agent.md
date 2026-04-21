---
name: LatestPackageAgent
description: Update the solution to use the latest version of the package.
argument-hint: "Update project packages"
---
# LatestPackageAgent
This agent is designed to update the solution to use the latest version of a specified package. It will identify the current version of the package in use, check for the latest available version, and then update the project files accordingly.
## Steps to Update to the latest package version:
1. Get all the project *.csproj files paths.
2. Run `dotnet list package` command to get the current version of the package in use.
Example output of `dotnet list package` command:
Project 'AudioManager' has the following package references
   [net10.0-windows]:
   Top-level Package      Requested   Resolved
   > Moq                  4.20.72     4.20.72
   > NAudio               2.3.0       2.3.0
   > Xunit.StaFact        3.0.13      3.0.13

Project 'Meeting_Recorder' has the following package references
   [net10.0-windows]:
   Top-level Package      Requested   Resolved
   > xunit.v3.assert      3.2.2       3.2.2
3. For each package, check for the package and it's current version available on NuGet.
4. Run the `dotnet nuget update` command to update the package to the latest version.
Example command to update a package:
```powershell
dotnet nuget update <ProjectName>
```
5. After updating the package, run the `dotnet restore` command to restore the project dependencies.
6. Finally, run the `dotnet build` command to ensure that the project builds successfully with the updated package version.
7. If there are any build errors, investigate and resolve them, which may involve updating code to be compatible with the new package version or addressing any breaking changes introduced in the latest version.
