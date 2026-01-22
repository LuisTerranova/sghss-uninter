using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.DTOs.Prontuario;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.ProntuarioEndpoints;

public class GetProntuarioByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id:int}", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(AppDbContext context
        , int id
        , int pageSize = Configuration.DefaultPageSize
        , int pageNumber = Configuration.DefaultPageNumber)
    {
        var prontuario = await context.Prontuarios
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProntuarioDetalhesDTO
            {
                Id = p.Id,
                AnamneseGeral = p.AnamneseGeral,
                Consultas = p.Consultas
                    .OrderByDescending(c => c.DataHora)
                    .Skip(pageSize*(pageNumber -1))
                    .Take(pageSize)
                    .Select(c => new ConsultaDTO
                    {
                        Id = c.Id,
                        DataHora = c.DataHora,
                        Anamnese = c.Anamnese,
                        Status = c.Status,
                        PacienteId = c.PacienteId
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return prontuario == null
            ? Results.BadRequest("Prontuario nao existe")
            : Results.Ok(prontuario);
    }
}