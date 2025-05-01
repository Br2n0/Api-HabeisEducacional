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
- [ ] Teste dos endpoints da API (em andamento)
- [ ] Validação e refinamento das funcionalidades
- [ ] Implementação da autenticação e autorização
- [ ] Documentação completa da API
- [ ] Testes unitários e de integração

## Próximos Passos

- Resolver o erro de conexão "TypeError: NetworkError when attempting to fetch resource" no Swagger
- Implementar testes completos para todos os endpoints
- Refinar as validações e tratamento de erros
- Implementar autenticação JWT

## Funcionalidades

- Gerenciamento de Cursos (CRUD)
- Gerenciamento de Alunos (CRUD)
- Matrículas de Alunos em Cursos (CRUD)
- Emissão e Validação de Certificados

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

- `Controllers/`: Controladores da API
- `Models/`: Entidades do domínio
- `DTOs/`: Objetos de transferência de dados
- `Data/`: Contexto do banco de dados

## Endpoints Principais

### Cursos
- `GET /api/Cursos`: Lista todos os cursos
- `GET /api/Cursos/{id}`: Obtém um curso específico
- `POST /api/Cursos`: Cria um novo curso
- `PUT /api/Cursos/{id}`: Atualiza um curso existente
- `DELETE /api/Cursos/{id}`: Remove um curso

### Alunos
- `GET /api/Alunos`: Lista todos os alunos
- `GET /api/Alunos/{id}`: Obtém um aluno específico
- `POST /api/Alunos`: Cria um novo aluno
- `PUT /api/Alunos/{id}`: Atualiza um aluno existente
- `DELETE /api/Alunos/{id}`: Remove um aluno
- `POST /api/Alunos/Login`: Realiza login do aluno

### Matrículas
- `GET /api/Matriculas`: Lista todas as matrículas
- `GET /api/Matriculas/{id}`: Obtém uma matrícula específica
- `POST /api/Matriculas`: Cria uma nova matrícula
- `PUT /api/Matriculas/{id}`: Atualiza uma matrícula existente
- `DELETE /api/Matriculas/{id}`: Remove uma matrícula
- `GET /api/Matriculas/aluno/{alunoId}`: Lista matrículas de um aluno

### Certificados
- `GET /api/Certificados`: Lista todos os certificados
- `GET /api/Certificados/{id}`: Obtém um certificado específico
- `POST /api/Certificados`: Emite um novo certificado
- `GET /api/Certificados/aluno/{alunoId}`: Lista certificados de um aluno
- `GET /api/Certificados/validar/{codigo}`: Valida um certificado pelo código 