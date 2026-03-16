param(
    [int]$SmokeSeconds = 12,
    [switch]$SkipSmoke
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

Write-Host "[Phase0] Baseline start: $(Get-Date -Format s)"

Write-Host "[Phase0] dotnet restore"
dotnet restore "CHIFA Pro.sln" -v minimal

Write-Host "[Phase0] dotnet build Debug"
dotnet build "CHIFA Pro.sln" -c Debug -v minimal

Write-Host "[Phase0] dotnet build Release"
dotnet build "CHIFA Pro.sln" -c Release -v minimal

$licensePath = Join-Path $env:APPDATA "DevExpress\DevExpress_License.txt"
if (Test-Path $licensePath) {
    Write-Host "[Phase0] DevExpress license file: FOUND ($licensePath)"
} else {
    Write-Host "[Phase0] DevExpress license file: MISSING ($licensePath)"
}

if (-not $SkipSmoke) {
    Write-Host "[Phase0] Startup smoke: CHIFA.Server ($SmokeSeconds sec)"
    $server = Start-Process dotnet -ArgumentList 'run --project "CHIFA.Server/CHIFA.Server.csproj" -c Debug' -WorkingDirectory $root -PassThru
    Start-Sleep -Seconds $SmokeSeconds
    if (-not $server.HasExited) {
        Stop-Process -Id $server.Id -Force
        Write-Host "[Phase0] CHIFA.Server smoke: StartedAndStopped"
    } else {
        Write-Host "[Phase0] CHIFA.Server smoke: ExitedEarly Code=$($server.ExitCode)"
    }

    Write-Host "[Phase0] Startup smoke: CHIFA.Pro ($SmokeSeconds sec)"
    $client = Start-Process dotnet -ArgumentList 'run --project "CHIFA Pro/CHIFA.Pro.csproj" -c Debug' -WorkingDirectory $root -PassThru
    Start-Sleep -Seconds $SmokeSeconds
    if (-not $client.HasExited) {
        Stop-Process -Id $client.Id -Force
        Write-Host "[Phase0] CHIFA.Pro smoke: StartedAndStopped"
    } else {
        Write-Host "[Phase0] CHIFA.Pro smoke: ExitedEarly Code=$($client.ExitCode)"
    }
}

Write-Host "[Phase0] Baseline completed: $(Get-Date -Format s)"
