namespace Api_HabeisEducacional.Models
{
    public class Aluno
    {
        public int ID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTime Data_Cadastro { get; set; }

        // Propriedades de navegação
        public ICollection<Matricula>? Matriculas { get; set; }
        public ICollection<Certificado>? Certificados { get; set; }
    }
}
