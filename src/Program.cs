var builder = WebApplication.CreateSlimBuilder(args);

builder.AddArchitectures()
        .AddServices();

var app = builder.Build();

app.MapHomeEndpoints();

app.UseArchitectures();

app.Run();
