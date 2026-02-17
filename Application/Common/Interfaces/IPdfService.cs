namespace playwright.test.implementing.Application.Common.Interfaces;

public interface IPdfService
{
    Task<byte[]> GeneratePdfAsync(string html, CancellationToken cancellationToken = default);  

}