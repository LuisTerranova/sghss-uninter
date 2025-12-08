using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sghss_uninter.Models;

namespace sghss_uninter.Data.Mappings;

public class PacienteMap : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("Pacientes");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.Nome).HasMaxLength(100).IsRequired();
        builder.Property(p => p.DataNasc).HasColumnType("date").IsRequired();
        builder.Property(p => p.Email).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Cpf).HasMaxLength(11);
        builder.Property(p => p.Telefone).HasMaxLength(11);


        builder.HasOne(p => p.Prontuario)
            .WithOne(p => p.Paciente)
            .HasForeignKey<Prontuario>(p => p.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}