using System;
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
                    var resultado = await service.CalcularResultadoAsync(alunoId);

                    return Results.Ok(resultado);
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
        }
    }
}