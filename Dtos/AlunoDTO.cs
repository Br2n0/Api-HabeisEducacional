using System.ComponentModel.DataAnnotations;

namespace Api_HabeisEducacional.DTOs
{
    public class AlunoDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Data_Cadastro { get; set; }
    }

    public class AlunoCreateDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email fornecido não é válido")]
        [StringLength(150, ErrorMessage = "O email não pode ter mais que 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }

    public class AlunoLoginDTO
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O email fornecido não é válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;
    }

    public class AlunoUpdateDTO
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string? Nome { get; set; }

        [EmailAddress(ErrorMessage = "O email fornecido não é válido")]
        [StringLength(150, ErrorMessage = "O email não pode ter mais que 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(255, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 255 caracteres")]
        public string? Senha { get; set; }
    }
}
