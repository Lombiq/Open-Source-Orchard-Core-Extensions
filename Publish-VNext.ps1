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
    A flag indicating whether the release is a pre-release. The default is $false.

.PARAMETER UpdateReferences
    A flag indicating whether to update the references in other projects. The default is $false.

.PARAMETER Interactive
    A flag indicating whether to prompt the user for confirmation before creating and pushing the new tag. The default
    is $true.

.EXAMPLE
    .\Publish-VNext.ps1 -Path .\src\Libraries\Lombiq.HelpfulLibraries\ -PreRelease $True -Issue "OC-123"
    -UpdateReferences $true

.EXAMPLE
    .\Publish-VNext.ps1 -Path .\src\Libraries\Lombiq.HelpfulLibraries\ -Type 'major' -UpdateReferences $true

.NOTES
    The script throws an exception if the necessary parameters are not provided or if there is a problem determining
    the next version number.
#>
param(
    [string]$Path,
    [string]$Type,
    [string]$Issue,
    [bool]$PreRelease = $false,
    [bool]$UpdateReferences = $false,
    [bool]$Interactive = $true
)

$origin = Get-Location
$newVersion = ''

if ($PreRelease -eq $false -and -not $PSBoundParameters.ContainsKey('Type'))
{
    throw 'Must set a version Type (major, minor, patch)'
    if ($Type -ne 'major' -and $Type -ne 'minor' -and $Type -ne 'patch')
    {
        throw 'Invalid version Type must be major, minor, or patch'
    }
}

if ($PreRelease -eq $true -and -not $PSBoundParameters.ContainsKey('Issue'))
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
    if ($latestTags[0] -match 'v\d+\.\d+\.\d+')
    {
        $latestVersion = $matches[0]
        $version = $latestVersion -split '\.'
        $major = [int]$version[0].Substring(1)
        $minor = [int]$version[1]
        $patch = [int]$version[2].Substring(0, 1)

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

        if ($PreRelease -eq $true)
        {
            # Check if there were any pre-release tags for this Issue already in latestTags array
            $preReleaseTags = $latestTags | Where-Object { $PSItem -match ([regex]::escape($Issue)) }
            if ($preReleaseTags.Count -gt 0)
            {
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

        if ($Interactive -eq $true)
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

    if ($UpdateReferences -eq $true)
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
