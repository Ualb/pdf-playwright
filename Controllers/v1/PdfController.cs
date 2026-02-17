using Microsoft.AspNetCore.Mvc;
using playwright.test.implementing.Application.Transactions;

namespace playwright.test.implementing.Controllers.v1;

[ApiController]
[Route("api/[controller]")]
public class PdfController(LocalBank localBank) : ControllerBase
{ 

    [HttpGet("generate")]
    public async Task<IActionResult> Generate(CancellationToken ct)
    {
        var pdf = await localBank.ExecuteAsync("Ulises López", ct);
        return File(pdf, "application/pdf", "reporte.pdf");
    }
}