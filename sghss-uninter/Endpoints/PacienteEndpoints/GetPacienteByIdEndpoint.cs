using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.PacienteEndpoints;

public class GetPacienteByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .RequireAuthorization("Medico&Admin");

    private static async Task<IResult> HandleAsync(AppDbContext context
        , ClaimsPrincipal user
        , int id
        , int pageSize = Configuration.DefaultPageSize
        , int pageNumber = Configuration.DefaultPageNumber)
        //WIP Adicionar cancellation token onde for necessario
    {
        if (user.IsInRole("MEDICO"))
        {
            if (!int.TryParse(user.FindFirst("medicoid")?.Value, out var medicoId))
                return Results.BadRequest("Identificador do medico invalido");
            
            var temVinculo = await context.Consultas
                .AsNoTracking()
                .AnyAsync(c => c.PacienteId == id && c.MedicoId == medicoId);

            if (!temVinculo)
                return Results.Problem("Acesso negado, voce nao tem vinculo com este paciente");
        }

        var query = context.Pacientes
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PacienteMaisConsultasDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Cpf = p.Cpf,
                Idade = p.Idade,
                Consultas = p.Consultas
                    .OrderByDescending(c => c.DataHora)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .Select(c => new ConsultaDTO
                    {
                        Id = c.Id,
                        DataHora = c.DataHora,
                        Anamnese = c.Anamnese,
                        PacienteId = c.PacienteId,
                    }).ToList()
            });

        var paciente = await query
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        
        return paciente == null 
            ? Results.NotFound("Paciente nao encontrado") 
            : Results.Ok(new Response<PacienteMaisConsultasDTO>(paciente, "Paciente encontrado com sucesso"));
    }

}