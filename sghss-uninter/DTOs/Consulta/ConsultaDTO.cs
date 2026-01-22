namespace sghss_uninter.DTOs;

public class ConsultaDTO
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Anamnese { get; set; }
    public int PacienteId { get; set; }
    public bool Status { get; set; }
}