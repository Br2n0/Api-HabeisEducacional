using System;
using System.ComponentModel.DataAnnotations;
using Api_HabeisEducacional.Models.Common;
using Api_HabeisEducacional.Models.Events;

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
    /// 🔄 MELHORIA: Agora herda de EntidadeBase para suportar Eventos de Domínio
    /// </summary>
    public class Matricula : EntidadeBase
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

        // ═══════════════════════════════════════════════════════════════
        // 🔄 CONSTRUTORES
        // ═══════════════════════════════════════════════════════════════
        
        /// <summary>
        /// Construtor padrão necessário para o Entity Framework
        /// Inicializa com status Ativa por padrão
        /// </summary>
        public Matricula()
        {
            _status = StatusMatricula.Ativa;
        }

        /// <summary>
        /// Construtor para criar uma nova matrícula
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="dataMatricula">Data da matrícula</param>
        public Matricula(int alunoId, int cursoId, DateTime dataMatricula)
        {
            Aluno_ID = alunoId;
            Curso_ID = cursoId;
            Data_Matricula = dataMatricula;
            _status = StatusMatricula.Ativa;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔄 MELHORIA: STATUS COM EVENTOS DE DOMÍNIO
        // ═══════════════════════════════════════════════════════════════
        
        /// <summary>
        /// Campo privado para armazenar o status atual
        /// Usado pelo property público para controlar mudanças
        /// </summary>
        private StatusMatricula _status;
        
        /// <summary>
        /// Status atual da matrícula com EVENTOS DE DOMÍNIO
        /// BENEFÍCIOS DOS EVENTOS:
        /// - Rastreabilidade de mudanças de estado
        /// - Possibilidade de triggers automáticos (emails, notificações)
        /// - Auditoria automática de alterações
        /// - Desacoplamento de efeitos colaterais
        /// - Facilita integração com outros sistemas
        /// </summary>
        [Required]
        public StatusMatricula Status 
        { 
            get => _status;
            private set
            {
                // Só dispara evento se houve mudança real de status
                if (_status != value)
                {
                    var statusAntigo = _status;
                    _status = value;
                    // Dispara evento de domínio para rastrear a mudança
                    AdicionarEvento(new MatriculaStatusAlteradoEvent(ID, statusAntigo, value));
                }
            }
        }

        /* CÓDIGO ANTERIOR (mantido para estudo):
        /// <summary>
        /// Status atual da matrícula
        /// Convertido para string no banco de dados via Fluent API
        /// </summary>
        [Required]
        public StatusMatricula Status { get; set; } = StatusMatricula.Ativa; // Status padrão
        */

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
        // 🔄 MÉTODOS DE DOMÍNIO MELHORADOS
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
        /// 🔄 MÉTODO MELHORADO: Marca a matrícula como concluída
        /// BENEFÍCIOS:
        /// - Validação de regras de negócio (só pode concluir se estiver ativa)
        /// - Disparo automático de evento de domínio
        /// - Encapsulamento da lógica de transição de estado
        /// </summary>
        public void Concluir()
        {
            if (Status != StatusMatricula.Ativa)
                throw new InvalidOperationException("Apenas matrículas ativas podem ser concluídas");
            
            Status = StatusMatricula.Concluida; // Dispara evento automaticamente
        }

        /* MÉTODO ANTERIOR (mantido para estudo):
        public void MarcarComoConcluida() => Status = StatusMatricula.Concluida;
        */

        /// <summary>
        /// 🔄 MÉTODO MELHORADO: Cancela a matrícula
        /// BENEFÍCIOS:
        /// - Validação de regras de negócio (não pode cancelar se já concluída)
        /// - Disparo automático de evento de domínio
        /// - Proteção contra operações inválidas
        /// </summary>
        public void Cancelar()
        {
            if (Status == StatusMatricula.Concluida)
                throw new InvalidOperationException("Matrículas concluídas não podem ser canceladas");
            
            Status = StatusMatricula.Cancelada; // Dispara evento automaticamente
        }

        /* MÉTODO ANTERIOR (mantido para estudo):
        public void Cancelar() => Status = StatusMatricula.Cancelada;
        */
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DAS MELHORIAS IMPLEMENTADAS
═══════════════════════════════════════════════════════════════════════════════════

1️⃣ EVENTOS DE DOMÍNIO:
   ✅ Rastreamento automático de mudanças de status
   ✅ Possibilidade de triggers (emails, notificações)
   ✅ Auditoria automática
   ✅ Desacoplamento de efeitos colaterais

2️⃣ VALIDAÇÕES DE DOMÍNIO:
   ✅ Regras de negócio encapsuladas nos métodos
   ✅ Proteção contra transições inválidas de estado
   ✅ Exceções com mensagens claras

3️⃣ ENCAPSULAMENTO:
   ✅ Status privado com controle via property
   ✅ Métodos específicos para cada transição
   ✅ Lógica de negócio dentro da entidade

🎯 BENEFÍCIOS OBTIDOS:
- Código mais seguro e consistente
- Melhor rastreabilidade de mudanças
- Facilita testes unitários
- Prepara para integrações futuras
- Melhora a manutenibilidade

═══════════════════════════════════════════════════════════════════════════════════
*/
