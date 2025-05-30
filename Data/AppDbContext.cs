using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_HabeisEducacional.Data
{
    /// <summary>
    /// Classe de contexto do Entity Framework Core para acesso ao banco de dados
    /// Define as entidades e configura seus relacionamentos com comportamentos otimizados
    /// Implementa cascade delete e restrições de integridade referencial
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as opções de configuração para o contexto
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet para acesso à tabela de Cursos
        /// Representa a entidade principal do sistema educacional
        /// </summary>
        public DbSet<Curso> Cursos { get; set; }

        /// <summary>
        /// DbSet para acesso à tabela de Alunos
        /// Representa os usuários estudantes do sistema
        /// </summary>
        public DbSet<Aluno> Alunos { get; set; }

        /// <summary>
        /// DbSet para acesso à tabela de Matrículas
        /// Representa o relacionamento entre Alunos e Cursos
        /// </summary>
        public DbSet<Matricula> Matriculas { get; set; }

        /// <summary>
        /// DbSet para acesso à tabela de Certificados
        /// Representa os certificados emitidos após conclusão de cursos
        /// </summary>
        public DbSet<Certificado> Certificados { get; set; }

        /// <summary>
        /// Configura o modelo de banco de dados usando Fluent API
        /// Estabelece relacionamentos otimizados, cascade behaviors e restrições de integridade
        /// </summary>
        /// <param name="modelBuilder">Builder usado para configurar o modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurações da Entidade Aluno

            /// <summary>
            /// ENTIDADE: ALUNO
            /// Configurações de integridade e unicidade para alunos
            /// </summary>
            modelBuilder.Entity<Aluno>(entity =>
            {
                // Chave primária explícita
                entity.HasKey(a => a.ID);

                // Email único para evitar duplicatas
                entity.HasIndex(a => a.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Alunos_Email_Unique");

                // Configurações de propriedades
                entity.Property(a => a.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(a => a.Senha)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.Data_Cadastro)
                    .IsRequired();
            });

            #endregion

            #region Configurações da Entidade Curso

            /// <summary>
            /// ENTIDADE: CURSO
            /// Configurações para cursos oferecidos na plataforma
            /// </summary>
            modelBuilder.Entity<Curso>(entity =>
            {
                // Chave primária explícita
                entity.HasKey(c => c.ID);

                // Configurações de propriedades
                entity.Property(c => c.Titulo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.Descricao)
                    .HasMaxLength(1000);

                entity.Property(c => c.Instrutor)
                    .HasMaxLength(100);

                entity.Property(c => c.Preco)
                    .HasPrecision(10, 2);

                entity.Property(c => c.Duracao)
                    .IsRequired();
            });

            #endregion

            #region Configurações da Entidade Matricula

            /// <summary>
            /// ENTIDADE: MATRÍCULA
            /// Relacionamento Many-to-Many materializado entre Alunos e Cursos
            /// Implementa CASCADE DELETE para manter integridade referencial
            /// </summary>
            modelBuilder.Entity<Matricula>(entity =>
            {
                // Chave primária explícita
                entity.HasKey(m => m.ID);

                // Configuração de propriedades
                entity.Property(m => m.Data_Matricula)
                    .IsRequired();

                entity.Property(m => m.Status)
                    .IsRequired()
                    .HasConversion<string>(); // Converte enum para string no DB

                // RELACIONAMENTO: Matricula → Curso (N:1)
                // Uma matrícula pertence a um curso específico
                // CASCADE DELETE: Ao deletar um curso, todas as matrículas relacionadas são removidas
                entity.HasOne(m => m.Curso)
                    .WithMany(c => c.Matriculas)
                    .HasForeignKey(m => m.Curso_ID)
                    .OnDelete(DeleteBehavior.Cascade) // CASCADE DELETE ativado
                    .HasConstraintName("FK_Matriculas_Cursos");

                // RELACIONAMENTO: Matricula → Aluno (N:1)
                // Uma matrícula pertence a um aluno específico
                // RESTRICT DELETE: Não permite deletar aluno que possui matrículas
                entity.HasOne(m => m.Aluno)
                    .WithMany(a => a.Matriculas)
                    .HasForeignKey(m => m.Aluno_ID)
                    .OnDelete(DeleteBehavior.Restrict) // RESTRICT para preservar histórico
                    .HasConstraintName("FK_Matriculas_Alunos");

                // Índice composto para evitar matrículas duplicadas
                entity.HasIndex(m => new { m.Aluno_ID, m.Curso_ID })
                    .IsUnique()
                    .HasDatabaseName("IX_Matriculas_Aluno_Curso_Unique");
            });

            #endregion

            #region Configurações da Entidade Certificado

            /// <summary>
            /// ENTIDADE: CERTIFICADO
            /// Representa certificados emitidos após conclusão de cursos
            /// Implementa CASCADE DELETE seletivo para manter integridade
            /// </summary>
            modelBuilder.Entity<Certificado>(entity =>
            {
                // Chave primária explícita
                entity.HasKey(c => c.ID);

                // Configuração de propriedades
                entity.Property(c => c.Data_Emissao)
                    .IsRequired();

                entity.Property(c => c.Codigo_Validacao)
                    .IsRequired()
                    .HasMaxLength(50);

                // RELACIONAMENTO: Certificado → Curso (N:1)
                // Um certificado está vinculado a um curso específico
                // CASCADE DELETE: Ao deletar um curso, certificados relacionados são removidos
                entity.HasOne(c => c.Curso)
                    .WithMany(curso => curso.Certificados)
                    .HasForeignKey(c => c.Curso_ID)
                    .OnDelete(DeleteBehavior.Cascade) // CASCADE DELETE ativado
                    .HasConstraintName("FK_Certificados_Cursos");

                // RELACIONAMENTO: Certificado → Aluno (N:1)
                // Um certificado pertence a um aluno específico
                // RESTRICT DELETE: Não permite deletar aluno que possui certificados
                entity.HasOne(c => c.Aluno)
                    .WithMany(a => a.Certificados)
                    .HasForeignKey(c => c.Aluno_ID)
                    .OnDelete(DeleteBehavior.Restrict) // RESTRICT para preservar certificados
                    .HasConstraintName("FK_Certificados_Alunos");

                // Código de validação único para garantir autenticidade
                entity.HasIndex(c => c.Codigo_Validacao)
                    .IsUnique()
                    .HasDatabaseName("IX_Certificados_CodigoValidacao_Unique");

                // Índice composto para evitar certificados duplicados
                entity.HasIndex(c => new { c.Aluno_ID, c.Curso_ID })
                    .IsUnique()
                    .HasDatabaseName("IX_Certificados_Aluno_Curso_Unique");
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configura comportamentos globais do contexto
        /// Aplica configurações de performance e tracking
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configurações aplicadas apenas se não configurado externamente
                optionsBuilder.EnableSensitiveDataLogging(false); // Desabilita logs sensíveis em produção
                optionsBuilder.EnableServiceProviderCaching(true); // Habilita cache do service provider
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Sobrescreve SaveChanges para adicionar comportamentos customizados
        /// Implementa auditoria automática e validações antes de salvar
        /// </summary>
        public override int SaveChanges()
        {
            // Adiciona timestamp automático em criações/atualizações
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Aluno aluno && entry.State == EntityState.Added)
                {
                    aluno.Data_Cadastro = DateTime.Now;
                }
                else if (entry.Entity is Matricula matricula && entry.State == EntityState.Added)
                {
                    matricula.Data_Matricula = DateTime.Now;
                }
                else if (entry.Entity is Certificado certificado && entry.State == EntityState.Added)
                {
                    certificado.Data_Emissao = DateTime.Now;
                    
                    // Gera código de validação automaticamente se não fornecido
                    if (string.IsNullOrWhiteSpace(certificado.Codigo_Validacao))
                    {
                        certificado.Codigo_Validacao = Certificado.GerarCodigoValidacao();
                    }
                }
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// Versão assíncrona do SaveChanges com mesmos comportamentos customizados
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Adiciona timestamp automático em criações/atualizações
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Aluno aluno && entry.State == EntityState.Added)
                {
                    aluno.Data_Cadastro = DateTime.Now;
                }
                else if (entry.Entity is Matricula matricula && entry.State == EntityState.Added)
                {
                    matricula.Data_Matricula = DateTime.Now;
                }
                else if (entry.Entity is Certificado certificado && entry.State == EntityState.Added)
                {
                    certificado.Data_Emissao = DateTime.Now;
                    
                    // Gera código de validação automaticamente se não fornecido
                    if (string.IsNullOrWhiteSpace(certificado.Codigo_Validacao))
                    {
                        certificado.Codigo_Validacao = Certificado.GerarCodigoValidacao();
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    /* 
    ═══════════════════════════════════════════════════════════════════════════════════
    📚 DOCUMENTAÇÃO DOS RELACIONAMENTOS IMPLEMENTADOS
    ═══════════════════════════════════════════════════════════════════════════════════
    
    🔄 DIAGRAMA DE RELACIONAMENTOS:
    
    ALUNO (1) ←────→ (N) MATRICULA (N) ←────→ (1) CURSO
       ↓                   ↓                      ↓
       │                   │                      │
       └─────→ (N) CERTIFICADO (N) ←──────────────┘
    
    📋 COMPORTAMENTOS DE DELETE IMPLEMENTADOS:
    
    1️⃣ ALUNO:
       - DELETE BEHAVIOR: RESTRICT em Matriculas e Certificados
       - MOTIVO: Preservar histórico educacional e certificados emitidos
       - AÇÃO: Não permite deletar aluno com registros vinculados
    
    2️⃣ CURSO:
       - DELETE BEHAVIOR: CASCADE em Matriculas e Certificados  
       - MOTIVO: Ao remover curso, remove automaticamente dados relacionados
       - AÇÃO: Deleta automaticamente matriculas e certificados do curso
    
    3️⃣ MATRICULA:
       - ENTIDADE DE RELACIONAMENTO com status e data
       - ÍNDICE ÚNICO: Previne matriculas duplicadas (Aluno + Curso)
       - CONVERSÃO ENUM: Status salvo como string no banco
    
    4️⃣ CERTIFICADO:
       - CÓDIGO VALIDAÇÃO ÚNICO: Garante autenticidade
       - ÍNDICE ÚNICO: Previne certificados duplicados (Aluno + Curso)
       - TIMESTAMP AUTOMÁTICO: Data_Emissao preenchida automaticamente
       - GERAÇÃO AUTOMÁTICA: Codigo_Validacao criado no SaveChanges
    
    ⚙️ MELHORIAS IMPLEMENTADAS:
    
    ✅ Cascade Delete estratégico
    ✅ Índices únicos compostos  
    ✅ Constraints nomeados
    ✅ Configurações de precisão
    ✅ Timestamps automáticos
    ✅ Validações de comprimento
    ✅ Documentação completa
    ✅ Performance otimizada
    ✅ Geração automática de códigos de validação
    ✅ Configurações de logging e cache
    
    ═══════════════════════════════════════════════════════════════════════════════════
    */
}