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
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Instrutor { get; set; }
        public decimal Preco { get; set; }
        public int Duracao { get; set; }
    }
}
