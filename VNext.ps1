# Call like this: .\VNext.ps1 -Path '\Path\to\project' -PreRelease $True -Issue '1234' -UpdateReferences $true
# or like this: .\VNext.ps1 -Path '\Path\to\project' -Type 'major' -UpdateReferences $true
param(
    [string]$Path,
    [string]$Type,
    [string]$Issue,
    [bool]$PreRelease = $false,
    [bool]$UpdateReferences = $false
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

#Get the latest release tags, we get the last 20 tags so we can determine the next version or pre-release version
$latestTags = git for-each-ref --sort=-creatordate --count=20 --format '%(refname:short)' refs/tags

#Check tags for a valid version number and try to determine the next version
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
            #Check if there were any pre-release tags for this Issue already in latestTags array
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

        Write-Output "New version: $newVersion"

        git tag $newVersion
        #git push origin tag $newVersion
    }
    else
    {
        Write-Output 'No valid tags found'
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
        #remove the v at the beginning of the version number
        $newVersion = $newVersion.Substring(1)

        # Call the Get-Solution-Projects.ps1 script to update the version number in all projects
        .\Get-Solution-Projects.ps1 -SolutionFilePath $Path -NewVersion $newVersion
    }
}
