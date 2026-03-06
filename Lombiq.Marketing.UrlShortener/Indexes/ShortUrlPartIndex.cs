using Lombiq.Marketing.UrlShortener.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Lombiq.Marketing.UrlShortener.Indexes;

public class ShortUrlPartIndex : MapIndex
{
    public string ContentItemId { get; set; }
    public string ShortUrl { get; set; }
    public string DestinationUrl { get; set; }
}

public class ShortUrlPartIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context) =>
        context.For<ShortUrlPartIndex>()
            .When(contentItem => contentItem.Has<ShortUrlPart>())
            .Map(contentItem =>
            {
                var shortUrlPart = contentItem.As<ShortUrlPart>();

                return new ShortUrlPartIndex
                {
                    ContentItemId = contentItem.ContentItemId,
                    ShortUrl = shortUrlPart.ShortUrl.Text,
                    DestinationUrl = shortUrlPart.DestinationUrl.Text,
                };
            });
}
