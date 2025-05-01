namespace Api_HabeisEducacional.Models
{
    public class Certificado
    {
        public int ID { get; set; }
        public DateTime Data_Emissao { get; set; }
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
        public string Codigo_Validacao { get; set; } = string.Empty;

        // Propriedades de navegação
        public Curso? Curso { get; set; }
        public Aluno? Aluno { get; set; }
    }
}
