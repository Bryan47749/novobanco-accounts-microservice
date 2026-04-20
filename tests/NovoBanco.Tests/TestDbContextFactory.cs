using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NovoBanco.Infrastructure.Persistence;

public static class TestDbContextFactory
{
    public static NovoBancoDbContext Create()
    {
        var options = new DbContextOptionsBuilder<NovoBancoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new NovoBancoDbContext(options);
    }
}
