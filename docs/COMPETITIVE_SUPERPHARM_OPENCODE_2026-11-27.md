# Competitive dossier — Super Pharm (SuperSoft DZ)

**Date:** 27 November 2026
**Author:** OpenCode (DeepSeek V4.1 Flash) — session of 27-11-2026
**Companion report:** `COMPETITIVE_SUPERPHARM_CODEX_2026-11-27.md` (same folder, independent review)

Scope: public website `supersoft-dz.com`, public Facebook page `Super.Pharm.2018`, public update channels, and our own two products (`CHIFA.Pro` / SmartCHIFA, `SmartPharm`).
Purpose: decide what to change in **SmartCHIFA** and **SmartPharm**.

> Method: the Facebook page was read from a logged-in browser session (public page only). Site data comes from the
> sitemaps, the 207-procedure guide, the pricing configurator and the release endpoints. No competitor code was accessed.
> Where a claim about our own apps is uncertain it is marked **(?)**, not asserted.

---

## 1. Who they are

| Item | Value |
|---|---|
| Brand | Super Pharm (SuperSoft) |
| Site | supersoft-dz.com — nginx/Ubuntu, server-rendered, **French only** |
| Facebook | "Super pharm - Logiciel DZ", **10K followers**, page created 2018 |
| Contact | +213 550 34 38 27 · contact@supersoft-dz.com |
| Desktop build | `SuperPharm 1.0.806`, **162 MB** package (Squirrel/Velopack `RELEASES`) |
| Mobile agent | `SuperChifaOfficine-Agent 0.1.8`, ~48 MB Windows service |
| Free product | "Super CHIFA OFFICINE" — Android app + agent reading the CHIFA DB read-only |
| Trial | 30 days, no card |
| Claim | "Logiciel d'officine nouvelle génération" + "place de marché des officines" |

They run **four surfaces at once**:

1. Desktop ERP (sales, stock, purchasing, CHIFA, treasury, payroll, statistics, accounting).
2. Free mobile reader (Android + Windows agent) that reads CHIFA.
3. Public data (11.7k medicines, 1.7k molecules, CNAS reference prices).
4. Network products (wholesaler offers, parapharmacie shop, job board, training).

Group 4 is where they look biggest and are actually empty. Group 3 is the real moat.

---

## 2. The content moat

Their sitemap **is** the strategy:

| Sitemap | URLs |
|---|---:|
| `pages.xml` | 43 |
| `medicaments-1/2/3.xml` | 5,000 + 5,000 + 1,752 = **11,752** |
| `dci.xml` | **1,776** |
| `boutique-1/2.xml` | 7 |
| `emploi-1.xml` | 6 |
| **Total** | **≈ 13,584** |

- Every drug page has `<title>`, canonical, Open Graph and **two JSON-LD blocks**.
- Content: commercial name, DCI, form/dosage, reference tariff (TR), reimbursement, generics.
- Freshness: "Mis à jour le 27/11/2026", plus a "new / withdrawn" feed.
- The intent captured is not "logiciel pharmacie": it is every pharmacist and student typing a **drug name, a molecule, or a CNAS tariff**.

On top of that they published:

- a **207-procedure** application guide with the exact UI labels (install, sale, stock, CHIFA, treasury, employees, statistics, settings, formulas);
- a **30-entry "Toutes les formules"** chapter: PPA, SHP, UG/ristourne, landed cost, timbre, CHIFA reimbursement, majorations, cash theory, ABC, G50, SCF.

That does three jobs: **SEO**, **pre-sale trust** ("no black box"), **support deflection**.

Our public surface today:

| Site | Type | Indexed URLs | Notes |
|---|---|---:|---|
| smartchifa.smartappdz.org | static one-pager FR + AR | **2** | good copy, screenshots, pricing, install steps |
| smartpharm.cc | Blazor **WASM SPA** | **0** (sitemap 404) | AR/FR/EN, /mobile /download /payment /wiki /demo /go, but **one `<title>` for every route** |

**Read:** we are invisible exactly where they are strong. This is the cheapest gap to close, and it compounds.

---

## 3. Pricing (public, HT)

| Plan | Buy once | Subscription | What is gated |
|---|---:|---:|---|
| Start / Essentiel | 79,000 DA (1 post) | 20,000 DA/yr + 10,000/post | no mobile, no portal, no interactions, no employees, no exchanges, no shop |
| Pro | 114,000 DA (2 posts) | 29,000 DA/yr + 13,000/post | mobile, rejets, employees, exchanges, shop |
| Max / Premium | 199,000 DA (3 posts) | 45,000 DA/yr + 15,000/post | web portal, multi-pharmacy, 1,000 AI invoices/month/post |

Add-ons and fine print they publish:

- After year 1: service **15,000–22,500 DA/yr**; online services **+12,600–22,500 DA/yr**; AI optional.
- AI quotas **per post per month**: 15 / 100 / 1,000.
- Installation **and data migration on quote**.
- Buying within 12 months refunds the subscription paid.
- **One free year** for pharmacies opened less than a year ago.
- Prices HT; VAT added.

The page is a **configurator** (1–8 posts, buy vs subscribe side by side) plus a 15-question FAQ. It removes the "how much really?" objection before the first call.

**Our pricing:** SmartCHIFA 12,000 DA/yr or 50,000 DA lifetime, everything included (mobile included). SmartPharm: **"Contact us"** — hidden.

On the reader category we are ~40% below their entry plan and we include mobile, which their entry plan does not. On the ERP category we have **no public price at all**, which is worse than being expensive.

---

## 4. What they actually ship

From the 207 procedures and the pricing matrix:

**Counter**
Free sale, CHIFA sale, magistral preparations, returns, services, customer orders, split CASH/TPE, cash drawer, session open/close, variance, ticket reprint, fiscal invoice with TVA 19% / exonéré, timbre. Keyboard-first: F3 cash, F6 TPE, Esc, command palette.

**CHIFA**
CHIFA_OFFICINE windows embedded in the sale screen; card read; beneficiary; send; **sign with the pharmacist's biometric card**; blacklist detection. Instances with reasons (missing product, doctor/hospital stamp, scribble, medical control, emergency), phone-camera capture of prescription + ID, voice notes, transfer between instances. Rejets per line or whole prescription with reason, recalc, "mark corrected". Invoice import. Chronic projections + "chroniques non revenus" call list with outcomes. CNAS reconciliation (billed vs wired, per bordereau, unpaid). Read-only CHIFA journals.

**Stock / purchasing**
Lots, expiry, multi-dépôt, supplier entries, credit notes, exchanges, inventories, breakage, expired outputs, UG/ristourne landing cost, margin alerts, velocity/reorder-point/coverage, central purchasing with draft/sent orders.

**Back office**
21 statistics screens (5-axis CA, margin, ABC, rotation×margin, stock value/expiry, supplier debt, treasury variance, employee performance, cross-analysis). G50 prep, SCF balance, timbre register. Payroll (173.33 h, 9%, IRG entered manually). Granular permissions, audit log, PIN on sensitive actions.

**Platform**
Android mobile, web portal, multi-pharmacy consolidation, remote PC access, Telegram bot with quotas, AI invoice OCR, product by photo, e-shop, online reservation flow into "Commandes en ligne" with stock/price recheck at checkout.

This is a **complete ERP**. We should not pretend otherwise.

---

## 5. Where they are weak (evidence)

| Weakness | Evidence |
|---|---|
| **Marketplace is empty** | `/offres`: "0 produits · 0 offres · 0 grossistes" |
| **Shop is a demo** | `/boutique`: 3 products, "chez 1 officine" |
| **Job board is empty** | 6 ads, 1 offer / 5 CVs, all "il y a 1 mois" |
| **Training is vapour** | "Les premiers parcours arrivent bientôt" |
| **Mobile is Android-only and gated** | Play **closed test**, "devenir testeur" by email, Gmail with Algeria country, agent + QR |
| **No iOS** | their install page: "l'application, sur votre téléphone Android" |
| **Public web is French-only** | no `/ar`, no Arabic metadata; guide and tariffs FR |
| **Pricing is complex** | 3 tiers × buy/subscribe × per-post × service renewal × online services × AI quota |
| **Onboarding is gated** | supplier invoice photo required, then manual validation |
| **Social proof is thin** | 10K followers but **1 review**, 7 years old; posts at 15–40 likes |
| **Heavy installer** | 162 MB desktop package |
| **Docs admit defects** | bonus stored as amount instead of rate; cash "theoretical" overwritten; TVA on remise; two divergent margin formulas |
| **Server PC dependency** | "le serveur doit rester allumé", LAN clients, phone reads through it |
| **Mobile monetised twice** | bundled from Pro up, or via "Services en ligne + IA" after year 1 |

But one strength we must respect: **their reels work**.

- Page reels show **286K, 23K, 14K, 11K, 5.4K** views.
- Their best post ("Restez branchés avec votre pharmacie même si vous êtes absents") = **104 likes / 16 comments / 8 shares** — exactly the remote-access promise.
- They run paid boosts ("Learn More", "Book now" appear in the feed).
- Content formula: problem first (bordereaux lost to a virus, destocking by barcode, "0 errors, crazy speed"), 20–60 s screen recording, FR with some AR.

**Read:** distribution engine = short video + problem-first demos + paid boosts. The website is the trust layer and the SEO net.

---

## 6. Feature matrix (honest)

Legend: ✔ present · ◐ partial · ✗ absent · ? to verify.

| Capability | Super Pharm | SmartCHIFA | SmartPharm |
|---|:--:|:--:|:--:|
| POS / free sale / TPE split | ✔ | ✗ | ✔ |
| Stock, lots, expiry | ✔ | ✗ (by design) | ✔ |
| Purchasing, suppliers, orders | ✔ | ✗ | ✔ |
| Multi-dépôt | ✔ | ✗ | ✗ |
| Magistral preparations | ✔ | ✗ | ✗ |
| CHIFA card reader / signing | ✔ | reads DB | ✔ (Assist) |
| Rejets per line + recalc | ✔ | ✗ (`RejetDetailDto` exists, unused) | ◐ (bordereau level) |
| Bordereaux + CNAS reconciliation | ✔ | ✔ | ✔ |
| Chronic forecast / non-return call list | ✔ | ✔ | ◐ (report only) |
| Psychotrope legal register | ✔ | ✔ | ✔ (Annex 12/13 reports) |
| Drug–drug interactions | ✔ (Pro+) | ✗ | ✗ (removed 25-08-26, `OPEN_WORK.md:49`) |
| Payroll | ✔ | ✗ | ◐ payslip + attendance, no IRG/contracts |
| G50 / GL / SCF balance | ✔ | ✗ | ✗ (timbre done in `FiscalInvoiceService`) |
| Statistics depth | ✔ 21 screens | ✔ strong | ◐ analytics services + reports, unindexed |
| Granular roles / audit | ✔ | ✗ (one shared admin password) | ◐ Role/UserRole exist, only `IsAdmin` enforced |
| Multi-poste LAN | ✔ | ✗ (single PC reader) | ✔ |
| Mobile app | Android beta | **PWA iOS + Android** | PWA + web |
| Web portal | ✔ | ✔ (gateway) | ✔ |
| AI invoice OCR | ✔ | ✗ | ✔ |
| Telegram alerts | ✔ (quota) | ✗ (push instead) | ✔ |
| E-shop / online orders | ◐ 3 SKUs | ✗ | ✗ |
| Wholesaler offers marketplace | ✗ empty | ✗ | ✗ |
| Zakat | ✗ | ✗ | ✔ |
| DGI fiscal invoice (gapless, immutable) | ◐ | ✗ | ✔ |
| CIB / virement / don / mixed tender | ◐ (TPE split) | ✗ | ✔ |
| Generic substitution | ◐ | ✗ | ✔ |
| Multi-tenant portal / sync | ✔ | ✔ (gateway) | ✗ (retired 24-11-26) |
| Arabic desktop UI | ? (FR screenshots) | ✗ (desktop FR only) | ✔ |
| Public drug database (SEO) | ✔ 13.5k pages | ✗ | ✗ |
| Public procedure guide | ✔ 207 | ✗ | ◐ small wiki |
| Transparent public pricing | ✔ | ✔ | ✗ |
| Trial | 30 days | 7 days | 7 days |

**Where we already win:** iPhone/PWA with no store, no approval, no Gmail; Arabic-first web; Zakat; **DGI-compliant fiscal invoicing** (gapless numbering, immutable snapshots, avoir/proforma); CIB / virement / don / mixed tender; generic substitution; one price with mobile included; a reader that touches nothing in CHIFA.

**Where they win:** stock/purchasing depth, payroll, accounting, multi-dépôt, magistral, per-line rejets, granular rights, 13.5k SEO pages, 207 procedures, 30-day trial, transparent configurator.

**SmartPharm verdict (code audit):** genuinely complete POS/ERP — ~65 DbSets, 59 WPF feature folders, 71 Blazor pages, ~76–81 report slices, 4 test suites, CI, benchmarks, CLI. Its weakest axes are exactly the ones that win tenders: **accounting (no GL/G50/SCF), permissions (Role exists, only `IsAdmin` is checked), and per-line CHIFA rejets**. Note: multi-tenancy and cloud sync were **retired on 24–25 Nov 2026** (`docs/features/platform-website.md`), so the portal is now one-DB-per-pharmacy; `CONTEXT.md` is stale on this point.

**SmartCHIFA verdict (code audit):** 8 projects — WinForms/DevExpress + LinqToDB reading the existing `CHIFA_OFFICINE` PostgreSQL schema, plus a PWA, a SignalR agent and a gateway. It is a **read-only CHIFA companion + BI + mobile relay**, not an ERP: no POS, no card reader, no stock, no purchasing, no credit, no payroll, no accounting. That is a valid product. The marketing must say so.

### 6.5 Must-fix before spending a dinar on growth

Found in the audit, ordered by risk:

1. **Leaked credentials.** A hardcoded Azure storage key + SAS sits in `publish.bat:36-37` and `publish-local.bat:34`, while the README says credentials come from environment variables. They are in git history: **rotate now**.
2. **Fabricated margin.** `Montant / 6` in `Statistics/*.cs`. The product sells "decisions"; a fake margin is a credibility bomb. Compute it from purchase data (we don't own it) or remove the KPI.
3. **Overclaims on the live site.** Arabic desktop UI (`website/index.html:582,791`) and per-invoice rejection reasons (`:645`) do not exist. Fix the copy or implement the feature.
4. **Telemetry may carry patient data**, with no opt-out (`Program.cs:74-102`, `XtraHelper.cs:173-249`) — Algerian law 18-07 exposure. Scrub exception payloads, add an opt-out.
5. **Backup.** Manual `pg_dump`/`pg_restore` from the server PC only, no schedule, retention or pre-restore backup (`DbChecker.cs:311-408`). The competitor ships scheduled backup + restore.
6. **Compliance.** The psychotrope register has no prescription number and the invoice-number column is hidden (`PsychotropesUc.cs:151-153`). That is an inspection finding waiting to happen.
7. **Dead or unreachable code.** `PrevisionChroniquesUc` is not wired into navigation (while mobile has the relances list); `RejetDetailDto` is unused; `BtnDeleteFacture` has a handler but no button; `CHIFA.Stat Velopack.yml` points at a missing project; stray `nul` file.
8. **Ops.** Log path `../logs/log.txt` can resolve under System32 (`Program.cs:37`); the startup update can restart without asking (`frmMain.cs:303→389`); restore runs `pg_restore -c` with no confirmation.

**Data accuracy:** `ProductStat.Total = Qt × Max(PPA)` instead of `Sum(Mont)`; `BordStatDto.MontantFacture` and `DailyStat` divide by facture count (zero-facture risk). Small fixes, outsized trust value.

---

## 7. What to do — ranked

### P0.0 — close §6.5 first (days, no product decision)

Rotate the Azure key · remove or fix the fake margin · correct the two website claims · telemetry opt-out · schedule backups · add the prescription number to the psychotrope register · delete the dead code · fix the log path.

Everything below assumes this is done: growth multiplies whatever the product actually is.

### P0 — this week (funnel + distribution, near-zero code)

1. **30-day trial** in both apps (`License_StartTrial` / `LicenseGate` currently say 7).
2. **Public price on smartpharm.cc** — annual + lifetime + per-post calculator. "Contact us" loses the comparison before it starts.
3. **Give smartpharm.cc a sitemap and real per-route metadata** (title, description, canonical, OG, JSON-LD). Today every route says "SmartPharm Platform" and `sitemap.xml` is 404.
4. **Public changelog** ("Notes de version") on both sites — dated, one line per change. Proves velocity, kills the "is it maintained?" doubt.
5. **Reels, not posters.** 20–30 s screen recordings, FR + AR, same problem-first formula that works for them: a bordereau lost to a virus, a rejet recovered, 3-second invoice search, phone access. Their reels pull 10K–286K views; our static ads won't.
6. **Ask 10 friendly pharmacies for a Facebook recommendation + a 20 s video.** They have 1 review; ten would be a visible difference.

### P0.5 — engineering hygiene (days, no product decision)

These are real defects found in the audit, not nice-to-haves:

- **Secrets:** `ServiceTestBase` hard-codes a DB password (`AGENTS.md:105`) and `AppSettings.cs` ships default secrets (`AGENTS.md:94`). Remove and rotate.
- **Polling:** `SmartPharm.Assist` polls every second (`ChifaInjectionService`) — move to adaptive backoff.
- **DbContext pooling:** not enabled on a high-churn POS; enable `AddPooledDbContextFactory`.
- **Analytics in memory:** `P0AnalyticsService.cs:34-52` and `InventoryAlertService.cs:31-59` load whole lot/product tables into memory; `StatisticService` aggregates `SaleItems` per period. Use SQL aggregation + covering indexes (see the existing `docs/research/2026-11-25-slow-query-audit.md`).
- **Stale docs:** `CONTEXT.md` still describes multi-tenancy/sync removed on 24-11-26; `docs/wiki` is stale against `features/FEATURE_INDEX.md`.

### P1 — 2 to 4 weeks (highest leverage)

7. **Public drug/DCI catalogue** on smartchifa.smartappdz.org (and cross-linked from smartpharm.cc): 11,752 medicines + 1,776 molecules you already hold, server-rendered, FR + AR, canonical + JSON-LD + sitemap. **This is the single highest-ROI build in this document.**
8. **Public procedure guide**, generated from the app's own screens: 30 procedures first (install, connect CHIFA DB, bordereaux, rejets, psychotropes, mobile pairing), same labels as the UI.
9. **iPhone install page** (`/iphone`): 3 screenshots, "Ajouter à l'écran d'accueil", no store, no account approval.
10. **SmartCHIFA — interaction check on ordonnances.** First source/build a DCI interaction dataset (SmartPharm's is absent, see §10); then flag interactions on delivered meds with patient age. A read-only overlay that catches interactions is a cheap, loud differentiator.
11. **SmartCHIFA — wire `PrevisionChroniquesUc` (currently unreachable) and extend it into "chroniques non revenus"** with call outcomes. Mobile already has the relances list; the desktop screen exists but is not in navigation. Their guide documents the call-list workflow; it directly creates revenue for the pharmacy.
12. **SmartPharm — per-line rejets** (quantity, reason, recalc of caisse/assuré/majorations, "mark corrected"). Parity with both the competitor and SmartCHIFA.
13. **SmartPharm — enforce the role matrix.** `Role`/`UserRole` exist but only `IsAdmin` is enforced (`Features/Auth/AuthService.cs:285`). Add object × action permissions, then the audit UI on top of the existing `Audits`/`AuditService`.

### P2 — 1 to 3 months (depth)

14. **G50 / TVA / timbre pack** for the accountant, one-click PDF + Excel. Timbre is already computed; the rest is aggregation.
15. **Magistral preparations** (ingredients, cost, barcode, pending list at the counter).
16. **Multi-dépôt** (depot on lots and stock movements).
17. **Nightly pre-aggregation for statistics** + SQL aggregation for the heavy reads (`P0AnalyticsService`, `InventoryAlertService`, `StatisticsService`) instead of loading tables into memory. Our own benchmark shows `GetAllBordereaux` at **1.6 s** under LinqToDB vs 58 ms in Dapper.
18. **Lot-profit suggestion at CHIFA dispensing** (pick the lot that maximises the majoration/FAE envelope). They advertise it; we have the lot costs.
19. **Freemium answer to their free app**: a free read-only mobile tier for SmartCHIFA (dashboard + last invoices), paid for bordereaux/rejets/psychotropes/stats. Their free app is a funnel; ours should be too.
20. **Optional invoice-OCR prefill at signup** (like theirs) — but **never a gate**. Verify at payment, not at trial.

### Don't do (yet)

Wholesaler offers marketplace · parapharmacie marketplace · job board · training academy · multi-pharmacy consolidation.
All are empty on their side; entering now splits focus and buys us a cold-start problem.

---

## 8. Positioning lines to test

- **vs their ERP:** "Super Pharm remplace votre officine. SmartCHIFA la comprend." (reader, no migration, no risk)
- **vs their mobile:** "Sur iPhone comme sur Android, sans Play Store, sans liste d'attente."
- **vs their pricing:** "Un prix. Tout inclus. À vie si vous voulez."
- **vs their data story:** "Vos données ne quittent pas la pharmacie — l'app lit, elle ne copie pas."
- **vs their complexity:** "Pas de palier, pas de quota, pas de service à renouveler."

---

## 9. Numbers to watch (90 days)

| Metric | Today (estimate) | Target |
|---|---:|---:|
| Indexed public pages (both sites) | ~2 | 5,000+ |
| Trial → paid (SmartCHIFA) | unknown | +30% relative |
| Install → first CHIFA read | unknown | < 10 min |
| Facebook reviews | 0 | 10 |
| Reels published / month | 0 | 8 (4 FR, 4 AR) |
| Support tickets "how do I…" | unknown | −30% after guide |

---

## 10. Open questions

- Their installed base (paying pharmacies) — not public. 10K followers ≠ 10K customers; 1 review suggests low review culture.
- Is their published CNAS catalogue licensed or scraped? Legal exposure if scraped — do not copy it; use the official nomenclature.
- Do they have an Arabic app UI? Screenshots and guide are French.
- Their ad spend in DZ (Ad Library was not reachable in this session).
- Churn after year 1, when service + online fees land.
- SmartPharm: drug–drug interactions are **absent** (removed 25-08-26, `OPEN_WORK.md:49`); only pictorial contraindications remain in the external DrugSafety product. Item 10 is therefore "build/source a DCI interaction dataset", not "surface existing data".
