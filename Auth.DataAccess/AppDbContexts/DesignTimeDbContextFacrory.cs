using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Auth.DataAccess.AppDbContexts;

public class DesignTimeDbContextFacrory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=185.74.5.76;Port=;Database=; Username=postgres; Password=;");

        return new AppDbContext(optionsBuilder.Options);
    }
}
