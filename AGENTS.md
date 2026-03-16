# AGENTS.md - CHIFA Pro Codebase Guide

## Project Overview

CHIFA Pro is a healthcare management desktop application with the following architecture:
- **CHIFA Pro**: WinForms client application
- **CHIFA.Server**: WPF gRPC server
- **CHIFA.DAL**: Data Access Layer using LinqToDB
- **CHIFA.Contract**: DTOs, interfaces, and shared utilities

**Tech Stack**: .NET 10, PostgreSQL, LinqToDB, gRPC (protobuf-net), DevExpress WinForms, Serilog, Velopack

---

## Build Commands

```bash
# Build entire solution
dotnet build "CHIFA Pro.sln"

# Build in Release mode
dotnet build "CHIFA Pro.sln" -c Release

# Build specific project
dotnet build "CHIFA Pro/CHIFA.Pro.csproj"
dotnet build "CHIFA.Server/CHIFA.Server.csproj"
dotnet build "CHIFA.DAL/CHIFA.DAL.csproj"
dotnet build "CHIFA.Contract/CHIFA.Contract.csproj"

# Restore dependencies
dotnet restore "CHIFA Pro.sln"

# Clean build artifacts
dotnet clean "CHIFA Pro.sln"

# Publish for deployment
dotnet publish "CHIFA Pro/CHIFA.Pro.csproj" -c Release -r win-x64 --self-contained
dotnet publish "CHIFA.Server/CHIFA.Server.csproj" -c Release -r win-x64 --self-contained
```

## Test Commands

```bash
# No test project exists currently
# When adding tests, follow this pattern:
dotnet test                                    # Run all tests
dotnet test --filter "FullyQualifiedName~TestName"  # Run specific test
dotnet test -c Release                         # Run in Release mode
```

## Lint/Analysis Commands

```bash
# .NET analyzers are enabled via EnableNETAnalyzers in Directory.Build.props
# Build output includes analyzer warnings

# Format code (if editorconfig is added)
dotnet format "CHIFA Pro.sln"
```

---

## Project Structure

```
CHIFA Pro/
├── Directory.Build.props     # Global build settings (target framework, nullable, etc.)
├── CHIFA Pro/                # WinForms client
│   ├── Views/                # UserControls and Forms (UI layer)
│   ├── Helpers/              # Navigation, logging extensions, settings
│   └── GlobalUsings.cs       # Common imports for client
├── CHIFA.Server/             # WPF gRPC server
│   ├── Helpers/              # GrpcServer, SingleInstance, UpdateService
│   └── Views/                # WPF MainWindow
├── CHIFA.DAL/                # Data Access Layer
│   └── DataServices/         # ChifaService, StatisticsService (Singleton pattern)
├── CHIFA.Contract/           # Shared contracts
│   ├── Dtos/                 # Data Transfer Objects
│   ├── Grpc/                 # Service interfaces (IChifaService, IStatisticsService)
│   ├── Helpers/              # PredicateBuilder, extension methods, thresholds
│   └── Statistics/           # Statistics DTOs
```

---

## Code Style Guidelines

### Imports and Global Usings

Each project has a `GlobalUsings.cs` file. Add commonly used imports there:

```csharp
// Pattern used in GlobalUsings.cs
global using System.Linq.Expressions;
global using CHIFA.Contract.Dtos;
global using CHIFA.Contract.Helpers;
```

**Do NOT** add imports to individual files that are already in GlobalUsings.cs.

### Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `ChifaService`, `FactureDto` |
| Interfaces | IPascalCase | `IChifaService`, `INavigable` |
| Methods | PascalCase | `GetAllFacturesAsync` |
| Properties | PascalCase | `NumFact`, `Montant` |
| Private fields | _camelCase | `_instance`, `_server` |
| Constants | PascalCase | `HighPriceThreshold` |
| UserControls | PascalCase with `Uc` suffix | `HomeUc`, `FacturesUc` |

**Note**: Some files have inconsistent naming (e.g., `assuresUC.cs`, `borderauxUC.Designer.cs`). New files should follow PascalCase consistently.

### File Organization

- One class per file
- File name matches class name
- Designer files for WinForms: `MyControl.cs` + `MyControl.Designer.cs`

### Async/Await Patterns

```csharp
// CORRECT: Use ValueTask for async methods returning data
public async ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(...)

// CORRECT: Use await using for IAsyncDisposable resources
await using var db = new ChifaDb();

// CORRECT: Use ConfigureAwait(false) in library code
await db.Factures.ToListAsync().ConfigureAwait(false);

// CORRECT: Fire-and-forget with explicit discard (in UI event handlers only)
_ = LoadDataAsync();

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

### Service Pattern (Singleton)

Services use the singleton pattern (to be refactored to DI later):

```csharp
public class ChifaService : IChifaService
{
    private static ChifaService? _instance;
    public static ChifaService Instance => _instance ??= new();
    
    // Instance properties
    public Period Period { get; } = new();
}
```

### Expression Predicates

Use the `PredicateBuilder` for building complex queries:

```csharp
// Combine predicates with And/Or extensions
predicate = predicate.And(x => x.DateFact > yearAgo);
predicate = predicate.Or(x => x.Ts == true);

// Set period using the extension method
predicate = predicate.SetPeriod(period);
```

### DTOs

```csharp
public class FactureDto
{
    public string? NumFact { get; init; }  // init for immutable properties
    public DateTime? DateFact { get; set; }
    public decimal? Montant { get; set; }
    
    // Computed properties are acceptable
    public string? Time => DateFact.HasValue 
        ? TimeOnly.FromDateTime(DateFact.Value).ToShortTimeString() 
        : "";
}
```

### Error Handling

```csharp
// Use the Log extension method for exceptions
catch (Exception ex)
{
    ex.Log();
}

// In Program.cs, set up global handlers:
AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
Application.ThreadException += ThreadException_Handler;
Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
```

### Business Constants

Use `MedicalThresholds` class for magic numbers:

```csharp
// CHIFA.Contract/Helpers/MedicalThresholds.cs
public static class MedicalThresholds
{
    public const decimal HighPriceThreshold = 1000m;
    public const int HighQuantityThreshold = 3;
    public const int MediumTreatmentDurationDays = 30;
    public const int LongTreatmentDurationDays = 60;
    public const int VeryLongTreatmentDurationDays = 80;
}
```

---

## UI Patterns (WinForms)

### Navigation

```csharp
// UserControl must implement INavigable
public partial class HomeUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "HOME";
    public Image Image => FrmMain.Image(0);
}

// Navigate using extension method
this.NavigateTo<FacturesUc>();
```

### Data Loading

```csharp
private async Task ReLoadDataAsync()
{
    try
    {
        Cursor = Cursors.WaitCursor;
        var data = await Task.Run(async () => 
            await StatisticsService.Instance.GetThisWeekStatsAsync());
        bindingSource.DataSource = data;
        Cursor = Cursors.Default;
    }
    catch (Exception ex)
    {
        ex.Log();
    }
}
```

---

## gRPC Service Pattern

```csharp
// Interface in CHIFA.Contract/Grpc/
public interface IChifaService
{
    ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(
        bool? last = false, 
        bool? ts = false, 
        Period? period = null, 
        Expression<Func<Facture, bool>>? predicate = default);
}

// Implementation in CHIFA.DAL/DataServices/
public class ChifaService : IChifaService
{
    public async ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(...)
    {
        await using var db = new ChifaDb();
        return await db.Factures.Where(predicate).ToListAsync();
    }
}
```

---

## Logging

Serilog is configured in Program.cs:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Usage
Log.Information("Server started successfully");
Log.Error(ex, "Failed to start server");
```

---

## Database Access (LinqToDB)

```csharp
await using var db = new ChifaDb();

// Query with projections to DTO
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

1. **Target Framework**: .NET 10 Windows (configured in Directory.Build.props)
2. **Nullable**: Enabled project-wide - use `?` for nullable types
3. **Implicit Usings**: Enabled - common namespaces are auto-imported
4. **No Tests**: Project currently has no test project
5. **Culture**: Application uses `fr-FR` culture by default
6. **Single Instance**: Both client and server enforce single instance via `SingleInstance` class
