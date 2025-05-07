using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
// VERSÃO ANTIGA - Imports necessários para a versão anterior
/*
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.Models;
using System.Security.Cryptography;
using System.Text;
*/
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Services;

namespace Api_HabeisEducacional.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar operações relacionadas a alunos
    /// </summary>
=======
using Microsoft.EntityFrameworkCore;
using Api_HabeisEducacional.Data;
using Api_HabeisEducacional.DTOs;
using Api_HabeisEducacional.Models;
using System.Security.Cryptography;
using System.Text;

namespace Api_HabeisEducacional.Controllers
{
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
<<<<<<< HEAD
        /*
        // VERSÃO ANTERIOR: Acesso direto ao banco de dados
        private readonly AppDbContext _context;
        */
        
        // VERSÃO NOVA: Acesso via camada de serviço
        private readonly IAlunoService _alunoService;

        // Construtor com injeção de dependência do serviço
        public AlunosController(IAlunoService alunoService)
        {
            // _context = context; // Removido na versão nova
            _alunoService = alunoService;
=======
        private readonly AppDbContext _context;

        public AlunosController(AppDbContext context)
        {
            _context = context;
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // GET: api/Alunos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunos()
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            var alunos = await _alunoService.GetAllAsync();
            return Ok(alunos);
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
            return await _context.Alunos
                .Select(a => new AlunoDTO
                {
                    ID = a.ID,
                    Nome = a.Nome,
                    Email = a.Email,
                    Data_Cadastro = a.Data_Cadastro
                })
                .ToListAsync();
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // GET: api/Alunos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoDTO>> GetAluno(int id)
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            var aluno = await _alunoService.GetByIdAsync(id);
            
            if (aluno == null)
                return NotFound();
                
            return aluno;
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // POST: api/Alunos
        [HttpPost]
        public async Task<ActionResult<AlunoDTO>> PostAluno(AlunoCreateDTO alunoDto)
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            try
            {
                var aluno = await _alunoService.CreateAsync(alunoDto);
                return CreatedAtAction(nameof(GetAluno), new { id = aluno.ID }, aluno);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // PUT: api/Alunos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluno(int id, AlunoCreateDTO alunoDto)
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            try
            {
                await _alunoService.UpdateAsync(id, alunoDto);
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // DELETE: api/Alunos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            try
            {
                await _alunoService.DeleteAsync(id);
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
<<<<<<< HEAD
            */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
        }

        // POST: api/Alunos/login
        [HttpPost("login")]
        public async Task<ActionResult<AlunoDTO>> Login(AlunoLoginDTO loginDto)
        {
<<<<<<< HEAD
            // VERSÃO NOVA: Usando o serviço
            var aluno = await _alunoService.AuthenticateAsync(loginDto);
            
            if (aluno == null)
                return Unauthorized("Email ou senha inválidos");
                
            return aluno;
            
            /* VERSÃO ANTERIOR: Acesso direto ao banco de dados
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
            */
        }

        /* Métodos auxiliares da versão anterior - comentados para evitar conflitos
=======
        }

>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
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
<<<<<<< HEAD
        */
=======
>>>>>>> fc260eac049c8e985783caa4ca0e8af4e7b74150
    }
}
