namespace sghss_uninter.DTOs;

public class ConsultaListaDTO
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Anamnese { get; set; }
    public string NomePaciente { get; set; }
    public bool Status { get; set; }
}