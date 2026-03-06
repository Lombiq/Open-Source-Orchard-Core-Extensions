using Lombiq.Marketing.UrlShortener.Indexes;
using Lombiq.Marketing.UrlShortener.Models;
using Lombiq.Marketing.UrlShortener.Services;
using Microsoft.Extensions.Caching.Memory;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using YesSql;

namespace Lombiq.Marketing.UrlShortener.Handlers;

public class ShortUrlPartHandler : ContentPartHandler<ShortUrlPart>
{
    private readonly IUrlShorteningService _urlShorteningService;
    private readonly IUpdateModelAccessor _updateModelAccessor;
    private readonly ISession _session;
    private readonly IMemoryCache _memoryCache;

    private ShortUrlPart _previousShortUrlPart;

    public ShortUrlPartHandler(
        IUrlShorteningService urlShorteningService,
        IUpdateModelAccessor updateModelAccessor,
        IMemoryCache memoryCache,
        ISession session)
    {
        _urlShorteningService = urlShorteningService;
        _updateModelAccessor = updateModelAccessor;
        _memoryCache = memoryCache;
        _session = session;
    }

    public override async Task InitializingAsync(InitializingContentContext context, ShortUrlPart part)
    {
        part.ShortUrl.Text = await GenerateRandomShortUrlAsync();
        part.ContentItem.Apply(part);
    }

    public override Task UpdatingAsync(UpdateContentContext context, ShortUrlPart part)
    {
        _previousShortUrlPart = part;
        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, ShortUrlPart part) => UpdateShortUrlAsync(part);

    public override Task UpdatedAsync(UpdateContentContext context, ShortUrlPart part) => UpdateShortUrlAsync(part);

    public override Task RemovedAsync(RemoveContentContext context, ShortUrlPart part) => _urlShorteningService.DeleteShortUrlAsync(part.ContentItem);

    private async Task UpdateShortUrlAsync(ShortUrlPart part)
    {
        if (string.IsNullOrEmpty(part.ShortUrl.Text))
        {
            _updateModelAccessor.ModelUpdater.ModelState.AddModelError(
                nameof(ShortUrlPart.ShortUrl),
                "The short URL is required.");
        }

        if (string.IsNullOrEmpty(part.DestinationUrl.Text))
        {
            _updateModelAccessor.ModelUpdater.ModelState.AddModelError(
                nameof(ShortUrlPart.DestinationUrl),
                "The destination URL is required.");
        }

        if (!Uri.IsWellFormedUriString(part.ShortUrl.Text, UriKind.Relative) || !part.ShortUrl.Text.StartsWith('/'))
        {
            _updateModelAccessor.ModelUpdater.ModelState.AddModelError(
                nameof(ShortUrlPart.ShortUrl),
                "The short URL must be a valid relative URL (for example: /short-url).");
        }

        if (!Uri.TryCreate(part.DestinationUrl.Text, UriKind.RelativeOrAbsolute, out var destinationUri) ||
            !destinationUri.IsWellFormedOriginalString())
        {
            _updateModelAccessor.ModelUpdater.ModelState.AddModelError(
                nameof(ShortUrlPart.DestinationUrl),
                "The destination URL must be a valid URL.");
        }

        if (destinationUri != null && !destinationUri.IsAbsoluteUri && !destinationUri.OriginalString.StartsWith('/'))
        {
            _updateModelAccessor.ModelUpdater.ModelState.AddModelError(
                nameof(ShortUrlPart.DestinationUrl),
                "The destination URL must be an absolute URL or a relative URL starting with '/'.");
        }

        if (!_updateModelAccessor.ModelUpdater.ModelState.IsValid || _previousShortUrlPart.ShortUrl.Text == part.ShortUrl.Text)
        {
            return;
        }

        if (!await _urlShorteningService.UpdateShortUrlAsync(_previousShortUrlPart.ShortUrl.Text, part))
        {
            _updateModelAccessor.ModelUpdater.ModelState.AddModelError(
                nameof(ShortUrlPart.ShortUrl),
                "The short URL must be unique. The provided short URL is already in use.");
        }

        _previousShortUrlPart = part;
    }

    private async Task<string> GenerateRandomShortUrlAsync()
    {
        // TODO: Move these into admin settings.
        const int minLength = 10;
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        using var rng = RandomNumberGenerator.Create();

        var isUnique = false;
        var randomShortUrl = string.Empty;
        while (!isUnique)
        {
            randomShortUrl = new string(Enumerable.Repeat(validChars, minLength).Select(text => text[rng.Next(0, text.Length)]).ToArray());

            if (!_memoryCache.TryGetValue($"/{randomShortUrl}", out _))
            {
                isUnique = (await _session.QueryIndex<ShortUrlPartIndex>(index => index.ShortUrl == randomShortUrl).FirstOrDefaultAsync()) == null;
            }
        }

        return $"/{randomShortUrl}";
    }
}
