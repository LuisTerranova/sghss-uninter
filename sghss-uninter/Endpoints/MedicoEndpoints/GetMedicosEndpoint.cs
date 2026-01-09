using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

namespace sghss_uninter.Endpoints.MedicoEndpoints;

 public class GetMedicosEndpoint : IEndpoint
 {
     public static void Map(IEndpointRouteBuilder app)
         => app.MapGet("/", HandleAsync)
             .RequireAuthorization("Admin&Medico");

     private static async Task<IResult> HandleAsync(AppDbContext context
         , ClaimsPrincipal user
         , int pagenumber = Configuration.DefaultPageNumber
         , int pagesize = Configuration.DefaultPageSize)
     {
         var query = context.Medicos.AsNoTracking()
             .Skip((pagenumber - 1) * pagesize)
             .Take(pagesize)
             .OrderBy(c => c.Nome);

         if (user.IsInRole("MEDICO"))
         {
             var dadosMedico = await query
                 .Select(m => new MedicoDTO 
                 {
                     Nome = m.Nome,
                     Crm = m.Crm,
                     Especialidade = m.Especialidade
                 })
                 .ToListAsync();

             return Results.Ok(dadosMedico);
         }

         var dadosAdmin = await query
             .Skip((pagenumber - 1) * pagesize)
             .Take(pagesize)
             .ToListAsync();
         return Results.Ok(dadosAdmin);
     }
 }