#Requires -RunAsAdministrator
<#
  Installs CHIFA.Agent as a Windows service that starts with Windows and restarts itself if it crashes.
  Fill Agent:GatewayUrl and Agent:AgentKey in appsettings.json first.
    .\install-agent.ps1            install (or reinstall) and start
    .\install-agent.ps1 -Uninstall stop and remove
#>
param([switch]$Uninstall)

$ErrorActionPreference = 'Stop'
$name = 'CHIFA.Agent'
$exe = Join-Path $PSScriptRoot 'CHIFA.Agent.exe'

if (Get-Service $name -ErrorAction SilentlyContinue) {
    Stop-Service $name -ErrorAction SilentlyContinue
    sc.exe delete $name | Out-Null
    Start-Sleep -Seconds 2
}
if ($Uninstall) { return }

sc.exe create $name binPath= "`"$exe`"" start= delayed-auto DisplayName= 'SmartCHIFA - Agent mobile' | Out-Null
sc.exe description $name 'Relie la base CHIFA locale au serveur SmartCHIFA pour l''application mobile.' | Out-Null
# Restart after 5 s, 10 s, then every minute; forget failures after a day.
sc.exe failure $name reset= 86400 actions= restart/5000/restart/10000/restart/60000 | Out-Null

# appsettings.json holds the agent key: only administrators and the service may read it.
$settings = Join-Path $PSScriptRoot 'appsettings.json'
icacls $settings /inheritance:r /grant:r 'SYSTEM:(R)' 'Administrators:(F)' | Out-Null

Start-Service $name
Get-Service $name
