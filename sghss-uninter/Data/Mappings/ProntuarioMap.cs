using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sghss_uninter.Models;

namespace sghss_uninter.Data.Mappings;

public class ProntuarioMap : IEntityTypeConfiguration<Prontuario>
{
    public void Configure(EntityTypeBuilder<Prontuario> builder)
    {
        builder.ToTable("Prontuarios");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.HasMany(p => p.Consultas)
            .WithOne(c => c.Prontuario)
            .HasForeignKey(c => c.ProntuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}