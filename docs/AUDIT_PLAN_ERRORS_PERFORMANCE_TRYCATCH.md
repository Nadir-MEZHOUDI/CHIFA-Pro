# CHIFA Pro Audit Plan (Errors, Performance, Try-Catch Coverage)

Date: 2026-03-16
Scope: Static multi-level audit + build validation (no production code changes)
Status: Investigation completed, plan ready for implementation (Phase 2 execution completed on 2026-03-16)
Phase 0 execution: Completed on 2026-03-16 (baseline script + smoke run + local license check)
Phase 1 execution: Completed on 2026-03-16 (try-catch hardening + startup guards + server loop cancellation)
Phase 2 execution: Completed on 2026-03-16 (debounce/cancellation quick wins + UI thread cleanup + log rendering optimization)

## 1) Audit Levels Performed

1. Exception-safety scan
- Reviewed UI event handlers, async void boundaries, startup/bootstrap, background threads, and disposal paths.

2. Performance-risk scan
- Reviewed data-loading flows, DB query patterns, UI refresh frequency, thread usage, and resource lifecycles.

3. Build and analyzer validation
- Executed restore/build to detect compile and analyzer warnings that can hide runtime risks.

## 2) Command Validation Results

- `dotnet restore "CHIFA Pro.sln" -v minimal`: Success, 0 warnings, 0 errors.
- `dotnet build "CHIFA Pro.sln" -c Debug -v minimal`: Success, 2 warnings, 0 errors.
- Current warnings: DevExpress licensing only (`DX1000`, `DX1001`) from `CHIFA Pro/CHIFA.Pro.csproj`.
- No nullable (`CS86xx`), async, disposal, or threading analyzer warnings reported in this build mode.

## 3) High-Priority Findings (Fix First)

### A) Missing try-catch in high-risk async UI paths

1) `CHIFA Pro/Views/OfficineUC.cs:13`
- `async void` load handler awaits DB call without local try-catch.
- Risk: unhandled exception can crash UI flow during data access errors.

2) `CHIFA Pro/Views/NomenclaturUC.cs:21`
- `async void` grid focus-change handler triggers DB call without guard.
- Risk: repeated focus changes can surface unhandled faults.

3) `CHIFA Pro/Views/frmHistory.cs:20` and `CHIFA Pro/Views/frmHistory.cs:76`
- Load path awaits data-loading methods where one path lacks local exception handling.
- Risk: load-time exceptions propagate through async void boundary.

### B) Server UI slowdown and memory pressure

4) `CHIFA.Server/Views/MainWindow.xaml.cs:45`
- Infinite background loop updates logs every 500ms and refreshes full text each cycle.
- Risk: increasing memory usage and UI lag over uptime.

5) `CHIFA.Server/App.xaml.cs:14`
- In-memory `StringWriter` accumulates logs without bounded retention.
- Risk: long-running sessions can degrade progressively.

### C) Heavy data materialization before aggregation

6) `CHIFA.DAL/DataServices/ChifaService.cs:377`, `:396`, `:399`, `:445`, `:464`
7) `CHIFA.DAL/DataServices/StatisticsService.cs:195`, `:216`
- Large datasets are materialized and grouped in memory in multiple paths.
- Risk: high CPU/RAM usage and slow response on larger databases.

## 4) Medium-Priority Findings

1) Startup and disposal resilience
- `CHIFA Pro/Program.cs:35`, `CHIFA.Server/App.xaml.cs:24`: bootstrap paths need stronger top-level fault containment.
- `CHIFA.Server/Helpers/GrpcServer.cs:86`: disposal order may race with async stop path.

2) Query storms from UI events
- `CHIFA Pro/Views/FacturesUC.cs:87`, `:88`, `:136`
- `CHIFA Pro/Views/TraitSpecUc.cs:46`, `:47`, `:118`
- `CHIFA Pro/Views/StatisticsUc.cs:31`, `:37`, `:185`, `:186`
- Risk: repeated rapid events trigger overlapping DB reloads.

3) Blocking work on startup path
- `CHIFA Pro/Views/HomeUc.cs:124`, `CHIFA Pro/Helpers/DbChecker.cs:90`, `:72`
- Risk: startup delay/freeze due to process wait and synchronous work before first await.

4) Resource lifecycle hygiene
- `CHIFA.DAL/DataServices/ChifaService.cs:344`
- `CHIFA Pro/Views/frmMain.cs:290`
- `CHIFA Pro/Helpers/DbChecker.cs:31`, `:132`
- `CHIFA Pro/Helpers/XtraHelper.cs:129`
- Risk: pressure on handles/connections over long sessions.

5) UI threading and reentrancy
- `CHIFA Pro/Views/frmMain.cs:120`, `:165`, `:335`
- `CHIFA Pro/Helpers/XtraHelper.cs:23`, `:40`
- Risk: intermittent jank/reentrancy bugs due to `Application.DoEvents()` and mixed context handling.

## 5) Try-Catch Coverage Summary

Covered reasonably well:
- Several WinForms screens already follow `try-catch + ex.Log()` convention.

Coverage gaps to prioritize:
- `async void` handlers in `OfficineUC`, `NomenclaturUC`, `frmHistory`, `frmTraitSpec`.
- Background worker loop and startup/disposal boundaries on server side.

## 6) Implementation Plan (Phased)

### Phase 0 - Baseline and safety checks (0.5 day)
- Enable a repeatable baseline run (restore/build + startup smoke).
- Confirm DevExpress license setup for local/CI to reduce warning noise.

### Phase 1 - Crash prevention and exception boundaries (1 day)
- Add/standardize try-catch at all uncovered `async void` event boundaries.
- Add guarded startup wrappers for client/server bootstrap.
- Harden server background loop with cancellation and protected dispatch.

Acceptance criteria:
- No unhandled exceptions during normal navigation and startup/shutdown smoke tests.

### Phase 2 - Performance quick wins (1-2 days)
- Replace full-log refresh with incremental, bounded log buffering.
- Add debounce + cancellation for high-frequency UI reload triggers.
- Remove redundant `Task.Run` wrappers around already async I/O.
- Remove/replace `Application.DoEvents()` where possible.

Acceptance criteria:
- Noticeable reduction in UI lag during filtering/navigation and in server log view over time.

### Phase 3 - Data access optimization (2-4 days)
- Move heavy grouping/aggregation from client memory to DB-side projections.
- Reduce selected columns and introduce paging for heavy grids.
- Validate query count and execution time on realistic data volume.

Acceptance criteria:
- Lower RAM spikes and faster load time for statistics/trait/product-heavy screens.

### Phase 4 - Resource and disposal hardening (1 day)
- Normalize `using/await using` for DB connections, process, and ping objects.
- Fix stop/dispose sequencing in gRPC server lifecycle.

Acceptance criteria:
- Stable long-session behavior without progressive handle/connection pressure.

## 7) Validation Matrix for the Improvement Work

- Functional smoke: open main screens, perform filter/date changes, navigate repeatedly.
- Stability smoke: run server/client for extended session and monitor responsiveness.
- Data stress: test with larger dataset snapshots to validate query/load improvements.
- Build gate: `dotnet build "CHIFA Pro.sln"` remains green.

## 8) Risks and Dependencies

- DevExpress licensing warnings can obscure build signal until configured.
- Data optimization changes may alter result-shape ordering; requires careful UI regression checks.
- Event-debounce and cancellation need UX tuning to avoid perceived delayed response.

## 9) Recommended Fix Order (ROI)

1. Missing try-catch at async UI boundaries.
2. Server log loop and unbounded log memory.
3. Debounce/cancel high-frequency reload events.
4. Remove blocking startup waits and redundant Task.Run usage.
5. DB-side aggregation and paging for heavy statistics screens.
