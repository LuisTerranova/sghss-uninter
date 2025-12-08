using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sghss_uninter.Models;

namespace sghss_uninter.Data.Mappings;

public class MedicoMap : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> builder)
    {
        builder.ToTable("Medicos");
        builder.HasKey(c => c.Id);
        
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(m => m.Nome).HasMaxLength(100).IsRequired();
        builder.Property(m => m.Email).HasMaxLength(100).IsRequired();
        builder.Property(m => m.Telefone).HasMaxLength(11).IsRequired();
        builder.Property(m => m.Cpf).HasMaxLength(11).IsRequired();
        builder.Property(m => m.Crm).HasMaxLength(12).IsRequired();
        
        builder.HasMany(m => m.Consultas)
            .WithOne(m => m.Medico)
            .HasForeignKey(m => m.MedicoId);
    }
}