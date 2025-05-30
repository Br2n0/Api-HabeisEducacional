using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "O ID do curso é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do curso inválido")]
        public int Curso_ID { get; set; }

        [Required(ErrorMessage = "O ID do aluno é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do aluno inválido")]
        public int Aluno_ID { get; set; }
    }

    public class MatriculaUpdateDTO
    {
        [Required(ErrorMessage = "O status da matrícula é obrigatório")]
        public StatusMatricula Status { get; set; }
    }
}
