using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using System.Security.Cryptography;
using System.Text;

namespace Api_HabeisEducacional.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlunosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Alunos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunos()
        {
            return await _context.Alunos
                .Select(a => new AlunoDTO
                {
                    ID = a.ID,
                    Nome = a.Nome,
                    Email = a.Email,
                    Data_Cadastro = a.Data_Cadastro
                })
                .ToListAsync();
        }

        // GET: api/Alunos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoDTO>> GetAluno(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            };
        }

        // POST: api/Alunos
        [HttpPost]
        public async Task<ActionResult<AlunoDTO>> PostAluno(AlunoCreateDTO alunoDto)
        {
            // Verificar se o email já está em uso
            if (await _context.Alunos.AnyAsync(a => a.Email == alunoDto.Email))
            {
                return BadRequest("Email já está em uso.");
            }

            var aluno = new Aluno
            {
                Nome = alunoDto.Nome,
                Email = alunoDto.Email,
                Senha = HashSenha(alunoDto.Senha),
                Data_Cadastro = DateTime.Now
            };

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAluno), new { id = aluno.ID }, new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            });
        }

        // PUT: api/Alunos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluno(int id, AlunoCreateDTO alunoDto)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            // Verificar se o email já está em uso por outro aluno
            var emailExistente = await _context.Alunos
                .FirstOrDefaultAsync(a => a.Email == alunoDto.Email && a.ID != id);
            if (emailExistente != null)
            {
                return BadRequest("Email já está em uso por outro aluno.");
            }

            aluno.Nome = alunoDto.Nome;
            aluno.Email = alunoDto.Email;
            
            // Atualizar senha apenas se uma nova senha for fornecida
            if (!string.IsNullOrEmpty(alunoDto.Senha))
            {
                aluno.Senha = HashSenha(alunoDto.Senha);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlunoExists(id))
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

        // DELETE: api/Alunos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            // Verificar se existem matrículas para este aluno
            var existemMatriculas = await _context.Matriculas.AnyAsync(m => m.Aluno_ID == id);
            if (existemMatriculas)
            {
                return BadRequest("Não é possível excluir um aluno que possui matrículas.");
            }

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Alunos/login
        [HttpPost("login")]
        public async Task<ActionResult<AlunoDTO>> Login(AlunoLoginDTO loginDto)
        {
            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(a => a.Email == loginDto.Email);

            if (aluno == null || aluno.Senha != HashSenha(loginDto.Senha))
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            return new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            };
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.ID == id);
        }

        private string HashSenha(string senha)
        {
            // Em produção, use um algoritmo de hash mais seguro com salt
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
