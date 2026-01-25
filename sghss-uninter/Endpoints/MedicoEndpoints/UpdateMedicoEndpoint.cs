using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;

namespace sghss_uninter.Endpoints.MedicoEndpoints;

public class UpdateMedicoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:int}", HandleAsync)
            .RequireAuthorization("Administrador");

    private static async Task<IResult> HandleAsync(AppDbContext context
    ,AtualizarMedicoDTO medicoDto
    , int id)
    {
        //WIP buscas com chave primaria usar findasync(id)
        var medico = await context.Medicos
            .FirstOrDefaultAsync(m => m.Id == id);
        
        medico.Nome = medicoDto.Nome;
        medico.Email = medicoDto.Email;
        medico.Telefone = medicoDto.Telefone;
        medico.Especialidade = medicoDto.Especialidade;

        var linhasAfetadas = await context.SaveChangesAsync();

        return linhasAfetadas == 0
            ? Results.BadRequest("Nenhuma alteracao foi realizada.")
            : Results.NoContent();
    }
}