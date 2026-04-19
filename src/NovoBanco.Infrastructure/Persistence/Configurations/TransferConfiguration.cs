using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovoBanco.Domain.Entities;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.ToTable("transfers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Reference)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Reference)
            .IsUnique();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(x => x.FromAccountId);
        builder.HasIndex(x => x.ToAccountId);
    }
}
