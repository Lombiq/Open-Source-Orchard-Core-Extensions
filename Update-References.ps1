# Call like this: .\Update-References.ps1 -ProjectToFind "ProjectName" -NewVersion "1.0.0"
param(
    [string]$ProjectToFind, # Name of the project to check reference for
    [string]$NewVersion # New version number to set
)

# Get all .csproj files recursively
$projectFiles = Get-ChildItem -Path .\ -Recurse -Filter '*.csproj'

# Loop through each project file to see if it has a reference to the project we are looking for
ForEach ($projectFile in $projectFiles)
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
