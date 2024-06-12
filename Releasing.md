# Releasing a new version

We have a few handy scripts you can use to release a new NuGet package version of any project in this repository.

The main script is the Publish-VNext script. It will take care of automatically determining the new version number, creating and pushing the git tag (which will start a NuGet build), updating the references in the other projects in the repository.

## Publish-VNext

The main script to use is `Publish-VNext.ps1`. It's a PowerShell script that you can run from the root of the repository. It will take care of everything for you given the right input.

### Publish-VNext - Parameters

- `Path`: The path to the project you are releasing e.g. `.\src\Libraries\Lombiq.HelpfulLibraries\`.
- `Type`: The type of the release. Can be `major`, `minor`, or `patch` (or can be omitted if doing a pre-release).
- `PreRelease`: The flag to indicate if this is a pre-release.
- `Issue`: The issue number that this release is related to, is only used if it is a pre-release.
- `UpdateReferences`: The flag to indicate if the references should be updated to this new version in the other projects in the repository.
- `NonInteractive`: The flag to indicate if the script should skip asking for confirmation before pushing changes to git.

## Publish-VNext - Usage and examples

### Releasing a new major version

```pwsh
.\Publish-VNext.ps1 -Path \path\to\project\ -Type "major" -UpdateReferences
```

### Example release of a new major version

1. Open a PowerShell terminal.
2. Navigate to the root of the repository.
3. Run the script with the necessary parameters. For example to release a new major version of the `Lombiq.HelpfulLibraries` project and update the references in other projects, run the following command

    ```pwsh
    .\Publish-VNext.ps1 -Path .\src\Libraries\Lombiq.HelpfulLibraries\ -Type "major" -UpdateReferences
    ```

4. Review the changes made by the script while waiting for the NuGet build to finish.
5. Push the changes.
6. Do the same steps for any other projects that you want to release.

### Example release of a pre-release version

1. Open a PowerShell terminal.
2. Navigate to the root of the repository.
3. Run the script with the necessary parameters. For example to release a new major version of the `Lombiq.HelpfulLibraries` project and update the references in other projects, run the following command

    ```pwsh
    .\Publish-VNext.ps1 -Path .\src\Libraries\Lombiq.HelpfulLibraries\ -Prerelease -Issue "OC-123"
    ```

## Update-References

This script will scan the entire solution for references to the project you want to update and update them to the new version.

The Publish-VNext script will update the references in the other projects in the repository if you provide the `UpdateReferences` flag. It will call this script for you.

Should you want to update the references manually you can use the `Update-References.ps1` script. It's a script that you can run from the root of the repository. It will take care of updating the references in the other projects for you.

### Update-References - Parameters

- `ProjectToFind`: The project to find.
- `NewVersion`: The new version to update to.

## Update-References - Usage and examples

### Updating references to a new version

```pwsh
.\Update-References.ps1 -ProjectToFind "Lombiq.HelpfulLibraries" -NewVersion "1.2.3"
```
