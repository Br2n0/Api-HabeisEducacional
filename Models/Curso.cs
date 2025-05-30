using System.ComponentModel.DataAnnotations;

namespace Api_HabeisEducacional.Models
{
    /// <summary>
    /// Entidade que representa um curso oferecido na plataforma educacional
    /// Configurada com CASCADE DELETE para matrículas e certificados
    /// Entidade principal do sistema educacional
    /// </summary>
    public class Curso
    {
        /// <summary>
        /// Identificador único do curso
        /// Chave primária da entidade
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Título do curso
        /// Campo obrigatório com limite de 200 caracteres
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do curso
        /// Campo opcional com limite de 1000 caracteres
        /// </summary>
        [StringLength(1000)]
        public string? Descricao { get; set; }

        /// <summary>
        /// Nome do instrutor responsável pelo curso
        /// Campo opcional com limite de 100 caracteres
        /// </summary>
        [StringLength(100)]
        public string? Instrutor { get; set; }

        /// <summary>
        /// Preço do curso em decimal com precisão 10,2
        /// Configurado no AppDbContext com precisão específica
        /// </summary>
        [Required]
        [Range(0, 999999.99)]
        public decimal Preco { get; set; }

        /// <summary>
        /// Duração do curso em horas
        /// Campo obrigatório para planejamento de estudos
        /// </summary>
        [Required]
        [Range(1, 9999)]
        public int Duracao { get; set; } // em horas

        // ═══════════════════════════════════════════════════════════════
        // PROPRIEDADES DE NAVEGAÇÃO
        // Configuradas no AppDbContext com relacionamentos otimizados
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Coleção de matrículas do curso
        /// Relacionamento: Curso → Matricula (1:N)
        /// Configurado com CASCADE DELETE (ao deletar curso, remove matrículas)
        /// </summary>
        public ICollection<Matricula>? Matriculas { get; set; }

        /// <summary>
        /// Coleção de certificados do curso
        /// Relacionamento: Curso → Certificado (1:N)
        /// Configurado com CASCADE DELETE (ao deletar curso, remove certificados)
        /// </summary>
        public ICollection<Certificado>? Certificados { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // MÉTODOS UTILITÁRIOS
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Retorna as matrículas ativas do curso
        /// </summary>
        /// <returns>Coleção de matrículas com status Ativo</returns>
        public IEnumerable<Matricula> GetMatriculasAtivas()
        {
            return Matriculas?.Where(m => m.IsAtiva()) ?? Enumerable.Empty<Matricula>();
        }

        /// <summary>
        /// Retorna as matrículas concluídas do curso
        /// </summary>
        /// <returns>Coleção de matrículas com status Concluído</returns>
        public IEnumerable<Matricula> GetMatriculasConcluidas()
        {
            return Matriculas?.Where(m => m.IsConcluida()) ?? Enumerable.Empty<Matricula>();
        }

        /// <summary>
        /// Conta o total de alunos matriculados (ativos) no curso
        /// </summary>
        /// <returns>Número de alunos com matrícula ativa</returns>
        public int TotalAlunosAtivos()
        {
            return GetMatriculasAtivas().Count();
        }

        /// <summary>
        /// Conta o total de alunos que concluíram o curso
        /// </summary>
        /// <returns>Número de alunos que concluíram</returns>
        public int TotalAlunosConcluidos()
        {
            return GetMatriculasConcluidas().Count();
        }

        /// <summary>
        /// Conta o total de certificados emitidos para o curso
        /// </summary>
        /// <returns>Número de certificados emitidos</returns>
        public int TotalCertificadosEmitidos()
        {
            return Certificados?.Count() ?? 0;
        }

        /// <summary>
        /// Calcula a taxa de conclusão do curso (certificados / matrículas totais)
        /// </summary>
        /// <returns>Percentual de conclusão (0-100)</returns>
        public double TaxaConclusao()
        {
            var totalMatriculas = Matriculas?.Count() ?? 0;
            if (totalMatriculas == 0) return 0;

            var totalConcluidos = TotalAlunosConcluidos();
            return Math.Round((double)totalConcluidos / totalMatriculas * 100, 2);
        }

        /// <summary>
        /// Verifica se o curso está popular (mais de 10 alunos ativos)
        /// </summary>
        /// <returns>True se o curso for popular</returns>
        public bool IsPopular() => TotalAlunosAtivos() > 10;

        /// <summary>
        /// Verifica se o curso é gratuito
        /// </summary>
        /// <returns>True se o preço for zero</returns>
        public bool IsGratuito() => Preco == 0;

        /// <summary>
        /// Retorna a formatação do preço em moeda brasileira
        /// </summary>
        /// <returns>String formatada com o preço</returns>
        public string GetPrecoFormatado()
        {
            return IsGratuito() ? "Gratuito" : Preco.ToString("C");
        }

        /// <summary>
        /// Calcula o preço por hora do curso
        /// </summary>
        /// <returns>Valor por hora em decimal</returns>
        public decimal PrecoPorHora()
        {
            return Duracao > 0 ? Preco / Duracao : 0;
        }

        /// <summary>
        /// Verifica se um aluno específico já está matriculado no curso
        /// </summary>
        /// <param name="alunoId">ID do aluno a verificar</param>
        /// <returns>True se o aluno possui matrícula ativa</returns>
        public bool TemAlunoMatriculado(int alunoId)
        {
            return GetMatriculasAtivas().Any(m => m.Aluno_ID == alunoId);
        }

        /// <summary>
        /// Verifica se um aluno específico já concluiu o curso
        /// </summary>
        /// <param name="alunoId">ID do aluno a verificar</param>
        /// <returns>True se o aluno concluiu o curso</returns>
        public bool AlunoConcluiu(int alunoId)
        {
            return GetMatriculasConcluidas().Any(m => m.Aluno_ID == alunoId);
        }

        /// <summary>
        /// Retorna uma descrição resumida do curso
        /// </summary>
        /// <returns>String com título, duração e preço</returns>
        public string GetResumo()
        {
            return $"{Titulo} - {Duracao}h - {GetPrecoFormatado()}";
        }

        /// <summary>
        /// Categoriza o curso por duração
        /// </summary>
        /// <returns>Categoria do curso (Rápido/Médio/Longo)</returns>
        public string GetCategoriaDuracao()
        {
            return Duracao switch
            {
                <= 10 => "Curso Rápido",
                <= 40 => "Curso Médio",
                _ => "Curso Longo"
            };
        }
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DA ENTIDADE CURSO
═══════════════════════════════════════════════════════════════════════════════════

🎯 PROPÓSITO:
   - Representa os cursos oferecidos na plataforma
   - Centraliza informações acadêmicas e comerciais
   - Controla matrículas e emissão de certificados

🔗 RELACIONAMENTOS:
   - Curso → Matricula (1:N) com CASCADE DELETE
   - Curso → Certificado (1:N) com CASCADE DELETE
   
📊 CONFIGURAÇÕES NO BANCO:
   - Preco: Precisão decimal (10,2)
   - Titulo: Obrigatório, máximo 200 caracteres
   - Descricao: Opcional, máximo 1000 caracteres
   - Instrutor: Opcional, máximo 100 caracteres
   
⚙️ COMPORTAMENTOS:
   - CASCADE DELETE remove matrículas e certificados ao deletar curso
   - Validações de range para preço e duração
   - Métodos de análise e relatórios integrados

💰 MODELO DE NEGÓCIO:
   - Suporte a cursos gratuitos (preço = 0)
   - Cálculo automático de preço por hora
   - Categorização por duração

📈 ANALYTICS INTEGRADAS:
   - Taxa de conclusão automática
   - Contadores de alunos ativos/concluídos
   - Identificação de cursos populares
   - Métricas de certificação

🎓 FUNCIONALIDADES EDUCACIONAIS:
   - Rastreamento de progresso dos alunos
   - Controle de elegibilidade para certificação
   - Análise de performance do curso
   - Relatórios de engajamento

🗑️ COMPORTAMENTO DE DELETE:
   - CASCADE DELETE remove automaticamente:
     * Todas as matrículas do curso
     * Todos os certificados do curso
   - Motivo: Manter consistência ao remover cursos descontinuados

═══════════════════════════════════════════════════════════════════════════════════
*/
