[T4Scaffolding.Scaffolder(Description = "RobustHaven.IntegrationTests.T4WithNUnitFeature")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$featureFile,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

#http://mvcscaffolding.codeplex.com/wikipage?title=T4Scaffolding%27s%20cmdlets%20for%20use%20in%20custom%20scaffolders
#http://mvcscaffolding.codeplex.com/discussions/347469
#http://msdn.microsoft.com/en-us/library/ms228957%28v=vs.90%29.aspx


$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$file = Get-ProjectItem $featureFile -Project $Project
$inputFilePath = $($file.Properties.Item("FullPath").Value)

Add-ProjectItemViaTemplate "_XFeature" -Template T4WithNUnitFeatureTemplate `
	-Model @{ Namespace = $namespace; InputFilePath = $inputFilePath; } `
	-SuccessMessage "Added T4WithNUnitFeature output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force