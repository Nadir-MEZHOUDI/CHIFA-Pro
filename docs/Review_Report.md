# CHIFA Pro Review Report

Date: 2026-03-09

## 1. Executive summary

The codebase has a solid high-level direction (WinForms client + WPF gRPC server + DAL + shared contracts), and it builds cleanly on the current stack. The main risks are concentrated in runtime safety (server lifecycle), security/configuration hygiene, and maintainability drift caused by mixed responsibilities and duplicated patterns across projects.

Most improvements can be done incrementally without changing the chosen architecture or introducing conflicting paradigms.

## 2. Critical issues

1. **Deadlock risk in gRPC server startup/stop path**  
   - `CHIFA.Server/Helpers/GrpcServer.cs:19` acquires `_serverLock` in `StartAsync`, then calls `Stop()` at `CHIFA.Server/Helpers/GrpcServer.cs:23`, and `Stop()` tries to acquire the same lock again at `CHIFA.Server/Helpers/GrpcServer.cs:53`. This can block startup permanently.

2. **Unsafe disposal sequence in gRPC server**  
   - `CHIFA.Server/Helpers/GrpcServer.cs:92` disposes `_serverLock` before calling `Stop()` at `CHIFA.Server/Helpers/GrpcServer.cs:93`. This can trigger disposal/cleanup races and unobserved failures.

3. **Secrets and credentials embedded in source**  
   - DB password defaults exist in `CHIFA Pro/Helpers/DbChecker.cs:19`, `CHIFA Pro/Helpers/Settings/AppSettings.cs:32`, and `CHIFA.Server/Helpers/Settings/AppSettings.cs:33`.  
   - Bridge API codes are hardcoded in `CHIFA.Server/Views/MainWindow.xaml.cs:30` and `CHIFA.Server/Views/MainWindow.xaml.cs:32`.

4. **Layering violations from UI directly accessing database objects**  
   - `CHIFA Pro/Views/frmMain.cs:272` creates `ChifaDb` directly in UI code. This bypasses the service layer and couples presentation to persistence concerns.

## 3. Medium issues

1. **Shared mutable state via singleton services + mutable `Period`**  
   - `CHIFA.DAL/DataServices/ChifaService.cs:10`, `CHIFA.DAL/DataServices/StatisticsService.cs:11`, `CHIFA.Contract/Dtos/Period.cs:7`.  
   Multiple screens mutate shared date filters; behavior depends on interaction order.

2. **In-memory post-processing on potentially large datasets**  
   - Methods load large lists then group/filter in memory, e.g. `CHIFA.DAL/DataServices/ChifaService.cs:237`, `CHIFA.DAL/DataServices/ChifaService.cs:399`, `CHIFA.DAL/DataServices/StatisticsService.cs:218`.  
   This increases memory pressure and latency as data grows.

3. **Resource lifetime inconsistencies**  
   - Missing `await using` in `CHIFA.DAL/DataServices/ChifaService.cs:344` and non-disposed DB context in `CHIFA Pro/Views/frmMain.cs:272`.

4. **Busy-loop background logging updater**  
   - `CHIFA.Server/Views/MainWindow.xaml.cs:47` runs `while (true)` with UI-dispatch every 500ms (`CHIFA.Server/Views/MainWindow.xaml.cs:53`, `CHIFA.Server/Views/MainWindow.xaml.cs:56`) and no cancellation strategy.

5. **Unnecessary async complexity (`Task.Run` around async I/O)**  
   - `CHIFA Pro/Helpers/XtraHelper.cs:51` and `CHIFA Pro/Views/HomeUc.cs:18` add thread hopping without clear benefit.

6. **Service contract couples to persistence models**  
   - `CHIFA.Contract/Grpc/IChifaService.cs:10` and related signatures expose `DataModel` entities/expressions at contract boundary. This makes future schema evolution harder.

## 4. Minor issues

1. **Naming inconsistencies across files/types**  
   - Examples: `CHIFA Pro/Views/assuresUC.Designer.cs`, `CHIFA Pro/Views/borderauxUC.Designer.cs`, `CHIFA Pro/Views/FacturesUC.cs` vs `HomeUc` naming style.

2. **Duplicate infrastructure helpers across projects**  
   - Similar `AppStartup`, `SettingsToRegistry`, and exception-log helpers are duplicated between client and server (`CHIFA Pro/Helpers/AppStartup.cs:5`, `CHIFA.Server/Helpers/AppStartup.cs:6`, `CHIFA Pro/Helpers/Settings/SettingsToRegistry.cs:6`, `CHIFA.Server/Helpers/Settings/SettingsToRegistry.cs:8`).

3. **Mixed responsibilities in utility classes**  
   - `CHIFA Pro/Helpers/XtraHelper.cs` combines UI grid binding, logging, network scanning, and environment configuration.

4. **Magic numbers still present outside threshold constants**  
   - Example: `CHIFA.Contract/Helpers/MedicalThresholds.cs` exists, but raw values remain in places like `CHIFA.Contract/Dtos/TraitDetailsDto.cs:13` and timing/retry values in `CHIFA.Server/Helpers/GrpcServer.cs:13` and `CHIFA.Server/Helpers/GrpcServer.cs:14`.

5. **`Application.DoEvents()` usage in async flow**  
   - `CHIFA Pro/Views/frmMain.cs:117` and `CHIFA Pro/Views/frmMain.cs:317` can cause re-entrancy side effects.

## 5. Suggested refactorings

1. **Fix server lifecycle first (no architectural rewrite)**  
   - Split stop logic into an internal `StopCoreAsync` that assumes lock ownership, and call it from `StartAsync` without re-entering semaphore.
   - Implement `IAsyncDisposable` for `GrpcServer` and await shutdown deterministically.

2. **Harden configuration/secrets handling**  
   - Keep env-var approach, but remove plaintext defaults for passwords/tokens.  
   - Centralize config reads in one settings source per app and validate at startup with clear operator-facing error messages.

3. **Enforce service boundary in UI layer**  
   - Replace direct `new ChifaDb()` usage in forms with calls through existing services.  
   - Keep current singleton direction for now, but add small facade interfaces where needed to reduce coupling.

4. **Stabilize period/filter state ownership**  
   - Avoid static mutable date state (`Period.MinDate`/`MaxDate`).  
   - Return min/max from service methods and bind them per screen instance.

5. **Reduce memory-heavy query paths**  
   - Push grouping/projection deeper into LinqToDB queries where possible; avoid `ToListAsync` before grouping unless required by provider constraints.

6. **Consolidate duplicated helpers safely**  
   - Move shared helper logic (registry serialization, startup registration, logging extension) into one shared project/file set used by both apps.

7. **Normalize naming and structure gradually**  
   - Standardize new and touched files to one convention (`*Uc`, PascalCase file names), and rename legacy outliers during related feature work to avoid risky mass-rename.

## 6. Things that should remain unchanged

1. **Current project split (`CHIFA Pro`, `CHIFA.Server`, `CHIFA.DAL`, `CHIFA.Contract`)**: the separation is directionally correct.
2. **LinqToDB + PostgreSQL stack**: good fit for current performance/control profile.
3. **DevExpress WinForms and WPF usage**: aligned with desktop product requirements.
4. **Serilog + Velopack choices**: appropriate for observability and desktop update flow.
5. **Medical threshold centralization concept** in `CHIFA.Contract/Helpers/MedicalThresholds.cs:3` should be kept and expanded.

## 7. Priority order

1. **Immediate (P0)**: fix `GrpcServer` lock/dispose lifecycle (`CHIFA.Server/Helpers/GrpcServer.cs`).
2. **Immediate (P0)**: remove embedded credentials/tokens and enforce startup validation for missing config.
3. **Short term (P1)**: eliminate UI-layer direct DB usage and route through existing services.
4. **Short term (P1)**: remove shared static mutable period state and make filter state screen-local.
5. **Short term (P1)**: resolve high-impact query memory hotspots (`ChifaService`, `StatisticsService`).
6. **Medium term (P2)**: consolidate duplicate helper implementations between client/server.
7. **Medium term (P2)**: naming/structure normalization and cleanup of minor async/UI anti-patterns.
