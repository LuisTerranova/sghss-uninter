using Microsoft.AspNetCore.Identity;
using sghss_uninter.Api;
using sghss_uninter.Models.Identity;

namespace sghss_uninter.Endpoints.UsuarioEdpoints;

public class LoginEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("login", HandleAsync)
            .WithName("AdminLoginEndpoint")
            .WithDescription("Login como medico/administrador");

    private record LoginRequest(string Email, string Password);
    
    private static async Task<IResult> HandleAsync(SignInManager<ApplicationUser>  signInManager
    , LoginRequest loginRequest)
    {
        var result = await signInManager.PasswordSignInAsync(
            loginRequest.Email, 
            loginRequest.Password, 
            isPersistent: false, 
            lockoutOnFailure: false 
        );
        
        return result.Succeeded 
            ? Results.Ok(new { message = "Login realizado com sucesso." }) 
            : Results.Unauthorized();
    }
}