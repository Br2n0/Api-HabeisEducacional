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
    /// Interface que define as operações disponíveis para gerenciamento de alunos
    /// </summary>
    public interface IAlunoService
    {
        // Busca todos os alunos cadastrados
        Task<IEnumerable<AlunoDTO>> GetAllAsync();
        
        // Busca um aluno pelo ID
        Task<AlunoDTO?> GetByIdAsync(int id);
        
        // Cria um novo aluno
        Task<AlunoDTO> CreateAsync(AlunoCreateDTO dto);
        
        // Atualiza um aluno existente
        Task UpdateAsync(int id, AlunoCreateDTO dto);
        
        // Remove um aluno do sistema
        Task DeleteAsync(int id);
        
        // Autentica um aluno (login)
        Task<AlunoDTO?> AuthenticateAsync(AlunoLoginDTO dto);
    }

    /// <summary>
    /// Implementação do serviço de alunos que contém a lógica de negócios
    /// 🔄 MELHORADO: Agora usa AutoMapper para conversões automáticas
    /// </summary>
    public class AlunoService : IAlunoService
    {
        // Contexto do banco de dados usado para acesso aos dados
        private readonly AppDbContext _db;
        
        // ✅ NOVO: AutoMapper para conversões automáticas entre entidades e DTOs
        private readonly IMapper _mapper;

        // 🔄 MELHORADO: Construtor com injeção do AutoMapper
        /// <summary>
        /// Construtor com injeção de dependências
        /// </summary>
        /// <param name="db">Contexto do banco de dados</param>
        /// <param name="mapper">AutoMapper para conversões DTO ↔ Entity</param>
        public AlunoService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper; // Injeta o AutoMapper configurado no Program.cs
        }

        /// <summary>
        /// Obtém todos os alunos cadastrados
        /// 🔄 MELHORADO: Usa AutoMapper para conversão automática
        /// </summary>
        public async Task<IEnumerable<AlunoDTO>> GetAllAsync()
        {
            // ✅ NOVO: Busca alunos e converte com AutoMapper (1 linha vs 7 linhas)
            var alunos = await _db.Alunos.ToListAsync();
            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
            
            /* CÓDIGO ANTERIOR (mantido para estudo):
            return await _db.Alunos
                .Select(a => new AlunoDTO
                {
                    ID = a.ID,
                    Nome = a.Nome,
                    Email = a.Email,
                    Data_Cadastro = a.Data_Cadastro
                })
                .ToListAsync();
            
            BENEFÍCIOS DA MUDANÇA:
            ✅ Menos código para manter (2 linhas vs 7 linhas)
            ✅ Menos chance de erro (não precisa lembrar de todas as propriedades)
            ✅ Mais fácil de evoluir (mudanças no DTO são automáticas)
            ✅ Type-safe (verificação em compile-time)
            */
        }

        /// <summary>
        /// Obtém um aluno específico pelo ID
        /// 🔄 MELHORADO: Usa AutoMapper para conversão automática
        /// </summary>
        public async Task<AlunoDTO?> GetByIdAsync(int id)
        {
            // Busca o aluno pelo ID
            var aluno = await _db.Alunos.FindAsync(id);
            
            // ✅ NOVO: AutoMapper converte automaticamente ou retorna null
            return _mapper.Map<AlunoDTO?>(aluno);
            
            /* CÓDIGO ANTERIOR (mantido para estudo):
            // Se não encontrar, retorna null
            if (aluno == null) return null;
            
            // Converte para DTO e retorna
            return new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            };
            
            BENEFÍCIOS DA MUDANÇA:
            ✅ AutoMapper trata automaticamente valores null
            ✅ Código mais limpo (1 linha vs 8 linhas)
            ✅ Menos verificações manuais necessárias
            ✅ Consistência com outros métodos
            */
        }

        /// <summary>
        /// Cria um novo aluno
        /// </summary>
        public async Task<AlunoDTO> CreateAsync(AlunoCreateDTO dto)
        {
            // Validações adicionais
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new InvalidOperationException("O nome não pode estar vazio");
            
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new InvalidOperationException("O email não pode estar vazio");
            
            if (string.IsNullOrWhiteSpace(dto.Senha))
                throw new InvalidOperationException("A senha não pode estar vazia");
            
            if (dto.Nome.Length < 3)
                throw new InvalidOperationException("O nome deve ter pelo menos 3 caracteres");
            
            if (dto.Senha.Length < 6)
                throw new InvalidOperationException("A senha deve ter pelo menos 6 caracteres");

            // Remove espaços extras do início e fim
            dto.Nome = dto.Nome.Trim();
            dto.Email = dto.Email.Trim();

            // Verifica se já existe aluno com o mesmo email
            if (await _db.Alunos.AnyAsync(a => a.Email == dto.Email))
                throw new InvalidOperationException("Email já está em uso");

            // ✅ NOVO: AutoMapper converte DTO para entidade automaticamente
            var aluno = _mapper.Map<Aluno>(dto);
            aluno.Data_Cadastro = DateTime.Now; // Define data de cadastro

            // Adiciona no banco e salva
            _db.Alunos.Add(aluno);
            await _db.SaveChangesAsync();

            // ✅ NOVO: AutoMapper converte entidade para DTO automaticamente
            return _mapper.Map<AlunoDTO>(aluno);
            
            /* CÓDIGO ANTERIOR (mantido para estudo):
            // Cria um novo aluno com os dados do DTO
            var aluno = new Aluno
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                Data_Cadastro = DateTime.Now
            };
            
            // Retorna o DTO do aluno criado
            return new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            };
            
            BENEFÍCIOS DA MUDANÇA:
            ✅ Redução significativa de código (3 linhas vs 14 linhas)
            ✅ Menos chance de erro (propriedades mapeadas automaticamente)
            ✅ Configuração centralizada no AutoMapperProfile
            ✅ Mais fácil de manter e evoluir
            */
        }

        /// <summary>
        /// Atualiza os dados de um aluno existente
        /// </summary>
        public async Task UpdateAsync(int id, AlunoCreateDTO dto)
        {
            // Validações adicionais
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new InvalidOperationException("O nome não pode estar vazio");
            
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new InvalidOperationException("O email não pode estar vazio");
            
            if (dto.Nome.Length < 3)
                throw new InvalidOperationException("O nome deve ter pelo menos 3 caracteres");

            // Remove espaços extras do início e fim
            dto.Nome = dto.Nome.Trim();
            dto.Email = dto.Email.Trim();

            // Busca o aluno pelo ID
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado");

            // Verifica se o email informado já está em uso por outro aluno
            if (await _db.Alunos.AnyAsync(a => a.Email == dto.Email && a.ID != id))
                throw new InvalidOperationException("Email já está em uso por outro aluno");

            // ✅ NOVO: AutoMapper atualiza propriedades automaticamente
            _mapper.Map(dto, aluno);
            
            // Só atualiza a senha se uma nova senha foi fornecida
            if (!string.IsNullOrEmpty(dto.Senha))
            {
                if (dto.Senha.Length < 6)
                    throw new InvalidOperationException("A senha deve ter pelo menos 6 caracteres");
                    
                aluno.Senha = dto.Senha;
            }
            
            /* CÓDIGO ANTERIOR (mantido para estudo):
            // Atualiza os dados
            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;
            
            BENEFÍCIOS DA MUDANÇA:
            ✅ Método Map(source, destination) atualiza objeto existente
            ✅ Todas as propriedades são atualizadas automaticamente
            ✅ Menos linhas de código para manter
            ✅ Consistência com padrão de mapeamento da aplicação
            */

            // Salva as alterações
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um aluno do sistema
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            // Busca o aluno pelo ID
            var aluno = await _db.Alunos.FindAsync(id);
            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado");

            // Verifica se o aluno possui matrículas
            var temMatriculas = await _db.Matriculas.AnyAsync(m => m.Aluno_ID == id);
            if (temMatriculas)
                throw new InvalidOperationException("Não é possível excluir aluno com matrículas");

            // Remove o aluno
            _db.Alunos.Remove(aluno);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Realiza a autenticação do aluno (login)
        /// 🔄 MELHORADO: Usa AutoMapper para conversão automática
        /// </summary>
        public async Task<AlunoDTO?> AuthenticateAsync(AlunoLoginDTO dto)
        {
            // Busca o aluno pelo email
            var aluno = await _db.Alunos
                .FirstOrDefaultAsync(a => a.Email == dto.Email);

            // Verifica se encontrou o aluno e se a senha está correta
            if (aluno == null || aluno.Senha != dto.Senha)
                return null;

            // ✅ NOVO: AutoMapper converte automaticamente
            return _mapper.Map<AlunoDTO>(aluno);
            
            /* CÓDIGO ANTERIOR (mantido para estudo):
            // Retorna os dados do aluno autenticado
            return new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            };
            
            BENEFÍCIOS DA MUDANÇA:
            ✅ Consistência com outros métodos da classe
            ✅ Redução de código repetitivo
            ✅ Facilita futuras mudanças no AlunoDTO
            ✅ Padrão uniforme de conversões na aplicação
            */
        }
    }
} 