using Microsoft.AspNetCore.Identity;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Endpoints.UsuarioEdpoints;
using sghss_uninter.Models;
using sghss_uninter.Models.Identity;

namespace sghss_uninter.Endpoints.MedicoEndpoints;

public class CreateMedicoEndpoint : IEndpoint
{
public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("registro", HandleAsync)
        .RequireAuthorization("Administrador")
        .WithName("RegistroMedicoEndpoint")
        .WithDescription("Registro de medicos");

private static async Task<IResult> HandleAsync(UserManager<ApplicationUser> userManager
    , RoleManager<IdentityRole> roleManager
    , AppDbContext context
    , MedicoRegistroDTO registro)
{
    var user = new ApplicationUser { UserName = registro.Email, Email = registro.Email };
    var result = await userManager.CreateAsync(user, registro.Password);

    if (!result.Succeeded)
        return Results.BadRequest("Erro ao criar medico");
    
    const string medicoRole = "MEDICO";
    
    if (!await roleManager.RoleExistsAsync(medicoRole))
    {
        await roleManager.CreateAsync(new IdentityRole(medicoRole));
    }

    await userManager.AddToRoleAsync(user, medicoRole);
    
    var novoMedico = new Medico
    {
        Nome = registro.Nome,
        Email = registro.Email,
        Crm = registro.Crm,
        Cpf = registro.Cpf,
        Telefone = registro.Telefone,
        ApplicationUserId = user.Id,
        Especialidade = registro.Especialidade,
    };
    
    context.Medicos.Add(novoMedico);
    await context.SaveChangesAsync();

    return Results.Created($"/api/medicos/{novoMedico.Id}", 
        new { message = "Médico e usuário registrados com sucesso.", MedicoId = novoMedico.Id });
}
}