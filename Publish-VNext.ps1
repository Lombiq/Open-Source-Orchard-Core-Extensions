<#
.SYNOPSIS
    This script publishes a new version or pre-release version of a specified project.

.DESCRIPTION
    The script gets the latest release tags, checks them for a valid version number, and determines the next version or
    pre-release version.
    It then (optionally) prompts the user for confirmation before creating and pushing the new tag to git.
    If the UpdateReferences flag is provided, it calls the Update-References.ps1 script to update the version number in
    all projects.

.PARAMETER Path
    The path to the project to be released.

.PARAMETER Type
    The type of the release. It can be 'major', 'minor', or 'patch'.

.PARAMETER Issue
    The issue number. This is required for pre-releases.

.PARAMETER PreRelease
    A flag indicating whether the release is a pre-release.

.PARAMETER UpdateReferences
    A flag indicating whether to update the references in other projects.

.PARAMETER NonInteractive
    A flag indicating whether to omit prompting the user for confirmation before creating and pushing the new tag.

.EXAMPLE
    .\Publish-VNext.ps1 -Path .\src\Libraries\Lombiq.HelpfulLibraries\ -PreRelease -Issue "OC-123"
    -UpdateReferences

.EXAMPLE
    .\Publish-VNext.ps1 -Path .\src\Libraries\Lombiq.HelpfulLibraries\ -Type 'major' -UpdateReferences

.NOTES
    The script throws an exception if the necessary parameters are not provided or if there is a problem determining
    the next version number.
#>
param(
    [string]$Path,
    [string]$Type,
    [string]$Issue,
    [switch]$PreRelease,
    [switch]$UpdateReferences,
    [switch]$NonInteractive
)

$origin = Get-Location
$newVersion = ''

if (-not $PreRelease -and -not $PSBoundParameters.ContainsKey('Type'))
{
    throw 'Must set a version Type ("major", "minor", "patch")'
    if ($Type -ne 'major' -and $Type -ne 'minor' -and $Type -ne 'patch')
    {
        throw 'Invalid version Type must be "major", "minor", or "patch"'
    }
}

if ($PreRelease -and -not $PSBoundParameters.ContainsKey('Issue'))
{
    throw 'Pre-releases must have an Issue number'
}

if ($PSBoundParameters.ContainsKey('Path'))
{
    Set-Location $Path
}

# Get the latest release tags, we get the last 20 tags so we can determine the next version or pre-release version
$latestTags = git for-each-ref --sort=-creatordate --count=20 --format '%(refname:short)' refs/tags

# Check tags for a valid version number and try to determine the next version
try
{
    if ($latestTags[0] -match '^v\d+\.\d+\.\d+')
    {
        $version = [Version]$matches[0].Substring(1)
        $major = $version.Major
        $minor = $version.Minor
        $patch = $version.Build

        switch ($Type)
        {
            'major'
            {
                $major++
                $minor = 0
                $patch = 0
            }
            'minor'
            {
                $minor++
                $patch = 0
            }
            'patch'
            {
                $patch++
            }
        }

        $newVersion = "v$major.$minor.$patch"

        if ($PreRelease)
        {
            # Check if there were any pre-release tags for this Issue already in latestTags array
            $preReleaseTags = $latestTags | Where-Object { $PSItem -match ([regex]::Escape($Issue)) }
            if ($preReleaseTags)
            {
                # Cast to array if it's a single string
                if($preReleaseTags -is [string])
                {
                    $preReleaseTags = @($preReleaseTags)
                }
                $latestPreRelease = $preReleaseTags[0]
                $preReleaseVersion = $latestPreRelease -split '\.'
                $preReleaseVersionNr = [int]$preReleaseVersion[3]
                $preReleaseVersionNr++
                $newVersion += "-alpha.$preReleaseVersionNr.$Issue"
            }
            else
            {
                $newVersion += "-alpha.1.$Issue"
            }
        }

        if (-not $NonInteractive)
        {
            # Prompt the user for confirmation
            $prompt = "This action will create and push the following tag to git $newVersion," +
            ' Are you sure you want to continue? ([Y]es / [N]o)'
            $confirmation = Read-Host $prompt

            # Check the user's response
            if ($confirmation -ne 'Y')
            {
                Write-Output 'Operation cancelled by the user.'
                exit
            }
        }

        git tag $newVersion
        git push origin tag $newVersion
    }
    else
    {
        Write-Output 'No valid tags were found'
    }
}
catch
{
    throw 'There was a problem determining the next version number, please add a manual tag.'
}
finally
{
    Set-Location $origin

    if ($UpdateReferences)
    {
        # Remove the "v" at the beginning of the version number
        $newVersion = $newVersion.Substring(1)

        # Update the version number in all projects
        $solutionProjects = dotnet sln list
        foreach ($projectName in $solutionProjects)
        {
            if ($projectName -like '*.csproj*')
            {
                .\Update-References.ps1 -ProjectToFind $projectName -NewVersion $NewVersion
            }
        }
    }
}
