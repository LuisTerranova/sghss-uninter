using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;
using sghss_uninter.Models.Identity;

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
        if (user == null || !user.IsInRole("MEDICO")) return Results.Unauthorized();
        var consulta = new Consulta
        {
            DataHora = consultaDto.DataHora,
            MedicoId = consultaDto.MedicoId,
            PacienteId = consultaDto.PacienteId,
            ProntuarioId = consultaDto.ProntuarioId
        };
            
        await context.Consultas.AddAsync(consulta);
        await context.SaveChangesAsync();
            
        return Results.Ok(consulta);
    }
}