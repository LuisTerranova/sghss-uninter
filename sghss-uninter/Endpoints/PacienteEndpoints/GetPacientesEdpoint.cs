using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.PacienteEndpoints;

public class GetPacientesEdpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(AppDbContext context
    , CancellationToken ct
    , ClaimsPrincipal user
    , int pageSize = Configuration.DefaultPageSize
    , int pageNumber = Configuration.DefaultPageNumber)
    {
        var query = context.Pacientes
            .AsNoTracking();
        
        if (user.IsInRole("MEDICO"))
        {
            if (!int.TryParse(user.FindFirst("medicoid")?.Value, out var medicoId))
                return Results.BadRequest("Identificador do medico invalido");

            query = query
                .Where(p => p.Consultas.Any(c => c.MedicoId == medicoId));
        } 
        var listaPacientes = await query
            .OrderByDescending(p => p.Nome)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .Select(p => new PacienteDetalhesSImplesDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Idade = p.Idade
            })
            .ToListAsync(ct);
        
        var totalCount = await query.CountAsync(ct);

        return listaPacientes == null
            ? Results.NotFound("Nenhum paciente encontrado.")
              : Results.Ok(new PagedResponse<List<PacienteDetalhesSImplesDTO>>(listaPacientes
                , totalCount
                , pageNumber
                , pageSize));
    }

}