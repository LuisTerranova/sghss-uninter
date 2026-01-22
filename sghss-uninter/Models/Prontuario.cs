using System.Text.Json.Serialization;

namespace sghss_uninter.Models;

public class Prontuario
{
    public int Id { get; set; }
    [JsonIgnore]
    public IList<Consulta> Consultas { get; set; } =  new List<Consulta>();
    public int PacienteId { get; set; }
    public string AnamneseGeral { get; set; }
    [JsonIgnore]
    public Paciente Paciente { get; set; }
}