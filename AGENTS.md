# AGENTS.md - CHIFA Pro Codebase Guide

## Project Overview

CHIFA Pro is a healthcare management desktop application:
- **CHIFA Pro**: WinForms client (DevExpress UI)
- **CHIFA.DAL**: Data Access Layer (LinqToDB, PostgreSQL)
- **CHIFA.Contract**: DTOs, interfaces, shared utilities

**Tech Stack**: .NET 10, PostgreSQL, LinqToDB, DevExpress WinForms, Serilog, Velopack

---

## Build Commands

```bash
dotnet build "CHIFA.Pro.sln"                 # Build solution
dotnet build "CHIFA.Pro.sln" -c Release     # Release build
dotnet restore "CHIFA.Pro.sln"              # Restore dependencies
dotnet clean "CHIFA.Pro.sln"                 # Clean artifacts
dotnet publish "CHIFA Pro/CHIFA.Pro.csproj" -c Release -r win-x64 --self-contained
```

## Test Commands

```bash
dotnet test                                          # Run all tests
dotnet test --filter "Category!=DbFunctional&Category!=Perf" # Run single test
dotnet test -c Release                               # Release mode tests
```

## Lint/Format Commands

```bash
dotnet format "CHIFA.Pro.sln"                # Format all files
dotnet format "CHIFA.Pro.sln" --verify-no-changes  # Check formatting
```
.NET analyzers are enabled via `EnableNETAnalyzers` in Directory.Build.props.

---

## Project Structure

```
CHIFA Pro/
├── Directory.Build.props    # Global build settings
├── CHIFA Pro/               # WinForms client
│   ├── Views/               # UserControls (UI layer)
│   ├── Helpers/             # NavigationService, XtraHelper, extensions
│   └── GlobalUsings.cs
├── CHIFA.DAL/               # Data Access Layer
│   └── DataServices/        # Singleton services (ChifaService, StatisticsService)
└── CHIFA.Contract/          # Shared contracts
    ├── Dtos/                # Data Transfer Objects
    ├── Grpc/                # Service interfaces (IChifaService, IStatisticsService)
    ├── Helpers/             # PredicateBuilder, MedicalThresholds, extensions
    └── Statistics/          # Statistics DTOs
```

---

## Code Style Guidelines

### Global Usings

Each project has `GlobalUsings.cs`. **Do NOT** duplicate imports in source files:

```csharp
// CHIFA.Pro/GlobalUsings.cs
global using System.Net.Http;
global using CHIFA.Contract.Dtos;
global using CHIFA.Contract.Helpers;

// CHIFA.DAL/GlobalUsings.cs
global using System.Linq.Expressions;
global using LinqToDB;
global using DataModel;
```

### Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `ChifaService`, `FactureDto` |
| Interfaces | IPascalCase | `IChifaService`, `INavigable` |
| Methods | PascalCase | `GetAllFacturesAsync` |
| Properties | PascalCase | `NumFact`, `Montant` |
| Private fields | _camelCase | `_instance`, `_server` |
| Constants | PascalCase | `HighPriceThreshold` |
| UserControls | PascalCase + `Uc` suffix | `HomeUc`, `FacturesUc` |

### Async/Await Patterns

```csharp
// Use ValueTask for async methods returning data
public async ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(...)

// Use await using for IAsyncDisposable resources
await using var db = new ChifaDb();

// Use ConfigureAwait(false) in library code (DAL, Contract)
await db.Factures.ToListAsync().ConfigureAwait(false);

// Fire-and-forget in UI event handlers only
_ =LoadDataAsync();
```

### Error Handling

```csharp
// Always use the Log extension method in catch blocks
catch (Exception ex)
{
    ex.Log();
}

// Event handlers should have try-catch
private async void Button_Click(object sender, EventArgs e)
{
    try
    {
        await LoadDataAsync();
    }
    catch (Exception ex)
    {
        ex.Log();
    }
}
```

### Singleton Service Pattern

```csharp
public class ChifaService : IChifaService
{
    private static ChifaService? _instance;
    public static ChifaService Instance => _instance ??= new();    
    public Period Period { get; } = new();
}
```

### DTOs

```csharp
public class FactureDto
{
    public string? NumFact { get; init; }    // init for immutable
    public DateTime? DateFact { get; set; }
    public decimal? Montant { get; set; }    
    public string? Time => DateFact?.ToShortTimeString() ?? "";
}
```

### Expression Predicates

```csharp
// Use PredicateBuilder for complex queries
predicate = predicate.And(x => x.DateFact > yearAgo);
predicate = predicate.Or(x => x.Ts == true);
predicate = predicate.SetPeriod(period);
```

### Business Constants

Use `MedicalThresholds` for magic numbers:

```csharp
// Located in CHIFA.Contract/Helpers/MedicalThresholds.cs
public static class MedicalThresholds
{
    public const decimal HighPriceThreshold = 1000m;
    public const int HighQuantityThreshold = 3;
    public const int MediumTreatmentDurationDays = 30;
}
```

---

## UI Patterns (WinForms/DevExpress)

### Navigation

```csharp
// UserControl implements INavigable
public partial class HomeUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "HOME";
    public Image Image => FrmMain.Image(0);
}

// Navigate using extension method
this.NavigateTo<FacturesUc>();
```

### Data Loading Pattern

```csharp
private async Task ReLoadDataAsync()
{
    try
    {
        Cursor = Cursors.WaitCursor;
        var data = await Task.Run(async () => 
            await StatisticsService.Instance.GetThisWeekStatsAsync());
        bindingSource.DataSource = data;
    }
    catch (Exception ex)
    {
        ex.Log();
    }
    finally
    {
        Cursor = Cursors.Default;
    }
}
```

---

## Database Access (LinqToDB)

```csharp
await using var db = new ChifaDb();

var list = await db.Factures
    .Where(predicate)
    .Select(f => new FactureDto
    {
        NumFact = f.NumFact,
        DateFact = f.DateFact
    })
    .ToListAsync()
    .ConfigureAwait(false);
```

---

## Important Notes

1. **Nullable**: Enabled project-wide -use `?` for nullable types
2. **Implicit Usings**: Enabled - common namespaces auto-imported
3. **Culture**: Application uses `fr-FR` culture by default
4. **Single Instance**: Both client/server enforce single instance via `SingleInstance` class
5. **Target Framework**: .NET 10 Windows (`net10.0-windows`)