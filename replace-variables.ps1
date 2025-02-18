$env:ProductName= 'Trados Studio'
$env:ProductNameWithEdition= 'Trados Studio 2024'
$env:ProductVersion= 'Studio18'
$env:VersionNumber= '18'
$env:VisualStudioEdition= 'Microsoft Visual Studio 2022'
$env:PluginPackedPath= '%AppData%\Trados\Trados Studio\18\Plugins\Packages\'
$env:PluginUnpackedPath= '%AppData%\Trados\Trados Studio\18\Plugins\Unpacked\'
$env:InstallationFolder= 'C:\Program Files (x86)\Trados\Trados Studio\Studio18'
$env:DefaultProjectsFolder= 'C:\Users\UserName\Documents\Studio 2024\Projects'
$env:StudioDocumentsFolderName= 'Studio 2024'
$env:AppSigningEmail= 'app-signing@sdl.com'
$env:ServerProductName= 'Trados GroupShare'
$env:ServerProductNameWithVersion= 'Trados GroupShare 2020 SR1'
$env:DotNetVersion= '.Net Framework 4.8'

$files = Get-ChildItem **/*.md -Recurse
$files | ForEach-Object { 
    $filecontent = Get-Content $_
    $envVars = Get-ChildItem Env:
    foreach ($envVar in $envVars) {
        $placeholder = "<Var:$($envVar.Name)>"
        $filecontent = $filecontent -replace [regex]::Escape($placeholder), "Var:$($envVar.Name)"
    }
    Set-Content $_ -Value $filecontent
}
         