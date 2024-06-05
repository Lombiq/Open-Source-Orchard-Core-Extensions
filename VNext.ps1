# Call like this: .\VNext.ps1 -path "\path\to\project" -prerelease $True -issue "1234" -commitChanges $true
# or like this: .\VNext.ps1 -path "\path\to\project" -type "major" -commitChanges $true
param(
    [string]$path,
    [string]$type,
    [string]$issue,
    [bool]$prerelease = $false,
    [bool]$commitChanges = $false
)

$origin = Get-Location
$newVersion = ""

if ($prerelease -eq $false -and !$PSBoundParameters.ContainsKey("type")) {
    throw "Must set a version type (major, minor, patch)"
    if($type -ne "major" -and $type -ne "minor" -and $type -ne "patch") {
        throw "Invalid version type must be major, minor, or patch"
    }
}

if ($prerelease -eq $true -and !$PSBoundParameters.ContainsKey("issue")) {
    throw "Pre-releases must have an issue number"
}

if ($PSBoundParameters.ContainsKey("path")) {
    Set-Location $path
}

#Get the latest release tags, we get the last 20 tags so we can determine the next version or pre-release version
$latestTags = git for-each-ref --sort=-creatordate --count=20 --format '%(refname:short)' refs/tags

#Check tags for a valid version number and try to determine the next version
try {
    if ($latestTags[0] -match "v\d+\.\d+\.\d+") {
        $latestVersion = $matches[0]
        $version = $latestVersion -split "\."
        $major = [int]$version[0].Substring(1)
        $minor = [int]$version[1]
        $patch = [int]$version[2].Substring(0,1)

        switch ($type) {
            "major" {
                $major++
                $minor = 0
                $patch = 0
            }
            "minor" {
                $minor++
                $patch = 0
            }
            "patch" {
                $patch++
            }
        }

        $newVersion = "v$major.$minor.$patch"

        if ($prerelease -eq $true) {
            #Check if there were any pre-release tags for this issue already in latestTags array
            $preReleaseTags = $latestTags | Where-Object {$_ -match ([regex]::escape($issue))}
            if ($preReleaseTags.Count -gt 0) {
                $latestPreRelease = $preReleaseTags[0]
                $preReleaseVersion = $latestPreRelease -split "\."
                $preReleaseVersionNr = [int]$preReleaseVersion[3]
                $preReleaseVersionNr++
                $newVersion += "-alpha.$preReleaseVersionNr.$issue"
            } else {
                $newVersion += "-alpha.1.$issue"
            }
        }

        Write-Output "New version: $newVersion"

        git tag $newVersion
        git push origin tag $newVersion
    } else {
        Write-Output "No valid tags found"
    }
} catch {
    throw "There was a problem determining the next version number, please add a manual tag."
} finally {
    Set-Location $origin

    if($commitChanges -eq $true) {
        #remove the v at the beginnging of the version number
        $newVersion = $newVersion.Substring(1)

        # Call the Get-Solution-Projects.ps1 script to update the version number in all projects
        .\Get-Solution-Projects.ps1 -solutionFilePath $path -newVersion $newVersion
    }
}
