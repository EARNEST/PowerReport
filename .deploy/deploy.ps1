$solutionPath = "..\PowerReport.sln"

#$msbuild = (Resolve-Path HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\* | Get-ItemProperty -Name MSBuildToolsPath | Select-Object -Last 1).MSBuildToolsPath + "MSBuild.exe"
$msbuild = &"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe

# restore and build
.\nuget.exe restore $solutionPath
& $msbuild $solutionPath /p:Configuration=Release

$exePath = "..\PowerReport.Runner\bin\Release\PowerReport.Runner.exe"

& $exePath uninstall
& $exePath install

Start-Service -Name "PowerPositionExtractor"