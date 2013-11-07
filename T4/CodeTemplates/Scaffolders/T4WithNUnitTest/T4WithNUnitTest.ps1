[T4Scaffolding.Scaffolder(Description = "T4WithNUnitTest")][CmdletBinding()]
param(
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$Scenario,
	[switch]$IsLeaf = $true,
	[string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate "_Test" -Template Template `
	-Model @{ Namespace = $namespace; Scenario = $Scenario; IsLeaf=($IsLeaf -eq $true) } `
	-SuccessMessage "Added Template output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force