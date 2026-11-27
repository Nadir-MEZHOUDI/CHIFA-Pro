# Super Pharm vs SmartCHIFA & SmartPharm — competitive analysis

**Date:** 27 November 2026
**Author:** Claude Code (Claude Opus 5.5) — session of 27-11-2026
**Companion reports:** `COMPETITIVE_SUPERPHARM_CODEX_2026-11-27.md`, `COMPETITIVE_SUPERPHARM_OPENCODE_2026-11-27.md` (same folder, independent reviews)
**Live doc:** https://claude.ai/artifact/SxcxxhQo2KiyVLSAn2go9s

Super Pharm now gives away **Super CHIFA OFFICINE**, a free phone app built the same way as SmartCHIFA's agent and phone app, to pull pharmacies toward its paid point-of-sale software. SmartCHIFA should answer with a free basic tier and lead with what the free app lacks. SmartPharm should match Super Pharm's two headline features: AI invoice import and drug-interaction checks.

## The competitor at a glance

SuperSoft sells one main product, Super Pharm, and uses a free CHIFA companion app as the way in.

| Item | What they offer |
| --- | --- |
| Super Pharm (point of sale) | Windows pharmacy management: sales and CHIFA sales, stock by lot, purchases, clients and credit, statistics, multi-PC on a local network |
| Super CHIFA OFFICINE | **Free.** A Windows service reads the CHIFA database without changing it and shows CHIFA invoices, CNAS bordereaux, receivables and medical-control opinions on paired phones. Android app is in closed testing. |
| Purchase prices | Start 79,000 DA (1 PC) · Pro 114,000 DA (2 PCs) · Max 199,000 DA (3 PCs), then 15,000–22,500 DA per year, plus online services |
| Subscription prices | 20,000 / 29,000 / 45,000 DA per year for 1 PC; online services and AI +30% |
| Offers | 30-day free trial, no card. A free Premium year for pharmacies under one year old. Renewal price guaranteed for 3 years. |
| Headline features | AI import of supplier invoices from a photo or PDF; DCI interaction checks; Telegram alerts and a nightly report |
| Facebook | "Super pharm - Logiciel DZ": about 10K followers, 1 review, a WhatsApp button (0550 34 38 27). Short French and Arabic videos about #chifaofficine every 4–6 weeks, with 10–38 likes each. |
| Facebook topics | Free Super CHIFA app; protecting bordereaux from viruses and PC failures; following up chronic patients before their renewal dates; interactions in prescriptions; "ready for free and CHIFA sales" |

Prices exclude VAT and come from their pricing page.

## Feature comparison

SmartCHIFA already does more CHIFA analysis than their free app. SmartPharm trails Super Pharm on AI invoice import, interactions and the phone app.

| Feature | Super Pharm / Super CHIFA | SmartCHIFA | SmartPharm |
| --- | --- | --- | --- |
| CHIFA invoices and bordereaux on phone | Yes, free (Android, closed test) | Yes (web app, Android + iPhone) | Thin (Home, Stock, Debts, Stats only) |
| Pre-bordereau audit, rejets, psychotropes, contrôle médical, blacklist | Rejets and contrôle médical | Yes | Partly (bordereaux, CHIFA sync) |
| Chronic-patient renewal follow-up | Marketed on Facebook | Built (`PrevisionChroniquesUc`) but not in the menu | Planned (Telegram reminders) |
| Drug-interaction checks | Yes (DCI) | No | Blocked: no data source |
| AI supplier-invoice import | Yes, photo or PDF, monthly quota | n/a | Tesseract OCR |
| Alerts | Telegram, push, nightly report | Web push | Telegram |
| Backups | Scheduled local backup | Local backup and restore | Automatic local backups |
| Arabic interface | No (French only) | Website in Arabic | Arabic-first web app |
| Signed installer | No (SmartScreen warning) | No signing step found in the build pipeline | Not checked |
| Free trial | 30 days | 7 days | Not shown |
| Price shown publicly | Yes, detailed | 12,000 DA/yr or 50,000 DA lifetime | No (set in the admin database) |
| Parapharmacy shop, wholesaler offers | Yes | No | No |

## Where they are weak

Each weakness below is a message SmartCHIFA or SmartPharm can use in ads.

- **French only.** No Arabic interface or website.
- **Android only, and not released.** The phone app is in closed testing; iPhone is not mentioned.
- **Phone and portal cost extra.** The phone app, web portal and multi-pharmacy view are paid online services.
- **Unsigned installers.** Their own install guide warns about the Windows SmartScreen message.
- **Complicated pricing.** Tiers, per-PC add-ons, 30% renewals, +30% for online services and monthly AI quotas.
- **Little proof.** The site shows no phone number, client count, testimonials or case studies; Facebook has 1 review and low engagement.
- **Training is empty.** The training page says the first courses are "coming soon".
- **Installation and migration are quote-only.** No public prices.

## SmartCHIFA: prioritized actions

The free competitor app makes a basic invoice and bordereau viewer worth nothing on its own. SmartCHIFA has to win on analysis, iPhone support and Arabic.

- [ ] **Free basic tier.** Factures and bordereaux on the phone for free. Charge for pre-bordereau audit, rejets, psychotropes, chronic-patient forecasting, blacklist and alerts. Use the free tier to lead pharmacies to SmartPharm, as they do.
- [ ] **Chronic-patient refill reminders.** Add `PrevisionChroniquesUc` to the main menu, add a phone page with call and WhatsApp links, and send a daily push. A separate session is working on this.
- [ ] **"Android + iPhone, today" in every ad.** The phone app is a web app, so it already runs on iPhone; theirs is Android-only and unreleased.
- [ ] **Off-PC bordereau backup.** Their pitch is a local backup. The agent already keeps an outbound connection, so offer an encrypted offsite copy of the CHIFA database.
- [ ] **Sign the installer.** Buy a code-signing certificate and add signing to the Velopack pipeline, so users see no SmartScreen warning.
- [ ] **30-day trial** instead of 7 days, to match theirs.
- [ ] **Arabic marketing.** Short Arabic videos on Facebook every week, a WhatsApp button on the page, and customer testimonials.
- [ ] **Fix the licence text.** `README.md` and `LICENSE` call the product MIT open-source, which contradicts selling licences.
- [ ] **Check how a lifetime licence is issued.** The site sells 50,000 DA lifetime, but SmartApp.Licensing only issues licences with an end date.

## SmartPharm: prioritized actions

SmartPharm already matches most of Super Pharm's point-of-sale features. The gaps are the two features they advertise most, plus the phone app and public pricing.

- [ ] **Publish one simple price** on smartpharm.cc, phone app included. Their price list is detailed but complicated.
- [ ] **AI supplier-invoice import.** Replace Tesseract OCR with AI extraction from a photo or PDF. It is their headline feature, and SmartPharm already has the import flow to build on.
- [ ] **Drug-interaction alerts.** Use the `SmartPharm.DrugSafety` packs as the data source, if they hold interaction data, to unblock the item in `docs\OPEN_WORK.md`.
- [ ] **Bring SmartPharm.Mobile up to CHIFA.Mobile.** Port the factures, bordereaux, rejets and patient pages; both apps use the same agent and gateway design.
- [ ] **Importer from other pharmacy software,** including Super Pharm, so switching takes one step. Their migration help is quote-only.
- [ ] **An offer for new pharmacies,** for example a free first year for pharmacies under one year old, to match theirs.

## Not worth copying yet

Their parapharmacy shop, wholesaler offers board and certified training are unproven; the training page has no courses yet.

## Open questions

- What do their wholesaler, offers, guide, drugs, shop and jobs pages say? They were not read.
- What are their meta descriptions? They could not be read.
- Is the SmartPharm installer signed?

## Sources

- [supersoft-dz.com home](https://supersoft-dz.com/)
- [Le logiciel](https://supersoft-dz.com/logiciel)
- [Tarifs](https://supersoft-dz.com/tarifs)
- [Super CHIFA OFFICINE](https://supersoft-dz.com/super-chifa.html)
- [Guide d'installation](https://supersoft-dz.com/installation)
- [Formation](https://supersoft-dz.com/formation)
- [Super pharm - Logiciel DZ on Facebook](https://www.facebook.com/Super.Pharm.2018)
