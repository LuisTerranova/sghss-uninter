using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;

namespace sghss_uninter.Endpoints.PacienteEndpoints;

public class UpdatePacienteEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:int}", HandleAsync)
            .RequireAuthorization("Administrador");

    private static async Task<IResult> HandleAsync(AppDbContext context
        , AtualizarPacienteDTO pacienteDto
        , int id)
    {
        var paciente = await context.Pacientes
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (paciente == null)
            return Results.NotFound("paciente nao encontrado");
        
        paciente.Nome = pacienteDto.Nome;
        paciente.Email = pacienteDto.Email;
        paciente.Telefone = pacienteDto.Telefone;
        
        var linhasAfetadas = await context.SaveChangesAsync();

        return linhasAfetadas == 0 
            ? Results.BadRequest("Nenhuma alteracao foi realizada") 
            : Results.NoContent();
    }
}