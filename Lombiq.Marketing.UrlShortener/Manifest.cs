using Lombiq.Marketing.UrlShortener.Constants;
using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Lombiq URL Shortener",
    Author = "Lombiq Technologies",
    Version = "0.0.1",
    Description = "Create short URLs that redirect to long URLs.",
    Website = "https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions"
)]

[assembly: Feature(
    Id = FeatureIds.UrlShortener,
    Name = "Lombiq URL Shortener",
    Category = "Marketing",
    Description = "Adds a Short URL content type for managing short redirects.",
    Dependencies =
    []
)]
