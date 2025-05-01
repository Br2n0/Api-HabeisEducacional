namespace Api_HabeisEducacional.DTOs
{
    public class CertificadoDTO
    {
        public int ID { get; set; }
        public DateTime Data_Emissao { get; set; }
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
        public string Codigo_Validacao { get; set; } = string.Empty;
        public string CursoTitulo { get; set; } = string.Empty;
        public string AlunoNome { get; set; } = string.Empty;
    }

    public class CertificadoCreateDTO
    {
        public int Curso_ID { get; set; }
        public int Aluno_ID { get; set; }
    }
    
    public class CertificadoValidacaoDTO
    {
        public string Codigo_Validacao { get; set; } = string.Empty;
    }
}
