using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Api_HabeisEducacional.Services
{
    /// <summary>
    /// Interface que define as operações disponíveis para gerenciamento de certificados
    /// </summary>
    public interface ICertificadoService
    {
        // Busca todos os certificados
        Task<IEnumerable<CertificadoDTO>> GetAllAsync();
        
        // Busca um certificado pelo ID
        Task<CertificadoDTO?> GetByIdAsync(int id);
        
        // Busca certificados de um aluno específico
        Task<IEnumerable<CertificadoDTO>> GetByAlunoIdAsync(int alunoId);
        
        // Busca certificados de um curso específico
        Task<IEnumerable<CertificadoDTO>> GetByCursoIdAsync(int cursoId);
        
        // Busca um certificado pelo código de validação
        Task<CertificadoDTO?> GetByCodigoValidacaoAsync(string codigo);
        
        // Emite um novo certificado
        Task<CertificadoDTO> EmitirAsync(CertificadoCreateDTO dto);
        
        // Valida um certificado pelo código
        Task<bool> ValidarAsync(CertificadoValidacaoDTO dto);
    }

    /// <summary>
    /// Implementação do serviço de certificados que contém a lógica de negócios
    /// </summary>
    public class CertificadoService : ICertificadoService
    {
        // Contexto do banco de dados usado para acesso aos dados
        private readonly AppDbContext _db;

        // Construtor com injeção de dependência do contexto
        public CertificadoService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtém todos os certificados emitidos
        /// </summary>
        public async Task<IEnumerable<CertificadoDTO>> GetAllAsync()
        {
            // Busca todos os certificados com curso e aluno associados
            return await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .Select(c => new CertificadoDTO
                {
                    ID = c.ID,
                    Data_Emissao = c.Data_Emissao,
                    Curso_ID = c.Curso_ID,
                    Aluno_ID = c.Aluno_ID,
                    Codigo_Validacao = c.Codigo_Validacao,
                    CursoTitulo = c.Curso != null ? c.Curso.Titulo : string.Empty,
                    AlunoNome = c.Aluno != null ? c.Aluno.Nome : string.Empty
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um certificado específico pelo ID
        /// </summary>
        public async Task<CertificadoDTO?> GetByIdAsync(int id)
        {
            // Busca o certificado pelo ID incluindo curso e aluno
            var certificado = await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.ID == id);
            
            // Se não encontrar, retorna null
            if (certificado == null) return null;
            
            // Converte para DTO e retorna
            return new CertificadoDTO
            {
                ID = certificado.ID,
                Data_Emissao = certificado.Data_Emissao,
                Curso_ID = certificado.Curso_ID,
                Aluno_ID = certificado.Aluno_ID,
                Codigo_Validacao = certificado.Codigo_Validacao,
                CursoTitulo = certificado.Curso?.Titulo ?? string.Empty,
                AlunoNome = certificado.Aluno?.Nome ?? string.Empty
            };
        }

        /// <summary>
        /// Obtém todos os certificados de um aluno específico
        /// </summary>
        public async Task<IEnumerable<CertificadoDTO>> GetByAlunoIdAsync(int alunoId)
        {
            // Verifica se o aluno existe
            if (!await _db.Alunos.AnyAsync(a => a.ID == alunoId))
                throw new KeyNotFoundException("Aluno não encontrado");

            // Busca os certificados do aluno
            return await _db.Certificados
                .Include(c => c.Curso)
                .Where(c => c.Aluno_ID == alunoId)
                .Select(c => new CertificadoDTO
                {
                    ID = c.ID,
                    Data_Emissao = c.Data_Emissao,
                    Curso_ID = c.Curso_ID,
                    Aluno_ID = c.Aluno_ID,
                    Codigo_Validacao = c.Codigo_Validacao,
                    CursoTitulo = c.Curso != null ? c.Curso.Titulo : string.Empty,
                    AlunoNome = string.Empty // Não precisa carregar o nome do aluno, já sabemos qual é
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtém todos os certificados de um curso específico
        /// </summary>
        public async Task<IEnumerable<CertificadoDTO>> GetByCursoIdAsync(int cursoId)
        {
            // Verifica se o curso existe
            if (!await _db.Cursos.AnyAsync(c => c.ID == cursoId))
                throw new KeyNotFoundException("Curso não encontrado");

            // Busca os certificados do curso
            return await _db.Certificados
                .Include(c => c.Aluno)
                .Where(c => c.Curso_ID == cursoId)
                .Select(c => new CertificadoDTO
                {
                    ID = c.ID,
                    Data_Emissao = c.Data_Emissao,
                    Curso_ID = c.Curso_ID,
                    Aluno_ID = c.Aluno_ID,
                    Codigo_Validacao = c.Codigo_Validacao,
                    CursoTitulo = string.Empty, // Não precisa carregar o título do curso, já sabemos qual é
                    AlunoNome = c.Aluno != null ? c.Aluno.Nome : string.Empty
                })
                .ToListAsync();
        }

        /// <summary>
        /// Busca um certificado pelo código de validação
        /// </summary>
        public async Task<CertificadoDTO?> GetByCodigoValidacaoAsync(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                throw new ArgumentException("Código de validação é obrigatório");

            // Busca o certificado pelo código incluindo curso e aluno
            var certificado = await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.Codigo_Validacao == codigo);
            
            // Se não encontrar, retorna null
            if (certificado == null) return null;
            
            // Converte para DTO e retorna
            return new CertificadoDTO
            {
                ID = certificado.ID,
                Data_Emissao = certificado.Data_Emissao,
                Curso_ID = certificado.Curso_ID,
                Aluno_ID = certificado.Aluno_ID,
                Codigo_Validacao = certificado.Codigo_Validacao,
                CursoTitulo = certificado.Curso?.Titulo ?? string.Empty,
                AlunoNome = certificado.Aluno?.Nome ?? string.Empty
            };
        }

        /// <summary>
        /// Emite um novo certificado para uma conclusão de curso
        /// </summary>
        public async Task<CertificadoDTO> EmitirAsync(CertificadoCreateDTO dto)
        {
            // Verifica se o aluno existe
            var aluno = await _db.Alunos.FindAsync(dto.Aluno_ID);
            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado");

            // Verifica se o curso existe
            var curso = await _db.Cursos.FindAsync(dto.Curso_ID);
            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado");

            // Verifica se o aluno concluiu o curso (através de uma matrícula concluída)
            var matriculaConcluida = await _db.Matriculas
                .AnyAsync(m => m.Aluno_ID == dto.Aluno_ID && 
                               m.Curso_ID == dto.Curso_ID && 
                               m.Status == StatusMatricula.Concluida);
            
            if (!matriculaConcluida)
                throw new InvalidOperationException("Aluno não concluiu este curso");

            // Verifica se já existe um certificado para este aluno neste curso
            var certificadoExistente = await _db.Certificados
                .AnyAsync(c => c.Aluno_ID == dto.Aluno_ID && c.Curso_ID == dto.Curso_ID);
            
            if (certificadoExistente)
                throw new InvalidOperationException("Certificado já emitido para este aluno neste curso");

            // Gera um código de validação único
            string codigoValidacao = GerarCodigoValidacao(dto.Aluno_ID, dto.Curso_ID);

            // Cria um novo certificado
            var certificado = new Certificado
            {
                Data_Emissao = DateTime.Now,
                Curso_ID = dto.Curso_ID,
                Aluno_ID = dto.Aluno_ID,
                Codigo_Validacao = codigoValidacao
            };

            // Adiciona no banco e salva
            _db.Certificados.Add(certificado);
            await _db.SaveChangesAsync();

            // Retorna o DTO do certificado emitido
            return new CertificadoDTO
            {
                ID = certificado.ID,
                Data_Emissao = certificado.Data_Emissao,
                Curso_ID = certificado.Curso_ID,
                Aluno_ID = certificado.Aluno_ID,
                Codigo_Validacao = certificado.Codigo_Validacao,
                CursoTitulo = curso.Titulo,
                AlunoNome = aluno.Nome
            };
        }

        /// <summary>
        /// Valida um certificado pelo código
        /// </summary>
        public async Task<bool> ValidarAsync(CertificadoValidacaoDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Codigo_Validacao))
                return false;

            // Verifica se existe um certificado com este código
            return await _db.Certificados.AnyAsync(c => c.Codigo_Validacao == dto.Codigo_Validacao);
        }

        /// <summary>
        /// Método auxiliar para gerar um código de validação único para o certificado
        /// </summary>
        private string GerarCodigoValidacao(int alunoId, int cursoId)
        {
            // Combina ID do aluno, ID do curso e timestamp atual para garantir unicidade
            string inputData = $"{alunoId}-{cursoId}-{DateTime.Now.Ticks}";
            
            // Usa SHA256 para criar um hash
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            
            // Converte para string hexadecimal (16 primeiros caracteres)
            var builder = new StringBuilder();
            for (int i = 0; i < 8; i++) // Usa apenas os primeiros 8 bytes (16 caracteres)
                builder.Append(bytes[i].ToString("x2"));
                
            return builder.ToString().ToUpper(); // Retorna em maiúsculas para melhor legibilidade
        }
    }
} 