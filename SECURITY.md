# Security Policy

## Reporting a vulnerability

**Do not open public issues for security problems.**

Please report vulnerabilities through GitHub's **private security advisory** mechanism:

1. Go to the repository's **Security** tab.
2. Click **Report a vulnerability** and fill in the details.
3. Alternatively, contact the maintainers privately via email (visible on the GitHub profile).

Please include:

- Steps to reproduce
- Impact assessment (what can be attacked, which data is exposed)
- Suggested fix, if you have one

## Supported versions

Only the latest `master` branch and the latest tagged release receive security fixes.

## Secrets policy

This project stores no credentials in source. Database passwords are read from
environment variables (`CHIFA_DB_PASSWORD`, `CHIFA_TEST_DB_PWD`). If you find a
committed credential, treat it as compromised and rotate it, then report it.
