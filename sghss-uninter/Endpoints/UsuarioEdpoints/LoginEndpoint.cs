using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Data;
using sghss_uninter.Models.Identity;

namespace sghss_uninter.Endpoints.UsuarioEdpoints;

public class LoginEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("login", HandleAsync)
            .WithName("AdminLoginEndpoint")
            .WithDescription("Login como medico/administrador");

    private record LoginRequest(string Email, string Password);
    
    private static async Task<IResult> HandleAsync(
        SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager,
        AppDbContext context, 
        LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null) return Results.Unauthorized();

        var result = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

        if (!result.Succeeded) return Results.Unauthorized();
        await signInManager.SignOutAsync();

        await signInManager.SignInAsync(user, isPersistent: false);

        return Results.Ok(new { message = "Login realizado com sucesso." });
    }
}