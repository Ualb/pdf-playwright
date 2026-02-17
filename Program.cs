using playwright.test.implementing.Application.Common.Interfaces;
using playwright.test.implementing.Application.Transactions;
using playwright.test.implementing.Services;

var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddControllers();
// services 
builder.Services.AddSingleton<IPdfService, PlaywrightPdfService>();
builder.Services.AddScoped<LocalBank>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
}

app.MapControllers();
app.UseHttpsRedirection(); 
app.UseStaticFiles();  

app.Run();