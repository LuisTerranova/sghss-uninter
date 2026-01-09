using sghss_uninter.Api;
using sghss_uninter.Data;

namespace sghss_uninter.Endpoints.MedicoEndpoints;

public class DeleteMedicoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .RequireAuthorization("Administrador");

    private static async Task<IResult> HandleAsync(AppDbContext context,
        int id)
    {
        var medico = await context.Medicos.FindAsync(id);
        
        if (medico == null)
            return Results.NotFound("Medico nao encontrado");
        
        context.Medicos.Remove(medico);
        await context.SaveChangesAsync();
        
        return Results.NoContent();
    }
}