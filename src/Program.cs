Env.Load();

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddArchitectures()
        .AddServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()         
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var cs = Environment.GetEnvironmentVariable("POSTGRES_CS");
Console.WriteLine($"Using connection string: {cs}");

try
{
    using var conn = new NpgsqlConnection(cs);
    conn.Open();
    Console.WriteLine("PostgreSQL connection successful.");
    conn.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"PostgreSQL connection failed: {ex.Message}");
}

var app = builder.Build();

app.MapHomeEndpoints();
app.MapAlunoEndpoints();
app.MapRespostaEndpoints();
app.MapPerguntaEndpoints();
app.MapResultadoEndpoints();
app.UseCors();
app.UseArchitectures();

app.Run();
