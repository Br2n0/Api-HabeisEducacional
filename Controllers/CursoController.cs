using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
<<<<<<< HEAD
using Api_HabeisEducacional.Services;

namespace Api_HabeisEducacional.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar operações relacionadas a cursos
    /// </summary>
=======

namespace Api_HabeisEducacional.Controllers
{
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
<<<<<<< HEAD
        // VERSÃO ANTERIOR: Acesso direto ao banco de dados
        private readonly AppDbContext _context;
        
        // VERSÃO NOVA: Acesso via camada de serviço
        private readonly ICursoService _cursoService;

        // Construtor com injeção de dependência do serviço e contexto
        public CursosController(AppDbContext context, ICursoService cursoService)
        {
            _context = context;
            _cursoService = cursoService;
=======
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoDTO>>> GetCursos()
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            var cursos = await _cursoService.GetAllAsync();
            return Ok(cursos);
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetCurso(int id)
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            var curso = await _cursoService.GetByIdAsync(id);
            
            if (curso == null)
                return NotFound();
                
            return curso;
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<CursoDTO>> PostCurso(CursoCreateDTO cursoDto)
        {
<<<<<<< HEAD
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
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, CursoCreateDTO cursoDto)
        {
<<<<<<< HEAD
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
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
<<<<<<< HEAD
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
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
        }

        /* Método auxiliar da versão anterior - comentado para evitar conflitos
=======
        }

>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.ID == id);
        }
<<<<<<< HEAD
        */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
    }
}
