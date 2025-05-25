using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Registar controllers
builder.Services.AddControllers();

// Logging do TeamsAPI
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configurar Swagger com info geral
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MicrosoftAPI",
        Version = "v1",
        Description = "API integrada com Microsoft Graph para Outlook e Teams"
    });
});

var app = builder.Build();

// Configurar pipeline HTTP com Swagger sempre ativo para facilitar testes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicrosoftAPI V1");
    c.RoutePrefix = string.Empty;  // Swagger na raiz do site
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();