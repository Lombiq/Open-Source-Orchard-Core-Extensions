using Lombiq.Marketing.UrlShortener.Indexes;
using Lombiq.Marketing.UrlShortener.Models;
using Microsoft.Extensions.Caching.Memory;
using OrchardCore.ContentManagement;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.Marketing.UrlShortener.Services;

public class UrlShorteningService : IUrlShorteningService
{
    private readonly ISession _session;
    private readonly IMemoryCache _memoryCache;

    public UrlShorteningService(ISession session, IMemoryCache memoryCache)
    {
        _session = session;
        _memoryCache = memoryCache;
    }

    public Task<string> GetDestinationUrlAsync(string shortUrl) =>
        _memoryCache.GetOrCreateAsync(
            shortUrl,
            async _ => (await _session.QueryIndex<ShortUrlPartIndex>(index => index.ShortUrl == shortUrl).FirstOrDefaultAsync()).DestinationUrl);

    public async Task<bool> IsShortUrlUniqueAsync(ShortUrlPart shortUrlPart)
    {
        // Check if the short URL already exists in the cache. This is a quick check to avoid hitting the database if
        // we already know the short URL is taken.
        if (_memoryCache.TryGetValue(shortUrlPart.ShortUrl.Text, out _))
        {
            return false;
        }

        // Check if the short URL already exists in the database.
        if (await _session.Query<ContentItem, ShortUrlPartIndex>(index =>
                index.ShortUrl == shortUrlPart.ShortUrl.Text &&
                index.ContentItemId != shortUrlPart.ContentItem.ContentItemId)
            .FirstOrDefaultAsync() is { } existingShortUrl)
        {
            // Cache the existing short URL to prevent future database hits for the same short URL.
            _memoryCache.Set(shortUrlPart.ShortUrl.Text, existingShortUrl.As<ShortUrlPart>().DestinationUrl);
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateShortUrlAsync(string previousShortUrl, ShortUrlPart shortUrlPart)
    {
        // If the short URL is being changed, we need to check if the new short URL is unique.
        if (!await IsShortUrlUniqueAsync(shortUrlPart))
        {
            return false;
        }

        _memoryCache.Set(shortUrlPart.ShortUrl.Text, shortUrlPart.DestinationUrl.Text);
        return true;
    }

    public Task DeleteShortUrlAsync(ContentItem shortUrlContentItem)
    {
        // Remove the short URL from the cache to ensure it doesn't return stale data after deletion.
        _memoryCache.Remove(shortUrlContentItem.As<ShortUrlPart>().ShortUrl.Text);
        return Task.CompletedTask;
    }
}
