using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;

namespace Api_HabeisEducacional.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoDTO>>> GetCursos()
        {
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
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetCurso(int id)
        {
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
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<CursoDTO>> PostCurso(CursoCreateDTO cursoDto)
        {
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
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, CursoCreateDTO cursoDto)
        {
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
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
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
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.ID == id);
        }
    }
}
