using System.Text.Json.Serialization;

namespace sghss_uninter.Models;

public class Medico
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cpf { get; init; }
    public int Crm { get; init; }
    public string ApplicationUserId { get; init; }
    [JsonIgnore]
    public IList<Consulta> Consultas { get; set; } = new List<Consulta>();
}