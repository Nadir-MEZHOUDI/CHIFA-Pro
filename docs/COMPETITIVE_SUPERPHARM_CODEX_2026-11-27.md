# Super Pharm: Competitive Review and Recommendations

**Date:** 27 November 2026  
**Products reviewed:** SmartCHIFA (CHIFA.Pro) and SmartPharm  
**Competitor:** Super Pharm / SuperSoft

## Executive conclusion

Super Pharm's strongest advantage is its ecosystem. It attracts pharmacists with a free CHIFA companion, a public medicines database, and professional services, then offers a paid pharmacy management system.

Position SmartCHIFA as an easy way for pharmacists to understand and manage CHIFA, and use it as a bridge to SmartPharm as the complete pharmacy management system. Much of the necessary functionality already exists in the projects; usability, reliability, and presentation should be assessed before building duplicate features.

## Scope and limitations

This review draws on the competitor's public website and a limited source-code review of the two projects. The competitor's feature descriptions are its own claims; its software was not independently tested. Its [Facebook page](https://www.facebook.com/Super.Pharm.2018) could not be reliably accessed, so its posts, engagement, and advertising performance were not assessed. Source-code presence does not establish that a feature is complete or production-ready.

## Competitor offer

| Area | Public offer | Implication |
| --- | --- | --- |
| Free CHIFA companion | Android app for invoices, bordereaux, CNAS receivables, medical control, and dispensing. A local Windows service reads CHIFA data and pairs with the phone by QR code. | Direct competition for SmartCHIFA, particularly mobile access and installation simplicity. |
| Pharmacy management | Advertises offline counter operations, CHIFA, stock, expiry, purchasing, cash, reports, and employee permissions. | SmartPharm already covers many of these areas; compare real workflows and reliability before duplicating features. |
| Remote access and alerts | Mobile, web, Telegram alerts, and activity reports, depending on the plan. | Demonstrate the most useful existing functions using concrete examples. |
| Public medicines catalogue | Search by commercial name, DCI, or barcode, with equivalents, reference prices, and changes. | An acquisition channel for pharmacists who are not yet ready to buy software. |
| Customer acquisition | Public pricing, a 30-day trial, and a detailed online guide. | Reduce uncertainty about pricing, installation, support, and first use. |

Sources: [Super CHIFA OFFICINE](https://supersoft-dz.com/super-chifa.html), [Super Pharm pricing](https://supersoft-dz.com/tarifs), [medicines catalogue](https://supersoft-dz.com/medicaments), and [user guide](https://supersoft-dz.com/guide).

The competitor's free CHIFA app page describes manual account approval and Android closed-test enrollment; it also says its Windows installer is not yet signed. These could create an opportunity for a smoother installation, provided our own onboarding is tested with new users.

## Current product capabilities observed

### SmartCHIFA / CHIFA.Pro

The repository contains chronic-patient forecasting, early-refill analysis, rejection handling, psychotropics, patient statistics, and mobile pages. Mobile notifications include chronic-patient reminders, rejected payments, and configurable patient or drug watchlists. The PC agent needs to be running for remote access.

### SmartPharm

The project documentation describes sales, CHIFA/CNAS/CASNOS, stock and expiry, purchasing, debts, cash reconciliation, backups, Telegram, and a web workspace. The source also contains invoice extraction, generic substitution, mobile, and fiscal invoice modules. Their presence does not establish completeness or production readiness.

The separate DrugSafety project has a foundation for curated drug information and offline distribution; its backlog identifies remaining classification work. This could differentiate the products if data quality, provenance, and limitations are clear.

## Recommended priorities

### 1. Make SmartCHIFA useful in the first 10 minutes

- Guide the user through CHIFA detection, connection testing, and phone pairing.
- Show connection state clearly: connected, PC unavailable, or data last updated at a specific time.
- Put actionable items on the home screen: rejected invoices, outstanding bordereaux, and chronic patients due soon.
- Extend the existing chronic-patient forecast with follow-up outcomes such as called, unavailable, treatment changed, and completed. Measure actual use.
- Consider a useful free tier and paid advanced reporting or automation. Decide the boundary after testing demand.

**Measure:** percentage of new users who connect and see a useful report without assistance.

### 2. Sell SmartPharm through proven daily workflows

Demonstrate and benchmark these scenarios:

1. Receive a supplier invoice and verify its lines.
2. Sell a product and process a CHIFA prescription.
3. Find an equivalent and check stock and expiry.
4. Close the cash session and inspect discrepancies.
5. Restore a backup on another machine.

Measure time, clicks, errors, and required assistance; include network interruption and restart tests. Verify invoice-extraction and substitution accuracy and release readiness before advertising them.

**Measure:** time per workflow and support interventions during a pilot.

### 3. Improve trust and installation

- Sign the Windows installer and updates, and make the publisher identity visible.
- Test backup restoration, not just backup creation.
- Explain exactly what data stays locally, what passes through servers, what is retained, and for how long.
- Verify the architecture before publishing privacy claims. Existing website language suggesting data never leaves the pharmacy needs checking against remote access, where data may transit to a phone.

### 4. Build a public pharmacist resource

Start a searchable Algerian medicines catalogue with commercial name, DCI, form, available equivalents, sources, and update dates. Add change alerts once the data pipeline is dependable. DrugSafety could later supply professionally reviewed safety information, with source, last review date, and limitations displayed clearly.

### 5. Make the offer easier to understand

At the time of review, the competitor publicly listed annual **HT** prices of 20,000 DA for Essentiel, 29,000 DA for Pro, and 45,000 DA for Premium, each for one PC, plus charges for additional PCs. It also advertised perpetual licences and a 30-day trial. These are published prices, not proof of actual transaction prices. See [pricing](https://supersoft-dz.com/tarifs).

Clearly show on our site:

- What SmartCHIFA does and what is free.
- What SmartPharm adds.
- Price per pharmacy and number of included PCs.
- Installation, support, updates, and backup terms.
- What happens if a subscription ends.
- A short demo and a direct way to request installation.

If a shorter trial is retained, ensure a pharmacist can reach a meaningful result within it.

## Proposed 30-day sequence

| Period | Action | Deliverable |
| --- | --- | --- |
| Week 1 | Test SmartCHIFA installation and the five SmartPharm workflows with 3–5 pharmacists. | Ranked list of failures, delays, and confusing screens. |
| Week 2 | Fix the largest onboarding and reliability issues; verify restoration and remote-access wording. | Repeatable installation and recovery process. |
| Week 3 | Improve the SmartCHIFA dashboard and record short, real demonstrations. | Three videos: CHIFA monitoring, chronic patients, and mobile access. |
| Week 4 | Publish clear product pages and pricing; pilot the revised offer. | Measurable trial signups, activations, and support requests. |

## Facebook advertising ideas

Test three distinct angles using real screenshots and only verified release features:

1. Know what CNAS still owes you.
2. Find patients whose chronic treatment is due.
3. Follow your pharmacy from your phone.

These are proposed creative angles, not conclusions drawn from the competitor's Facebook page.

## Next step

Conduct a hands-on audit of actual screens and installation flows, then convert the findings into an implementation backlog for both applications.
