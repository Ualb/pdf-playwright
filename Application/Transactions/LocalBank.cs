using playwright.test.implementing.Application.Common.Interfaces;

namespace playwright.test.implementing.Application.Transactions;

public class LocalBank(IPdfService pdfService, IWebHostEnvironment environment)
{
    public async Task<byte[]> ExecuteAsync(string client, CancellationToken ct)
    {
        var templatePath = Path.Combine(
            environment.ContentRootPath,
            "Assets",
            "Templates",
            "comprobante_es_2.html");

        var html = await File.ReadAllTextAsync(templatePath, ct);

        // html = html.Replace("{{Cliente}}", client)
            // .Replace("{{Fecha}}", DateTime.Now.ToString("dd/MM/yyyy"));
            
        html = html.Replace("{{Monto}}", "c$ 12,400.00")
                .Replace("{{Fecha}}", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("{{Origen}}", "33276892")
                .Replace("{{Destino}}", "4446892")
                .Replace("{{Referencia}}", "23321");

        return await pdfService.GeneratePdfAsync(html, ct);
    }
}