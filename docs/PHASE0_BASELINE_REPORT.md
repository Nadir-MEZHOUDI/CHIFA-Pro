# Phase 0 Baseline Report

Date: 2026-03-16
Project: CHIFA Pro
Related plan: `AUDIT_PLAN_ERRORS_PERFORMANCE_TRYCATCH.md`

## What Was Executed

- `dotnet restore "CHIFA Pro.sln" -v minimal`
- `dotnet build "CHIFA Pro.sln" -c Debug -v minimal`
- `dotnet build "CHIFA Pro.sln" -c Release -v minimal`
- Startup smoke (12s each):
  - `dotnet run --project "CHIFA.Server/CHIFA.Server.csproj" -c Debug`
  - `dotnet run --project "CHIFA Pro/CHIFA.Pro.csproj" -c Debug`

## Results Summary

- Restore: success.
- Build Debug: success, 0 warnings, 0 errors.
- Build Release: success, 0 warnings, 0 errors.
- Startup smoke:
  - Server: `StartedAndStopped` (no immediate startup crash observed).
  - Client: `StartedAndStopped` (no immediate startup crash observed).

## Warning Baseline

- Current baseline warnings: none.
- DevExpress license path detected: `C:\Users\nadir\AppData\Roaming\DevExpress\DevExpress_License.txt`.
- Note: earlier exploratory build had `DX1000`/`DX1001` in trial mode; baseline rerun completed cleanly.

## License Setup Check (Phase 0 item)

- Local machine: DevExpress license file is present in `%AppData%\DevExpress` (detected by build output).
- CI state: currently not verified in pipeline execution.
- CI note: `azure-pipelines.yml` references `CHIFA.Stat.csproj`, which does not match current solution projects. This should be corrected before using CI as the baseline gate.

## Repeatable Baseline Script

- Added script: `phase0-baseline.ps1`
- Usage:

```powershell
./phase0-baseline.ps1
./phase0-baseline.ps1 -SkipSmoke
./phase0-baseline.ps1 -SmokeSeconds 20
```

## Phase 0 Status

- Baseline run: completed.
- Startup smoke: completed.
- DevExpress local check: completed.
- DevExpress CI readiness: pending (pipeline alignment/update required).
