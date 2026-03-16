# CHIFA Pro

تطبيق CHIFA Pro هو نظام مكتبي لإدارة بيانات الرعاية الصحية، مبني على منصة .NET 10 ويعمل فوق PostgreSQL.

## نظرة عامة على الحل

يتكون المشروع من أربع وحدات رئيسية:

- `CHIFA Pro/`: عميل **WinForms** (الواجهة الرئيسية للمستخدم)
- `CHIFA.Server/`: خادم **WPF + gRPC**
- `CHIFA.DAL/`: طبقة الوصول للبيانات باستخدام **LinqToDB**
- `CHIFA.Contract/`: العقود المشتركة (DTOs, واجهات gRPC, Helpers)

## التقنيات المستخدمة

- .NET 10 (Windows)
- WinForms + DevExpress
- WPF
- gRPC (protobuf-net)
- PostgreSQL
- LinqToDB
- Serilog
- Velopack

## المتطلبات

- .NET SDK 10
- PostgreSQL
- نظام تشغيل Windows

## البناء والتشغيل

### بناء كامل الحل

```bash
dotnet build "CHIFA Pro.sln"
```

### بناء مشروع محدد

```bash
dotnet build "CHIFA Pro/CHIFA.Pro.csproj"
dotnet build "CHIFA.Server/CHIFA.Server.csproj"
dotnet build "CHIFA.DAL/CHIFA.DAL.csproj"
dotnet build "CHIFA.Contract/CHIFA.Contract.csproj"
```

### استعادة الحزم

```bash
dotnet restore "CHIFA Pro.sln"
```

### النشر (Release)

```bash
dotnet publish "CHIFA Pro/CHIFA.Pro.csproj" -c Release -r win-x64 --self-contained
dotnet publish "CHIFA.Server/CHIFA.Server.csproj" -c Release -r win-x64 --self-contained
```

## بنية المجلدات

```text
CHIFA.Pro/
├── CHIFA Pro/        # تطبيق WinForms
├── CHIFA.Server/     # خادم WPF + gRPC
├── CHIFA.DAL/        # Data Access Layer (LinqToDB)
└── CHIFA.Contract/   # DTOs, عقود, أدوات مساعدة
```

## ملاحظات مهمة

- التحليل الثابت (.NET Analyzers) مفعّل عبر `Directory.Build.props`.
- `Nullable` مفعّل على مستوى الحل.
- لا يوجد مشروع اختبارات مخصص حالياً.
- الثقافة الافتراضية في التطبيق هي `fr-FR`.
