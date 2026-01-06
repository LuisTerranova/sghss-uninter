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
            .RequireAuthorization(policy => policy.RequireRole("MEDICO"));

    private static async Task<IResult> HandleAsync(ConsultaDTO consultaDto
        , ClaimsPrincipal user
        , AppDbContext context)
    {
        var pacienteExiste = await context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == consultaDto.PacienteId);

        if (pacienteExiste == null)
            return Results.NotFound("Paciente não encontrado.");

        var prontuario = await context.Prontuarios
            .FirstOrDefaultAsync(p => p.PacienteId == consultaDto.PacienteId);

        if (prontuario == null)
        {
            prontuario = new Prontuario
            {
                PacienteId = consultaDto.PacienteId,
            };
            await context.Prontuarios.AddAsync(prontuario);
            await context.SaveChangesAsync();
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var medico = await context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ApplicationUserId == userIdClaim);

        if (medico == null) return Results.BadRequest("Perfil de médico não localizado.");

        var consulta = new Consulta
        {
            DataHora = consultaDto.DataHora,
            MedicoId = medico.Id,
            PacienteId = consultaDto.PacienteId,
            ProntuarioId = prontuario.Id,
            Anamnese = consultaDto.Anamnese
        };

        await context.Consultas.AddAsync(consulta);
        await context.SaveChangesAsync();

        return Results.Created($"/consultas/{consulta.Id}", new
        {
            consulta.Id,
            consulta.DataHora,
            consulta.Anamnese,
            consulta.MedicoId,
            consulta.PacienteId,
            consulta.ProntuarioId
        });
    }
}