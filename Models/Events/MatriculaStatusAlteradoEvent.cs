using Api_HabeisEducacional.Models.Common;

namespace Api_HabeisEducacional.Models.Events
{
    /// <summary>
    /// 🔄 EVENTO DE DOMÍNIO: MatriculaStatusAlteradoEvent
    /// PROPÓSITO: Capturar e comunicar mudanças no status de matrículas
    /// BENEFÍCIOS:
    /// - Rastreabilidade de mudanças críticas
    /// - Desacoplamento de efeitos colaterais
    /// - Facilita auditoria e compliance
    /// - Permite triggers automáticos
    /// - Integração com outros sistemas
    /// </summary>
    public class MatriculaStatusAlteradoEvent : DomainEvent
    {
        /// <summary>
        /// ID da matrícula que teve o status alterado
        /// Usado para identificar qual registro foi afetado
        /// </summary>
        public int MatriculaId { get; }
        
        /// <summary>
        /// Status anterior da matrícula
        /// Importante para entender a transição de estado
        /// Usado para validações e regras de negócio
        /// </summary>
        public StatusMatricula StatusAntigo { get; }
        
        /// <summary>
        /// Novo status da matrícula
        /// Representa o estado atual após a mudança
        /// Usado para determinar ações específicas
        /// </summary>
        public StatusMatricula NovoStatus { get; }

        /// <summary>
        /// Construtor do evento com informações completas da mudança
        /// </summary>
        /// <param name="matriculaId">ID da matrícula afetada</param>
        /// <param name="statusAntigo">Status antes da mudança</param>
        /// <param name="novoStatus">Status após a mudança</param>
        public MatriculaStatusAlteradoEvent(int matriculaId, StatusMatricula statusAntigo, StatusMatricula novoStatus)
        {
            MatriculaId = matriculaId;
            StatusAntigo = statusAntigo;
            NovoStatus = novoStatus;
            // OcorreuEm é definido automaticamente pela classe base DomainEvent
        }
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DO EVENTO DE DOMÍNIO
═══════════════════════════════════════════════════════════════════════════════════

🎯 PROPÓSITO DO EVENTO:
   - Notificar outras partes do sistema sobre mudanças de status
   - Permitir ações automatizadas baseadas na mudança
   - Manter histórico detalhado de transições
   - Facilitar integração com sistemas externos

🔄 CENÁRIOS DE USO:

   📧 MATRÍCULA CONCLUÍDA (Ativa → Concluída):
      - Enviar email de parabéns
      - Gerar certificado automaticamente
      - Atualizar estatísticas do curso
      - Notificar sistema de gamificação

   ❌ MATRÍCULA CANCELADA (Ativa → Cancelada):
      - Enviar pesquisa de satisfação
      - Processar reembolso se aplicável
      - Liberar vaga para lista de espera
      - Atualizar métricas de abandono

   🔄 REATIVAÇÃO (Cancelada → Ativa):
      - Enviar boas-vindas novamente
      - Restaurar acesso ao conteúdo
      - Atualizar relatórios de engajamento

⚙️ IMPLEMENTAÇÃO:
   - Evento é disparado automaticamente no setter do Status
   - Processado no SaveChangesAsync do AppDbContext
   - Timestamp automático via classe base DomainEvent
   - Informações completas para rastreabilidade

🏗️ ARQUITETURA:
   - Segue padrão Domain Events
   - Desacopla efeitos colaterais da lógica principal
   - Facilita testes unitários
   - Permite evolução sem quebrar código existente

📈 BENEFÍCIOS:
   ✅ Auditoria completa de mudanças
   ✅ Automação de processos de negócio
   ✅ Melhor experiência do usuário
   ✅ Integração simplificada
   ✅ Manutenibilidade do código

═══════════════════════════════════════════════════════════════════════════════════
*/ 