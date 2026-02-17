using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyAuth.Data.Context;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // when running on macOS/Linux the LocalDB provider is unavailable; read a connection from 
        // environment variable `MYAUTH_CONNECTION` or fall back to the same string used by the API.
        var conn = Environment.GetEnvironmentVariable("MYAUTH_CONNECTION")
                   ?? "Server=localhost,1433;Database=MyAuthDb_SQL;User Id=sa;Password=SuaSenhaForte123;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(conn);
        return new AppDbContext(optionsBuilder.Options);
    }
}
