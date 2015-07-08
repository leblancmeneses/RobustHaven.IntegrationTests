[T4Scaffolding.Scaffolder(Description = "RobustHaven.IntegrationTests.T4WithNUnitScenario")][CmdletBinding()]
param(
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$Scenario,
	[string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate "_XScenario" -Template T4WithNUnitScenarioTemplate `
	-Model @{ Namespace = $namespace; Scenario = $Scenario; } `
	-SuccessMessage "Added Template output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force