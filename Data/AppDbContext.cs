using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_HabeisEducacional.Data
{
    /// <summary>
    /// Classe de contexto do Entity Framework Core para acesso ao banco de dados
    /// Define as entidades e configura seus relacionamentos
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
        /// </summary>
        public DbSet<Curso> Cursos { get; set; }

        /// <summary>
        /// DbSet para acesso à tabela de Alunos
        /// </summary>
        public DbSet<Aluno> Alunos { get; set; }

        /// <summary>
        /// DbSet para acesso à tabela de Matrículas
        /// </summary>
        public DbSet<Matricula> Matriculas { get; set; }

        /// <summary>
        /// DbSet para acesso à tabela de Certificados
        /// </summary>
        public DbSet<Certificado> Certificados { get; set; }

        /// <summary>
        /// Configura o modelo de banco de dados usando Fluent API
        /// Estabelece relacionamentos entre entidades e restrições de unicidade
        /// </summary>
        /// <param name="modelBuilder">Builder usado para configurar o modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração de Aluno
            // Define que o email do aluno deve ser único no banco de dados
            modelBuilder.Entity<Aluno>()
                .HasIndex(a => a.Email)
                .IsUnique();

            // Configuração de Matricula
            // Relacionamento: Muitas matrículas podem pertencer a um curso (N:1)
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Matriculas)
                .HasForeignKey(m => m.Curso_ID);

            // Relacionamento: Muitas matrículas podem pertencer a um aluno (N:1)
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Aluno)
                .WithMany(a => a.Matriculas)
                .HasForeignKey(m => m.Aluno_ID);

            // Configuração de Certificado
            // Relacionamento: Muitos certificados podem pertencer a um curso (N:1)
            modelBuilder.Entity<Certificado>()
                .HasOne(c => c.Curso)
                .WithMany(c => c.Certificados)
                .HasForeignKey(c => c.Curso_ID);

            // Relacionamento: Muitos certificados podem pertencer a um aluno (N:1)
            modelBuilder.Entity<Certificado>()
                .HasOne(c => c.Aluno)
                .WithMany(a => a.Certificados)
                .HasForeignKey(c => c.Aluno_ID);

            // Define que o código de validação do certificado deve ser único
            // para garantir a integridade do sistema de validação
            modelBuilder.Entity<Certificado>()
                .HasIndex(c => c.Codigo_Validacao)
                .IsUnique();
        }
    }
}