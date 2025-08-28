using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Dtos;

namespace TestesFOILMinimalApi.Endpoints
{
    public static class RespostasEndPoint
    {
        public static void MapRespostaEndpoints(this WebApplication app)
        {
            app.MapPost("/respostas", async (IRespostaService service, RespostasDto.RespostaCreateDto input) =>
            {
                try
                {
                    var resposta = await service.SaveAsync(input, recalcResultado: true);
                    return Results.Created($"/respostas/{resposta.Id}", resposta);
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
