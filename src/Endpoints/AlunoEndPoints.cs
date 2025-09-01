using TestesFOILMinimalApi.Abstractions;
using static TestesFOILMinimalApi.Dtos.AlunosDto;

namespace TestesFOILMinimalApi.Endpoints
{
    public static class AlunoEndPoints
    {
        public static void MapAlunoEndpoints(this WebApplication app)
        {
            app.MapPost("/alunos", async (IAlunoService service, AlunoCreateDto input) =>
            {
                try
                {
                    var aluno = await service.CreateAsync(input);
                    return Results.Created($"/alunos/{aluno.Id}", aluno);
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

            app.MapGet("/alunos", async (IAlunoService service) =>
            {
                try
                {
                    var alunos = await service.ListAsync();
                    return Results.Ok(alunos);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapGet("/alunos/{id:guid}", async (IAlunoService service, Guid id) =>
            {
                try
                {
                    var aluno = await service.GetByIdAsync(id);
                    return aluno is null ? Results.NotFound() : Results.Ok(aluno);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });

            app.MapPut("/alunos/{id:guid}", async (IAlunoService service, Guid id, AlunoUpdateDto input) =>
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

            app.MapDelete("/alunos/{id:guid}", async (IAlunoService service, Guid id) =>
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
