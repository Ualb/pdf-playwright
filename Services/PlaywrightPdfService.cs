using Microsoft.Playwright;
using playwright.test.implementing.Application.Common.Interfaces;

namespace playwright.test.implementing.Services;

public class PlaywrightPdfService: IPdfService, IAsyncDisposable
{
    private readonly SemaphoreSlim _pageSemaphore;
    private readonly IBrowser _browser;
    private readonly IPlaywright _playwright;

    private const int MaxConcurrentPages = 5;

    public PlaywrightPdfService()
    {
        _pageSemaphore = new SemaphoreSlim(MaxConcurrentPages);

        _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();

        _browser = _playwright.Chromium
            .LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            })
            .GetAwaiter()
            .GetResult();
    }

    public async Task<byte[]> GeneratePdfAsync(string html, CancellationToken cancellationToken)
    {
        if (!await _pageSemaphore.WaitAsync(TimeSpan.FromSeconds(3), cancellationToken))
            throw new TimeoutException("No hay capacidad disponible para generar el PDF.");

        IPage? page = null;

        try
        {
            page = await _browser.NewPageAsync();

            await page.SetContentAsync(html, new PageSetContentOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });
            
            await page.EmulateMediaAsync(new PageEmulateMediaOptions
            {
                Media = Media.Screen
            }); 
            
            await page.SetViewportSizeAsync(1240, 1754);
            
            return await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4",
                PrintBackground = true,
                Scale = 1,
                Margin = new Margin
                {
                    Top = "25mm",
                    Bottom = "25mm",
                    Left = "35mm",
                    Right = "35mm"
                }
            });
        }
        finally
        {
            if (page != null)
                await page.CloseAsync();

            _pageSemaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
        _pageSemaphore.Dispose();
    }
}