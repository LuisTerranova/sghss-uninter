namespace sghss_uninter.DTOs;

public class PacienteMaisConsultasDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string   Cpf { get; set; }
    public int Idade { get; set; }
    public IList<ConsultaDTO> Consultas { get; set; }
}