using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;

namespace sghss_uninter.Endpoints.ConsultaEndpoints;

public class GetConsultaByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(AppDbContext context
        , ClaimsPrincipal user
        , int id)
    {
        var query = context.Consultas
            .AsNoTracking()
            .Where(c => c.Id == id);
        
        if (user.IsInRole("MEDICO"))
        {
            var medicoId = int.Parse(user.FindFirst("medicoid")?.Value);

            if (medicoId == 0)
                return Results.Unauthorized();
                
            query = query.Where(c => c.MedicoId == medicoId);
        }
        
        var consulta = query.FirstOrDefaultAsync();
        
        
        return consulta == null 
            ? Results.NotFound() 
            : Results.Ok(consulta);
    }

}