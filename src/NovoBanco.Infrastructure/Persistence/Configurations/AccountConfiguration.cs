using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValueSql("gen_random_uuid()::text");

        builder.HasIndex(x => x.AccountNumber)
            .IsUnique();

        builder.Property(x => x.Balance)
            .HasPrecision(18, 2);

        builder.ToTable(t =>
            t.HasCheckConstraint("CK_Account_Balance", "\"Balance\" >= 0"));

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(x => x.CustomerId);

        builder.HasIndex(x => x.CustomerId);

        builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>() 
                .HasDefaultValue(AccountType.SAVINGS);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(AccountStatus.ACTIVE);

    }

}
