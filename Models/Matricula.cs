namespace Api_HabeisEducacional.Models
{
    public enum StatusMatricula
    {
        Ativa,
        Concluida,
        Cancelada
    }

    public class Matricula
    {
        public int ID { get; set; }
        public DateTime Data_Matricula { get; set; }
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
        public StatusMatricula Status { get; set; }

        // Propriedades de navegação
        public Curso? Curso { get; set; }
        public Aluno? Aluno { get; set; }
    }
}
