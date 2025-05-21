using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using Api_HabeisEducacional.Services;

namespace Api_HabeisEducacional.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar operações relacionadas a cursos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        // VERSÃO ANTERIOR: Acesso direto ao banco de dados
        private readonly AppDbContext _context;
        
        // VERSÃO NOVA: Acesso via camada de serviço
        private readonly ICursoService _cursoService;

        // Construtor com injeção de dependência do serviço e contexto
        public CursosController(AppDbContext context, ICursoService cursoService)
        {
            _context = context;
            _cursoService = cursoService;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoDTO>>> GetCursos()
        {
            // VERSÃO NOVA: Usando o serviço
            var cursos = await _cursoService.GetAllAsync();
            return Ok(cursos);
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
            return await _context.Cursos
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
            */
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetCurso(int id)
        {
            // VERSÃO NOVA: Usando o serviço
            var curso = await _cursoService.GetByIdAsync(id);
            
            if (curso == null)
                return NotFound();
                
            return curso;
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return new CursoDTO
            {
                ID = curso.ID,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                Instrutor = curso.Instrutor,
                Preco = curso.Preco,
                Duracao = curso.Duracao
            };
            */
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<CursoDTO>> PostCurso(CursoCreateDTO cursoDto)
        {
            // VERSÃO NOVA: Usando o serviço
            try 
            {
                var curso = await _cursoService.CreateAsync(cursoDto);
                return CreatedAtAction(nameof(GetCurso), new { id = curso.ID }, curso);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
            var curso = new Curso
            {
                Titulo = cursoDto.Titulo,
                Descricao = cursoDto.Descricao,
                Instrutor = cursoDto.Instrutor,
                Preco = cursoDto.Preco,
                Duracao = cursoDto.Duracao
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.ID }, new CursoDTO
            {
                ID = curso.ID,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                Instrutor = curso.Instrutor,
                Preco = curso.Preco,
                Duracao = curso.Duracao
            });
            */
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, CursoCreateDTO cursoDto)
        {
            // VERSÃO NOVA: Usando o serviço
            try
            {
                await _cursoService.UpdateAsync(id, cursoDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            curso.Titulo = cursoDto.Titulo;
            curso.Descricao = cursoDto.Descricao;
            curso.Instrutor = cursoDto.Instrutor;
            curso.Preco = cursoDto.Preco;
            curso.Duracao = cursoDto.Duracao;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
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
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            // VERSÃO NOVA: Usando o serviço
            try
            {
                await _cursoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            // Verificar se existem matrículas para este curso
            var existemMatriculas = await _context.Matriculas.AnyAsync(m => m.Curso_ID == id);
            if (existemMatriculas)
            {
                return BadRequest("Não é possível excluir um curso que possui alunos matriculados.");
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
            */
        }

        // Método auxiliar para versão anterior
        /*
        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.ID == id);
        }
        */
    }
}
