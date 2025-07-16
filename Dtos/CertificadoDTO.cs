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
        [Required(ErrorMessage = "O ID da matrícula é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da matrícula inválido")]
        public int Matricula_ID { get; set; }
    }

    public class CertificadoUpdateDTO
    {
        [StringLength(100, ErrorMessage = "O documento não pode ter mais que 100 caracteres")]
        public string? Documento { get; set; }
    }
    
    public class CertificadoValidacaoDTO
    {
        [Required(ErrorMessage = "O código de validação é obrigatório")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "O código de validação deve ter entre 6 e 50 caracteres")]
        public string Codigo_Validacao { get; set; } = string.Empty;
    }
}
