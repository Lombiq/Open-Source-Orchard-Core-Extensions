# Lombiq's Open-Source Orchard Core Extensions

[![Build and Test](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/actions/workflows/build-and-test.yml) [![Build and Test Windows](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/actions/workflows/build-and-test-windows.yml/badge.svg)](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/actions/workflows/build-and-test-windows.yml) [![GitHub commit activity](https://img.shields.io/github/commit-activity/m/Lombiq/Open-Source-Orchard-Core-Extensions)](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/commits/dev) [![GitHub last commit](https://img.shields.io/github/last-commit/Lombiq/Open-Source-Orchard-Core-Extensions)](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/commits/dev) [![GitHub contributors](https://img.shields.io/github/contributors/Lombiq/Open-Source-Orchard-Core-Extensions)](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/graphs/contributors)

## About

Looking for some useful Orchard Core extensions? Here's a bundle solution of all of Lombiq's open-source Orchard Core extensions (modules and themes). Clone and try them out!

This is an [Orchard Core](https://orchardcore.net/) Visual Studio solution that contains most of [Lombiq](https://lombiq.com)'s open-source Orchard modules and themes, as well as related utilities and libraries. Please keep in mind that only those extensions are included which use the latest released version of Orchard (i.e. the very cutting-edge ones depending on a nightly build are not yet here).

This also serves as an example of an ASP.NET Core web app using Orchard from NuGet.

Note that this solution also has an Orchard 1 counterpart, [Lombiq's Open-Source Orchard Extensions](https://github.com/Lombiq/Open-Source-Orchard-Extensions).

## Prerequisites and getting started

- You need Node.js for building client-side assets in multiple projects. Check out [Lombiq Node.js Extensions Readme](https://github.com/Lombiq/NodeJs-Extensions/#prerequisites) for details.
- To develop and test scripts written in PowerShell, install and use [PowerShell 7](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell) instead of Windows PowerShell (i.e., up to version 5.x). Static code analysis is only executed through PowerShell 7.
- Since the extensions are included as git submodules when cloning this repo be sure to initialize submodules: When using a GUI this will most possibly happen by default, and when using the command line use the `--recurse-submodules` switch. If you cloned without initializing submodules, then you can run `git submodule update --init --recursive` to initialize them later.

## Included Projects

### Libraries

- [`Lombiq.HelpfulLibraries`](https://github.com/Lombiq/Helpful-Libraries): Various libraries that can be handy when developing for Orchard Core, to be used from your own Orchard modules.
- [`Lombiq.HelpfulLibraries.Cli`](https://github.com/Lombiq/Helpful-Libraries/tree/dev/Lombiq.HelpfulLibraries.Cli): Command Line Libraries. This project helps with executing command line calls.
- [`Lombiq.HelpfulLibraries.LinqToDb`](https://github.com/Lombiq/Helpful-Libraries/tree/dev/Lombiq.HelpfulLibraries.LinqToDb): With the help of this project you can write LINQ expressions and run them with a [YesSql](https://github.com/sebastienros/yessql) `ISession` extension method to query from the DB instead of writing plain SQL queries. Uses the [LINQ to DB project](https://linq2db.github.io/). You can watch a demo video of the project [here](https://www.youtube.com/watch?v=ldJOdCSsWJo).
- [`Lombiq.HelpfulLibraries.Refit`](https://github.com/Lombiq/Helpful-Libraries/tree/dev/Lombiq.HelpfulLibraries.Refit): Adds helpers for working with [Refit](https://github.com/reactiveui/refit).
- [`Lombiq.HelpfulLibraries.RestEase`](https://github.com/Lombiq/Helpful-Libraries/tree/dev/Lombiq.HelpfulLibraries.RestEase): Adds a typed HTTP client to the service collection using RestEase.
- [`Lombiq.HelpfulLibraries.Targets`](https://github.com/Lombiq/Helpful-Libraries/tree/dev/Lombiq.HelpfulLibraries.Targets): Targets project which references all Helpful Libraries. Only necessary for NuGet publishing, just as _Lombiq.HelpfulLibraries.sln_.
- [`Lombiq.OrchardCoreApiClient`](https://github.com/Lombiq/Orchard-Core-API-Client): A client library for communicating with the [Orchard Core](https://www.orchardcore.net/) web APIs, it contains an implementation for the tenant management API and a console application for testing and demonstration.

### Modules

- [`Lombiq.AuditTrailExtensions`](https://github.com/Lombiq/Audit-Trail-Extensions): A module with additional features for the [Audit Trail module](https://docs.orchardcore.net/en/latest/docs/reference/modules/AuditTrail/) in Orchard Core.
- [`Lombiq.ChartJs`](https://github.com/Lombiq/Orchard-Chart.js): An Orchard Core wrapper around the [Chart.js](https://www.chartjs.org/) library for displaying datasets as various charts.
- [`Lombiq.ContentEditors`](https://github.com/Lombiq/Orchard-Content-Editors): With the help of this module you can create custom multi-page editors with async loading and saving. Watch a demo video of it from the Orchard Community Meeting [here](https://www.youtube.com/watch?v=WJ4dmbG9oVs).
- [`Lombiq.ContentEditors.Samples`](https://github.com/Lombiq/Orchard-Content-Editors/tree/dev/Lombiq.ContentEditors.Samples): Example Orchard Core module that makes use of Lombiq Content Editors.
- [`Lombiq.DataTables`](https://github.com/Lombiq/Orchard-Data-Tables): An Orchard Core wrapper around the [DataTables](https://datatables.net/) library for displaying tabular data from custom data sources.
- [`Lombiq.DataTables.Samples`](https://github.com/Lombiq/Orchard-Data-Tables/tree/dev/Lombiq.DataTables.Samples): Example Orchard Core module that makes use of Lombiq Data Tables for Orchard Core.
- [`Lombiq.HelpfulExtensions`](https://github.com/Lombiq/Helpful-Extensions): Orchard Core module containing some handy extensions (e.g. useful content types and widgets). This module is also available on all sites of [DotNest, the Orchard Core SaaS](https://dotnest.com/).
- [`Lombiq.HelpfulLibraries.Samples`](https://github.com/Lombiq/Helpful-Libraries/tree/dev/Lombiq.HelpfulLibraries.Samples): Example Orchard Core module that makes use of Lombiq Helpful Libraries.
- [`Lombiq.Hosting.Azure.ApplicationInsights`](https://github.com/Lombiq/Orchard-Azure-Application-Insights): This [Orchard Core](https://orchardcore.net/) module enables easy integration of [Azure Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) telemetry into Orchard. Just install the module, configure the instrumentation key from a configuration source (like the _appsettings.json_ file) as normally for AI, and collected data will start appearing in the Azure Portal. As seen on [the Orchard community meeting](https://www.youtube.com/watch?v=NKKR4R3UPog). Note that this module has an Orchard 1 version in the [`dev-orchard-1` branch](https://github.com/Lombiq/Orchard-Azure-Application-Insights/tree/dev-orchard-1). Note that the module depends on [Helpful Libraries](https://github.com/Lombiq/Helpful-Libraries/).
- [`Lombiq.Hosting.BuildVersionDisplay`](https://github.com/Lombiq/Hosting-Build-Version-Display): Module to display the build version (i.e. .NET assembly version or other) on the admin of the `Default` tenant. This is useful to see at a glance which version of the app you have to deployed currently to the given environment.
- [`Lombiq.Hosting.MediaTheme.Bridge`](https://github.com/Lombiq/Hosting-Media-Theme): With the help of this module, you'll be able to host your theme assets and templates from Media Library. It can be used together with the [`Lombiq.Hosting.MediaTheme`](https://github.com/Lombiq/Hosting-Media-Theme).
- [`Lombiq.Hosting.Tenants.Admin.Login`](https://github.com/Lombiq/Hosting-Tenants/tree/dev/Lombiq.Hosting.Tenants.Admin.Login): With the help of this module, you can log in from the Default tenant's admin dashboard to any other tenants as an administrator user. This is useful if you manage a lot of customer tenants and don't want to create individual logins of yours for each of them.
- [`Lombiq.Hosting.Tenants.EnvironmentRobots`](https://github.com/Lombiq/Hosting-Tenants/tree/dev/Lombiq.Hosting.Tenants.EnvironmentRobots): With the help of this module, you can ensure that your sites will not be indexed by search bots. Check out a demo video of this module [here](https://youtu.be/Rp3ao2ZbNRs).
- [`Lombiq.Hosting.Tenants.FeaturesGuard`](https://github.com/Lombiq/Hosting-Tenants/tree/dev/Lombiq.Hosting.Tenants.FeaturesGuard): A module that makes it possible to conditionally enable and conditionally keep enabled, or entirely prevent enabling, configurable sets of features on tenants.
- [`Lombiq.Hosting.Tenants.IdleTenantManagement`](https://github.com/Lombiq/Hosting-Tenants/tree/dev/Lombiq.Hosting.Tenants.IdleTenantManagement): With the help of this module, you can ensure that any tenant where the feature is enabled will shutdown after a preset time is elapsed. This can be used to free up resources.
- [`Lombiq.Hosting.Tenants.Maintenance`](https://github.com/Lombiq/Hosting-Tenants/tree/dev/Lombiq.Hosting.Tenants.Maintenance): With the help of this module, you can execute maintenance tasks on tenants.
- [`Lombiq.Hosting.Tenants.Management`](https://github.com/Lombiq/Hosting-Tenants/tree/dev/Lombiq.Hosting.Tenants.Management): With the help of this module, you can set restrictions on tenant creation and set tenant level shell settings in the tenant editor.
- [`Lombiq.JsonEditor`](https://github.com/Lombiq/Orchard-JSON-Editor): Orchard Core module for displaying a JSON Editor like on [jsoneditoronline.org](https://jsoneditoronline.org/). Watch a demo video of it from the Orchard Community Meeting [here](https://www.youtube.com/watch?app=desktop&v=nFhRjhXTKAY).
- [`Lombiq.LoginAsAnybody`](https://github.com/Lombiq/Orchard-Login-as-Anybody): With the help of this module, a user with the site owner permission can log in as anybody on the admin dashboard.
- [`Lombiq.OSOCE.Samples`](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/tree/dev/src/Modules/Lombiq.OSOCE.Samples): This is a placeholder module that will contain sample code for UI tests and demonstration content for our open-source Orchard Core extensions.
- [`Lombiq.Privacy`](https://github.com/Lombiq/Orchard-Privacy): Orchard module containing features related to data protection/privacy and the EU law on it, the [General Data Protection Regulation](https://eur-lex.europa.eu/legal-content/EN/TXT/?qid=1462439808430&uri=CELEX:32016R0679) (GDPR). Check out a demo video of this module [here](https://www.youtube.com/watch?v=GnyYL9Zdo8Q). **Important!** Using this module will not make your site GDPR-compliant alone. Do not forget to create a privacy policy page that you need to link to from the various consent-asking features.
- [`Lombiq.TrainingDemo`](https://github.com/Lombiq/Orchard-Training-Demo-Module): Demo Orchard Core module for training purposes guiding you to become an Orchard developer. Note that this module also has an Orchard 1.x version in the [`dev-orchard-1` branch of the repository](https://github.com/Lombiq/Orchard-Training-Demo-Module/tree/dev-orchard-1). If you prefer tutorial videos more then check out the [Dojo Course 3, the complete Orchard Core tutorial series](https://orcharddojo.net/orchard-training/dojo-course-3-the-full-orchard-core-tutorial).
- [`Lombiq.UIKit`](https://github.com/Lombiq/Orchard-UIKit): This module contains reusable shapes like text editors, custom-style checkboxes, dropdown editors, and in the future potentially more complex editors. [Here](https://www.youtube.com/watch?v=PONfn2K8AHg) you can also see a demo of it.
- [`Lombiq.VueJs`](https://github.com/Lombiq/Orchard-Vue.js): [Orchard Core](https://orchardproject.net/) module that contains [Vue.js](https://vuejs.org/) and commonly used Vue.js components to be used in other Vue.js apps as dependencies. Provides extensibility to create Vue.js component templates as Orchard Core shapes making them able to override in themes or modules.
- [`Lombiq.Walkthroughs`](https://github.com/Lombiq/Orchard-Walkthroughs): Do you want to learn about the most important Orchard Core features with a hands-on tutorial that guides you with interactive tooltips, right there in Orchard Core? This module is for you! Check out a demo video of it [here](https://www.youtube.com/watch?v=mWfngBADvX0).

### Themes

- [`Lombiq.BaseTheme`](https://github.com/Lombiq/Orchard-Base-Theme): This theme contains infrastructure for custom Bootstrap 5 themes with standardized zones and built-in front end menu display.
- [`Lombiq.BaseTheme.Samples`](https://github.com/Lombiq/Orchard-Base-Theme/tree/dev/Lombiq.BaseTheme.Samples): A sample theme that demonstrates the features of the [`Lombiq.BaseTheme`](https://github.com/Lombiq/Orchard-Base-Theme).
- [`Lombiq.Hosting.MediaTheme`](https://github.com/Lombiq/Hosting-Media-Theme): With the help of this theme and its dependency, the [`Lombiq.Hosting.MediaTheme.Bridge`](https://github.com/Lombiq/Hosting-Media-Theme), you'll be able to host your theme assets and templates from Media Library.
- [`Lombiq.Hosting.MediaTheme.Tests.Theme`](https://github.com/Lombiq/Hosting-Media-Theme): A theme only used for testing that demonstrates the local development version of a Media Theme.

### Utilities

- [`Lombiq.Hosting.MediaTheme.Targets`](https://github.com/Lombiq/Hosting-Media-Theme): Provides automatic Media Theme package creation during build.
- [`Lombiq.NodeJs.Extensions`](https://github.com/Lombiq/NodeJs-Extensions): Configurable and MSBuild-integrated frontend asset pipelines for SCSS, JS, MD, and external libraries implemented using npm scripts. Uses NPM Targets under the hood.
- [`Lombiq.Npm.Targets`](https://github.com/Lombiq/NPM-Targets): Provides automatic NPM package installation and a custom NPM command execution before building a .NET project. This way it is possible for example to manage assets (e.g. .scss files or images) in a folder that will be automatically compiled into the _wwwroot_ folder on build, which then can be excluded from the version control system.
- [`Lombiq.SetupExtensions`](https://github.com/Lombiq/Setup-Extensions): Extensions for setting up an Orchard Core application.

### Testing

- [`Lombiq.Tests`](https://github.com/Lombiq/Testing-Toolbox): General and unit testing extensions and helpers, mostly for ASP.NET Core and Orchard Core. Also see our [UI Testing Toolbox](https://github.com/Lombiq/UI-Testing-Toolbox).
- [`Lombiq.Tests.UI`](https://github.com/Lombiq/UI-Testing-Toolbox/tree/dev/Lombiq.Tests.UI): Web UI testing toolbox mostly for Orchard Core applications. Everything you need to do UI testing with Selenium for an Orchard app is here.
- [`Lombiq.Tests.UI.AppExtensions`](https://github.com/Lombiq/UI-Testing-Toolbox/tree/dev/Lombiq.Tests.UI.AppExtensions): UI testing-related configuration extensions for the web app under test. Note that the module depends on [Helpful Libraries](https://github.com/Lombiq/Helpful-Libraries).
- [`Lombiq.Tests.UI.Samples`](https://github.com/Lombiq/UI-Testing-Toolbox/tree/dev/Lombiq.Tests.UI.Samples): Example UI testing project. The whole project is heavily documented to teach you how to write UI tests with the UI Testing Toolbox. It guides you through this process just like the [Lombiq Training Demo for Orchard Core](https://github.com/Lombiq/Orchard-Training-Demo-Module) teaches Orchard Core and Orchard 1 development.
- [`Lombiq.Tests.UI.Shortcuts`](https://github.com/Lombiq/UI-Testing-Toolbox/tree/dev/Lombiq.Tests.UI.Shortcuts): Provides some useful shortcuts for common operations that UI tests might want to do or check, e.g. turning features on or off, or logging in users. This way, UI tests needn't use multi-step UI processes to do these operations (and thus implicitly be coupled with and tests those features). Note that the module depends on [Helpful Libraries](https://github.com/Lombiq/Helpful-Libraries).

### Tools

- [`Lombiq.Analyzers`](https://github.com/Lombiq/.NET-Analyzers): .NET code analyzers and code convention settings for [Lombiq](https://lombiq.com) projects, predominantly for [Orchard Core](https://orchardcore.net/) apps but also any .NET apps. We use these to enforce common standards across all our .NET projects, including e.g. all of our [open-source Orchard Core extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions).
- [`Lombiq.Analyzers.PowerShell`](https://github.com/Lombiq/PowerShell-Analyzers): PowerShell static code analysis via [PSScriptAnalyzer](https://github.com/PowerShell/PSScriptAnalyzer) and Lombiq's recommended configuration for it. Use it from the CLI, in GitHub Actions, or integrated into MSBuild builds.
- [`Lombiq.GitHub.Actions`](https://github.com/Lombiq/GitHub-Actions): Some common workflows and actions for GitHub Actions shared between Lombiq projects, e.g. to build and test Orchard Core apps, publish packages to NuGet, verify that a Git submodule pull request has a corresponding superproject one. These can be invoked from any other repository's build.

## Samples and Recipes

You can activate various sample content in the site:

- [`Lombiq.JsonEditor`](https://github.com/Lombiq/Orchard-JSON-Editor): Go to Recipes in the admin dashboard and select "Lombiq Open Source Orchard Core Extensions - JSON Editor Sample".
  - Content Items > JSON Example Page: Shows the sample item in action. Edit shows the JSON Editor where you can change the JSON value. View demonstrates simple JavaScript consuming the JSON content.
  - Content Definition > Content Types > JSON Example Page > JsonExampleField: You can edit the this field's JSON Editor's configuration object here. Check out the Templates property!
- [`Lombiq.UITestingToolbox`](https://github.com/Lombiq/UI-Testing-Toolbox): Web UI testing toolbox mostly for Orchard Core applications. Check out [its samples](https://github.com/Lombiq/UI-Testing-Toolbox/blob/dev/Lombiq.Tests.UI.Samples/Readme.md) in this solution.
- [`Lombiq.TrainingDemo`](https://github.com/Lombiq/Orchard-Training-Demo-Module/): Select the _Training Demo_ setup recipe when you first run the site.
- [`Lombiq.DataTables`](https://github.com/Lombiq/Orchard-Data-Tables):
  - Go to Features in the admin dashboard and select "Lombiq Data Tables - Samples".
  - Go to Recipes in the admin dashboard and select "Lombiq Data Tables - Sample Content - Employee".
- [`Lombiq.BaseTheme`](https://github.com/Lombiq/Orchard-Base-Theme):
  - The "TEST: Basic Orchard Features" setup recipe automatically sets it up. If not using it, run the "Lombiq Orchard Core Base Theme - Layers and Zones" recipe, and then enable the theme in Admin → Design → Themes.
  - In case of theme development you can use the "Lombiq Orchard Core Base Theme - Styling Demo" theme to test against some common HTML elements.
- [`Lombiq.ChartJs`](https://github.com/Lombiq/Orchard-Chart.js): Go to Recipes in the admin dashboard and select "Lombiq Chart.js - Sample Content - Income/Expense".
- [`Lombiq.Privacy`](https://github.com/Lombiq/Orchard-Privacy): Go to Recipes in the admin dashboard and select "Lombiq Privacy - Sample Content - Privacy, Forms, Workflows".
- [`Lombiq.Hosting.MediaTheme`](https://github.com/Lombiq/Hosting-Media-Theme): Go to Recipes in the admin dashboard and select "Lombiq Hosting - Media Theme - Samples".

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions and love letters are warmly welcome. You can send them to us via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.

### Adding a new extension or significant new features

When adding a new extension, or significant new features to existing extensions, do the following:

- For user-facing features add a recipe to it, demonstrating its usage with sample data. In that case, refer to it in the above section.
- If no data is needed or if the feature is more library-like, add a sample project (or in addition to the recipe). Put this project into the root of the submodule, so to have the main project's and sample project's folders side by side.
- Add lower-level unit/integration tests as necessary with the [Lombiq Testing Toolbox for Orchard Core](https://github.com/Lombiq/Testing-Toolbox/).
- If the sample project includes MVC actions, create a service that inherits from `MainMenuNavigationProviderBase` and adds front-end main menu items. The top level menu item should have the project's shortened name and the submenu items the individual actions. If you have several controllers, use separators and labels as you can see in the [TrainingDemoNavigationProvider.cs](https://github.com/Lombiq/Orchard-Training-Demo-Module/blob/dev/Navigation/TrainingDemoNavigationProvider.cs).
- If the feature is user-facing, also add UI test extension method(s) with the [Lombiq UI Testing Toolbox for Orchard Core](https://github.com/Lombiq/UI-Testing-Toolbox/) that assert on some important aspects, and execute them from a new UI test in `Lombiq.OSOCE.Tests.UI` (for inspiration, see the examples there). These methods are also meant to be executed from UI tests in other projects when testing how it integrated with other features. If you've added a demo recipe or sample project to it then utilize that in the test too (see `ExecuteRecipeDirectlyAsync()`). For this, you'll also need to enable the feature in _Lombiq.OSOCE.Tests.recipe_.
- If the project is published on NuGet, reference it from the app in the `Lombiq.OSOCE.NuGet` solution as well, and enable its features in the _Lombiq.OSOCE.NuGet.Tests.recipe_. If it has UI testing methods, also run them from `Lombiq.OSOCE.NuGet.Tests.UI`.
- If an extension is added, removed or significantly updated in this project, then add, remove or update its description under the "Included Projects" section of this Readme.

### Opening pull requests

- Open a pull request in this repository for every submodule pull request. That way, static code analysis and complex tests can run. If you forget to do so, a check in the submodule will fail. Once you've created the PR here, just click "Re-run jobs" in your submodule.
- If you see build errors under your pull request then check out its details: It will send you to the failed step in the Actions tab. You can also click on the "Summary" on the sidebar to download the artifacts (logs, screenshots, etc) and in case of failed UI tests you find the annotations linking to which test failed.
- Open a pull request for all but trivial changes (like typos) so we can nicely track them, including when generating release notes for the next release.

### Dependencies between Lombiq projects

When making a Lombiq project depend on another one from this solution, apart from adding a project reference and dependency in the extension manifest for Orchard Core extensions, also add a conditional package reference. This way, when published to NuGet, dependencies will still work. See the project file of `Lombiq.HelpfulExtensions` for an example.

You can just have project references between projects in the same repo though if both projects are published on NuGet (like between projects of the [UI Testing Toolbox](https://github.com/Lombiq/UI-Testing-Toolbox)) since those will be turned into package dependencies automatically.

You can use the `NuGetBuild` switch in the root _Directory.Build.props_ file to make all projects use NuGet references so you can update Lombiq packages for the whole solution.
