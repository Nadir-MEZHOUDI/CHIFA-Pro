# CHIFA Pro - Architecture Review

**Date:** March 17, 2026  
**Reviewer:** Senior .NET Architect  
**Project:** CHIFA Pro Healthcare Management System  
**Tech Stack:** .NET 10, WinForms, PostgreSQL, LinqToDB

---

## Executive Summary

CHIFA Pro is a **3-project solution** implementing a desktop application for healthcare management. The architecture follows a **2-tier pattern** with a direct service layer. The codebase is well-structured for a desktop application but shows signs of **rapid growth** with some architectural debt that can be addressed incrementally.

**Overall Assessment:** **6.5/10**

**Strengths:**
- Clean separation between client (CHIFA.Pro) and server (CHIFA.Server)
- Proper use of DTOs for data transfer
- Consistent async/await patterns with cancellation tokens
- Good use of LinqToDB for data access
- Expression-based predicate composition for flexible queries
- Proper resource disposal (IAsyncDisposable, SemaphoreSlim)

**Concerns:**
- Singleton pattern used instead of Dependency Injection
- UI layer directly calls data services through static instances
- Business logic scattered between UI and data services
- No validation layer
- No test coverage
- Service interface mixed with domain models
- Period state is both instance and static (confusion)

---

## 1. Current Architecture (As-Is)

### 1.1 Solution Structure

```
CHIFA Pro Solution/
├── CHIFA.Pro (WinForms Client)          - 12,477+ lines in Views
│   ├── Views/                            - 19 UserControls
│   ├── Helpers/                          - Navigation, Settings, Extensions
│   └── Program.cs                        - Entry point, logging, culture setup
│
├── CHIFA.Services (Service Layer)            - Direct service access
│
├── CHIFA.Services (Service Layer)            - DataServices + DTOs + Helpers
│
    ├── Dtos/                             - DTO files
    ├── DataServices/                     - ChifaService, StatisticsService, ScopeService
    ├── Helpers/                          - PredicateBuilder, MedicalThresholds
    └── Statistics/                       - Statistics DTOs
```

### 1.2 Entry Points

**Client (CHIFA.Pro/Program.cs:33)**
```csharp
[STAThread]
private static void Main()
{
    - SingleInstance check
    - Serilog configuration (file + console in DEBUG)
    - Exception handling setup (3 handlers)
    - Velopack update check
    - Culture set to fr-FR
    - Run FrmMain (main form)
}
```

**Server (CHIFA.Server/App.xaml.cs:25)**
```csharp
public App()
{
    - SingleInstance check
    - Serilog configuration (file + console + TextWriter)
    - Velopack update check
    - WPF MainWindow startup
    - Direct service instantiation (static Instance)
}
```

### 1.3 UI Layer Structure

**Pattern:** Tab-based navigation with singleton main form
- **FrmMain** (373 lines): Main shell with AccordionControl menu
- **NavigationService** (31 lines): Extension method-based navigation
- **19 UserControls** implementing `INavigable`

**Typical View Pattern (FacturesUC.cs:213)**
```csharp
public partial class FacturesUc : XtraUserControl, INavigable
{
    - Debounced reload with CancellationTokenSource
    - SemaphoreSlim for concurrency control
    - Direct call to ChifaService.Instance
    - Expression predicate building in UI
    - DevExpress grid binding with async loading
}
```

**Concerns:**
- UI builds Expression predicates (lines 160-168 in FacturesUC.cs)
- Direct service access via static singleton (46 usages in Views/)
- Business rules embedded in UI (TS, LongDuree coloring logic)
- No ViewModel layer (tight coupling to DTOs)

### 1.4 Domain/Business Logic Placement

**Current State: Scattered**

1. **In UI Layer (CHIFA.Pro/Views/):**
   - Predicate building for filtering
   - Threshold-based UI coloring (lines 40-46, 53-59 in FacturesUC.cs)
   - Business rule interpretation

2. **In Data Services (CHIFA.Services/DataServices/):**
   - Complex LINQ projections (ChifaService.cs lines 377-452)
   - Medical threshold application (lines 49, 73, 191, 219)
   - Grouping and transformation logic
   - Extension methods for domain operations (StatisticsService.cs:300-303)

3. **In Contract Helpers:**
   - MedicalThresholds constants
   - PredicateBuilder utilities

**Result:** No clear "domain layer" - business logic lives where it's needed, not where it belongs.

### 1.5 Data Access Pattern

**Technology:** LinqToDB 6.x with PostgreSQL
**Connection:** Via environment variables (ChifaDb from CHIFA2.Data package)

**Pattern (from ChifaService.cs:15-38):**
```csharp
public async ValueTask<IEnumerable<BordereauDto>> GetAllBordereauxAsync(...)
{
    await using var db = new ChifaDb();  // Per-request instance
    
    var list = await db.Bordereaus
        .Where(predicate.SetPeriod(Period))  // Extension method
        .Select(x => new BordereauDto { ... })
        .OrderByDescending(x => x.FirstFacture)
        .ToListAsync()
        .ConfigureAwait(false);
    
    return list;
}
```

**Characteristics:**
- **Good:** Async disposal, ConfigureAwait(false), ValueTask usage
- **Good:** No DbContext pooling needed (LinqToDB handles connections)
- **Good:** Projection to DTOs in database query
- **Concern:** ChifaDb comes from external package (CHIFA2.Data 4.3.8)
- **Concern:** Connection string via environment variables only
- **Concern:** No retry policies or resilience patterns

### 1.6 Cross-Cutting Concerns

#### Logging (Serilog)
- **Configured in:** Program.cs (both projects)
- **Sinks:** File (rolling daily), Console (DEBUG only)
- **Usage:** Extension method `ex.Log()` in XtraHelper.cs:138
- **Coverage:** Exception handling, server lifecycle events

#### Validation
- **Status:** ❌ **NOT PRESENT**
- No input validation
- No DTO validation attributes
- No FluentValidation or similar

#### Caching
- **Status:** ❌ **NOT PRESENT**
- Every request queries database
- Period.MinDate/MaxDate are static (suspicious shared state)

#### Error Handling
- **Good:** 3-layer exception handling in Program.cs:82-93
  - AppDomain.UnhandledException
  - Application.ThreadException  
  - TaskScheduler.UnobservedTaskException
- **Good:** try-catch with logging in async operations
- **Concern:** Generic ex.Log() - no error categorization
- **Concern:** No circuit breaker for DB connection failures

### 1.7 Dependency Direction and Coupling

**Dependency Graph:**
```
CHIFA.Pro ──────────► CHIFA.Services ──────► CHIFA.Services ──────► CHIFA2.Data (external)
               │                              │                        │
               │                              │                        ▼
               └─────────────────────────────►│                   DataModel
                                              │                   (Facture, Bordereau, etc.)
CHIFA.Server ───────► CHIFA.Services
```

**Key Issues:**

1. **Tight Coupling to Static Singletons**
   - `ChifaService.Instance` (used 46 times in Views/)
   - `StatisticsService.Instance` (used frequently)
   - **Impact:** Cannot mock for testing, hard to swap implementations

2. **UI ➜ Data Direct Coupling**
   - Views directly call DAL services
   - No intermediate application/service layer
   - **Impact:** Business logic leaks into UI

3. **External Package Dependency**
   - CHIFA2.Data package provides `ChifaDb` and `DataModel`
   - Contract layer depends on `DataModel` (Facture, Bordereau entities)
   - **Impact:** Domain models defined externally, limited control

4. **Period State Confusion (Period.cs:1-10)**
   ```csharp
   public class Period
   {
       public DateTime? From { get; set; } = DateTime.Today.AddYears(-2);
       public DateTime? To { get; set; } = DateTime.Today;
       public static DateTime MaxDate { get; set; }  // ⚠️ Shared state
       public static DateTime MinDate { get; set; }  // ⚠️ Shared state
   }
   ```
   - Mixing instance and static state
   - MinDate/MaxDate set once at startup, shared globally
   - **Impact:** Confusing, error-prone

5. **Service Interface Pollution (ChifaService.cs:6-42)**
   - Interface methods accept `DataModel` entities (Facture, Bordereau, etc.)
   - Interface methods accept `Expression<Func<Facture, bool>>`
   - **Impact:** direct service layer tightly coupled to database schema

### 1.8 Coupling Hotspots

**Priority Hotspots:**

1. **ChifaService.Instance** (CHIFA.Services/DataServices/ChifaService.cs:7-8)
   - 535-line god class
   - Static singleton pattern
   - **Used in:** 46 locations across Views/
   - **Risk Level:** HIGH

2. **Expression Predicates in UI** (FacturesUC.cs:160-168)
   ```csharp
   Expression<Func<Facture, bool>> predicate = f => true;
   predicate = patterns.Aggregate(predicate, 
       (current, p) => current.And(f => 
           f.DetailFacts.Any(d => d.Medicament.FullName!.Contains(p))));
   ```
   - Business logic in UI layer
   - Database schema knowledge in views
   - **Risk Level:** HIGH

3. **Period as Service State** (ChifaService.cs:10)
   ```csharp
   public Period Period { get; } = new();  // Instance property on singleton
   ```
   - Service has mutable state
   - Shared across all callers
   - **Risk Level:** MEDIUM

4. **No Abstraction Over ChifaDb**
   - Direct instantiation everywhere: `new ChifaDb()`
   - Cannot swap, cannot test
   - **Risk Level:** MEDIUM

---

## 2. Architecture Issues List (Prioritized)

### 🔴 CRITICAL Issues

#### Issue #1: No Dependency Injection

**Symptom:**
- Cannot write unit tests
- Tight coupling throughout codebase
- Every view directly references `ChifaService.Instance` and `StatisticsService.Instance`

**Root Cause:**
- Legacy singleton pattern: `public static ChifaService Instance => _instance ??= new();`
- No DI container configured in Program.cs
- Services accessed via static Instance pattern

**Risk Level:** HIGH  
**Impact:** Testing, Maintenance, Extensibility

**Suggested Fix (3 options):**

1. **Option A: Microsoft.Extensions.DependencyInjection (Recommended)**
   - Add DI container to Program.cs
   - Register services as scoped/singleton
   - Inject into views via constructor
   - **Effort:** M (requires view changes)
   - **Benefits:** Standard, minimal dependencies

2. **Option B: Keep Singleton, Add Abstraction**
   - Extract interfaces from services
   - Allow singleton to hold test implementation
   - **Effort:** S
   - **Benefits:** Minimal change, enables mocking

3. **Option C: Service Locator Pattern**
   - Create ServiceProvider wrapper
   - Register services centrally
   - Views request from locator
   - **Effort:** S
   - **Benefits:** Quick migration path

**Estimated Effort:** M (1-2 days for Option A)

---

#### Issue #2: Business Logic in UI Layer

**Symptom:**
- Views build LINQ expressions
- Medical rules hardcoded in grid coloring
- Cannot reuse rules across clients

**Root Cause:**
- No application/service layer between UI and DAL
- Expression predicates built in view code (FacturesUC.cs:160-168)
- Threshold constants referenced directly in UI

**Risk Level:** HIGH  
**Impact:** Code Duplication, Maintainability

**Evidence:**
```csharp
// FacturesUC.cs:160-168 - Business logic in UI
var patterns = txt.Split(" ", ...);
predicate = patterns.Aggregate(predicate, 
    (current, p) => current.And(f => 
        f.DetailFacts.Any(d => d.Medicament.FullName!.Contains(p))));
```

**Suggested Fix (3 options):**

1. **Option A: Create Application Services Layer**
   - New project: CHIFA.Application
   - Services handle expression building, filtering, business rules
   - Views call application services only
   - **Effort:** M-L
   - **Benefits:** Proper layering, testable business logic

2. **Option B: Move Logic to Existing Services**
   - Add query methods to ChifaService with semantic names
   - Example: `SearchFacturesByMedicationAsync(string[] keywords, ...)`
   - **Effort:** M
   - **Benefits:** Quick, keeps 3-tier structure

3. **Option C: Extract to Helper Classes**
   - Create FactureQueryBuilder, PredicateFactory
   - Shared between UI and services
   - **Effort:** S
   - **Benefits:** Minimal refactor, improves reuse

**Estimated Effort:** M (1 week for Option B)

---

#### Issue #3: No Test Coverage

**Symptom:**
- Zero test projects in solution
- Cannot verify business rules
- Regression risk on every change

**Root Cause:**
- No testing culture/infrastructure
- Singletons make testing hard
- Tight coupling to external dependencies

**Risk Level:** MEDIUM  
**Impact:** Quality, Confidence, Maintenance

**Suggested Fix:**

1. **Phase 1: Add Test Project + xUnit**
   - Create CHIFA.Tests project
   - Add xUnit, FluentAssertions, Moq
   - **Effort:** S (1 day)

2. **Phase 2: Test Business Logic**
   - Once DI is in place, test services
   - Focus on ChifaService methods with complex logic
   - **Effort:** M (per-service)

3. **Phase 3: Integration Tests**
   - Test against real PostgreSQL (Testcontainers)
   - Verify LINQ projections
   - **Effort:** M

**Estimated Effort:** S-M (setup) + ongoing

---

### 🟡 HIGH Priority Issues

#### Issue #4: God Services

**Symptom:**
- ChifaService.cs: 535 lines, 22 public methods
- StatisticsService.cs: 304 lines, 8 public methods
- Hard to navigate, test, maintain

**Root Cause:**
- No feature-based slicing
- All queries in one service
- Grew organically without refactoring

**Risk Level:** MEDIUM  
**Impact:** Maintainability, Cognitive Load

**Suggested Fix:**

1. **Option A: Split by Aggregate**
   - FactureService, BordereauService, BeneficiareService, MedicamentService
   - Each owns its domain
   - **Effort:** M-L
   - **Benefits:** Clear boundaries

2. **Option B: Split by Feature (Vertical Slice)**
   - Each feature gets its own service class
   - Example: GetFacturesWithDetails/GetFacturesWithDetails.Service.cs
   - **Effort:** L
   - **Benefits:** High cohesion, scalable

3. **Option C: Partial Classes**
   - Split ChifaService into ChifaService.Factures.cs, ChifaService.Bordereaux.cs
   - Quick organization win
   - **Effort:** S
   - **Benefits:** Easy, maintains compatibility

**Estimated Effort:** S-M (Option C: 2 days)

---

#### Issue #5: No Validation Layer

**Symptom:**
- Invalid data can reach database
- No input sanitization
- No DTO validation

**Root Cause:**
- No validation framework
- Trust all inputs
- No defensive coding

**Risk Level:** MEDIUM  
**Impact:** Data Integrity, Security

**Suggested Fix:**

1. **Option A: FluentValidation**
   - Add FluentValidation package
   - Create validators for DTOs
   - Validate in application/service layer
   - **Effort:** M
   - **Benefits:** Industry standard, powerful

2. **Option B: Data Annotations**
   - Add [Required], [Range], etc. to DTOs
   - Validate manually in services
   - **Effort:** S
   - **Benefits:** Lightweight, built-in

**Estimated Effort:** S-M

---

#### Issue #6: Shared State in Period

**Symptom:**
- Period.MinDate/MaxDate are static (Period.cs:7-8)
- Period instance is property of singleton service (ChifaService.cs:10)
- Confusion between instance and static state

**Root Cause:**
- MinDate/MaxDate loaded once at startup (ChifaService.cs:532-533)
- Shared across all requests
- Mixed design (instance + static)

**Risk Level:** MEDIUM  
**Impact:** Confusion, Potential Bugs

**Suggested Fix:**

1. **Option A: Make Period Fully Immutable**
   - Remove static properties
   - Pass Period explicitly everywhere
   - Load min/max once in config
   - **Effort:** M
   - **Benefits:** Thread-safe, clear semantics

2. **Option B: Separate Range from Filter**
   - Create DateRange (min/max) as separate concept
   - Keep Period as filter only
   - **Effort:** S
   - **Benefits:** Clear separation of concerns

**Estimated Effort:** S (2-3 days)

---

### 🟢 MEDIUM Priority Issues

#### Issue #7: No Caching

**Symptom:**
- Every request queries database
- Reference data (Formes, Centres, Specialites) fetched repeatedly

**Root Cause:**
- No caching infrastructure
- Focus on correctness over performance

**Risk Level:** LOW  
**Impact:** Performance (not critical for desktop app)

**Suggested Fix:**

1. **Add Memory Cache for Reference Data**
   - Cache Formes, Centres, Specialites, Utilisateurs
   - 1-hour sliding expiration
   - **Effort:** S

**Estimated Effort:** S (1 day)

---

#### Issue #8: External Package Dependency (CHIFA2.Data)

**Symptom:**
- ChifaDb and DataModel (Facture, Bordereau, etc.) come from external package
- Limited control over schema
- Contract layer depends on external models

**Root Cause:**
- Database entities generated/maintained separately
- Package reference: CHIFA2.Data 4.3.8

**Risk Level:** MEDIUM  
**Impact:** Dependency on external maintenance

**Suggested Fix:**

1. **Option A: Keep External, Map to Internal**
   - Create internal domain models
   - Map DataModel to domain models in DAL
   - Contract layer depends on internal models
   - **Effort:** L
   - **Benefits:** Full control, isolation

2. **Option B: Generate Internally**
   - Use LinqToDB T4 templates to generate models locally
   - Remove CHIFA2.Data dependency
   - **Effort:** M
   - **Benefits:** Independence

3. **Option C: Accept Dependency**
   - If CHIFA2.Data is stable and maintained, keep it
   - **Effort:** None
   - **Risk:** Vendor lock-in

**Estimated Effort:** M-L or accept current state

---

#### Issue #9: No Resilience Patterns

**Symptom:**
- Single DB connection failure stops app
- No retry logic
- No circuit breaker

**Root Cause:**
- Desktop app assumption (single user)
- PostgreSQL connection failures handled at app level only

**Risk Level:** LOW  
**Impact:** User Experience (rare but impactful)

**Suggested Fix:**

1. **Add Polly for Resilience**
   - Retry policy (3 attempts)
   - Circuit breaker for DB
   - **Effort:** S

**Estimated Effort:** S (1 day)

---

#### Issue #10: Service Interface Couples to Database Schema

**Symptom:**
- ChifaService methods accept `Expression<Func<Facture, bool>>`
- Callers must reference DataModel
- Cannot serialize expressions over wire (unused in client-server scenario?)

**Root Cause:**
- Interface designed for local calls, not distributed
- LinqToDB expressions exposed in contract

**Risk Level:** MEDIUM  
**Impact:** Coupling, Maintainability

**Note:** Services are accessed directly via singleton instances.

**Suggested Fix:**

1. **Option A: Keep Direct Access (Current Design)**
   - Remove CHIFA.Server project
   - Keep direct singleton access
   - **Effort:** S

2. **Option B: Fix Service Design**
   - Create request/response DTOs
   - Remove Expression parameters
   - Use filter objects instead
   - **Effort:** M-L

**Estimated Effort:** S-M (depends on investigation)

---

## 3. Dependency Injection Migration Notes

**Current Pattern:**
```csharp
// ChifaService.cs
public class ChifaService
{
    private static ChifaService? _instance;
    public static ChifaService Instance => _instance ??= new();
}

// Usage in FacturesUC.cs
await ChifaService.Instance.GetAllFacturesAsync(...)
```

**Target Pattern (Option A: DI):**
```csharp
// Program.cs
var services = new ServiceCollection();
services.AddSingleton<ChifaService>();
var provider = services.BuildServiceProvider();

// FacturesUC.cs
private readonly ChifaService _chifaService;
public FacturesUC(ChifaService chifaService)
{
    _chifaService = chifaService;
}
```

**Challenge:** WinForms UserControls instantiated by designer, not DI.

**Solution:** Service Locator Bridge
```csharp
public static class ServiceLocator
{
    public static IServiceProvider Provider { get; set; }
}

// In view
_chifaService = ServiceLocator.Provider.GetRequiredService<ChifaService>();
```

---

## 4. Code Quality Observations

### Positive Patterns

1. **Async/Await Best Practices**
   - ConfigureAwait(false) in library code (30 usages)
   - Proper cancellation token usage (FacturesUC.cs)
   - Debounced reloads with SemaphoreSlim

2. **Resource Disposal**
   - `await using var db = new ChifaDb()`
   - SemaphoreSlim disposal in Disposed event

3. **Expression Composition**
   - PredicateBuilder.And/Or extension methods
   - Reusable query building

4. **Separation of Concerns (Partial)**
   - DTOs separate from entities
   - Contract layer shared between projects

### Negative Patterns

1. **Magic Strings**
   - File paths in AppSettings
   - Environment variable names scattered

2. **Mixed Responsibilities**
   - Period class (instance + static)
   - XtraHelper (logging + data loading + network scanning)

3. **No Interfaces for DAL**
   - ChifaService directly used everywhere
   - Cannot mock for testing

---

## 5. Technical Debt Summary

| Category | Debt Level | Remediation Cost | Priority |
|----------|------------|------------------|----------|
| No Dependency Injection | HIGH | M | 1 |
| Business Logic in UI | HIGH | M-L | 2 |
| No Test Coverage | HIGH | M (setup) + ongoing | 3 |
| God Services | MEDIUM | M | 4 |
| No Validation | MEDIUM | S-M | 5 |
| Shared State (Period) | MEDIUM | S | 6 |
| No Caching | LOW | S | 7 |
| No Resilience | LOW | S | 8 |

**Total Estimated Effort:** 3-4 weeks for Phases A+B (see REFACTOR_PLAN.md)

---

## 6. Security Observations

1. **Credentials in Code** (AppSettings.cs:31-33)
   - DB password in environment variable or default
   - Should use secure storage (Windows Credential Manager)

2. **No Input Validation**
   - SQL injection risk (mitigated by LinqToDB parameterization)
   - But no business rule validation

3. **Logging Sensitive Data**
   - Exception logging may include sensitive info
   - Review log sanitization

---

## 7. Performance Observations

1. **No N+1 Queries** (Good)
   - Proper use of Select projections
   - Eager loading where needed

2. **Large Result Sets**
   - GetAllFacturesAsync may return thousands of rows
   - Consider pagination

3. **UI Thread Blocking**
   - Good use of async/await
   - ConfigureAwait pattern applied

---

## 8. Recommendations Summary

### Immediate (Phase A - 1-2 days)
1. Add test project infrastructure
2. Extract Period static state to configuration
3. Document service usage

### Short Term (Phase B - 1-2 weeks)
4. Introduce DI container (Microsoft.Extensions.DependencyInjection)
5. Split ChifaService using partial classes
6. Add validation layer (FluentValidation or Data Annotations)
7. Extract predicate building to query builders

### Medium Term (Phase C - 2-4 weeks)
8. Create application service layer
9. Add caching for reference data
10. Write integration tests
11. Add resilience policies (Polly)

### Long Term (Future)
12. Consider Vertical Slice Architecture for new features
13. Evaluate service access patterns
14. Migrate to feature folders

---

## Conclusion

CHIFA Pro has a **solid foundation** with good async patterns, proper disposal, and clean data access. The main issues are:

1. **Testability** (no DI, tight coupling)
2. **Separation of Concerns** (business logic in UI)
3. **Service Design** (god classes, singletons)

These can be addressed **incrementally** without a rewrite. The refactor plan prioritizes **quick wins** (testing, validation) followed by **structural improvements** (DI, service splitting).

The codebase is maintainable and shows good .NET practices overall. With focused refactoring over 3-4 weeks, it can reach **8/10** architecture quality.

---

**Next Steps:** Review REFACTOR_PLAN.md for staged implementation.
