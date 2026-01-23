using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs.Prontuario;

namespace sghss_uninter.Endpoints.ProntuarioEndpoints;

public class UpdateProntuarioEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:int}", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(AppDbContext context
    ,int id
    ,AtualizarProntuarioDTO atualizarProntuarioDto)
    {
        var prontuario = await context.Prontuarios
            .FirstOrDefaultAsync(p => p.Id == id);

        prontuario.AnamneseGeral = atualizarProntuarioDto.AnamneseGeral;
        
        var linhasAfetadas = await context.SaveChangesAsync();

        return linhasAfetadas == 0 
            ? Results.BadRequest("Nenhuma alteracao foi realizada") 
            : Results.NoContent();
    }
}