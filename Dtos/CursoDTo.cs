using System.ComponentModel.DataAnnotations;

namespace Api_HabeisEducacional.DTOs
{
    public class CursoDTO
    {
        public int ID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Instrutor { get; set; }
        public decimal Preco { get; set; }
        public int Duracao { get; set; }
    }

    public class CursoCreateDTO
    {
        [Required(ErrorMessage = "O título do curso é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição não pode ter mais que 1000 caracteres")]
        public string? Descricao { get; set; }

        [StringLength(100, ErrorMessage = "O nome do instrutor não pode ter mais que 100 caracteres")]
        public string? Instrutor { get; set; }

        [Required(ErrorMessage = "O preço do curso é obrigatório")]
        [Range(0, 99999.99, ErrorMessage = "O preço deve estar entre 0 e 99.999,99")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A duração do curso é obrigatória")]
        [Range(1, 1000, ErrorMessage = "A duração deve estar entre 1 e 1000 horas")]
        public int Duracao { get; set; }
    }
}
