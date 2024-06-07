param(
    [string]$SolutionFilePath,
    [string]$NewVersion
)

$solutionFile = Get-ChildItem -Path $SolutionFilePath -Filter *.sln | Select-Object -First 1

if (-not $solutionFile)
{
    throw 'No solution file found in the specified path.'
}

if (-not $NewVersion)
{
    throw 'No new version specified.'
}

# Read the .sln file line by line
$solutionFileLines = Get-Content $solutionFile

# Initialize an empty array to hold the project names
$projectNames = @()

# Loop through each line in the .sln file
foreach ($line in $solutionFileLines)
{
    # Check if the line starts with "Project"
    if ($line -match '^Project\("' -and $line -like '*.csproj*')
    {
        # Extract the project name from the line
        $projectName = ($line -split ',')[0].Trim('"').Split('=')[1].Trim(' "')

        # Add the project name to the array
        $projectNames += $projectName
    }
}

# Loop through each project name and call the Update-References.ps1 script to update the version number
foreach ($projectName in $projectNames)
{
    .\Update-References.ps1 -ProjectToFind $projectName -NewVersion $NewVersion
}
