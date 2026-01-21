using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.ConsultaEndpoints;

public class GetConsultasEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user
    , AppDbContext context
    , int page = Configuration.DefaultPageNumber
    , int pageSize = Configuration.DefaultPageSize)
    {
        var query = context.Consultas.AsNoTracking();
        
        //Roda apena se o usuario for medico para mostrar consultas
        //especificas dele.
        if (user.IsInRole("MEDICO"))
        {
            if (!int.TryParse(user.FindFirst("medicoid")?.Value, out var medicoId))
                return Results.BadRequest("Identificador do medico invalido");
            
            query = query.Where(c => c.MedicoId == medicoId);
        }
        
        var totalCount = await query.CountAsync();

        var consultas = await query
            .OrderByDescending(c => c.DataHora)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ConsultaListaDTO
            {
                Id = c.Id,
                DataHora = c.DataHora,
                Anamnese = c.Anamnese,
                NomePaciente = c.Paciente.Nome
            })
            .ToListAsync();
        
        return Results.Ok(new PagedResponse<List<ConsultaListaDTO>>(consultas, totalCount, page, pageSize));
    }
}