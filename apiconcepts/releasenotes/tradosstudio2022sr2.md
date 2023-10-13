Release Notes for <Var:ProductNameWithEdition> SR2
===================

# Added API licensing support for subscription
Project automation API calls that require a license can now be used in standalone apps if <Var:ProductName> is licensed via subscription, in addition to the already supported scenarios when <Var:ProductName> is licensed with a standalone license key or network server license.

> [!NOTE]
>
> Starting with <Var:ProductNameWithEdition> SR2, if you are building a standalone application that consumes Project Automation APIs that require a license, you must call `LicenseManager.ReleaseLicense()` before the application exits. See [Project Automation Overview](../projectautomation/overview.md) for more details.
