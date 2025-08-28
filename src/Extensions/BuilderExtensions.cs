using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Services;

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

            builder.Services.ConfigureHttpJsonOptions(opt =>
            {
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.SerializerOptions.TypeInfoResolverChain.Add(new DefaultJsonTypeInfoResolver());
            });

            return builder;
        } 

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPerguntaService, PerguntaService>();
            builder.Services.AddScoped<IRespostaService, RespostaService>();
            builder.Services.AddScoped<ResultadoService>();
            builder.Services.AddScoped<IAlunoService, AlunoService>(); 



            return builder; 
        }
    }
}
