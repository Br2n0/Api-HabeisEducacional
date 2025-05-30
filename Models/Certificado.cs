using System.ComponentModel.DataAnnotations;

namespace Api_HabeisEducacional.Models
{
    /// <summary>
    /// Entidade que representa um certificado emitido após conclusão de curso
    /// Vincula um aluno específico a um curso específico com comprovação de conclusão
    /// Configurada com CASCADE DELETE no curso e RESTRICT no aluno (ver AppDbContext)
    /// </summary>
    public class Certificado
    {
        /// <summary>
        /// Identificador único do certificado
        /// Chave primária da entidade
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Data e hora de emissão do certificado
        /// Preenchido automaticamente pelo contexto (SaveChanges override)
        /// </summary>
        [Required]
        public DateTime Data_Emissao { get; set; }

        /// <summary>
        /// Chave estrangeira para o curso
        /// Relacionamento configurado com CASCADE DELETE
        /// </summary>
        [Required]
        public int Curso_ID { get; set; }

        /// <summary>
        /// Chave estrangeira para o aluno
        /// Relacionamento configurado com RESTRICT DELETE
        /// </summary>
        [Required]
        public int Aluno_ID { get; set; }

        /// <summary>
        /// Código único de validação do certificado
        /// Utilizado para verificar autenticidade do documento
        /// Configurado como índice único no AppDbContext
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Codigo_Validacao { get; set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // PROPRIEDADES DE NAVEGAÇÃO
        // Configuradas no AppDbContext com relacionamentos otimizados
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Propriedade de navegação para o curso relacionado
        /// Relacionamento: Certificado → Curso (N:1)
        /// </summary>
        public Curso? Curso { get; set; }

        /// <summary>
        /// Propriedade de navegação para o aluno relacionado
        /// Relacionamento: Certificado → Aluno (N:1)
        /// </summary>
        public Aluno? Aluno { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // MÉTODOS UTILITÁRIOS
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Gera um código de validação único para o certificado
        /// Formato: CERT-{YYYYMMDD}-{GUID_PARTE}
        /// </summary>
        /// <returns>Código de validação único</returns>
        public static string GerarCodigoValidacao()
        {
            var data = DateTime.Now.ToString("yyyyMMdd");
            var guid = Guid.NewGuid().ToString("N")[..8].ToUpper(); // Primeiros 8 caracteres
            return $"CERT-{data}-{guid}";
        }

        /// <summary>
        /// Verifica se o certificado é válido (possui código de validação)
        /// </summary>
        /// <returns>True se o certificado for válido</returns>
        public bool IsValido() => !string.IsNullOrWhiteSpace(Codigo_Validacao);

        /// <summary>
        /// Retorna uma descrição formatada do certificado
        /// </summary>
        /// <returns>String formatada com informações do certificado</returns>
        public string GetDescricao()
        {
            return $"Certificado emitido em {Data_Emissao:dd/MM/yyyy} - Código: {Codigo_Validacao}";
        }

        /// <summary>
        /// Calcula há quantos dias o certificado foi emitido
        /// </summary>
        /// <returns>Número de dias desde a emissão</returns>
        public int DiasDesdeEmissao()
        {
            return (DateTime.Now - Data_Emissao).Days;
        }

        /// <summary>
        /// Verifica se o certificado foi emitido recentemente (últimos 30 dias)
        /// </summary>
        /// <returns>True se foi emitido nos últimos 30 dias</returns>
        public bool IsRecente() => DiasDesdeEmissao() <= 30;
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DA ENTIDADE CERTIFICADO
═══════════════════════════════════════════════════════════════════════════════════

🎯 PROPÓSITO:
   - Representa comprovação de conclusão de curso por aluno
   - Garante integridade e autenticidade através de código único
   - Mantém histórico de certificações emitidas

🔗 RELACIONAMENTOS:
   - Certificado → Curso (N:1) com CASCADE DELETE
   - Certificado → Aluno (N:1) com RESTRICT DELETE
   
📊 ÍNDICES (Configurados no AppDbContext):
   - Índice único em Codigo_Validacao (previne códigos duplicados)
   - Índice único composto (Aluno_ID + Curso_ID) previne certificados duplicados
   
⚙️ COMPORTAMENTOS:
   - Data_Emissao preenchida automaticamente
   - Codigo_Validacao deve ser único no sistema
   - Métodos utilitários para validação e formatação

🔐 CÓDIGO DE VALIDAÇÃO:
   - Formato: CERT-YYYYMMDD-XXXXXXXX
   - Exemplo: CERT-20241201-A1B2C3D4
   - Gerado automaticamente pelo método GerarCodigoValidacao()

📈 CASOS DE USO:
   - Emissão após conclusão de matrícula
   - Validação de autenticidade por terceiros
   - Histórico de certificações do aluno
   - Comprovação de qualificação

═══════════════════════════════════════════════════════════════════════════════════
*/
