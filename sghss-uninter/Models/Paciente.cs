namespace sghss_uninter.Models;

public class Paciente
{
    public int Id { get; init; }
    public string Nome  { get; set; }
    public string Cpf  { get; init; }
    public DateTime DataNasc  { get; init; }
    public int Idade 
    { 
        get 
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNasc.Year;
            if (DataNasc.Date > hoje.AddYears(-idade))
            {
                idade--;
            }
            return idade;
        } 
    }
    public string Email  { get; set; }
    public string Telefone  { get; set; }
    public Prontuario Prontuario { get; set; }
}