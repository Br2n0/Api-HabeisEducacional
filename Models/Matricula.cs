using System.ComponentModel.DataAnnotations;

namespace Api_HabeisEducacional.Models
{
    /// <summary>
    /// Enumeração dos possíveis status de uma matrícula
    /// Utilizado para controlar o ciclo de vida da matrícula do aluno
    /// </summary>
    public enum StatusMatricula
    {
        /// <summary>
        /// Matrícula ativa - Aluno está cursando
        /// </summary>
        Ativa,
        
        /// <summary>
        /// Matrícula concluída - Aluno finalizou o curso
        /// </summary>
        Concluida,
        
        /// <summary>
        /// Matrícula cancelada - Aluno desistiu ou foi cancelada
        /// </summary>
        Cancelada
    }

    /// <summary>
    /// Entidade que representa a matrícula de um aluno em um curso específico
    /// Atua como entidade de relacionamento Many-to-Many materializada entre Aluno e Curso
    /// Configurada com CASCADE DELETE no curso e RESTRICT no aluno (ver AppDbContext)
    /// </summary>
    public class Matricula
    {
        /// <summary>
        /// Identificador único da matrícula
        /// Chave primária da entidade
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Data e hora em que a matrícula foi realizada
        /// Preenchido automaticamente pelo contexto (SaveChanges override)
        /// </summary>
        [Required]
        public DateTime Data_Matricula { get; set; }

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
        /// Status atual da matrícula
        /// Convertido para string no banco de dados via Fluent API
        /// </summary>
        [Required]
        public StatusMatricula Status { get; set; } = StatusMatricula.Ativa; // Status padrão

        // ═══════════════════════════════════════════════════════════════
        // PROPRIEDADES DE NAVEGAÇÃO
        // Configuradas no AppDbContext com relacionamentos otimizados
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Propriedade de navegação para o curso relacionado
        /// Relacionamento: Matricula → Curso (N:1)
        /// </summary>
        public Curso? Curso { get; set; }

        /// <summary>
        /// Propriedade de navegação para o aluno relacionado
        /// Relacionamento: Matricula → Aluno (N:1)
        /// </summary>
        public Aluno? Aluno { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // MÉTODOS UTILITÁRIOS
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Verifica se a matrícula está ativa
        /// </summary>
        /// <returns>True se a matrícula estiver ativa</returns>
        public bool IsAtiva() => Status == StatusMatricula.Ativa;

        /// <summary>
        /// Verifica se a matrícula foi concluída
        /// </summary>
        /// <returns>True se a matrícula estiver concluída</returns>
        public bool IsConcluida() => Status == StatusMatricula.Concluida;

        /// <summary>
        /// Verifica se a matrícula foi cancelada
        /// </summary>
        /// <returns>True se a matrícula estiver cancelada</returns>
        public bool IsCancelada() => Status == StatusMatricula.Cancelada;

        /// <summary>
        /// Marca a matrícula como concluída
        /// </summary>
        public void MarcarComoConcluida() => Status = StatusMatricula.Concluida;

        /// <summary>
        /// Cancela a matrícula
        /// </summary>
        public void Cancelar() => Status = StatusMatricula.Cancelada;
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DA ENTIDADE MATRICULA
═══════════════════════════════════════════════════════════════════════════════════

🎯 PROPÓSITO:
   - Materializa o relacionamento Many-to-Many entre Aluno e Curso
   - Armazena informações específicas da matrícula (data, status)
   - Permite controle do ciclo de vida da participação do aluno no curso

🔗 RELACIONAMENTOS:
   - Matricula → Curso (N:1) com CASCADE DELETE
   - Matricula → Aluno (N:1) com RESTRICT DELETE
   
📊 ÍNDICES (Configurados no AppDbContext):
   - Índice único composto (Aluno_ID + Curso_ID) previne matrículas duplicadas
   
⚙️ COMPORTAMENTOS:
   - Status padrão: Ativa
   - Data_Matricula preenchida automaticamente
   - Enum convertido para string no banco
   - Métodos utilitários para verificação de status

🔄 CICLO DE VIDA:
   1. Ativa (padrão) → Estudando
   2. Concluida → Curso finalizado com sucesso
   3. Cancelada → Desistência ou cancelamento administrativo

═══════════════════════════════════════════════════════════════════════════════════
*/
