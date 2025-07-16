using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using AutoMapper; // ✅ NOVO: Import do AutoMapper

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
        
        // Atualiza dados editáveis de um certificado
        Task<CertificadoDTO> UpdateAsync(int id, CertificadoUpdateDTO dto);
        
        // Valida um certificado pelo código
        Task<bool> ValidarAsync(CertificadoValidacaoDTO dto);
    }

    /// <summary>
    /// Implementação do serviço de certificados que contém a lógica de negócios
    /// 🔄 MELHORADO: Agora usa AutoMapper para conversões automáticas
    /// </summary>
    public class CertificadoService : ICertificadoService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper; // ✅ NOVO: AutoMapper

        public CertificadoService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todos os certificados emitidos
        /// 🔄 MELHORADO: Usa AutoMapper (2 linhas vs 11 linhas)
        /// </summary>
        public async Task<IEnumerable<CertificadoDTO>> GetAllAsync()
        {
            var certificados = await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CertificadoDTO>>(certificados);
        }

        /// <summary>
        /// Obtém um certificado específico pelo ID
        /// 🔄 MELHORADO: Usa AutoMapper (1 linha vs 10 linhas)
        /// </summary>
        public async Task<CertificadoDTO?> GetByIdAsync(int id)
        {
            var certificado = await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.ID == id);
            
            return _mapper.Map<CertificadoDTO?>(certificado);
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
        /// 🔄 MELHORADO: Usa AutoMapper (1 linha vs 10 linhas)
        /// </summary>
        public async Task<CertificadoDTO?> GetByCodigoValidacaoAsync(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                throw new ArgumentException("Código de validação é obrigatório");

            var certificado = await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.Codigo_Validacao == codigo);
            
            return _mapper.Map<CertificadoDTO?>(certificado);
        }

        /// <summary>
        /// Emite um novo certificado para uma conclusão de curso
        /// Dados são preenchidos automaticamente baseados no curso da matrícula
        /// </summary>
        public async Task<CertificadoDTO> EmitirAsync(CertificadoCreateDTO dto)
        {
            // Busca a matrícula com seus relacionamentos
            var matricula = await _db.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.ID == dto.Matricula_ID);

            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada");

            // Verifica se a matrícula está concluída
            if (matricula.Status != StatusMatricula.Concluida)
                throw new InvalidOperationException("A matrícula não está concluída, não é possível emitir o certificado");

            // Verifica se já existe um certificado para este aluno neste curso
            var certificadoExistente = await _db.Certificados
                .FirstOrDefaultAsync(c => c.Aluno_ID == matricula.Aluno_ID && c.Curso_ID == matricula.Curso_ID);
            
            if (certificadoExistente != null)
                throw new InvalidOperationException("Já existe um certificado emitido para este aluno neste curso");

            // Gera código de validação único
            string codigoValidacao = GerarCodigoValidacao(matricula.Aluno_ID, matricula.Curso_ID);

            // Cria o novo certificado com dados automáticos do curso
            var certificado = new Certificado
            {
                Data_Emissao = DateTime.Now,
                Curso_ID = matricula.Curso_ID,
                Aluno_ID = matricula.Aluno_ID,
                Codigo_Validacao = codigoValidacao,
                // Dados automáticos baseados no curso:
                CargaHoraria = matricula.Curso?.Duracao,
                Documento = $"Certificado de Conclusão - {matricula.Curso?.Titulo}"
            };

            // Adiciona no banco de dados e salva as mudanças
            _db.Certificados.Add(certificado);
            await _db.SaveChangesAsync();

            // ✅ NOVO: AutoMapper converte automaticamente
            var certificadoDto = _mapper.Map<CertificadoDTO>(certificado);
            certificadoDto.CursoTitulo = matricula.Curso?.Titulo ?? string.Empty;
            certificadoDto.AlunoNome = matricula.Aluno?.Nome ?? string.Empty;
            return certificadoDto;
        }

        /// <summary>
        /// Atualiza dados editáveis de um certificado (area, nivel, documento, nota, carga horaria)
        /// </summary>
        public async Task<CertificadoDTO> UpdateAsync(int id, CertificadoUpdateDTO dto)
        {
            var certificado = await _db.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (certificado == null)
                throw new ArgumentException("Certificado não encontrado.");

            // Mapeia apenas os campos editáveis do DTO para a entidade
            _mapper.Map(dto, certificado);

            _db.Certificados.Update(certificado);
            await _db.SaveChangesAsync();

            return _mapper.Map<CertificadoDTO>(certificado);
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