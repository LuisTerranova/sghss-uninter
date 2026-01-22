using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.PacienteEndpoints;

public class CreatePacienteEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .RequireAuthorization("Medico&Admin");


    private static async Task<IResult> HandleAsync(PacienteDTO pacienteDto
        , AppDbContext context)
    {
        if (pacienteDto == null) return Results.BadRequest();
        
        var novoPaciente = new Paciente
        {
            Nome = pacienteDto.Nome,
            Cpf = pacienteDto.Cpf,
            DataNasc = pacienteDto.DataNasc,
            Email = pacienteDto.Email,
            Telefone = pacienteDto.Telefone
        };
            
        await context.Pacientes.AddAsync(novoPaciente);
        await context.SaveChangesAsync();
        
        return Results.Created($"/v1/pacientes/{novoPaciente.Id}", 
            new { message = "Paciente registrado com sucesso"});

    }
}