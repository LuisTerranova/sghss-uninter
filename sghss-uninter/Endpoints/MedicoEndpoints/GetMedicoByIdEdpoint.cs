using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;

namespace sghss_uninter.Endpoints.MedicoEndpoints;

public class GetMedicoByIdEdpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(AppDbContext context
        , ClaimsPrincipal user
        , int id)
    {
        var query = context.Medicos
            .AsNoTracking()
            .Where(m => m.Id == id);

        if (user.IsInRole("Medico"))
        {
            var medicoId = int.Parse(user.FindFirst("medicoid").Value);
            
            if (medicoId == 0)
                return Results.Forbid();
            
            query = query.Where(m => m.Id == medicoId);
        }

        var medico = query.FirstOrDefaultAsync();
        
        return medico == null 
            ? Results.NotFound() 
            : Results.Ok(medico);
    }

}