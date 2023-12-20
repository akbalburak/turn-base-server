param (
    [string]$ModulePath = "D:\Github\turn-base-server\UGSModules\bin\Release\net6.0\publish\linux-x64",
    [string]$ModuleZipName = "UGSModules.zip",
    [string]$ModuleCcmName = "UGSModules.ccm",
    [string]$DeployPath = "D:\Github\turn-base-server\UGSModules\bin\Release\net6.0\publish\linux-x64",
    [string]$DllDestination = "D:\Github\turn-base\Assets\"
)

$ModuleZipPath = Join-Path -Path $ModulePath -ChildPath $ModuleZipName
$ModuleCcmPath = Join-Path -Path $ModulePath -ChildPath $ModuleCcmName

# Dosya silme
Remove-Item $ModuleCcmPath -Force

# Dosyaları zipleyip adını değiştirme
Compress-Archive -Path "$ModulePath\*" -DestinationPath $ModuleZipPath
Rename-Item -Path $ModuleZipPath -NewName $ModuleCcmName

# UGS deploy komutu
C:\ugs-windows-x64.exe deploy -p 798ef889-6fb0-4ac6-a004-b93983208327 -e development $ModuleCcmPath

Move-Item -Path "$ModulePath\ModuleDTOLayer.dll" -Destination $DllDestination -Force