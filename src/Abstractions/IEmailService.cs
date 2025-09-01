using TestesFOILMinimalApi.Services;
using static TestesFOILMinimalApi.Dtos.Email.EmailDto;

namespace TestesFOILMinimalApi.Abstractions
{
    public interface IEmailService
    {
        Task SendRelatorioCategoriasAsync(
            IEnumerable<CategoriaResumoItem> itens,
            string to,
            AlunoEmail aluno,
            string? subjectPrefix = null,
            CancellationToken ct = default);
    }
}
