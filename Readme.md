# Lombiq's Open-Source Orchard Core Extensions



[![TeamCity build status](https://ci.lombiq.com/app/rest/builds/buildType:id:OrchardExtensions_OSOCE_Developer_PullAndBuild/statusIcon.svg)](https://ci.lombiq.com/buildConfiguration/OrchardExtensions_OSOCE_Developer_PullAndBuild?mode=builds)


## About

Looking for some useful Orchard Core extensions? Here's a bundle solution of all of Lombiq's open-source Orchard Core extensions (modules and themes). Clone and try them out!

This is an [Orchard Core CMS](https://www.orchardcore.net/) Visual Studio solution that contains most of [Lombiq](https://lombiq.com)'s open-source Orchard modules and themes, as well as related utilities and libraries. Please keep in mind that only those extensions are included which use the latest released version of Orchard (i.e. the very cutting-edge ones depending on a nightly build are not yet here). Since the extensions are included as git submodules when cloning this repo set git to initialize submodules.

This also serves as an example of an ASP.NET Core web app using Orchard from NuGet.

**You'll need to install NPM and Gulp for the solution to build** since the Vue.js module depends on it. Check out [the module's Readme](https://github.com/Lombiq/Orchard-Vue.js#prerequisites) for details.

 Note that this solution also has an Orchard 1 counterpart, [Lombiq's Open-Source Orchard Extensions](https://github.com/Lombiq/Open-Source-Orchard-Extensions).



## Samples and Recipes

You can activate various sample content in the site:

- [Lombiq Training Demo for Orchard Core](https://github.com/Lombiq/Orchard-Training-Demo-Module/): Select the _Training Demo_ setup recipe when you first run the site.
- [Lombiq JSON Editor](https://github.com/Lombiq/Orchard-JSON-Editor): Go to Recipes in the admin dashboard and select "Lombiq Open Source Orchard Core Extensions - JSON Editor Sample".
  - Content Items > JSON Example Page: Shows the sample item in action. Edit shows the JSON Editor where you can change the JSON value. View demonstrates simple JavaScript consuming the JSON content.
  - Content Definition > Content Types > JSON Example Page > JsonExampleField: You can edit the this field's JSON Editor's configuration object here. Check out the Templates property!

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
