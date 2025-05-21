using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_HabeisEducacional.Services
{
    /// <summary>
    /// Interface que define as operações disponíveis para gerenciamento de matrículas
    /// </summary>
    public interface IMatriculaService
    {
        // Busca todas as matrículas
        Task<IEnumerable<MatriculaDTO>> GetAllAsync();
        
        // Busca uma matrícula pelo ID
        Task<MatriculaDTO?> GetByIdAsync(int id);
        
        // Busca matrículas de um aluno específico
        Task<IEnumerable<MatriculaDTO>> GetByAlunoIdAsync(int alunoId);
        
        // Busca matrículas de um curso específico
        Task<IEnumerable<MatriculaDTO>> GetByCursoIdAsync(int cursoId);
        
        // Cria uma nova matrícula
        Task<MatriculaDTO> CreateAsync(MatriculaCreateDTO dto);
        
        // Atualiza o status de uma matrícula
        Task UpdateStatusAsync(int id, MatriculaUpdateDTO dto);
        
        // Cancela uma matrícula
        Task CancelarAsync(int id);
        
        // Conclui uma matrícula
        Task ConcluirAsync(int id);
    }

    /// <summary>
    /// Implementação do serviço de matrículas que contém a lógica de negócios
    /// </summary>
    public class MatriculaService : IMatriculaService
    {
        // Contexto do banco de dados usado para acesso aos dados
        private readonly AppDbContext _db;

        // Construtor com injeção de dependência do contexto
        public MatriculaService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtém todas as matrículas cadastradas
        /// </summary>
        public async Task<IEnumerable<MatriculaDTO>> GetAllAsync()
        {
            // Busca todas as matrículas com curso e aluno associados
            return await _db.Matriculas
                .Include(m => m.Curso)
                .Include(m => m.Aluno)
                .Select(m => new MatriculaDTO
                {
                    ID = m.ID,
                    Data_Matricula = m.Data_Matricula,
                    Curso_ID = m.Curso_ID,
                    Aluno_ID = m.Aluno_ID,
                    Status = m.Status,
                    CursoTitulo = m.Curso != null ? m.Curso.Titulo : string.Empty,
                    AlunoNome = m.Aluno != null ? m.Aluno.Nome : string.Empty
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtém uma matrícula específica pelo ID
        /// </summary>
        public async Task<MatriculaDTO?> GetByIdAsync(int id)
        {
            // Busca a matrícula pelo ID incluindo curso e aluno
            var matricula = await _db.Matriculas
                .Include(m => m.Curso)
                .Include(m => m.Aluno)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            // Se não encontrar, retorna null
            if (matricula == null) return null;
            
            // Converte para DTO e retorna
            return new MatriculaDTO
            {
                ID = matricula.ID,
                Data_Matricula = matricula.Data_Matricula,
                Curso_ID = matricula.Curso_ID,
                Aluno_ID = matricula.Aluno_ID,
                Status = matricula.Status,
                CursoTitulo = matricula.Curso?.Titulo ?? string.Empty,
                AlunoNome = matricula.Aluno?.Nome ?? string.Empty
            };
        }

        /// <summary>
        /// Obtém todas as matrículas de um aluno específico
        /// </summary>
        public async Task<IEnumerable<MatriculaDTO>> GetByAlunoIdAsync(int alunoId)
        {
            // Verifica se o aluno existe
            if (!await _db.Alunos.AnyAsync(a => a.ID == alunoId))
                throw new KeyNotFoundException("Aluno não encontrado");

            // Busca as matrículas do aluno
            return await _db.Matriculas
                .Include(m => m.Curso)
                .Where(m => m.Aluno_ID == alunoId)
                .Select(m => new MatriculaDTO
                {
                    ID = m.ID,
                    Data_Matricula = m.Data_Matricula,
                    Curso_ID = m.Curso_ID,
                    Aluno_ID = m.Aluno_ID,
                    Status = m.Status,
                    CursoTitulo = m.Curso != null ? m.Curso.Titulo : string.Empty,
                    AlunoNome = string.Empty // Não precisa carregar o nome do aluno, já sabemos qual é
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtém todas as matrículas de um curso específico
        /// </summary>
        public async Task<IEnumerable<MatriculaDTO>> GetByCursoIdAsync(int cursoId)
        {
            // Verifica se o curso existe
            if (!await _db.Cursos.AnyAsync(c => c.ID == cursoId))
                throw new KeyNotFoundException("Curso não encontrado");

            // Busca as matrículas do curso
            return await _db.Matriculas
                .Include(m => m.Aluno)
                .Where(m => m.Curso_ID == cursoId)
                .Select(m => new MatriculaDTO
                {
                    ID = m.ID,
                    Data_Matricula = m.Data_Matricula,
                    Curso_ID = m.Curso_ID,
                    Aluno_ID = m.Aluno_ID,
                    Status = m.Status,
                    CursoTitulo = string.Empty, // Não precisa carregar o título do curso, já sabemos qual é
                    AlunoNome = m.Aluno != null ? m.Aluno.Nome : string.Empty
                })
                .ToListAsync();
        }

        /// <summary>
        /// Cria uma nova matrícula
        /// </summary>
        public async Task<MatriculaDTO> CreateAsync(MatriculaCreateDTO dto)
        {
            // Verifica se o aluno existe
            var aluno = await _db.Alunos.FindAsync(dto.Aluno_ID);
            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado");

            // Verifica se o curso existe
            var curso = await _db.Cursos.FindAsync(dto.Curso_ID);
            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado");

            // Verifica se já existe uma matrícula ativa para este aluno neste curso
            var matriculaExistente = await _db.Matriculas
                .FirstOrDefaultAsync(m => m.Aluno_ID == dto.Aluno_ID && 
                                          m.Curso_ID == dto.Curso_ID && 
                                          m.Status == StatusMatricula.Ativa);
                                          
            if (matriculaExistente != null)
                throw new InvalidOperationException("Aluno já está matriculado neste curso");

            // Cria uma nova matrícula
            var matricula = new Matricula
            {
                Data_Matricula = DateTime.Now,
                Curso_ID = dto.Curso_ID,
                Aluno_ID = dto.Aluno_ID,
                Status = StatusMatricula.Ativa
            };

            // Adiciona no banco e salva
            _db.Matriculas.Add(matricula);
            await _db.SaveChangesAsync();

            // Retorna o DTO da matrícula criada
            return new MatriculaDTO
            {
                ID = matricula.ID,
                Data_Matricula = matricula.Data_Matricula,
                Curso_ID = matricula.Curso_ID,
                Aluno_ID = matricula.Aluno_ID,
                Status = matricula.Status,
                CursoTitulo = curso.Titulo,
                AlunoNome = aluno.Nome
            };
        }

        /// <summary>
        /// Atualiza o status de uma matrícula
        /// </summary>
        public async Task UpdateStatusAsync(int id, MatriculaUpdateDTO dto)
        {
            // Busca a matrícula pelo ID
            var matricula = await _db.Matriculas.FindAsync(id);
            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada");

            // Atualiza o status
            matricula.Status = dto.Status;

            // Salva as alterações
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Cancela uma matrícula
        /// </summary>
        public async Task CancelarAsync(int id)
        {
            // Busca a matrícula pelo ID
            var matricula = await _db.Matriculas.FindAsync(id);
            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada");

            // Verifica se a matrícula já está cancelada
            if (matricula.Status == StatusMatricula.Cancelada)
                throw new InvalidOperationException("Matrícula já está cancelada");

            // Atualiza o status para cancelada
            matricula.Status = StatusMatricula.Cancelada;

            // Salva as alterações
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Conclui uma matrícula
        /// </summary>
        public async Task ConcluirAsync(int id)
        {
            // Busca a matrícula pelo ID
            var matricula = await _db.Matriculas.FindAsync(id);
            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada");

            // Verifica se a matrícula está ativa
            if (matricula.Status != StatusMatricula.Ativa)
                throw new InvalidOperationException("Apenas matrículas ativas podem ser concluídas");

            // Atualiza o status para concluída
            matricula.Status = StatusMatricula.Concluida;

            // Salva as alterações
            await _db.SaveChangesAsync();
        }
    }
} 