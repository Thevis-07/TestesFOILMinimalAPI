using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Dtos;

namespace TestesFOILMinimalApi.Endpoints
{
    public static class ResultadoEndpoints
    {
        public static void MapResultadoEndpoints(this WebApplication app)
        {
            app.MapGet("/resultados/calcular/{alunoId:guid}", async (IResultadoService service, Guid alunoId) =>
            {
                try
                {
                    var resultados = await service.CalcularResultadoAsync(alunoId);
                    if (resultados == null || !resultados.Any())
                        return Results.NotFound(new { error = "Nenhum resultado encontrado ou calculável para o aluno." });

                    return Results.Ok(resultados);
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
        }
    }
}