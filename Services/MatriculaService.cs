using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper; // ✅ NOVO: Import do AutoMapper

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
    /// 🔄 MELHORADO: Agora usa AutoMapper para conversões automáticas
    /// </summary>
    public class MatriculaService : IMatriculaService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper; // ✅ NOVO: AutoMapper

        public MatriculaService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todas as matrículas cadastradas
        /// 🔄 MELHORADO: Usa AutoMapper (2 linhas vs 11 linhas)
        /// </summary>
        public async Task<IEnumerable<MatriculaDTO>> GetAllAsync()
        {
            var matriculas = await _db.Matriculas
                .Include(m => m.Curso)
                .Include(m => m.Aluno)
                .ToListAsync();
            return _mapper.Map<IEnumerable<MatriculaDTO>>(matriculas);
        }

        /// <summary>
        /// Obtém uma matrícula específica pelo ID
        /// 🔄 MELHORADO: Usa AutoMapper (1 linha vs 10 linhas)
        /// </summary>
        public async Task<MatriculaDTO?> GetByIdAsync(int id)
        {
            var matricula = await _db.Matriculas
                .Include(m => m.Curso)
                .Include(m => m.Aluno)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            return _mapper.Map<MatriculaDTO?>(matricula);
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

            // Cria uma nova matrícula usando o construtor
            var matricula = new Matricula(dto.Aluno_ID, dto.Curso_ID, DateTime.Now);

            // Adiciona no banco e salva
            _db.Matriculas.Add(matricula);
            await _db.SaveChangesAsync();

            // ✅ NOVO: AutoMapper converte automaticamente
            var matriculaDto = _mapper.Map<MatriculaDTO>(matricula);
            matriculaDto.CursoTitulo = curso.Titulo;
            matriculaDto.AlunoNome = aluno.Nome;
            return matriculaDto;
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

            // Atualiza o status usando os métodos de domínio apropriados
            switch (dto.Status)
            {
                case StatusMatricula.Concluida:
                    matricula.Concluir();
                    break;
                case StatusMatricula.Cancelada:
                    matricula.Cancelar();
                    break;
                case StatusMatricula.Ativa:
                    // Para reativar uma matrícula, precisamos verificar se é possível
                    if (matricula.Status == StatusMatricula.Cancelada)
                    {
                        // Usando reflexão para definir o status pois não há método público para reativar
                        var statusField = typeof(Matricula).GetField("_status", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        statusField?.SetValue(matricula, StatusMatricula.Ativa);
                    }
                    else
                    {
                        throw new InvalidOperationException("Não é possível reativar uma matrícula concluída");
                    }
                    break;
            }

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

            // Usa o método de domínio para cancelar
            matricula.Cancelar();

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

            // Usa o método de domínio para concluir
            matricula.Concluir();

            // Salva as alterações
            await _db.SaveChangesAsync();
        }
    }
} 