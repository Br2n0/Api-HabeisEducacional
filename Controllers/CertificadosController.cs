using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using System.Security.Cryptography;
using System.Text;

namespace Api_HabeisEducacional.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento e emissão de certificados
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CertificadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados via injeção de dependência
        /// </summary>
        /// <param name="context">Contexto do banco de dados</param>
        public CertificadosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os certificados cadastrados no sistema
        /// </summary>
        /// <returns>Lista de certificados com informações do curso e aluno</returns>
        // GET: api/Certificados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificadoDTO>>> GetCertificados()
        {
            // Consulta os certificados incluindo os relacionamentos de Curso e Aluno
            // para obter informações adicionais como título do curso e nome do aluno
            return await _context.Certificados
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
        /// <param name="id">ID do certificado a ser obtido</param>
        /// <returns>Certificado encontrado ou NotFound se não existir</returns>
        // GET: api/Certificados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CertificadoDTO>> GetCertificado(int id)
        {
            // Busca o certificado incluindo os relacionamentos
            var certificado = await _context.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.ID == id);

            // Retorna erro 404 se não encontrar o certificado
            if (certificado == null)
            {
                return NotFound();
            }

            // Converte o modelo para DTO e retorna
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
        /// <param name="alunoId">ID do aluno</param>
        /// <returns>Lista de certificados do aluno ou NotFound se o aluno não existir</returns>
        // GET: api/Certificados/aluno/5
        [HttpGet("aluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<CertificadoDTO>>> GetCertificadosPorAluno(int alunoId)
        {
            // Verifica se o aluno existe
            var aluno = await _context.Alunos.FindAsync(alunoId);
            if (aluno == null)
            {
                return NotFound("Aluno não encontrado.");
            }

            // Consulta os certificados do aluno
            return await _context.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .Where(c => c.Aluno_ID == alunoId)
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
        /// Emite um novo certificado para um aluno em um curso
        /// </summary>
        /// <param name="certificadoDTO">Dados para emissão do certificado</param>
        /// <returns>Certificado emitido ou BadRequest se houver problemas</returns>
        // POST: api/Certificados
        [HttpPost]
        public async Task<ActionResult<CertificadoDTO>> PostCertificado(CertificadoCreateDTO certificadoDTO)
        {
            // Verifica se o aluno existe
            var aluno = await _context.Alunos.FindAsync(certificadoDTO.Aluno_ID);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado.");
            }

            // Verifica se o curso existe
            var curso = await _context.Cursos.FindAsync(certificadoDTO.Curso_ID);
            if (curso == null)
            {
                return BadRequest("Curso não encontrado.");
            }

            // Verifica se existe uma matrícula ativa do aluno no curso
            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.Aluno_ID == certificadoDTO.Aluno_ID 
                                       && m.Curso_ID == certificadoDTO.Curso_ID);
            
            if (matricula == null)
            {
                return BadRequest("O aluno não está matriculado neste curso.");
            }

            // Verifica se a matrícula está concluída (requisito para emitir certificado)
            if (matricula.Status != StatusMatricula.Concluida)
            {
                return BadRequest("A matrícula não está concluída, não é possível emitir o certificado.");
            }

            // Verifica se já existe um certificado para esta combinação de aluno e curso
            var certificadoExistente = await _context.Certificados
                .FirstOrDefaultAsync(c => c.Aluno_ID == certificadoDTO.Aluno_ID 
                                       && c.Curso_ID == certificadoDTO.Curso_ID);
            
            if (certificadoExistente != null)
            {
                return BadRequest("Já existe um certificado emitido para este aluno neste curso.");
            }

            // Gera código de validação único
            string codigoValidacao = GerarCodigoValidacao(certificadoDTO.Aluno_ID, certificadoDTO.Curso_ID);

            // Cria o novo certificado
            var certificado = new Certificado
            {
                Data_Emissao = DateTime.Now,
                Curso_ID = certificadoDTO.Curso_ID,
                Aluno_ID = certificadoDTO.Aluno_ID,
                Codigo_Validacao = codigoValidacao
            };

            // Adiciona no banco de dados e salva as mudanças
            _context.Certificados.Add(certificado);
            await _context.SaveChangesAsync();

            // Retorna o certificado criado com todas as informações
            return await GetCertificado(certificado.ID);
        }

        /// <summary>
        /// Valida um certificado através do código de validação
        /// </summary>
        /// <param name="codigo">Código de validação do certificado</param>
        /// <returns>Certificado válido ou NotFound se o código for inválido</returns>
        // GET: api/Certificados/validar/{codigo}
        [HttpGet("validar/{codigo}")]
        public async Task<ActionResult<CertificadoDTO>> ValidarCertificado(string codigo)
        {
            // Busca o certificado pelo código de validação
            var certificado = await _context.Certificados
                .Include(c => c.Curso)
                .Include(c => c.Aluno)
                .FirstOrDefaultAsync(c => c.Codigo_Validacao == codigo);

            // Retorna erro 404 se o código for inválido
            if (certificado == null)
            {
                return NotFound("Certificado não encontrado. O código de validação é inválido.");
            }

            // Retorna o certificado com todas as informações associadas
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
        /// Método utilitário para gerar um código de validação único usando SHA256
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="cursoId">ID do curso</param>
        /// <returns>Código de validação única de 16 caracteres em hexadecimal</returns>
        private string GerarCodigoValidacao(int alunoId, int cursoId)
        {
            // Combina informações para criar uma string única
            // Usa DateTime.Now.Ticks para garantir unicidade mesmo para o mesmo aluno/curso
            string dadosParaHash = $"{alunoId}-{cursoId}-{DateTime.Now.Ticks}";
            
            // Cria o hash SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dadosParaHash));
                
                // Converte para uma string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                
                // Retorna primeiros 16 caracteres para ter um código mais curto e legível
                return builder.ToString().Substring(0, 16).ToUpper();
            }
        }
    }
}