namespace sghss_uninter.Models;

public class Consulta
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public bool Status { get; set; }
    public string Anamnese { get; set; } = string.Empty;
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }
    public int ProntuarioId { get; set; }
    public Medico Medico { get; set; }
    public Paciente Paciente { get; set; }
    public Prontuario Prontuario { get; set; }
}