namespace TestesFOILMinimalApi.Dtos
{
    public class AlunosDto
    {
        public record AlunoCreateDto(string Nome, string Email, int Idade, string Curso, int Semestre);
        public record AlunoUpdateDto(string Email, int Idade, string Curso, int Semestre);
        public record AlunoReadDto(Guid Id, string Nome, string Email, int Idade, string Curso, int Semestre);
    }
}
