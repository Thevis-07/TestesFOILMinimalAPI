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
                    var resposta = await service.SaveAsync(input);
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

            app.MapPost("/respostas/bulk", async (
                IRespostaService service,
                RespostasDto.RespostaCreateListDto input) =>
            {
                try
                {
                    var respostas = await service.SaveManyAsync(input);
                    return Results.Ok(respostas);
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
