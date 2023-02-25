Update-VisualStudioSolutionNuGetPackages -Path (Get-Location).Path -ProjectNameFilter 'Lombiq.*' -PackageNameFilter 'Lombiq.*'
Write-Output 'Finished updating packages.'
