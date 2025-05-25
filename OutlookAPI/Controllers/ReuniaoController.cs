using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MicrosftAPI.Models;
using MicrosoftAPI.Models;

namespace TeamsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReuniaoController : ControllerBase
    {
        private readonly IConfiguration _configuracao;

        public ReuniaoController(IConfiguration configuracao)
        {
            _configuracao = configuracao;
        }

        private GraphServiceClient ObterClienteGraph()
        {
            var tenantId = _configuracao["AzureAdTeams:TenantId"];
            var clientId = _configuracao["AzureAdTeams:ClientId"];
            var clientSecret = _configuracao["AzureAdTeams:ClientSecret"];

            var credencial = new ClientSecretCredential(tenantId, clientId, clientSecret);

            return new GraphServiceClient(credencial);
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarReuniao(PedidoReuniaoModel dados)
        {
            var clienteGraph = ObterClienteGraph();

            try
            {
                if (!DateTime.TryParse(dados.DataHoraInicio, out var dataInicio) || !DateTime.TryParse(dados.DataHoraFim, out var dataFim))
                {
                    return BadRequest("Datas inválidas.");
                }

                /* Meeting organizer's email:
                   ------------------------------------ */
                var Organizador = dados.Organizador;
                /* ------------------------------------
                 Before setting the organizer's email above, ensure that:

                   1. The user exists in the Azure organization;

                   2. The user has active licenses for Microsoft Teams and Exchange Online;

                   3. The application (identified by ClientId/ClientSecret) has the following **Application** permissions:
                      • OnlineMeetings.ReadWrite.All
                      • Calendars.ReadWrite

                   4. If changing the organizer's email dynamically, confirm that the user meets the
                      above requirements and that the application's permissions are correctly assigned. */

                var eventoCalendario = new Event
                {
                    Subject = dados.Assunto,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Text,
                        Content = dados.Corpo
                    },
                    Start = new DateTimeTimeZone
                    {
                        DateTime = dataInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TimeZone = "UTC"
                    },
                    End = new DateTimeTimeZone
                    {
                        DateTime = dataFim.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TimeZone = "UTC"
                    },
                    Location = new Location
                    {
                        DisplayName = "Online"
                    },
                    Attendees = dados.Participantes.Select(email => new Attendee
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = email,
                            Name = email
                        },
                        Type = AttendeeType.Required
                    }).ToList(),
                    IsOnlineMeeting = true,
                    OnlineMeetingProvider = OnlineMeetingProviderType.TeamsForBusiness
                };

                var pedido = clienteGraph.Users[Organizador].Events.ToPostRequestInformation(eventoCalendario);
                pedido.Headers.Add("Prefer", "outlook.sendMeetingInvitations=\"true\"");

                var resultadoEvento = await clienteGraph.RequestAdapter.SendAsync<Event>(pedido, Event.CreateFromDiscriminatorValue, null, default);

                return Ok(new
                {
                    IdEvento = resultadoEvento?.Id,
                    UrlReuniao = resultadoEvento?.OnlineMeeting?.JoinUrl,
                    Assunto = resultadoEvento?.Subject,
                    DataCriacao = resultadoEvento?.CreatedDateTime
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpPatch("editar")]
        public async Task<IActionResult> EditarReuniao(EditarReuniaoModel dados, [FromQuery] string utilizador)
        {
            var clienteGraph = ObterClienteGraph();

            try
            {
                if (!DateTime.TryParse(dados.DataHoraInicio, out var dataInicio) ||
                    !DateTime.TryParse(dados.DataHoraFim, out var dataFim))
                {
                    return BadRequest("Datas inválidas.");
                }

                var eventoAtualizado = new Event
                {
                    Subject = dados.Assunto,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Text,
                        Content = dados.Corpo
                    },
                    Start = new DateTimeTimeZone
                    {
                        DateTime = dataInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TimeZone = "UTC"
                    },
                    End = new DateTimeTimeZone
                    {
                        DateTime = dataFim.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TimeZone = "UTC"
                    }
                };

                await clienteGraph.Users[utilizador].Events[dados.IdEvento].PatchAsync(eventoAtualizado);

                return Ok(new { IdEvento = dados.IdEvento });
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ResponseStatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpDelete("apagar")]
        public async Task<IActionResult> ApagarReuniao([FromQuery] string utilizador, [FromQuery] string idEvento)
        {
            var clienteGraph = ObterClienteGraph();

            try
            {
                await clienteGraph.Users[utilizador].Events[idEvento].DeleteAsync();

                return Ok(new { IdEvento = idEvento });
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ResponseStatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("obter")]
        public async Task<IActionResult> ObterReuniao([FromQuery] string utilizador, [FromQuery] string idEvento)
        {
            var clienteGraph = ObterClienteGraph();

            try
            {
                var evento = await clienteGraph.Users[utilizador].Events[idEvento].GetAsync();

                var detalhes = new
                {
                    Organizador = evento.Organizer?.EmailAddress?.Address ?? "",
                    Assunto = evento.Subject,
                    DataHoraInicio = evento.Start?.DateTime,
                    DataHoraFim = evento.End?.DateTime,
                    UrlReuniao = evento.OnlineMeeting?.JoinUrl,
                    IdEvento = evento.Id,
                    Participantes = evento.Attendees?.Select(a => a.EmailAddress?.Address ?? "").ToList()
                };

                return Ok(detalhes);
            }
            catch (ServiceException ex)
            {
                return StatusCode((int)ex.ResponseStatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
