#Requires -RunAsAdministrator
<#
  Installs (or updates) CHIFA.Agent as a Windows service, so phones are answered even when SmartCHIFA is
  closed. SmartCHIFA runs this script itself, with a UAC prompt, from « Application mobile ».

  Run it from SmartCHIFA's agent\ folder (the agent published complete). It is copied to Program Files:
  the SmartCHIFA folder is replaced at each update, and only administrators may change what a LocalSystem
  service runs. Its key comes from %ProgramData%\SmartCHIFA\mobile.json, written by SmartCHIFA when the
  pharmacy is linked.

    .\install-agent.ps1 [-ChifaHost serveur] [-ChifaPort 5432] [-ChifaFolder D:\CHIFA_OFFICINE]   install or update, then start
    .\install-agent.ps1 -Uninstall                                stop and remove
  Windows PowerShell 5.1 compatible: it is what every pharmacy PC has.
#>
param(
    [string]$Source = $PSScriptRoot,
    [string]$ChifaHost,
    [int]$ChifaPort = 0,
    [string]$ChifaFolder,
    [switch]$Uninstall
)

$ErrorActionPreference = 'Stop'
$name = 'CHIFA.Agent'
$target = Join-Path $env:ProgramFiles 'SmartCHIFA\Agent'
$log = Join-Path $env:ProgramData 'SmartCHIFA\agent-install.log'
New-Item -ItemType Directory -Force (Split-Path $log) | Out-Null
Start-Transcript -Path $log -Force | Out-Null

try {
    $settingsPath = Join-Path $target 'appsettings.json'
    $previous = $null
    if (Test-Path $settingsPath) { $previous = Get-Content $settingsPath -Raw | ConvertFrom-Json }

    if (Get-Service $name -ErrorAction SilentlyContinue) {
        Stop-Service $name -Force -ErrorAction SilentlyContinue
        sc.exe delete $name | Out-Null
        for ($i = 0; $i -lt 30 -and (Get-Service $name -ErrorAction SilentlyContinue); $i++) { Start-Sleep -Milliseconds 500 }
    }

    if ($Uninstall) {
        if (Test-Path $target) { Remove-Item $target -Recurse -Force }
        Write-Output 'Service removed.'
        return
    }

    if (-not (Test-Path (Join-Path $Source 'CHIFA.Agent.exe'))) { throw "CHIFA.Agent.exe not found in $Source" }
    if (Test-Path $target) { Remove-Item $target -Recurse -Force }
    New-Item -ItemType Directory -Force $target | Out-Null
    Copy-Item (Join-Path $Source '*') $target -Recurse -Force
    Write-Output "$((Get-ChildItem $target -Recurse -File).Count) files copied to $target"

    # The CHIFA database as SmartCHIFA sees it; an update keeps the previous values when none are given.
    $settings = Get-Content $settingsPath -Raw | ConvertFrom-Json
    if ($previous -and $previous.Chifa) { $settings.Chifa = $previous.Chifa }
    if ($ChifaHost) { $settings.Chifa.Host = $ChifaHost }
    if ($ChifaPort -gt 0) { $settings.Chifa.Port = $ChifaPort }
    if ($ChifaFolder) { $settings.Chifa | Add-Member -NotePropertyName Folder -NotePropertyValue $ChifaFolder -Force }
    $settings | ConvertTo-Json -Depth 10 | Set-Content $settingsPath -Encoding UTF8
    Write-Output "CHIFA database: $($settings.Chifa.Host):$($settings.Chifa.Port), folder: $($settings.Chifa.Folder)"

    $exe = Join-Path $target 'CHIFA.Agent.exe'
    sc.exe create $name binPath= "`"$exe`"" start= delayed-auto DisplayName= 'SmartCHIFA - Agent mobile' | Out-Null
    sc.exe description $name 'Relie la base CHIFA locale au serveur SmartCHIFA pour l''application mobile.' | Out-Null
    # Restart after 5 s, 10 s, then every minute; forget failures after a day.
    sc.exe failure $name reset= 86400 actions= restart/5000/restart/10000/restart/60000 | Out-Null

    Start-Service $name
    (Get-Service $name).WaitForStatus('Running', [TimeSpan]::FromSeconds(30))
    Write-Output 'Service running.'
}
finally {
    Stop-Transcript | Out-Null
}
