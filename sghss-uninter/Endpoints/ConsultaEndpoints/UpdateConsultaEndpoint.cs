using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;

namespace sghss_uninter.Endpoints.ConsultaEndpoints;

public class UpdateConsultaEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .RequireAuthorization("Medico");

    private static async Task<IResult> HandleAsync(int id,
        ClaimsPrincipal user,
        ConsultaDTO consultaDto,
        AppDbContext context)
    {
        //WIP trocar logica para medico poder atualizar apenas propria consulta
        if (user == null || !user.IsInRole("MEDICO")) return Results.Unauthorized();
        
        var consulta = await context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id);
        
        consulta.DataHora = consultaDto.DataHora;
        consulta.Anamnese = consultaDto.Anamnese;
        
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

}