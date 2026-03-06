using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Lombiq.Marketing.UrlShortener.Models;

public class ShortUrlPart : ContentPart
{
    public TextField ShortUrl { get; set; } = new();
    public TextField DestinationUrl { get; set; } = new();
}
