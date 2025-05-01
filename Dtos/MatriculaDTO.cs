using Api_HabeisEducacional.Models;

namespace Api_HabeisEducacional.DTOs
{
    public class MatriculaDTO
    {
        public int ID { get; set; }
        public DateTime Data_Matricula { get; set; }
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
        public StatusMatricula Status { get; set; }
        public string CursoTitulo { get; set; } = string.Empty;
        public string AlunoNome { get; set; } = string.Empty;
    }

    public class MatriculaCreateDTO
    {
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
    }

    public class MatriculaUpdateDTO
    {
        public StatusMatricula Status { get; set; }
    }
}
