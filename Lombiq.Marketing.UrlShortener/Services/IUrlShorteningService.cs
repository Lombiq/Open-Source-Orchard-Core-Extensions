using Lombiq.Marketing.UrlShortener.Models;
using OrchardCore.ContentManagement;
using System.Threading.Tasks;

namespace Lombiq.Marketing.UrlShortener.Services;

public interface IUrlShorteningService
{
    public Task<string> GetDestinationUrlAsync(string shortUrl);

    public Task<bool> IsShortUrlUniqueAsync(ShortUrlPart shortUrlPart);

    public Task<bool> UpdateShortUrlAsync(string previousShortUrl, ShortUrlPart shortUrlPart);

    public Task DeleteShortUrlAsync(ContentItem shortUrlContentItem);
}
