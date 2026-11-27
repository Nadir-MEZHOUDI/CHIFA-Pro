// Builds the Arabic page website/ar/index.html from website/index.html and the translations in ar.json.
// Run after any change to either: node deploy/website/build-ar.mjs
import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const here = path.dirname(fileURLToPath(import.meta.url));
const site = path.join(here, "../../website");
const SITE = "https://smartchifa.smartappdz.org";
const AR = JSON.parse(fs.readFileSync(path.join(here, "ar.json"), "utf8"));
let h = fs.readFileSync(path.join(site, "index.html"), "utf8");

const TITLE = "SmartCHIFA — برنامج الشفاء للصيدليات في الجزائر";
const DESC = "SmartCHIFA برنامج الشفاء للصيدليات الجزائرية: جداول الإرسال CNAS و CASNOS، المرفوضات، المؤثرات العقلية والإحصائيات على الحاسوب والهاتف. تجربة مجانية 7 أيام.";
const FONTS = "https://fonts.googleapis.com/css2?family=Bricolage+Grotesque:opsz,wght@12..96,800&family=IBM+Plex+Sans+Arabic:wght@400;500;600;700&display=swap";

const escText = s => s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
const escAttr = s => escText(s).replace(/"/g, "&quot;");
const once = (from, to) => {
  const n = typeof from === "string" ? h.split(from).length - 1 : (h.match(new RegExp(from.source, "g")) || []).length;
  if (n !== 1) throw new Error(`expected 1 match, got ${n}: ${String(from).slice(0, 80)}`);
  h = h.replace(from, () => to);
};

const used = new Set();
const missing = [];

// Element text and HTML, replaced from the last match backwards so earlier offsets stay valid.
const opens = [...h.matchAll(/<(\w+)\b[^>]*?\sdata-i18n(-html)?="([^"]+)"[^>]*>/g)].reverse();
for (const m of opens) {
  const [tag, isHtml, key] = [m[1], !!m[2], m[3]];
  const start = m.index + m[0].length;
  const re = new RegExp(`<(/?)${tag}\\b[^>]*>`, "gi");
  re.lastIndex = start;
  let depth = 1, end = -1, t;
  while ((t = re.exec(h))) {
    depth += t[1] ? -1 : 1;
    if (depth === 0) { end = t.index; break; }
  }
  if (end < 0) throw new Error(`no closing </${tag}> for ${key}`);
  if (AR[key] == null) { missing.push(key); continue; }
  used.add(key);
  h = h.slice(0, start) + (isHtml ? AR[key] : escText(AR[key])) + h.slice(end);
}

h = h.replace(/(<img\b[^>]*?\salt=")[^"]*("[^>]*?\sdata-i18n-alt="([^"]+)")/g, (all, a, b, key) => {
  if (AR[key] == null) { missing.push(key); return all; }
  used.add(key);
  return a + escAttr(AR[key]) + b;
});

// Head
once(`<html lang="fr" dir="ltr">`, `<html lang="ar" dir="rtl">\n<!-- Generated from ../index.html and deploy/website/ar.json by deploy/website/build-ar.mjs: edit those, not this file. -->`);
once(/<title>[^<]*<\/title>/, `<title>${TITLE}</title>`);
once(/<meta name="description" content="[^"]*">/, `<meta name="description" content="${escAttr(DESC)}">`);
once(`<link rel="canonical" href="${SITE}/">`, `<link rel="canonical" href="${SITE}/ar/">`);
once(`<meta property="og:url" content="${SITE}/">`, `<meta property="og:url" content="${SITE}/ar/">`);
once(/<meta property="og:title" content="[^"]*">/, `<meta property="og:title" content="${TITLE}">`);
once(/<meta property="og:description" content="[^"]*">/, `<meta property="og:description" content="${escAttr(DESC)}">`);
once(/<meta property="og:image:alt" content="[^"]*">/, `<meta property="og:image:alt" content="SmartCHIFA، برنامج الشفاء للصيدليات الجزائرية">`);
once(`<meta property="og:locale" content="fr_DZ">\n<meta property="og:locale:alternate" content="ar_DZ">`, `<meta property="og:locale" content="ar_DZ">\n<meta property="og:locale:alternate" content="fr_DZ">`);
once(/<meta name="twitter:title" content="[^"]*">/, `<meta name="twitter:title" content="${TITLE}">`);
once(/<meta name="twitter:description" content="[^"]*">/, `<meta name="twitter:description" content="${escAttr(DESC)}">`);
h = h.replaceAll(/https:\/\/fonts\.googleapis\.com\/css2\?[^"]*/g, FONTS);

const faq = [...h.matchAll(/<summary data-i18n="(q\d)">/g)].map(m => m[1]);
const faqLd = {
  "@context": "https://schema.org",
  "@type": "FAQPage",
  "inLanguage": "ar",
  "mainEntity": faq.map(q => ({ "@type": "Question", "name": AR[q], "acceptedAnswer": { "@type": "Answer", "text": AR["a" + q.slice(1)] } }))
};
once(/<script type="application\/ld\+json" id="ld-faq">[\s\S]*?<\/script>/, `<script type="application/ld+json" id="ld-faq">\n${JSON.stringify(faqLd, null, 2)}\n</script>`);

// Body
once(`<a class="lang" id="lang" href="/ar/" hreflang="ar" lang="ar">عربي</a>`, `<a class="lang" id="lang" href="/" hreflang="fr" lang="fr">Français</a>`);
once(`aria-label="Menu"`, `aria-label="القائمة"`);

fs.mkdirSync(path.join(site, "ar"), { recursive: true });
fs.writeFileSync(path.join(site, "ar/index.html"), h);

const unused = Object.keys(AR).filter(k => !used.has(k) && !/^[qa]\d$/.test(k));
if (missing.length) console.warn("No Arabic text for:", [...new Set(missing)].join(", "));
if (unused.length) console.warn("Unused in ar.json:", unused.join(", "));
console.log("website/ar/index.html written");
