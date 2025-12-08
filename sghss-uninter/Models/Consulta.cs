namespace sghss_uninter.Models;

public class Consulta
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; } = DateTime.Now;
    public bool Status { get; set; }
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }
    public int ProntuarioId { get; set; }
}