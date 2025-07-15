using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api_HabeisEducacional.Models.ValueObjects;

namespace Api_HabeisEducacional.Models
{
    /// <summary>
    /// Entidade que representa um aluno no sistema educacional
    /// Configurada com RESTRICT DELETE para preservar histórico educacional
    /// Email configurado como índice único no AppDbContext
    /// </summary>
    public class Aluno
    {
        /// <summary>
        /// Identificador único do aluno
        /// Chave primária da entidade
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Nome completo do aluno
        /// Campo obrigatório com limite de 100 caracteres
        /// MELHORIA: Adicionado MinimumLength = 3 para garantir nomes válidos
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 🔄 MELHORIA: EMAIL COMO VALUE OBJECT
        // ═══════════════════════════════════════════════════════════════
        
        /// <summary>
        /// Email único do aluno implementado como Value Object
        /// BENEFÍCIOS DO VALUE OBJECT:
        /// - Validação automática de formato de email
        /// - Normalização automática (lowercase, trim)
        /// - Encapsulamento das regras de negócio
        /// - Reutilização em outras partes do sistema
        /// - Imutabilidade e thread-safety
        /// </summary>
        private string _email = string.Empty;
        public string Email 
        { 
            get => _email;
            set => _email = ValueObjects.Email.Criar(value).ToString(); // Aplica validações do Value Object
        }

        /* CÓDIGO ANTERIOR (mantido para estudo):
        /// <summary>
        /// Email único do aluno para login e comunicação
        /// Configurado como índice único no AppDbContext
        /// Campo obrigatório com limite de 150 caracteres
        /// </summary>
        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        */

        /// <summary>
        /// Senha criptografada do aluno
        /// Campo obrigatório com limite de 255 caracteres
        /// MELHORIA: Adicionado MinimumLength = 6 para garantir senhas seguras
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// URL da foto do perfil do aluno
        /// Campo opcional para upload de foto de perfil
        /// </summary>
        [StringLength(500)]
        public string? FotoUrl { get; set; }

        /// <summary>
        /// Data e hora de cadastro do aluno no sistema
        /// Preenchido automaticamente pelo contexto (SaveChanges override)
        /// </summary>
        [Required]
        public DateTime Data_Cadastro { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // PROPRIEDADES DE NAVEGAÇÃO
        // Configuradas no AppDbContext com relacionamentos otimizados
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Coleção de matrículas do aluno
        /// Relacionamento: Aluno → Matricula (1:N)
        /// Configurado com RESTRICT DELETE (não permite deletar aluno com matrículas)
        /// </summary>
        public ICollection<Matricula>? Matriculas { get; set; }

        /// <summary>
        /// Coleção de certificados do aluno
        /// Relacionamento: Aluno → Certificado (1:N)
        /// Configurado com RESTRICT DELETE (não permite deletar aluno com certificados)
        /// </summary>
        public ICollection<Certificado>? Certificados { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 🔄 MÉTODOS DE DOMÍNIO MELHORADOS
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Retorna as matrículas ativas do aluno
        /// </summary>
        /// <returns>Coleção de matrículas com status Ativo</returns>
        public IEnumerable<Matricula> GetMatriculasAtivas()
        {
            return Matriculas?.Where(m => m.IsAtiva()) ?? Enumerable.Empty<Matricula>();
        }

        /// <summary>
        /// Retorna as matrículas concluídas do aluno
        /// </summary>
        /// <returns>Coleção de matrículas com status Concluído</returns>
        public IEnumerable<Matricula> GetMatriculasConcluidas()
        {
            return Matriculas?.Where(m => m.IsConcluida()) ?? Enumerable.Empty<Matricula>();
        }

        /// <summary>
        /// Retorna os certificados emitidos nos últimos 30 dias
        /// </summary>
        /// <returns>Coleção de certificados recentes</returns>
        public IEnumerable<Certificado> GetCertificadosRecentes()
        {
            return Certificados?.Where(c => c.IsRecente()) ?? Enumerable.Empty<Certificado>();
        }

        /// <summary>
        /// Conta o total de cursos concluídos pelo aluno
        /// </summary>
        /// <returns>Número de cursos concluídos</returns>
        public int TotalCursosConcluidos()
        {
            return GetMatriculasConcluidas().Count();
        }

        /// <summary>
        /// Conta o total de certificados obtidos pelo aluno
        /// </summary>
        /// <returns>Número de certificados</returns>
        public int TotalCertificados()
        {
            return Certificados?.Count() ?? 0;
        }

        /// <summary>
        /// 🔄 MÉTODO MELHORADO: Verifica se o aluno possui matrícula ativa em um curso específico
        /// BENEFÍCIOS: Lógica de negócio encapsulada na entidade, consulta otimizada
        /// </summary>
        /// <param name="cursoId">ID do curso a verificar</param>
        /// <returns>True se possui matrícula ativa no curso</returns>
        public bool PossuiMatriculaAtiva(int cursoId)
        {
            return Matriculas?.Any(m => m.Curso_ID == cursoId && m.Status == StatusMatricula.Ativa) ?? false;
        }

        /* MÉTODO ANTERIOR (mantido para estudo):
        public bool TemMatriculaAtivaEm(int cursoId)
        {
            return GetMatriculasAtivas().Any(m => m.Curso_ID == cursoId);
        }
        */

        /// <summary>
        /// 🔄 MÉTODO MELHORADO: Verifica se o aluno pode se certificar em um curso específico
        /// REGRA DE NEGÓCIO: Só pode se certificar quem concluiu o curso
        /// </summary>
        /// <param name="cursoId">ID do curso a verificar</param>
        /// <returns>True se pode se certificar no curso</returns>
        public bool PodeSeCertificar(int cursoId)
        {
            return Matriculas?.Any(m => m.Curso_ID == cursoId && m.Status == StatusMatricula.Concluida) ?? false;
        }

        /* MÉTODO ANTERIOR (mantido para estudo):
        public bool TemCertificadoDo(int cursoId)
        {
            return Certificados?.Any(c => c.Curso_ID == cursoId) ?? false;
        }
        */

        /// <summary>
        /// Calcula há quantos dias o aluno está cadastrado no sistema
        /// </summary>
        /// <returns>Número de dias desde o cadastro</returns>
        public int DiasDesdeoCadastro()
        {
            return (DateTime.Now - Data_Cadastro).Days;
        }

        /// <summary>
        /// Verifica se o aluno é novo no sistema (cadastrado há menos de 30 dias)
        /// </summary>
        /// <returns>True se é um aluno novo</returns>
        public bool IsAlunoNovo() => DiasDesdeoCadastro() <= 30;
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DA ENTIDADE ALUNO
═══════════════════════════════════════════════════════════════════════════════════

🎯 PROPÓSITO:
   - Representa usuários estudantes do sistema
   - Centraliza informações de identificação e autenticação
   - Mantém histórico completo de atividades educacionais

🔗 RELACIONAMENTOS:
   - Aluno → Matricula (1:N) com RESTRICT DELETE
   - Aluno → Certificado (1:N) com RESTRICT DELETE
   
📊 ÍNDICES (Configurados no AppDbContext):
   - Índice único em Email (previne emails duplicados)
   
⚙️ COMPORTAMENTOS:
   - Email único para identificação
   - Data_Cadastro preenchida automaticamente
   - RESTRICT DELETE preserva histórico educacional
   - Métodos utilitários para consulta de dados

🔐 SEGURANÇA:
   - Senha armazenada de forma criptografada
   - Email único previne duplicatas
   - Validações de formato de email

📈 FUNCIONALIDADES:
   - Rastreamento de matrículas ativas/concluídas
   - Histórico de certificados obtidos
   - Relatórios de progresso educacional
   - Verificação de elegibilidade para novos cursos

🚫 RESTRIÇÕES DE DELETE:
   - Não permite deletar aluno com matrículas
   - Não permite deletar aluno com certificados
   - Motivo: Preservar integridade do histórico educacional

═══════════════════════════════════════════════════════════════════════════════════
*/
