using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Data;

namespace TestesFOILMinimalApi.Extensions
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder AddArchitectures(this WebApplicationBuilder builder)
        {

            var cs = builder.Configuration.GetConnectionString("Postgres");

            builder.Services.AddDbContextPool<AppDbContext>(opt =>
            {
                opt.UseNpgsql(cs, npg =>
                    npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
                opt.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
            });

            // Para compatibilidade com timestamps antigos (se necessário)
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return builder;
        } 

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            return builder; 
        }
    }
}
