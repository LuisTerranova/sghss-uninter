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
        
        var customClaims = new List<Claim>();
        var medico = await context.Medicos
            .FirstOrDefaultAsync(m => m.ApplicationUserId == user.Id);
        
        if (medico != null)
            customClaims.Add(new Claim("medicoid", medico.Id.ToString()));
        
            
        await signInManager.SignInWithClaimsAsync(user, isPersistent: false, customClaims);

        return Results.Ok(new { message = "Login realizado com sucesso." });

    }
}