using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_HabeisEducacional.Services
{
    /// <summary>
    /// Interface que define as operações disponíveis para gerenciamento de cursos
    /// </summary>
    public interface ICursoService
    {
        // Busca todos os cursos cadastrados
        Task<IEnumerable<CursoDTO>> GetAllAsync();
        
        // Busca um curso pelo ID
        Task<CursoDTO?> GetByIdAsync(int id);
        
        // Cria um novo curso
        Task<CursoDTO> CreateAsync(CursoCreateDTO dto);
        
        // Atualiza um curso existente
        Task UpdateAsync(int id, CursoCreateDTO dto);
        
        // Remove um curso do sistema
        Task DeleteAsync(int id);
    }

    /// <summary>
    /// Implementação do serviço de cursos que contém a lógica de negócios
    /// </summary>
    public class CursoService : ICursoService
    {
        // Contexto do banco de dados usado para acesso aos dados
        private readonly AppDbContext _db;

        // Construtor com injeção de dependência do contexto
        public CursoService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtém todos os cursos cadastrados
        /// </summary>
        public async Task<IEnumerable<CursoDTO>> GetAllAsync()
        {
            // Busca todos os cursos e converte para DTO
            return await _db.Cursos
                .Select(c => new CursoDTO
                {
                    ID = c.ID,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Instrutor = c.Instrutor,
                    Preco = c.Preco,
                    Duracao = c.Duracao
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um curso específico pelo ID
        /// </summary>
        public async Task<CursoDTO?> GetByIdAsync(int id)
        {
            // Busca o curso pelo ID
            var curso = await _db.Cursos.FindAsync(id);
            
            // Se não encontrar, retorna null
            if (curso == null) return null;
            
            // Converte para DTO e retorna
            return new CursoDTO
            {
                ID = curso.ID,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                Instrutor = curso.Instrutor,
                Preco = curso.Preco,
                Duracao = curso.Duracao
            };
        }

        /// <summary>
        /// Cria um novo curso
        /// </summary>
        public async Task<CursoDTO> CreateAsync(CursoCreateDTO dto)
        {
            // Cria um novo curso com os dados do DTO
            var curso = new Curso
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Instrutor = dto.Instrutor,
                Preco = dto.Preco,
                Duracao = dto.Duracao
            };

            // Adiciona no banco e salva
            _db.Cursos.Add(curso);
            await _db.SaveChangesAsync();

            // Retorna o DTO do curso criado
            return new CursoDTO
            {
                ID = curso.ID,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                Instrutor = curso.Instrutor,
                Preco = curso.Preco,
                Duracao = curso.Duracao
            };
        }

        /// <summary>
        /// Atualiza os dados de um curso existente
        /// </summary>
        public async Task UpdateAsync(int id, CursoCreateDTO dto)
        {
            // Busca o curso pelo ID
            var curso = await _db.Cursos.FindAsync(id);
            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado");

            // Atualiza os dados
            curso.Titulo = dto.Titulo;
            curso.Descricao = dto.Descricao;
            curso.Instrutor = dto.Instrutor;
            curso.Preco = dto.Preco;
            curso.Duracao = dto.Duracao;

            // Salva as alterações
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica novamente se o curso existe
                if (!await _db.Cursos.AnyAsync(c => c.ID == id))
                    throw new KeyNotFoundException("Curso não encontrado");
                else
                    throw;
            }
        }

        /// <summary>
        /// Remove um curso do sistema
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            // Busca o curso pelo ID
            var curso = await _db.Cursos.FindAsync(id);
            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado");

            // Verifica se o curso possui matrículas
            var temMatriculas = await _db.Matriculas.AnyAsync(m => m.Curso_ID == id);
            if (temMatriculas)
                throw new InvalidOperationException("Não é possível excluir um curso com alunos matriculados");

            // Remove o curso
            _db.Cursos.Remove(curso);
            await _db.SaveChangesAsync();
        }
    }
} 