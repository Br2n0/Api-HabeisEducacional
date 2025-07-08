using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Api_HabeisEducacional.Services;

namespace Api_HabeisEducacional.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculasController : ControllerBase
    {
        // Comentado: Acesso direto ao contexto do banco de dados
        // private readonly AppDbContext _context;
        
        // Novo: Serviço de matrículas que encapsula a lógica de negócios
        private readonly IMatriculaService _matriculaService;

        // Construtor atualizado para receber o serviço via injeção de dependência
        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        // GET: api/Matriculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculas()
        {
            // Comentado: Acesso direto ao banco de dados
            /*
            return await _context.Matriculas
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
            */
            
            // Novo: Usa o serviço para obter todas as matrículas
            return Ok(await _matriculaService.GetAllAsync());
        }

        // GET: api/Matriculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MatriculaDTO>> GetMatricula(int id)
        {
            // Comentado: Acesso direto ao banco de dados
            /*
            var matricula = await _context.Matriculas
                .Include(m => m.Curso)
                .Include(m => m.Aluno)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (matricula == null)
            {
                return NotFound();
            }

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
            */
            
            // Novo: Usa o serviço para obter uma matrícula específica
            var matricula = await _matriculaService.GetByIdAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }
            
            return matricula;
        }

        // GET: api/Matriculas/aluno/5
        [HttpGet("aluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculasPorAluno(int alunoId)
        {
            // Comentado: Acesso direto ao banco de dados
            /*
            var aluno = await _context.Alunos.FindAsync(alunoId);
            if (aluno == null)
            {
                return NotFound("Aluno não encontrado.");
            }

            return await _context.Matriculas
                .Include(m => m.Curso)
                .Include(m => m.Aluno)
                .Where(m => m.Aluno_ID == alunoId)
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
            */
            
            // Novo: Usa o serviço para obter matrículas de um aluno
            try
            {
                return Ok(await _matriculaService.GetByAlunoIdAsync(alunoId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Matriculas
        [HttpPost]
        public async Task<ActionResult<MatriculaDTO>> PostMatricula(MatriculaCreateDTO matriculaDto)
        {
            // ✅ VALIDAÇÃO: Verificar ModelState antes de processar
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Comentado: Acesso direto ao banco de dados
            /*
            // Verificar se o aluno existe
            if (!await _context.Alunos.AnyAsync(a => a.ID == matriculaDto.Aluno_ID))
            {
                return BadRequest("Aluno não encontrado.");
            }

            // Verificar se o curso existe
            if (!await _context.Cursos.AnyAsync(c => c.ID == matriculaDto.Curso_ID))
            {
                return BadRequest("Curso não encontrado.");
            }

            // Verificar se o aluno já está matriculado neste curso
            var matriculaExistente = await _context.Matriculas
                .FirstOrDefaultAsync(m => 
                    m.Aluno_ID == matriculaDto.Aluno_ID && 
                    m.Curso_ID == matriculaDto.Curso_ID &&
                    m.Status != StatusMatricula.Cancelada);

            if (matriculaExistente != null)
            {
                return BadRequest("Aluno já está matriculado neste curso.");
            }

            var matricula = new Matricula
            {
                Data_Matricula = DateTime.Now,
                Curso_ID = matriculaDto.Curso_ID,
                Aluno_ID = matriculaDto.Aluno_ID,
                Status = StatusMatricula.Ativa
            };

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            // Carregar as informações relacionadas
            await _context.Entry(matricula).Reference(m => m.Curso).LoadAsync();
            await _context.Entry(matricula).Reference(m => m.Aluno).LoadAsync();

            return CreatedAtAction(nameof(GetMatricula), new { id = matricula.ID }, new MatriculaDTO
            {
                ID = matricula.ID,
                Data_Matricula = matricula.Data_Matricula,
                Curso_ID = matricula.Curso_ID,
                Aluno_ID = matricula.Aluno_ID,
                Status = matricula.Status,
                CursoTitulo = matricula.Curso?.Titulo ?? string.Empty,
                AlunoNome = matricula.Aluno?.Nome ?? string.Empty
            });
            */
            
            // Novo: Usa o serviço para criar uma nova matrícula
            try
            {
                var novaMatricula = await _matriculaService.CreateAsync(matriculaDto);
                return CreatedAtAction(nameof(GetMatricula), new { id = novaMatricula.ID }, novaMatricula);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Matriculas/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatusMatricula(int id, MatriculaUpdateDTO matriculaDto)
        {
            // ✅ VALIDAÇÃO: Verificar ModelState antes de processar
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Comentado: Acesso direto ao banco de dados
            /*
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }

            matricula.Status = matriculaDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            */
            
            // Novo: Usa o serviço para atualizar o status da matrícula
            try
            {
                await _matriculaService.UpdateStatusAsync(id, matriculaDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Matriculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatricula(int id)
        {
            // Comentado: Acesso direto ao banco de dados
            /*
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }

            // Em vez de excluir, alterar o status para Cancelada
            matricula.Status = StatusMatricula.Cancelada;
            await _context.SaveChangesAsync();

            return NoContent();
            */
            
            // Novo: Usa o serviço para cancelar a matrícula
            try
            {
                await _matriculaService.CancelarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Comentado: Método não é mais necessário, a verificação é feita no serviço
        /*
        private bool MatriculaExists(int id)
        {
            return _context.Matriculas.Any(e => e.ID == id);
        }
        */
        
        // POST: api/Matriculas/5/concluir
        [HttpPost("{id}/concluir")]
        public async Task<IActionResult> ConcluirMatricula(int id)
        {
            // Novo: Endpoint para concluir uma matrícula
            try
            {
                await _matriculaService.ConcluirAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
