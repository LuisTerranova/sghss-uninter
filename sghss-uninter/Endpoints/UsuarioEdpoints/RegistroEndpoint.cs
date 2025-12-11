using Microsoft.AspNetCore.Identity;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.Models.Identity;

namespace sghss_uninter.Endpoints.UsuarioEdpoints;

public class RegistroEndpoint : IEndpoint
{
    private record Registro(
        string Email,
        string Senha
    );

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("registro", HandleAsync)
            .WithName("RegistroAdminEndpoint")
            .WithDescription("Registro de administrador");

    private static async Task<IResult> HandleAsync(UserManager<ApplicationUser> userManager
    , RoleManager<IdentityRole>  roleManager
    , Registro registro)
    {
        var user = new ApplicationUser { UserName = registro.Email, Email = registro.Email};
        var result = await userManager.CreateAsync(user, registro.Senha);
        
        if (!result.Succeeded)
            return Results.BadRequest(result.Errors.Select(e => new { e.Code, e.Description }));
        
        const string adminRole = "ADMIN";
        
        if (!await roleManager.RoleExistsAsync(adminRole))
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        
        await userManager.AddToRoleAsync(user, adminRole);
        return Results.Ok();
    }
}
