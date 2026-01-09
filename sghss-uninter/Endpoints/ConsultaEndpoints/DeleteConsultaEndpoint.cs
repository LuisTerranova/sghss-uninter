using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;

namespace sghss_uninter.Endpoints.ConsultaEndpoints;

public class DeleteConsultaEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .RequireAuthorization("Medico");

    private static async Task<IResult> HandleAsync(AppDbContext context
        , ClaimsPrincipal user
        , int id)
    {
        var consulta = await context.Consultas
            .FirstOrDefaultAsync(c => c.Id == id);
        
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var medico = await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ApplicationUserId == userIdClaim);

        if (consulta == null || consulta.MedicoId != medico.Id) return Results.BadRequest();
        
        context.Remove(consulta);
        await context.SaveChangesAsync();
        return Results.NoContent();

    }

}