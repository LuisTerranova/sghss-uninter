namespace sghss_uninter.DTOs;

public class ConsultaDTO
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }
    public int ProntuarioId { get; set; }
}