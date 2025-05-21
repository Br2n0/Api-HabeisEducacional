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
- Adicionar cache para otimização de desempenho
- Implementar logging para monitoramento da aplicação

## Funcionalidades

- Gerenciamento de Cursos (CRUD)
- Gerenciamento de Alunos (CRUD)
- Matrículas de Alunos em Cursos (CRUD)
- Conclusão e cancelamento de matrículas
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
- `Services/`: Camada de serviços com lógica de negócios

## Padrão de Arquitetura

O projeto segue uma arquitetura em camadas:

1. **Camada de Apresentação (Controllers)**: Responsável por receber as requisições HTTP e retornar respostas.
2. **Camada de Serviços (Services)**: Contém a lógica de negócios e regras da aplicação.
3. **Camada de Acesso a Dados (Data)**: Gerencia a persistência e recuperação de dados.
4. **Camada de Domínio (Models)**: Define as entidades do domínio da aplicação.

A implementação dos serviços (`AlunoService`, `CursoService`, `MatriculaService`, `CertificadoService`) permite a separação clara de responsabilidades e facilita a manutenção e testabilidade do código.

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
- `PUT /api/Matriculas/{id}/status`: Atualiza o status de uma matrícula
- `DELETE /api/Matriculas/{id}`: Cancela uma matrícula
- `GET /api/Matriculas/aluno/{alunoId}`: Lista matrículas de um aluno
- `POST /api/Matriculas/{id}/concluir`: Conclui uma matrícula

### Certificados
- `GET /api/Certificados`: Lista todos os certificados
- `GET /api/Certificados/{id}`: Obtém um certificado específico
- `POST /api/Certificados`: Emite um novo certificado
- `GET /api/Certificados/aluno/{alunoId}`: Lista certificados de um aluno
- `GET /api/Certificados/validar/{codigo}`: Valida um certificado pelo código 