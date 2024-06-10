# Releasing a new version

For releasing a new version of any project in this repository to NuGet there are a few handy scripts that you can use.

The main script is the VNext script. It will take care of automatically determining the new version number, creating and pushing the git tag (which will start a NuGet build), updating the references in the other projects in the repository.

## VNext

> ⚠ This script will automatically create a tag and push it to git if successful. Make sure you are ready to release the new version before running this script.

The main script to use is `VNext.ps1`. It's a PowerShell script that you can run from the root of the repository. It will take care of everything for you given the right input.

### VNext - Parameters

- `Path`: The path to the project you are releasing e.g. `.\src\Libraries\Lombiq.HelpfulLibraries\`.
- `Type`: The type of the release. Can be `major`, `minor`, or `patch` (or can be omitted if doing a pre-release).
- `PreRelease`: Boolean flag to indicate if this is a pre-release.
- `Issue`: The issue number that this release is related to, is only used if it is a pre-release.
- `UpdateReferences`: Boolean flag to indicate if the references should be updated to this new version in the other projects in the repository.
- `Interactive`: Boolean flag to indicate if the script should ask for confirmation before pushing changes to git - defaults to true.

## VNext - Usage and examples

### Releasing a new major version

```shell
.\VNext.ps1 -path \path\to\project\ -type "major" -commitChanges $true
```

### Example release of a new major version

1. Open a PowerShell terminal.
2. Navigate to the root of the repository.
3. Run the script with the necessary parameters. For example to release a new major version of the `Lombiq.HelpfulLibraries` project and update the references in other projects, run the following command

    ```shell
    .\VNext.ps1 -path .\src\Libraries\Lombiq.HelpfulLibraries\ -type "major" -commitChanges $true
    ```

4. Review the changes made by the script while waiting for the NuGet build to finish.
5. Push the changes.
6. Do the same steps for any other projects that you want to release.

### Example release of a pre-release version

1. Open a PowerShell terminal.
2. Navigate to the root of the repository.
3. Run the script with the necessary parameters. For example to release a new major version of the `Lombiq.HelpfulLibraries` project and update the references in other projects, run the following command

    ```shell
    .\VNext.ps1 -path .\src\Libraries\Lombiq.HelpfulLibraries\ -prerelease $True -issue "OC-123"
    ```

## Update-References

This script will scan the entire solution for references to the project you want to update and update them to the new version.

The VNext script will update the references in the other projects in the repository if you provide the `UpdateReferences` flag. It will call this script for you.

Should you want to update the references manually you can use the `Update-References.ps1` script. It's a script that you can run from the root of the repository. It will take care of updating the references in the other projects for you.

### Update-References - Parameters

- `ProjectToFind`: The project to find.
- `NewVersion`: The new version to update to.

## Update-References - Usage and examples

### Updating references to a new version

```shell
.\Update-References.ps1 -ProjectToFind "Lombiq.HelpfulLibraries" -NewVersion "1.2.3"
```

## Get-Solution-Projects

This script will look for all projects in the solution file and look for references to these projects anywhere in the main solution, it will then call the `Update-References` script for each of the found items.

This script should not really be used on its own but is used by the `VNext` script in order to automatically update version numbers throughout the entire solution.

### Get-Solution-Projects - Parameters

- `SolutionFilePath`: The path to the solution file (of the project you are updating).
- `NewVersion`: The new version to update to.

### Get-Solution-Projects - Usage and examples

```shell
.\Get-Solution-Projects.ps1 -SolutionFilePath .\src\Libraries\Lombiq.HelpfulLibraries\ -NewVersion "1.2.3" 
```
