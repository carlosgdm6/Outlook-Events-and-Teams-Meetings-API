using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using MicrosoftAPI;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly IConfiguration _configuracao;

    public EventoController(IConfiguration configuracao)
    {
        _configuracao = configuracao;
    }

    private GraphServiceClient ObterClienteGraph()
    {
        var clientId = _configuracao["AzureAdOutlook:ClientId"];
        var tenantId = _configuracao["AzureAdOutlook:TenantId"];
        var clientSecret = _configuracao["AzureAdOutlook:ClientSecret"];

        var credencial = new ClientSecretCredential(tenantId, clientId, clientSecret);
        return new GraphServiceClient(credencial);
    }

    [HttpPost("criar")]
    public async Task<IActionResult> CriarEvento(PedidoEventoModel input)
    {
        var graphClient = ObterClienteGraph();

        var evento = new Event
        {
            Subject = input.Assunto,
            Body = new ItemBody
            {
                ContentType = BodyType.Text,
                Content = input.Conteudo,
            },
            Start = new DateTimeTimeZone
            {
                DateTime = input.DataHoraInicio,
                TimeZone = "Europe/Lisbon"
            },
            End = new DateTimeTimeZone
            {
                DateTime = input.DataHoraFim,
                TimeZone = "Europe/Lisbon"
            },
            Location = new Location
            {
                DisplayName = input.Localizacao
            }
        };

        try
        {
            var eventoCriado = await graphClient.Users[input.Email].Events.PostAsync(evento);
            return Ok(new { IdEvento = eventoCriado.Id });
        }
        catch (ServiceException ex)
        {
            return StatusCode((int)ex.ResponseStatusCode, ex.Message);
        }
    }

    [HttpPatch("editar")]
    public async Task<IActionResult> EditarEvento(EditarEventoModel input)
    {
        var graphClient = ObterClienteGraph();

        var eventoAtualizado = new Event
        {
            Subject = input.Assunto,
            Body = new ItemBody
            {
                ContentType = BodyType.Text,
                Content = input.Conteudo,
            },
            Start = new DateTimeTimeZone
            {
                DateTime = input.DataHoraInicio,
                TimeZone = "Europe/Lisbon"
            },
            End = new DateTimeTimeZone
            {
                DateTime = input.DataHoraFim,
                TimeZone = "Europe/Lisbon"
            }
        };

        try
        {
            await graphClient.Users[input.Email].Events[input.IdEvento].PatchAsync(eventoAtualizado);
            return Ok(new { IdEvento = input.IdEvento });
        }
        catch (ServiceException ex)
        {
            return StatusCode((int)ex.ResponseStatusCode, ex.Message);
        }
    }

    [HttpDelete("apagar")]
    public async Task<IActionResult> ApagarEvento(ApagarEventoModel input)
    {
        var graphClient = ObterClienteGraph();

        try
        {
            await graphClient.Users[input.Email].Events[input.IdEvento].DeleteAsync();
            return Ok(new { IdEvento = input.IdEvento });
        }
        catch (ServiceException ex)
        {
            return StatusCode((int)ex.ResponseStatusCode, ex.Message);
        }
    }

    [HttpGet("obter")]
    public async Task<IActionResult> ObterEvento(string IdEvento, [FromQuery] string emailUtilizador)
    {
        var graphClient = ObterClienteGraph();

        try
        {
            var evento = await graphClient.Users[emailUtilizador].Events[IdEvento].GetAsync();

            var detalhesEvento = new
            {
                evento.Subject,
                DataHoraInicio = evento.Start.DateTime,
                DataHoraFim = evento.End.DateTime,
                evento.Location.DisplayName,
                Descricao = evento.Body.Content
            };

            return Ok(detalhesEvento);
        }
        catch (ServiceException ex)
        {
            return StatusCode((int)ex.ResponseStatusCode, ex.Message);
        }
    }
}
