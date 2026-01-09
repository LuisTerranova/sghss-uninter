using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;

namespace sghss_uninter.Endpoints.ConsultaEndpoints;

public class GetConsultaByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .RequireAuthorization(policy => policy.RequireRole("MEDICO", "ADMIN"));

    private static async Task<IResult> HandleAsync(AppDbContext context
        , ClaimsPrincipal user
        , int id)
    {
        var query = context.Consultas
            .AsNoTracking()
            .Where(c => c.Id == id);
        
        if (user.IsInRole("MEDICO"))
        {
            var medicoIdClaim = user.FindFirst("medicoid")?.Value;
        
            if (string.IsNullOrEmpty(medicoIdClaim))
                return Results.Forbid();
        
            var medicoId = int.Parse(medicoIdClaim);


            query = query.Where(c => c.MedicoId == medicoId);
        }
        
        var consulta = query.FirstOrDefaultAsync();
        
        
        return consulta == null 
            ? Results.NotFound() 
            : Results.Ok(consulta);
    }

}