# Lombiq's Open-Source Orchard Core Extensions



[![TeamCity build status](https://ci.lombiq.com/app/rest/builds/buildType:id:OrchardExtensions_OSOCE_Developer_PullAndBuild/statusIcon.svg)](https://ci.lombiq.com/buildConfiguration/OrchardExtensions_OSOCE_Developer_PullAndBuild?mode=builds)


## About

Looking for some useful Orchard Core extensions? Here's a bundle solution of all of Lombiq's open-source Orchard Core extensions (modules and themes). Clone and try them out!

This is an [Orchard Core CMS](https://www.orchardcore.net/) Visual Studio solution that contains most of [Lombiq](https://lombiq.com)'s open-source Orchard modules and themes, as well as related utilities and libraries. Please keep in mind that only those extensions are included which use the latest released version of Orchard (i.e. the very cutting-edge ones depending on a nightly build are not yet here).

Since the extensions are included as git submodules when cloning this repo be sure to initialize submodules: When using a GUI this will most possibly happen by default, and when using the command line use the `--recurse-submodules` switch. If you cloned without initializing submodules, then you can run `git submodule update --init --recursive` to initialize them later.

This also serves as an example of an ASP.NET Core web app using Orchard from NuGet.

**You'll need to install NPM and Gulp for the solution to build** since multiple modules depend on them. Check out [the Vue.js module's Readme](https://github.com/Lombiq/Orchard-Vue.js#prerequisites) for details.

 Note that this solution also has an Orchard 1 counterpart, [Lombiq's Open-Source Orchard Extensions](https://github.com/Lombiq/Open-Source-Orchard-Extensions).



## Samples and Recipes

You can activate various sample content in the site:

- [Lombiq JSON Editor](https://github.com/Lombiq/Orchard-JSON-Editor): Go to Recipes in the admin dashboard and select "Lombiq Open Source Orchard Core Extensions - JSON Editor Sample".
  - Content Items > JSON Example Page: Shows the sample item in action. Edit shows the JSON Editor where you can change the JSON value. View demonstrates simple JavaScript consuming the JSON content.
  - Content Definition > Content Types > JSON Example Page > JsonExampleField: You can edit the this field's JSON Editor's configuration object here. Check out the Templates property!
- [Lombiq UI Testing Toolbox](https://github.com/Lombiq/UI-Testing-Toolbox): Web UI testing toolbox mostly for Orchard Core applications. Check out [its samples](https://github.com/Lombiq/UI-Testing-Toolbox/blob/dev/Lombiq.Tests.UI.Samples/Readme.md) in this solution.
- [Lombiq Training Demo for Orchard Core](https://github.com/Lombiq/Orchard-Training-Demo-Module/): Select the _Training Demo_ setup recipe when you first run the site.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.

### Adding a new extension
When adding a new extension, do the following:

- Enable its features in _Lombiq.OSOCE.Tests.recipe_ so at least they have some rudimentary UI test coverage.
- Ideally, add recipes to it, demonstrating its usage. In that case, refer to it in the above section.

### Opening pull requests
- Open a pull request in this repository for every submodule pull request. That way, static code analysis and complex tests can run.
- If you see build errors under your pull request then check out its details: The errors link to our TeamCity instance. Select "Log in as guest" when presented with a login screen.

### Dependencies between Lombiq projects
When making a Lombiq project depend on another one from this solution, apart from adding a project reference, also add a conditional package reference. This way, when published to NuGet, dependencies will still work. See the project file of `Lombiq.HelpfulExtensions` for an example
