using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sghss_uninter.Models;

namespace sghss_uninter.Data.Mappings;

public class ConsultaMap : IEntityTypeConfiguration<Consulta>
{
    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        builder.ToTable("Consultas");
        builder.HasKey(x => x.Id);
        
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();
        builder.Property(c => c.DataHora)
            .IsRequired();
        builder.Property(c => c.DataCadastro)
            .IsRequired();
        builder.Property(c => c.Status)
            .IsRequired();
        builder.Property(c => c.Anamnese)
            .IsRequired();
        
        builder.Property(x => x.MedicoId);
        builder.HasOne(c => c.Medico)
            .WithMany(m => m.Consultas)
            .HasForeignKey(c => c.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(c => c.ProntuarioId);
        builder.HasOne(c => c.Prontuario)
            .WithMany(c => c.Consultas)
            .HasForeignKey(c => c.ProntuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.PacienteId);
        builder.HasOne(c => c.Paciente)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}