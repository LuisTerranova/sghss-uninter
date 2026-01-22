using Microsoft.EntityFrameworkCore;
using sghss_uninter.Api;
using sghss_uninter.Endpoints.ConsultaEndpoints;
using sghss_uninter.Endpoints.MedicoEndpoints;
using sghss_uninter.Endpoints.PacienteEndpoints;
using sghss_uninter.Endpoints.ProntuarioEndpoints;
using sghss_uninter.Endpoints.UsuarioEdpoints;

namespace sghss_uninter.Endpoints;

public static class Endpoint 
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("/v1/medico")
            .MapEndpoint<CreateMedicoEndpoint>()
            .MapEndpoint<DeleteMedicoEndpoint>()
            .MapEndpoint<GetMedicoByIdEdpoint>()
            .MapEndpoint<GetMedicosEndpoint>();

        endpoints.MapGroup("/v1/admin")
            .MapEndpoint<LoginEndpoint>()
            .MapEndpoint<RegistroEndpoint>();

        endpoints.MapGroup("/v1/consultas")
            .MapEndpoint<CreateConsultaEndpoint>()
            .MapEndpoint<UpdateConsultaEndpoint>()
            .MapEndpoint<DeleteConsultaEndpoint>()
            .MapEndpoint<GetConsultaByIdEndpoint>()
            .MapEndpoint<GetConsultasEndpoint>();

        endpoints.MapGroup("/v1/pacientes")
            .MapEndpoint<CreatePacienteEndpoint>()
            .MapEndpoint<GetPacienteByIdEndpoint>()
            .MapEndpoint<GetPacientesEndpoint>();

        endpoints.MapGroup("/v1/prontuarios")
            .MapEndpoint<GetProntuarioByIdEndpoint>();


    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}