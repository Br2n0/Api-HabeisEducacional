# API Sistema de Gestão de Certificados - Hábeis Centro Educacional

> Professor, não fui adicionando as etapas quando fui criando pq não me atentei mas a partir de hoje vou fazendo isso certo.

API para gerenciamento e emissão de certificados de cursos preparatórios, desenvolvida em ASP.NET Core 8.0 com Entity Framework Core e MySQL.

## Status Atual do Projeto

- [x] Configuração inicial do projeto ASP.NET Core
- [x] Criação das entidades/modelos do domínio
- [x] Implementação dos DTOs para transferência de dados
- [x] Desenvolvimento dos controladores (CRUD)
- [x] Configuração do Entity Framework Core com MySQL
- [x] Criação da migração inicial
- [x] Aplicação da migração e criação das tabelas no banco de dados
- [x] Implementação da camada de serviços (Services) para separação de responsabilidades
- [x] Refatoração dos controladores para utilizar a camada de serviços
- [x] Implementação de validações de dados em múltiplas camadas
- [x] Implementação de relacionamentos muitos-para-um
- [x] Implementação de Value Objects (Email)
- [x] Implementação de Eventos de Domínio (MatriculaStatusAlteradoEvent)
- [x] Validações de Domínio melhoradas e encapsuladas
- [x] Arquitetura baseada em Domain-Driven Design (DDD)
- [ ] Teste dos endpoints da API (em andamento)
- [ ] Validação e refinamento das funcionalidades
- [ ] Implementação da autenticação e autorização
- [ ] Documentação completa da API
- [ ] Testes unitários e de integração

## 🔄 Melhorias Recentes Implementadas

### 🏗️ **Arquitetura Avançada**
- **Value Objects**: Implementação do Email como Value Object com validações automáticas
- **Eventos de Domínio**: Sistema de eventos para rastreabilidade de mudanças críticas
- **Validações de Domínio**: Regras de negócio encapsuladas nas entidades
- **Encapsulamento**: Melhor proteção de dados e lógica de negócio

### 📊 **Value Objects Implementados**
- **Email**: Validação automática, normalização e imutabilidade
  - Validação de formato via regex
  - Normalização automática (lowercase, trim)
  - Proteção contra emails inválidos
  - Thread-safe e reutilizável

### 🔔 **Eventos de Domínio**
- **MatriculaStatusAlteradoEvent**: Captura mudanças de status de matrículas
  - Rastreabilidade completa de transições
  - Triggers automáticos para ações de negócio
  - Auditoria automática de alterações
  - Facilita integração com outros sistemas

### ✅ **Validações Aprimoradas**
- Regras de negócio implementadas diretamente nas entidades
- Proteção contra transições inválidas de estado
- Validações de domínio com exceções específicas
- Encapsulamento de lógica de validação

## Próximos Passos

### 🎯 **Alta Prioridade**
- Implementar paginação nas consultas de lista
- Adicionar mais Value Objects (Nome, Senha, CodigoValidacao)
- Implementar handlers para processamento de eventos
- Resolver o erro de conexão "TypeError: NetworkError when attempting to fetch resource" no Swagger

### 📈 **Média Prioridade**
- Implementar Specification Pattern para consultas complexas
- Adicionar cache para otimização de desempenho
- Implementar Unit of Work pattern
- Separar configurações do DbContext em classes específicas

### 🚀 **Baixa Prioridade**
- Implementar CQRS (Command Query Responsibility Segregation)
- Adicionar HATEOAS para melhor navegabilidade da API
- Implementar distributed tracing
- Adicionar métricas de performance

## Funcionalidades

### 📚 **Gestão Educacional**
- Gerenciamento de Cursos (CRUD)
- Gerenciamento de Alunos (CRUD) 
- Matrículas de Alunos em Cursos (CRUD)
- Conclusão e cancelamento de matrículas com eventos automáticos
- Emissão e Validação de Certificados

### 🔒 **Validações e Segurança**
- Validações completas para evitar dados inválidos ou vazios
- Email como Value Object com validação automática
- Relacionamentos muitos-para-um implementados corretamente
- Regras de negócio encapsuladas nas entidades

### 📊 **Rastreabilidade e Auditoria**
- Eventos de domínio para mudanças críticas
- Processamento automático de eventos no contexto
- Timestamps automáticos para criações/atualizações
- Histórico completo de transições de estado

## Requisitos

- .NET 8.0 SDK
- MySQL Server 8.0 ou superior

## Configuração

1. Clone o repositório
2. Atualize a string de conexão no arquivo `appsettings.json` com suas credenciais do MySQL:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Database=api-habeiseducacional;User=root;Password=root;Port=3306;"
  }
}
```
3. Execute as migrações do banco de dados:

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Execute o projeto:

```
dotnet run
```

5. Acesse a documentação da API via Swagger: `http://localhost:5121/swagger`

## Estrutura do Projeto

### 📁 **Organização por Responsabilidade**
- `Controllers/`: Controladores da API
- `Models/`: Entidades do domínio
  - `Common/`: Classes base (EntidadeBase, DomainEvent, ValueObject)
  - `ValueObjects/`: Value Objects (Email)
  - `Events/`: Eventos de domínio (MatriculaStatusAlteradoEvent)
- `DTOs/`: Objetos de transferência de dados com validações
- `Data/`: Contexto do banco de dados e configurações de relacionamentos
- `Services/`: Camada de serviços com lógica de negócios e validações

## Padrão de Arquitetura

### 🏗️ **Arquitetura baseada em DDD (Domain-Driven Design)**

O projeto segue uma arquitetura em camadas com princípios de DDD:

1. **Camada de Apresentação (Controllers)**: Responsável por receber as requisições HTTP e retornar respostas.
2. **Camada de Aplicação (Services)**: Contém a lógica de aplicação e orquestração de operações.
3. **Camada de Domínio (Models)**: 
   - **Entidades**: Objetos com identidade (Aluno, Curso, Matricula, Certificado)
   - **Value Objects**: Objetos sem identidade (Email)
   - **Eventos de Domínio**: Comunicação de mudanças importantes
   - **Regras de Negócio**: Encapsuladas nas próprias entidades
4. **Camada de Infraestrutura (Data)**: Gerencia a persistência e recuperação de dados.

### 🔄 **Padrões Implementados**
- **Value Object Pattern**: Para tipos de dados com validações específicas
- **Domain Events Pattern**: Para rastreabilidade e desacoplamento
- **Factory Method Pattern**: Para criação segura de Value Objects
- **Repository Pattern**: Via Entity Framework Core
- **Dependency Injection**: Para desacoplamento de dependências

## Validações Implementadas

### 📋 **Múltiplas Camadas de Validação**
- **DTOs**: Validações usando Data Annotations
- **Serviços**: Validações adicionais de lógica de negócio
- **Domínio**: Regras de negócio encapsuladas nas entidades
- **Value Objects**: Validações automáticas e imutabilidade

### ✅ **Tipos de Validação**
- Verificação de campos obrigatórios
- Validação de formatos de dados (email, tamanhos mínimos e máximos)
- Prevenção contra dados vazios ou inválidos
- Validações de transição de estado
- Regras de negócio específicas do domínio

## Endpoints Principais

### Cursos
- `GET /api/Cursos`: Lista todos os cursos
- `GET /api/Cursos/with-relacionamentos`: Lista cursos com relacionamentos completos
- `GET /api/Cursos/{id}`: Obtém um curso específico
- `POST /api/Cursos`: Cria um novo curso
- `PUT /api/Cursos/{id}`: Atualiza um curso existente
- `DELETE /api/Cursos/{id}`: Remove um curso

### Alunos
- `GET /api/Alunos`: Lista todos os alunos
- `GET /api/Alunos/{id}`: Obtém um aluno específico
- `POST /api/Alunos`: Cria um novo aluno (com validação automática de email)
- `PUT /api/Alunos/{id}`: Atualiza um aluno existente
- `DELETE /api/Alunos/{id}`: Remove um aluno
- `POST /api/Alunos/Login`: Realiza login do aluno

### Matrículas
- `GET /api/Matriculas`: Lista todas as matrículas
- `GET /api/Matriculas/{id}`: Obtém uma matrícula específica
- `POST /api/Matriculas`: Cria uma nova matrícula
- `PUT /api/Matriculas/{id}/status`: Atualiza o status de uma matrícula (dispara eventos)
- `DELETE /api/Matriculas/{id}`: Cancela uma matrícula
- `GET /api/Matriculas/aluno/{alunoId}`: Lista matrículas de um aluno
- `POST /api/Matriculas/{id}/concluir`: Conclui uma matrícula (dispara eventos)

### Certificados
- `GET /api/Certificados`: Lista todos os certificados
- `GET /api/Certificados/{id}`: Obtém um certificado específico
- `POST /api/Certificados`: Emite um novo certificado
- `GET /api/Certificados/aluno/{alunoId}`: Lista certificados de um aluno
- `GET /api/Certificados/validar/{codigo}`: Valida um certificado pelo código

## 🔄 Eventos de Domínio Implementados

### 📧 **MatriculaStatusAlteradoEvent**
Disparado automaticamente quando o status de uma matrícula muda:

- **Matrícula Concluída**: 
  - Log de conclusão
  - Preparação para geração de certificado
  - Possibilidade de envio de email de parabéns

- **Matrícula Cancelada**:
  - Log de cancelamento
  - Preparação para pesquisa de satisfação
  - Possibilidade de processamento de reembolso

### 🔮 **Eventos Futuros Planejados**
- `CertificadoEmitidoEvent`: Para quando um certificado é gerado
- `AlunoMatriculadoEvent`: Para novas matrículas
- `CursoCriadoEvent`: Para novos cursos

## 🎯 Benefícios da Arquitetura Atual

### 💪 **Robustez**
- Código mais seguro com validações automáticas
- Proteção contra estados inválidos
- Encapsulamento de regras de negócio

### 📊 **Rastreabilidade**
- Eventos automáticos para mudanças críticas
- Auditoria completa de operações
- Facilita debugging e monitoramento

### 🔧 **Manutenibilidade**
- Código bem organizado e documentado
- Padrões consistentes aplicados
- Facilita adição de novas funcionalidades

### 🚀 **Escalabilidade**
- Preparado para crescimento
- Arquitetura flexível e extensível
- Facilita integração com outros sistemas 