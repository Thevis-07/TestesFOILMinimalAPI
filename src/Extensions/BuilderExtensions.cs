using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Options;
using TestesFOILMinimalApi.Services;

namespace TestesFOILMinimalApi.Extensions
{
    public static class BuilderExtensions
    {
        public static WebApplicationBuilder AddArchitectures(this WebApplicationBuilder builder)
        {

            var cs = Environment.GetEnvironmentVariable("POSTGRES_CS")
                     ?? builder.Configuration.GetConnectionString("Postgres");

            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException(
                    "Connection string do Postgres não encontrada. " +
                    "Defina POSTGRES_CS ou ConnectionStrings:Postgres."
                );

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var poolSize = builder.Configuration.GetValue<int?>("DbContextPoolSize") ?? 128;

            builder.Services.AddDbContextPool<AppDbContext>(opt =>
            {
                opt.UseNpgsql(cs, npg =>
                {
                    npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);

                    npg.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null
                    );

                    npg.CommandTimeout(60);
                });

                if (builder.Environment.IsDevelopment())
                {
                    opt.EnableDetailedErrors();
                    opt.EnableSensitiveDataLogging();

                    opt.LogTo(
                        Console.WriteLine,
                        LogLevel.Information
                    );
                }
            }, poolSize);

            builder.Services.ConfigureHttpJsonOptions(opt =>
            {
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.SerializerOptions.TypeInfoResolverChain.Add(new DefaultJsonTypeInfoResolver());
            });

            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

            return builder;
        } 

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IResultadoService, ResultadoService>();
            builder.Services.AddScoped<IPerguntaService, PerguntaService>();
            builder.Services.AddScoped<IRespostaService, RespostaService>();
            builder.Services.AddScoped<IAlunoService, AlunoService>(); 
            builder.Services.AddScoped<IEmailService, EmailService>();



            return builder; 
        }
    }
}
