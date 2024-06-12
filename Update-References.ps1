<#
.SYNOPSIS
    This script updates the version number of a specified project in all referencing .csproj files.

.DESCRIPTION
    The script recursively searches for all .csproj files and checks if they have a reference to the specified project.
    If a reference is found, the version number of the reference is updated to the new version number.

.PARAMETER ProjectToFind
    The name of the project to check references for.

.PARAMETER NewVersion
    The new version number to set for the project references.

.EXAMPLE
    .\Update-References.ps1 -ProjectToFind "ProjectName" -NewVersion "1.0.0"
#>
param(
    [Parameter(Mandatory=$true)]
    [string]$ProjectToFind,

    [Parameter(Mandatory=$true)]
    [string]$NewVersion
)

# Get all .csproj files recursively
$projectFiles = Get-ChildItem -Path .\ -Recurse -Filter '*.csproj'

# Loop through each project file to see if it has a reference to the project we are looking for
forEach ($projectFile in $projectFiles)
{
    # Create an XmlDocument object
    $projectXml = New-Object System.Xml.XmlDocument

    # Set PreserveWhitespace to true
    $projectXml.PreserveWhitespace = $true

    # Load the XML file
    $projectXml.Load($projectFile)

    # Find the ProjectReference nodes
    $projectReferences = $projectXml.Project.ItemGroup.PackageReference

    foreach ($projectReference in $projectReferences)
    {
        # Check if the Include attribute contains the project name
        if ($projectReference.Include -like "*$ProjectToFind*")
        {
            if ($projectReference.Version)
            {
                # Change the version if it has a version (is a nuget reference)
                $projectReference.Version = $NewVersion
                # Save the modified .csproj file
                $projectXml.Save($projectFile)
            }
        }
    }
}
