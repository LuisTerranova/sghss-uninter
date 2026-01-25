using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;

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

        //WIP tratar se caso o medico for ele mesmo retornar todas as infos, ou um endpoint
        //diferente para nao deixar este cluttered.
        if (user.IsInRole("MEDICO"))
        {
            var medico = await query.Select(m => new MedicoSimplesDTO
            {
                Nome = m.Nome,
                Crm = m.Crm,
                Especialidade = m.Especialidade
            })
            .FirstOrDefaultAsync();
            
            return medico == null 
                ? Results.NotFound() 
                : Results.Ok(medico);
        }

        var medicoCompleto = await query
            .Select(m => new MedicoDetalhesDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                Cpf = m.Cpf,
                Crm = m.Crm,
                Especialidade = m.Especialidade,
                Email = m.Email,
                Telefone = m.Telefone
            } )
            .FirstOrDefaultAsync();
        
        return medicoCompleto == null 
            ? Results.NotFound() 
            : Results.Ok(medicoCompleto);
    }

}