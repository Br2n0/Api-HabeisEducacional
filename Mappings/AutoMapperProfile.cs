using AutoMapper;
using Api_HabeisEducacional.Models;
using Api_HabeisEducacional.DTOs;

namespace Api_HabeisEducacional.Mappings
{
    /// <summary>
    /// Profile principal do AutoMapper para mapeamentos entre Entidades e DTOs
    /// 
    /// BENEFÍCIOS DO AUTOMAPPER:
    /// ✅ Reduz código boilerplate (menos linhas)
    /// ✅ Mapeamento automático por convenção de nomes
    /// ✅ Configuração centralizada e reutilizável
    /// ✅ Type-safe mapping (verificação em compile-time)
    /// ✅ Performance otimizada com caching interno
    /// ✅ Facilita manutenção e evolução dos DTOs
    /// 
    /// BOAS PRÁTICAS APLICADAS:
    /// 🔧 Configuração explícita para propriedades calculadas
    /// 🔧 Mapeamento reverso automático com ReverseMap()
    /// 🔧 Configuração de ignore para propriedades de navegação
    /// 🔧 Mapeamento condicional baseado em nulls
    /// 🔧 Profile separado para melhor organização
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ConfigurarMapeamentosAluno();
            ConfigurarMapeamentosCurso();
            ConfigurarMapeamentosMatricula();
            ConfigurarMapeamentosCertificado();
        }

        /// <summary>
        /// Configura mapeamentos para a entidade Aluno
        /// 
        /// MELHORIAS IMPLEMENTADAS:
        /// - Mapeamento automático Aluno ↔ AlunoDTO
        /// - AlunoCreateDTO → Aluno (ignore propriedades auto-geradas)
        /// - Configuração para ignorar coleções de navegação em DTOs
        /// </summary>
        private void ConfigurarMapeamentosAluno()
        {
            // Mapeamento bidirecional Aluno ↔ AlunoDTO
            CreateMap<Aluno, AlunoDTO>()
                .ReverseMap(); // Permite mapeamento nos dois sentidos automaticamente

            // Mapeamento AlunoCreateDTO → Aluno (usado na criação)
            CreateMap<AlunoCreateDTO, Aluno>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID é auto-gerado
                .ForMember(dest => dest.Data_Cadastro, opt => opt.Ignore()) // Data é auto-preenchida
                .ForMember(dest => dest.FotoUrl, opt => opt.Ignore()) // FotoUrl não é fornecida na criação
                .ForMember(dest => dest.Matriculas, opt => opt.Ignore()) // Coleções de navegação ignoradas
                .ForMember(dest => dest.Certificados, opt => opt.Ignore());

            // Mapeamento AlunoUpdateDTO → Aluno (usado na atualização)
            CreateMap<AlunoUpdateDTO, Aluno>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID não é alterado
                .ForMember(dest => dest.Data_Cadastro, opt => opt.Ignore()) // Data de cadastro não muda
                .ForMember(dest => dest.Matriculas, opt => opt.Ignore()) // Coleções de navegação ignoradas
                .ForMember(dest => dest.Certificados, opt => opt.Ignore());

            /* CÓDIGO ANTERIOR (conversão manual nos serviços):
            // Exemplo do que era feito manualmente:
            var alunoDto = new AlunoDTO
            {
                ID = aluno.ID,
                Nome = aluno.Nome,
                Email = aluno.Email,
                Data_Cadastro = aluno.Data_Cadastro
            };
            
            // AGORA: _mapper.Map<AlunoDTO>(aluno) - Uma linha apenas!
            */
        }

        /// <summary>
        /// Configura mapeamentos para a entidade Curso
        /// 
        /// MELHORIAS IMPLEMENTADAS:
        /// - Mapeamento automático com todas as propriedades
        /// - Configuração para criação sem propriedades auto-geradas
        /// </summary>
        private void ConfigurarMapeamentosCurso()
        {
            // Mapeamento bidirecional Curso ↔ CursoDTO
            CreateMap<Curso, CursoDTO>()
                .ReverseMap();

            // Mapeamento CursoCreateDTO → Curso (usado na criação)
            CreateMap<CursoCreateDTO, Curso>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID é auto-gerado
                .ForMember(dest => dest.Matriculas, opt => opt.Ignore()) // Coleções de navegação ignoradas
                .ForMember(dest => dest.Certificados, opt => opt.Ignore());

            /* CÓDIGO ANTERIOR (conversão manual):
            var cursoDto = new CursoDTO
            {
                ID = curso.ID,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                Instrutor = curso.Instrutor,
                Preco = curso.Preco,
                Duracao = curso.Duracao
            };
            
            // AGORA: _mapper.Map<CursoDTO>(curso) - Muito mais limpo!
            */
        }

        /// <summary>
        /// Configura mapeamentos para a entidade Matrícula
        /// 
        /// MELHORIAS IMPLEMENTADAS:
        /// - Mapeamento de propriedades calculadas (CursoTitulo, AlunoNome)
        /// - Configuração condicional baseada em navegação
        /// - Mapeamento apenas para entidade na criação
        /// </summary>
        private void ConfigurarMapeamentosMatricula()
        {
            // Mapeamento Matricula → MatriculaDTO (com propriedades calculadas)
            CreateMap<Matricula, MatriculaDTO>()
                .ForMember(dest => dest.CursoTitulo, 
                          opt => opt.MapFrom(src => src.Curso != null ? src.Curso.Titulo : string.Empty))
                .ForMember(dest => dest.AlunoNome, 
                          opt => opt.MapFrom(src => src.Aluno != null ? src.Aluno.Nome : string.Empty));

            // Mapeamento MatriculaCreateDTO → Matricula (usado na criação)
            CreateMap<MatriculaCreateDTO, Matricula>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID é auto-gerado
                .ForMember(dest => dest.Data_Matricula, opt => opt.Ignore()) // Data é auto-preenchida
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status é definido como Ativa por padrão
                .ForMember(dest => dest.Curso, opt => opt.Ignore()) // Propriedades de navegação ignoradas
                .ForMember(dest => dest.Aluno, opt => opt.Ignore());

            /* CÓDIGO ANTERIOR (conversão manual com verificações):
            var matriculaDto = new MatriculaDTO
            {
                ID = matricula.ID,
                Data_Matricula = matricula.Data_Matricula,
                Curso_ID = matricula.Curso_ID,
                Aluno_ID = matricula.Aluno_ID,
                Status = matricula.Status,
                CursoTitulo = matricula.Curso?.Titulo ?? string.Empty,
                AlunoNome = matricula.Aluno?.Nome ?? string.Empty
            };
            
            // AGORA: _mapper.Map<MatriculaDTO>(matricula) - Automático!
            */
        }

        /// <summary>
        /// Configura mapeamentos para a entidade Certificado
        /// 
        /// MELHORIAS IMPLEMENTADAS:
        /// - Mapeamento de propriedades de navegação
        /// - Configuração para ignorar código de validação na criação
        /// - Mapeamento otimizado para DTOs de consulta
        /// </summary>
        private void ConfigurarMapeamentosCertificado()
        {
            // Mapeamento Certificado → CertificadoDTO (com propriedades calculadas)
            CreateMap<Certificado, CertificadoDTO>()
                .ForMember(dest => dest.CursoTitulo, 
                          opt => opt.MapFrom(src => src.Curso != null ? src.Curso.Titulo : string.Empty))
                .ForMember(dest => dest.AlunoNome, 
                          opt => opt.MapFrom(src => src.Aluno != null ? src.Aluno.Nome : string.Empty));

            // Mapeamento CertificadoCreateDTO → Certificado (usado na criação)
            CreateMap<CertificadoCreateDTO, Certificado>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID é auto-gerado
                .ForMember(dest => dest.Data_Emissao, opt => opt.Ignore()) // Data é auto-preenchida
                .ForMember(dest => dest.Codigo_Validacao, opt => opt.Ignore()) // Código é gerado no serviço
                .ForMember(dest => dest.Curso, opt => opt.Ignore()) // Propriedades de navegação ignoradas
                .ForMember(dest => dest.Aluno, opt => opt.Ignore());

            // Mapeamento CertificadoUpdateDTO → Certificado (usado na atualização)
            CreateMap<CertificadoUpdateDTO, Certificado>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // ID não é alterado
                .ForMember(dest => dest.Data_Emissao, opt => opt.Ignore()) // Data de emissão não muda
                .ForMember(dest => dest.Codigo_Validacao, opt => opt.Ignore()) // Código não muda
                .ForMember(dest => dest.Curso_ID, opt => opt.Ignore()) // IDs de relacionamento não mudam
                .ForMember(dest => dest.Aluno_ID, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore()) // Propriedades de navegação ignoradas
                .ForMember(dest => dest.Aluno, opt => opt.Ignore());

            /* CÓDIGO ANTERIOR (conversão manual complexa):
            var certificadoDto = new CertificadoDTO
            {
                ID = certificado.ID,
                Data_Emissao = certificado.Data_Emissao,
                Curso_ID = certificado.Curso_ID,
                Aluno_ID = certificado.Aluno_ID,
                Codigo_Validacao = certificado.Codigo_Validacao,
                CursoTitulo = certificado.Curso?.Titulo ?? "Curso não encontrado",
                AlunoNome = certificado.Aluno?.Nome ?? "Aluno não encontrado"
            };
            
            // AGORA: _mapper.Map<CertificadoDTO>(certificado) - Simples e eficiente!
            */
        }
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DO AUTOMAPPER PROFILE
═══════════════════════════════════════════════════════════════════════════════════

🎯 OBJETIVO:
   Centralizar todas as configurações de mapeamento entre entidades e DTOs,
   reduzindo código boilerplate e melhorando a manutenibilidade.

⚡ BENEFÍCIOS DE PERFORMANCE:
   - Mapeamentos compilados e cacheados internamente
   - Reflection otimizada para propriedades
   - Reutilização de configurações em toda aplicação
   - Menos allocações de objetos intermediários

🔧 CONFIGURAÇÕES APLICADAS:
   - ReverseMap(): Permite mapeamento bidirecional automático
   - ForMember(): Configuração específica para propriedades complexas
   - Ignore(): Ignora propriedades que não devem ser mapeadas
   - MapFrom(): Define fonte customizada para propriedades calculadas

📊 PADRÕES DE USO:
   - CreateDTO → Entity: Para criação (ignora propriedades auto-geradas)
   - Entity → DTO: Para consultas (inclui propriedades calculadas)
   - DTO → Entity: Para atualizações (mapeia apenas campos editáveis)

🛡️ SEGURANÇA:
   - Propriedades sensíveis (IDs, Senhas) são tratadas adequadamente
   - Validação implícita através de tipos fortemente tipados
   - Prevenção de over-posting através de DTOs específicos

═══════════════════════════════════════════════════════════════════════════════════
*/ 