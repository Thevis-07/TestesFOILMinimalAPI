using TestesFOILMinimalApi.Abstractions;
using static TestesFOILMinimalApi.Dtos.PerguntasDtos;

namespace TestesFOILMinimalApi.Endpoints
{
    public static class PerguntaEndpoints
    {
        public static void MapPerguntaEndpoints(this WebApplication app)
        {
            app.MapPost("/perguntas", async (IPerguntaService service, PerguntaCreateDto input) =>
            {
                try
                {
                    var pergunta = await service.CreateAsync(input);
                    return Results.Created($"/perguntas/{pergunta.Id}", pergunta);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: 500,
                        title: "Erro interno no servidor"
                    );
                }
            });

            app.MapGet("/perguntas", async (IPerguntaService service) =>
            {
                try
                {
                    var perguntas = await service.ListAsync();
                    return Results.Ok(perguntas);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: 500,
                        title: "Erro interno no servidor"
                    );
                }
            });

            app.MapPut("/perguntas/{id:guid}", async (IPerguntaService service, Guid id, PerguntaUpdateDto input) =>
            {
                try
                {
                    var ok = await service.UpdateAsync(id, input);
                    return ok ? Results.NoContent() : Results.NotFound();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapDelete("/perguntas/{id:guid}", async (IPerguntaService service, Guid id) =>
            {
                try
                {
                    var ok = await service.DeleteAsync(id);
                    return ok ? Results.NoContent() : Results.NotFound();
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });
        }
    }
}
