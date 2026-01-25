namespace sghss_uninter.DTOs;

public class MedicoDetalhesDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cpf { get; init; }
    public string Crm { get; init; }
    public string Especialidade { get; set; }
}