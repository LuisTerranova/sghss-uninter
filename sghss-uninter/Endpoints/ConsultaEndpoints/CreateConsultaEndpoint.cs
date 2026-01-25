using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.ConsultaEndpoints;

public class CreateConsultaEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .RequireAuthorization("Medico");

    private static async Task<IResult> HandleAsync(ConsultaDTO consultaDto
        , ClaimsPrincipal user
        , AppDbContext context)
    {
        
        var medicoIdClaim = user.FindFirst("medicoid")?.Value;
        
        if (!int.TryParse(medicoIdClaim, out int medicoId) || medicoId <= 0)
            return Results.Forbid();
        
        var paciente = await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == consultaDto.PacienteId);

        if (paciente == null)
            return Results.NotFound("Paciente não encontrado.");

        var prontuario = await context.Prontuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PacienteId == consultaDto.PacienteId);

        if (prontuario == null)
        {
            prontuario = new Prontuario
            {
                PacienteId = consultaDto.PacienteId,
                AnamneseGeral = "Iniciado Prontuario"
            };
            await context.Prontuarios.AddAsync(prontuario);
            await context.SaveChangesAsync();
        }

        var consulta = new Consulta
        {
            DataHora = consultaDto.DataHora,
            MedicoId = medicoId,
            PacienteId = consultaDto.PacienteId,
            ProntuarioId = prontuario.Id,
            Anamnese = consultaDto.Anamnese
        };

        await context.Consultas.AddAsync(consulta);
        await context.SaveChangesAsync();

        return Results.Created($"/consultas/{consulta.Id}", consulta);
    }
}