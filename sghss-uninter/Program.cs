using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sghss_uninter;
using sghss_uninter.Data;
using sghss_uninter.DTOs;
using sghss_uninter.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }
}

#region Endpoints Paciente

app.MapPost("/pacientes", (AppDbContext db, [FromBody] PacienteDTO novoPaciente) =>
    {
        if (novoPaciente is null) {
            return Results.BadRequest("Dados do paciente invalidos");
        }

        var paciente = new Paciente
        {
            Nome = novoPaciente.Nome,
            Email = novoPaciente.Email,
            Telefone = novoPaciente.Telefone,
            DataNasc = novoPaciente.DataNasc,
            Cpf = novoPaciente.Cpf
        };
        db.Pacientes.Add(paciente);
        db.SaveChanges();
        var response = new Response<Paciente>(paciente, "paciente criado com sucesso!", 201);
        return Results.Created($"/pacientes/{paciente.Id}", response);
    })
    .WithOpenApi()
    .WithName("CriarPaciente");

app.MapGet("/pacientes", (AppDbContext db) =>
    {
        var pacientes = db.Pacientes
            .Include(p => p.Prontuario)
            .ToList();
        return Results.Ok(pacientes);
    })
    .WithOpenApi()
    .WithName("BuscarTodosPacientes");

app.MapPut("/pacientes/{id:int}", async (int id, AppDbContext db, [FromBody] PacienteDTO paciente) =>
    {
        if (paciente is null)
        {
            return Results.BadRequest("Dados do paciente invalidos");
        }
    
        try
        {
            var pacienteExistente = await db.Pacientes.FindAsync(id);
            if (pacienteExistente is null)
                return Results.NotFound("Paciente informado nao encontrado para atualizacao");
            
            pacienteExistente.Nome = paciente.Nome;
            pacienteExistente.Email = paciente.Email;
            pacienteExistente.Telefone = paciente.Telefone;
            
            db.Pacientes.Update(pacienteExistente);
            await db.SaveChangesAsync();
            
            return Results.Ok(paciente);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!db.Pacientes.Any(e => e.Id == id))
            {
                return Results.NotFound();
            }
            throw;
        }
    })
    .WithOpenApi()
    .WithName("AtualizarPaciente");

app.MapGet("/pacientes/{id:int}", async (AppDbContext db, int id) =>
    {
        var paciente = await db.Pacientes
            .Include(p => p.Prontuario)
            .Where(p  => p.Id == id)
            .FirstOrDefaultAsync();
        return paciente is null ? Results.NotFound() : Results.Ok(paciente);
    })
    .WithOpenApi()
    .WithName("ObterPacientePorId");

#endregion

#region Endpoints Medico

app.MapPost("/medicos", async (AppDbContext db, [FromBody] Medico novoMedico) =>
{
    if (novoMedico is null)
    {
        return Results.BadRequest("Dados do medico invalidos");
    }
    novoMedico.Consultas = new List<Consulta>();
    db.Medicos.Add(novoMedico);
    await db.SaveChangesAsync();
    return Results.Ok(novoMedico);
});

app.MapGet("/medicos", (AppDbContext db) =>
{
    var medicos = db.Medicos.ToList();
    return Results.Ok(medicos);
});

app.MapPut("/medicos", async (AppDbContext db, MedicoDTO medico) =>
{
    if (medico is null)
        return Results.NotFound("Dados do medico invalidos");
    db.Entry(medico).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.Ok(medico);
});

#endregion
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();