using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovoBanco.Domain.Entities;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

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

        builder.HasOne(x => x.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(x => x.AccountId);

        builder.HasIndex(x => new { x.AccountId, x.CreatedAt });
    }
}
