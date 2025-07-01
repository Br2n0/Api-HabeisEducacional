using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper; // ✅ NOVO: Import do AutoMapper

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
    /// 🔄 MELHORADO: Agora usa AutoMapper para conversões automáticas
    /// </summary>
    public class CursoService : ICursoService
    {
        // Contexto do banco de dados usado para acesso aos dados
        private readonly AppDbContext _db;
        
        // ✅ NOVO: AutoMapper para conversões automáticas entre entidades e DTOs
        private readonly IMapper _mapper;

        // 🔄 MELHORADO: Construtor com injeção do AutoMapper
        public CursoService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todos os cursos cadastrados
        /// 🔄 MELHORADO: Usa AutoMapper para conversão automática
        /// </summary>
        public async Task<IEnumerable<CursoDTO>> GetAllAsync()
        {
            // ✅ NOVO: AutoMapper converte automaticamente (2 linhas vs 9 linhas)
            var cursos = await _db.Cursos.ToListAsync();
            return _mapper.Map<IEnumerable<CursoDTO>>(cursos);
        }

        /// <summary>
        /// Obtém um curso específico pelo ID
        /// 🔄 MELHORADO: Usa AutoMapper para conversão automática
        /// </summary>
        public async Task<CursoDTO?> GetByIdAsync(int id)
        {
            var curso = await _db.Cursos.FindAsync(id);
            // ✅ NOVO: AutoMapper trata null automaticamente
            return _mapper.Map<CursoDTO?>(curso);
        }

        /// <summary>
        /// Cria um novo curso
        /// </summary>
        public async Task<CursoDTO> CreateAsync(CursoCreateDTO dto)
        {
            // Validações adicionais
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new InvalidOperationException("O título do curso não pode estar vazio");
            
            if (dto.Titulo.Length < 3)
                throw new InvalidOperationException("O título deve ter pelo menos 3 caracteres");
            
            if (dto.Preco < 0)
                throw new InvalidOperationException("O preço não pode ser negativo");
            
            if (dto.Duracao <= 0)
                throw new InvalidOperationException("A duração deve ser maior que zero");

            // Remove espaços extras do início e fim
            dto.Titulo = dto.Titulo.Trim();
            if (dto.Descricao != null) dto.Descricao = dto.Descricao.Trim();
            if (dto.Instrutor != null) dto.Instrutor = dto.Instrutor.Trim();

            // Verifica se já existe curso com o mesmo título
            if (await _db.Cursos.AnyAsync(c => c.Titulo == dto.Titulo))
                throw new InvalidOperationException("Já existe um curso com este título");

            // ✅ NOVO: AutoMapper converte DTO para entidade automaticamente
            var curso = _mapper.Map<Curso>(dto);

            _db.Cursos.Add(curso);
            await _db.SaveChangesAsync();

            // ✅ NOVO: AutoMapper converte entidade para DTO automaticamente
            return _mapper.Map<CursoDTO>(curso);
        }

        /// <summary>
        /// Atualiza os dados de um curso existente
        /// </summary>
        public async Task UpdateAsync(int id, CursoCreateDTO dto)
        {
            // Validações adicionais
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new InvalidOperationException("O título do curso não pode estar vazio");
            
            if (dto.Titulo.Length < 3)
                throw new InvalidOperationException("O título deve ter pelo menos 3 caracteres");
            
            if (dto.Preco < 0)
                throw new InvalidOperationException("O preço não pode ser negativo");
            
            if (dto.Duracao <= 0)
                throw new InvalidOperationException("A duração deve ser maior que zero");

            // Remove espaços extras do início e fim
            dto.Titulo = dto.Titulo.Trim();
            if (dto.Descricao != null) dto.Descricao = dto.Descricao.Trim();
            if (dto.Instrutor != null) dto.Instrutor = dto.Instrutor.Trim();

            // Busca o curso pelo ID
            var curso = await _db.Cursos.FindAsync(id);
            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado");

            // Verifica se o título já está em uso por outro curso
            if (await _db.Cursos.AnyAsync(c => c.Titulo == dto.Titulo && c.ID != id))
                throw new InvalidOperationException("Já existe outro curso com este título");

            // ✅ NOVO: AutoMapper atualiza propriedades automaticamente
            _mapper.Map(dto, curso);

            // Salva as alterações
            await _db.SaveChangesAsync();
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