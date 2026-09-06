# DOCUMENTATION - MeAccount

## Índice

1. [Visão Geral do Projeto](#1-visão-geral-do-projeto)
2. [Arquitetura DDD](#2-arquitetura-ddd)
3. [Bounded Contexts](#3-bounded-contexts)
4. [Stack Tecnológica](#4-stack-tecnológica)
5. [API Endpoints](#5-api-endpoints)
6. [Setup e Execução](#6-setup-e-execução)

---

## 1. Visão Geral do Projeto

O **MeAccount** é um sistema de gestão financeira pessoal desenvolvido com arquitetura DDD (Domain-Driven Design).

### Funcionalidades

- **Autenticação**: Registro e login com JWT
- **Categorias**: Classificação de transações (receitas/despesas)
- **Transações**: Registro de movimentações financeiras
- **Contas**: Gerenciamento de contas bancárias (entidade domain, API em desenvolvimento)
- **Dashboard**: Visão geral das finanças

---

## 2. Arquitetura DDD

### Estrutura de Pastas

```
MeAccount/
├── BoundedContexts/
│   ├── Account/                # Contas bancárias (domain + repository)
│   │   ├── Domain/
│   │   │   └── Entities/
│   │   └── Infrastructure/
│   │       └── Persistence/
│   │
│   ├── Category/               # Categorias (completo)
│   │   ├── Domain/
│   │   │   ├── Entities/
│   │   │   └── ValueObjects/
│   │   ├── Application/
│   │   │   ├── DTOs/
│   │   │   └── Services/
│   │   └── Infrastructure/
│   │       ├── API/
│   │       └── Persistence/
│   │
│   └── Transaction/            # Transações (completo)
│       ├── Domain/
│       │   ├── Entities/
│       │   └── ValueObjects/
│       ├── Application/
│       │   ├── DTOs/
│       │   └── Services/
│       └── Infrastructure/
│           ├── API/
│           └── Persistence/
│
├── SharedKernel/               # Classes compartilhadas
│   ├── Domain/
│   │   ├── Entity.cs
│   │   └── ValueObject.cs
│   └── Infrastructure/
│       └── BaseApiController.cs
│
├── Data/                       # Contexto EF Core
├── Controllers/                # Auth controller (legado)
├── Models/                     # User model (legado)
├── Services/                   # Auth service (legado)
└── Program.cs                  # Configuração
```

### Fluxo de uma Requisição

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Cliente   │───▶│  Controller │───▶│   Service   │───▶│  Repository │
│  (Angular)  │◀───│  (Authorize)│◀───│  (Negócio)  │◀───│  (Acesso)   │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
```

---

## 3. Bounded Contexts

### Context Mapping (Atual)

```
                    ┌─────────────────┐
                    │    IDENTITY     │
                    │  (legado root)  │
                    └────────┬────────┘
                             │
               ┌─────────────┼─────────────┐
               │             │             │
               ▼             ▼             ▼
     ┌─────────────┐  ┌─────────────┐  ┌─────────────┐
     │   ACCOUNT   │  │ TRANSACTION │  │   CATEGORY  │
     │  (Domain)   │◄─┤  (Completo) │─►│  (Completo) │
     └─────────────┘  └─────────────┘  └─────────────┘
```

### Dependências

| Contexto | Tipo | Depende de | Status |
|----------|------|------------|--------|
| **Identity** | Legado root | Nenhum | Auth funcional (AuthController + AuthService) |
| **Account** | Domain only | Nenhum | Entidade + Repository (sem controller/service) |
| **Category** | Completo | Nenhum | Repository → Service → Controller |
| **Transaction** | Completo | Account, Category | Repository → Service → Controller |

### SharedKernel

- **Entity**: Base class com `Id` (Guid)
- **ValueObject**: Base class abstrata com `GetEqualityComponents()`
- **BaseApiController**: Controller base com `GetUserId()` e atributos `[ApiController]`/`[Route]`

---

## 4. Stack Tecnológica

| Componente | Tecnologia | Versão |
|------------|------------|--------|
| **Runtime** | .NET | 10.0 |
| **Framework** | ASP.NET Core | 10.0 |
| **ORM** | Entity Framework Core | 10.0 |
| **Banco** | PostgreSQL | 17 |
| **Autenticação** | JWT Bearer | 8.x |
| **Identidade** | ASP.NET Core Identity | 10.0 |
| **Frontend** | Angular | 22 |

---

## 5. API Endpoints

### Auth

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/auth/register` | Registrar usuário | Não |
| POST | `/api/auth/login` | Login | Não |

### Categories

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/categories` | Listar categorias | JWT |
| GET | `/api/categories/{id}` | Buscar por ID | JWT |
| POST | `/api/categories` | Criar categoria | JWT |
| PUT | `/api/categories/{id}` | Atualizar categoria | JWT |
| DELETE | `/api/categories/{id}` | Excluir categoria | JWT |

### Transactions

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/transactions` | Listar transações | JWT |
| GET | `/api/transactions/{id}` | Buscar por ID | JWT |
| POST | `/api/transactions` | Criar transação | JWT |
| PUT | `/api/transactions/{id}` | Atualizar transação | JWT |
| DELETE | `/api/transactions/{id}` | Excluir transação | JWT |

---

## 6. Setup e Execução

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 17](https://www.postgresql.org/download/)
- [Node.js 24+](https://nodejs.org/)

### Backend

```bash
cd C:\Users\marce\Documents\MeAccount
dotnet restore
dotnet ef database update
dotnet run --launch-profile https
```

### Frontend

```bash
cd C:\Users\marce\Documents\MeAccount-Frontend
npm install
npx ng serve --proxy-config proxy.conf.json
```

### Acessos

- **Frontend**: http://localhost:4200
- **Backend**: https://localhost:7272
- **Swagger**: https://localhost:7272/swagger

---

*Documentação atualizada em: 27/07/2026*
*Arquitetura: DDD (Domain-Driven Design) com Bounded Contexts*
