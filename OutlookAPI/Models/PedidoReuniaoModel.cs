namespace MicrosftAPI.Models
{
    public class PedidoReuniaoModel
    {
        public string Organizador { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Corpo { get; set; } = string.Empty;
        public string DataHoraInicio { get; set; } = string.Empty;
        public string DataHoraFim { get; set; } = string.Empty;
        public List<string> Participantes { get; set; } = new();
    }
}
