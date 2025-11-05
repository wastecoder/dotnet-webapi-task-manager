# 💻 Task Manager – Web API em .NET

Este projeto foi desenvolvido para **praticar o uso de Web APIs no .NET** com foco em **boas práticas de arquitetura, mapeamento de DTOs, desacoplamento via services e integração com banco de dados PostgreSQL**.

A aplicação realiza o **gerenciamento de tarefas**, permitindo **criar, listar, filtrar, atualizar e remover tarefas**, utilizando o **Entity Framework Core** para persistência e o **PostgreSQL** via Docker.  
Também inclui **documentação automática com Swagger (Swashbuckle)** e um padrão de respostas de erro conforme a **RFC 7807 (Problem Details)**.


---


## 📖 Documentação da API

### 🧰 Endpoints Principais

| Método     | Rota         | Descrição                                      |
|------------|--------------|------------------------------------------------|
| **POST**   | `/task`      | Cria uma nova tarefa                           |
| **GET**    | `/task`      | Lista todas as tarefas (com filtros opcionais) |
| **GET**    | `/task/{id}` | Obtém uma tarefa pelo ID                       |
| **PUT**    | `/task/{id}` | Atualiza uma tarefa existente                  |
| **DELETE** | `/task/{id}` | Remove uma tarefa pelo ID                      |


### 🗂️ Schema de Task

```json
{
  "title": "string",
  "description": "string",
  "dueDate": "2025-11-04T09:07:56.453Z",
  "status": "Pending"
}
```

> **Observação:** o campo `status` aceita valores definidos no enum `ETaskStatus`: `Pending`, `Running`, `Completed` e `Cancelled`.


---


## 📂 Estrutura do Projeto

### API
```
dotnet-webapi-task-manager/TaskManager/
├── Controllers/
├── Services/
├── Domain/
│ ├── DTOs/
│ ├── Entities/
│ ├── Enums/
│ ├── Factories/
│ └── Interfaces/
├── Infrastructure/
│ └── Database/
├── Mappers/
├── Migrations/
├── Program.cs
└── appsettings.Development.json
```

> O ponto de entrada da aplicação é o **Program.cs**, que abrirá o Swagger ao executar.


---


## 🛠️ Funcionalidades

- [x] **Gerenciamento de Tarefas**
  - [x] Criação, listagem, atualização e exclusão de tarefas
  - [x] Filtros opcionais por **título**, **status** e **data de vencimento**
  - [x] Paginação de resultados (`Page`, `PageSize`, `TotalItems`, `Items`)

- [x] **DTOs (Data Transfer Objects)**
  - [x] `CreateTaskDto`, `UpdateTaskDto` e `TaskResponseDto`
  - [x] `PagedResponse` para paginação
  - [x] `ProblemDetailsDto` no formato **RFC 7807**

- [x] **Mapper**
  - [x] Conversões entre entidades e DTOs (`TaskMapper`)
  - [x] Conversão automática para respostas padronizadas

- [x] **Camadas bem definidas**
  - [x] **Domain** → Entidades, Enums e Interfaces
  - [x] **Infrastructure** → Contexto de banco de dados e EF Core
  - [x] **Services** → Regras de negócio e lógica assíncrona
  - [x] **Controllers** → Endpoints da API

- [x] **Banco de Dados via Docker**
  - [x] Container PostgreSQL configurado no `docker-compose.yml`
  - [x] Migrations geradas via `dotnet ef migrations`

- [x] **Swagger**
  - [x] Documentação automática dos endpoints
  - [x] Interface interativa para testes


---


## ⚙️ Tecnologias Utilizadas

- **.NET SDK 9.0** → plataforma principal
- **C# 12** → linguagem de desenvolvimento
- **Entity Framework Core** → ORM para persistência
- **PostgreSQL (via Docker)** → banco de dados relacional
- **Swashbuckle.AspNetCore** → documentação e testes via Swagger
- **Rider** → IDE utilizada no desenvolvimento


---


## 🧪 Como Executar o Projeto

1. Clone o repositório:
```bash
git clone https://github.com/wastecoder/dotnet-webapi-task-manager.git
cd dotnet-webapi-task-manager
```

2. Construa e suba os containers (API + PostgreSQL):
```bash
docker compose up -d --build
```

Use o parâmetro `--build` sempre que houver alterações no código.

3. Aguarde alguns segundos para o PostgreSQL inicializar.

A API aplicará automaticamente as migrations do Entity Framework ao iniciar.

4. Acesse o Swagger na porta [8080](http://localhost:8080/swagger) para testar os endpoints.

5. Caso queira parar os containers:
```bash
docker compose down
```


---


## 📈 Próximos Passos

- **🔍 Adicionar filtros avançados**
  - Permitir busca combinada por **título**, **status** e **intervalo de datas**.  

- **🧪 Implementar testes de unidade**
  - Criar testes para **TaskService** e **TaskMapper**.  

- **🧰 Adicionar validação de dados nos DTOs**
  - Utilizar **Data Annotations** (ex: `[Required]`, `[StringLength]`, `[Range]`, `[EnumDataType]`) para validar automaticamente as entradas recebidas nos DTOs.  
  - Retornar respostas padronizadas com **ProblemDetailsDto** em caso de erro de validação.

- **📦 Criar seed inicial de dados**
  - Inserir tarefas de exemplo automaticamente ao iniciar o ambiente de desenvolvimento.
