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
             .RequireAuthorization("Medico&Admin");

     private static async Task<IResult> HandleAsync(AppDbContext context
         , ClaimsPrincipal user
         ,CancellationToken ct
         , int pagenumber = Configuration.DefaultPageNumber
         , int pagesize = Configuration.DefaultPageSize)
     {
         var query = context.Medicos.AsNoTracking();
         var totalCount = await query.CountAsync(ct);
         
         var medicos = await query
             .OrderByDescending(m => m.Nome)
             .Skip((pagenumber - 1) * pagesize)
             .Take(pagesize)
             .Select(m => new MedicoSimplesDTO
             {
                 Nome = m.Nome,
                 Crm = m.Crm,
                 Especialidade = m.Especialidade
             })
             .ToListAsync(ct);
         
     return Results.Ok(new PagedResponse<List<MedicoSimplesDTO>>(medicos
             , totalCount
             , pagenumber
             , pagesize));
 }
 }