using Microsoft.AspNetCore.Identity;
using sghss_uninter.Api;
using sghss_uninter.Models.Identity;

namespace sghss_uninter.Endpoints.UsuarioEdpoints;

public class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
        => endpoints.MapPost("logout", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(SignInManager<ApplicationUser>  signInManager
    , HttpContext httpContext)
    {
            await signInManager.SignOutAsync();
            return Results.Ok(new { message = "Logout realizado e cookies expirados." });
    }
}