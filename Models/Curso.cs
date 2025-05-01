namespace Api_HabeisEducacional.Models
{
    public class Curso
    {
        public int ID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Instrutor { get; set; }
        public decimal Preco { get; set; }
        public int Duracao { get; set; } // em horas

        // Propriedades de navegação
        public ICollection<Matricula>? Matriculas { get; set; }
        public ICollection<Certificado>? Certificados { get; set; }
    }
}
