namespace sghss_uninter.DTOs.Prontuario;

public class ProntuarioDetalhesDTO
{
    public int Id { get; set; }
    public string AnamneseGeral { get; set; }
    public List<ConsultaDTO> Consultas { get; set; }
}