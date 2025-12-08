namespace sghss_uninter.Models;

public class Prontuario
{
    public int PacienteId { get; set; }
    public IList<Consulta> Consultas { get; set; } =  new List<Consulta>();
    public Paciente Paciente { get; set; }
}