var builder = WebApplication.CreateSlimBuilder(args);

builder.AddArchitectures()
        .AddServices();

var app = builder.Build();

app.MapHomeEndpoints();
app.MapAlunoEndpoints();
app.MapRespostaEndpoints();

app.UseArchitectures();

app.Run();
