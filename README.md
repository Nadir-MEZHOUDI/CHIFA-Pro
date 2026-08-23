# CHIFA Pro

<div align="center">

**Open-source pharmacy management suite for Windows** — a modern rewrite of the classic CHIFA officine workflow.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-blueviolet.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20x64-0078d4.svg)](https://www.microsoft.com/windows/)
[![CI](https://github.com/Nadir-MEZHOUDI/CHIFA-Pro/actions/workflows/ci.yml/badge.svg)](https://github.com/Nadir-MEZHOUDI/CHIFA-Pro/actions/workflows/ci.yml)
</div>

---

CHIFA Pro is a healthcare / pharmacy management desktop application. It keeps the familiar CHIFA officine
workflow (factures, bordereaux, patients, CNAS transactions, psychotropes tracking) while being rebuilt on a
modern stack: **.NET 10**, **PostgreSQL**, and **LinqToDB**.

> **عربي:** مشروع مفتوح المصدر لإدارة الصيدليات على ويندوز — واجهة WinForms مع خدمات مباشرة وقاعدة بيانات PostgreSQL.
> يهدف لمواصلة تطوير المشاركة المجتمعية وإبقاء العملية سهلة قدر الإمكان.

## ✨ Features

- **Direct service architecture**: WinForms client with direct service layer (CHIFA.Services via LinqToDB)
- **Complete officine workflow**: factures, bordereaux, patients, bénéficiaires, CNAS transactions
- **Psychotropes tracking** and controlled-substance reports
- **Rejects (rejets)** and anomalies audit view
- **Scope dashboard** with statistics (this week / this month / year)
- **Prevision chroniques** (chronic patients forecast) and refill forecasts
- **Backup / restore** integrated with the local PostgreSQL install
- **Automatic updates** via Velopack with delta updates
- **Single instance** enforcement on both client and server
- **Thread-safe data layer** with LinqToDB, async/await everywhere, `fr-FR` culture

## 🧱 Technology Stack

| Layer     | Technology                                        |
|-----------|---------------------------------------------------|
| Client    | WinForms + DevExpress (26.1+)                     |
| Data      | LinqToDB, Npgsql (PostgreSQL)                     |
| Logging   | Serilog                                           |
| Updates   | Velopack                                          |
| Tests     | xUnit, FluentAssertions, BenchmarkDotNet          |

### Solution layout

```text
CHIFA.Pro/
├── CHIFA Pro/         # WinForms client (UI)
├── CHIFA.Services/    # Services layer (DTOs, DataServices, LinqToDB)
├── CHIFA.Tests/       # xUnit tests (unit + DB functional + perf)
```

## ⚠️ Important licensing notice

- **DevExpress** is a **commercial** component library. Building the WinForms client requires an active
  DevExpress .NET subscription (the project uses `DevExpress.Win.Design`).
- A few internal packages (`CHIFA2.Data`, `SmartApp.Bridge`, `ReusableTheme`) are distributed separately on
  NuGet.org — they are being made public alongside this project.

## 📋 Requirements

- **Windows 10/11 (x64)**
- **.NET 10 SDK** (or the .NET 10 runtime for running published builds)
- **PostgreSQL 14+** (local instance, connection configured via environment variables)
- Optional: **DevExpress** subscription for building the client

## 🚀 Getting started

1. Clone the repository:

   ```bash
   git clone https://github.com/Nadir-MEZHOUDI/CHIFA-Pro.git
   cd CHIFA-Pro
   ```

2. Configure the database credentials **via environment variables** (never hardcoded):

   ```powershell
   $env:CHIFA_DB_PASSWORD = "your-strong-password"
   $env:PGPASSWORD        = "your-strong-password"   # backup/restore tooling
   ```

3. Build:

   ```bash
   dotnet build "CHIFA Pro.sln"
   ```

4. Run the client. The client shows a connection prompt on first launch.

## 🧪 Tests

```bash
# Unit tests only (no DB required)
dotnet test "CHIFA.Tests/CHIFA.Tests.csproj" --filter "Category!=DbFunctional&Category!=Perf"

# Full suite (requires a local PostgreSQL with data)
$env:CHIFA_TEST_DB_HOST = "localhost"
$env:CHIFA_TEST_DB_PWD  = "your-password"
dotnet test "CHIFA.Tests/CHIFA.Tests.csproj"
```

## 📦 Publish & updates

Velopack-based packaging scripts are included (`publish.bat`, `publish-local.bat`).
**Any storage credentials must be provided through environment variables** — never in the repo.

```powershell
$env:VPK_SAS = "sv=...&sig=..."   # SAS token or VPK_KEY
.\publish.bat
```

## 📚 Documentation

- [`docs/README.md`](docs/README.md) — original project overview (Arabic)
- [`AGENTS.md`](AGENTS.md) — codebase guide for contributors and AI agents
- `docs/*.md` — architecture review, phase reports, audit plans

## 🤝 Contributing

Contributions are welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for conventions and
[CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) for expected behaviour.

## 🛡️ Security

If you find a vulnerability, please **do not open a public issue** — report it via a private advisory
(see [SECURITY.md](SECURITY.md)).

## 💬 Community & support

- Issues & feature requests: [Issues](https://github.com/Nadir-MEZHOUDI/CHIFA-Pro/issues)
- Discussions: [Discussions](https://github.com/Nadir-MEZHOUDI/CHIFA-Pro/discussions)
- Pull requests: [Pull Requests](https://github.com/Nadir-MEZHOUDI/CHIFA-Pro/pulls)

## 📄 License

Released under the [MIT License](LICENSE).
