using System.ComponentModel.DataAnnotations;

namespace Api_HabeisEducacional.DTOs
{
    public class CertificadoDTO
    {
        public int ID { get; set; }
        public DateTime Data_Emissao { get; set; }
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
        public string Codigo_Validacao { get; set; } = string.Empty;
        public string? Area { get; set; }
        public string? Nivel { get; set; }
        public string? Documento { get; set; }
        public decimal? NotaFinal { get; set; }
        public int? CargaHoraria { get; set; }
        public string CursoTitulo { get; set; } = string.Empty;
        public string AlunoNome { get; set; } = string.Empty;
    }

    public class CertificadoCreateDTO
    {
        [Required(ErrorMessage = "O ID do curso é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do curso inválido")]
        public int Curso_ID { get; set; }

        [Required(ErrorMessage = "O ID do aluno é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do aluno inválido")]
        public int Aluno_ID { get; set; }

        [StringLength(100, ErrorMessage = "A área não pode ter mais que 100 caracteres")]
        public string? Area { get; set; }

        [StringLength(50, ErrorMessage = "O nível não pode ter mais que 50 caracteres")]
        public string? Nivel { get; set; }

        [StringLength(100, ErrorMessage = "O documento não pode ter mais que 100 caracteres")]
        public string? Documento { get; set; }

        [Range(0, 10, ErrorMessage = "A nota final deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }

        [Range(1, 9999, ErrorMessage = "A carga horária deve estar entre 1 e 9999 horas")]
        public int? CargaHoraria { get; set; }
    }

    public class CertificadoUpdateDTO
    {
        [StringLength(100, ErrorMessage = "A área não pode ter mais que 100 caracteres")]
        public string? Area { get; set; }

        [StringLength(50, ErrorMessage = "O nível não pode ter mais que 50 caracteres")]
        public string? Nivel { get; set; }

        [StringLength(100, ErrorMessage = "O documento não pode ter mais que 100 caracteres")]
        public string? Documento { get; set; }

        [Range(0, 10, ErrorMessage = "A nota final deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }

        [Range(1, 9999, ErrorMessage = "A carga horária deve estar entre 1 e 9999 horas")]
        public int? CargaHoraria { get; set; }
    }
    
    public class CertificadoValidacaoDTO
    {
        [Required(ErrorMessage = "O código de validação é obrigatório")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "O código de validação deve ter entre 6 e 50 caracteres")]
        public string Codigo_Validacao { get; set; } = string.Empty;
    }
}
