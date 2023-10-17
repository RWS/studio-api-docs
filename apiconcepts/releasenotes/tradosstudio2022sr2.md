Release Notes for <Var:ProductNameWithEdition> SR2
===================

# Added API licensing support for subscriptions
When creating standalone apps, you can now use Project automation API calls that need a license even if <Var:ProductName> is licensed through a subscription. This expands the list of supported scenarios where <Var:ProductName> is licensed through a perpetual license, or through a network server license.

> [!NOTE]
>
> Starting with <Var:ProductNameWithEdition> SR2, when developing a standalone application using Project Automation APIs that require a license, make sure to call `LicenseManager.ReleaseLicense()` before the application exits. See [Project Automation Overview](../projectautomation/overview.md) for more details.
