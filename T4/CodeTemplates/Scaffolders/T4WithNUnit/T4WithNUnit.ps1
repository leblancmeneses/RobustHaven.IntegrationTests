[T4Scaffolding.Scaffolder(Description = "RobustHaven.IntegrationTests.T4WithNUnit")][CmdletBinding()]
param(
	[string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate "MyWebTestContext" -Template MyWebTestContext `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added MyWebTestContext output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate "FeatureTestBase" -Template FeatureTestBase `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added FeatureTestBase output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate "Log" -Template Log `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added Log output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force