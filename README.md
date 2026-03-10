# LuzaBlog

Sistema de blog completo com ASP.NET Core 10, Clean Architecture, DDD, MediatR (CQRS), Entity Framework Core (SQLite), SignalR e React + Vite + TypeScript.

---

## Arquitetura

```
/
├── src/LuzaBlog.Api/          # Backend ASP.NET Core 10
│   ├── Domain/                # Entidades, Value Objects, Eventos de Domínio
│   ├── Application/           # Commands, Queries (MediatR/CQRS), DTOs
│   ├── Infrastructure/        # EF Core, JWT, SignalR Hub
│   └── Presentation/          # Controllers, Middleware
└── frontend/                  # React + Vite + TypeScript
```

### Camadas

| Camada | Responsabilidade |
|--------|-----------------|
| **Domain** | `Post` (aggregate root), `PostTitle`/`PostContent` (value objects), `PostPublishedEvent` |
| **Application** | MediatR commands/queries, interfaces `IPostRepository`, `IJwtTokenService` |
| **Infrastructure** | `ApplicationDbContext` (EF Core + Identity), `PostRepository`, `JwtTokenService`, `PostHub` (SignalR) |
| **Presentation** | `AuthController`, `PostsController`, `ExceptionHandlingMiddleware` |

### Princípios aplicados

- **SRP**: Cada handler MediatR tem uma única responsabilidade
- **DIP**: Application depende de interfaces; Infrastructure implementa
- **DDD**: Aggregate root com factory method, value objects com invariantes, domain events
- **CQRS**: Commands (escrita) e Queries (leitura) separados via MediatR

---

## Funcionalidades

- Registro e login com JWT
- Criar, editar (somente o autor) e excluir postagens
- Visualização pública de postagens
- **Notificações em tempo real** via SignalR: todos os usuários conectados recebem um alerta quando uma nova postagem é publicada

---

## Rodando localmente

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [dotnet-ef CLI](https://learn.microsoft.com/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

### Backend

```bash
cd src/LuzaBlog.Api
dotnet ef database update    # cria o banco SQLite (luza-blog.db)
dotnet run                   # inicia em http://localhost:5000
```

### Frontend

```bash
cd frontend
npm install
npm run dev                  # inicia em http://localhost:5173
```

O arquivo `frontend/.env.development` já aponta para `http://localhost:5000`.

---

## Variáveis de Ambiente

### Backend

| Variável | Descrição | Padrão (dev) |
|----------|-----------|--------------|
| `ConnectionStrings__DefaultConnection` | String de conexão SQLite | `Data Source=luza-blog.db` |
| `JwtSettings__Secret` | Chave secreta JWT (mín. 32 chars) | valor de dev no `appsettings.json` |
| `JwtSettings__Issuer` | Issuer do token | `LuzaBlog` |
| `JwtSettings__Audience` | Audience do token | `LuzaBlogClients` |
| `Cors__AllowedOrigins` | Origens permitidas (separadas por vírgula) | `http://localhost:5173` |

### Frontend

| Variável | Descrição |
|----------|-----------|
| `VITE_API_URL` | URL base da API |
| `VITE_HUB_URL` | URL do hub SignalR |

---

## API Endpoints

### Auth

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/auth/register` | — | Registrar novo usuário |
| POST | `/api/auth/login` | — | Login e obter JWT |

### Posts

| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| GET | `/api/posts` | — | Listar todas as postagens |
| GET | `/api/posts/{id}` | — | Buscar postagem por ID |
| POST | `/api/posts` | JWT | Criar nova postagem |
| PUT | `/api/posts/{id}` | JWT | Editar postagem (somente autor) |
| DELETE | `/api/posts/{id}` | JWT | Excluir postagem (somente autor) |

### SignalR

Hub disponível em `/hubs/posts`. Evento emitido: `PostPublished` com payload:

```json
{ "postId": "guid", "title": "string", "authorId": "string" }
```

---

## Deploy

### Frontend → Vercel

1. Conecte o repositório no Vercel
2. Defina o **Root Directory** como `frontend`
3. O Vercel detecta Vite automaticamente (build: `npm run build`, output: `dist`)
4. Adicione as variáveis de ambiente no painel do Vercel:
   - `VITE_API_URL` → URL do backend em produção
   - `VITE_HUB_URL` → `<URL_BACKEND>/hubs/posts`
5. O arquivo `frontend/vercel.json` já configura os rewrites para o React Router

### Backend → Railway / Render

1. Conecte o repositório
2. Defina o **Root Directory** como `src/LuzaBlog.Api`
3. Build: `dotnet publish -c Release -o out`
4. Start: `dotnet out/LuzaBlog.Api.dll`
5. Variáveis de ambiente obrigatórias:
   - `JwtSettings__Secret` — string aleatória longa (32+ chars)
   - `Cors__AllowedOrigins` — URL do frontend no Vercel
   - `ConnectionStrings__DefaultConnection` — `Data Source=/data/luza-blog.db` (disco persistente)
6. O banco de dados é migrado automaticamente na inicialização

---

## Tecnologias

| Área | Tecnologia |
|------|-----------|
| Backend framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 9 + SQLite |
| Auth | ASP.NET Identity + JWT Bearer |
| Mensageria interna | MediatR 12 (CQRS + Domain Events) |
| Tempo real | SignalR (built-in) |
| Frontend framework | React 18 + Vite 5 + TypeScript |
| HTTP Client | Axios |
| Estado global | Zustand (com persist) |
| Roteamento | React Router v6 |
| Cliente SignalR | @microsoft/signalr |
