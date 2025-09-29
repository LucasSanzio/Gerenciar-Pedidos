# 📌 ProjetoRevendedores

Sistema de pedidos para revendedores com catálogo, carrinho, pedidos, relatórios e operação offline-first.

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 6**: plataforma principal do backend.
- **ASP.NET Core**: construção da API RESTful.
- **Entity Framework Core 6**: ORM para acesso e mapeamento do banco.
- **MediatR**: orquestração dos comandos e handlers seguindo CQRS.
- **FluentValidation**: validações robustas das entradas.
- **AutoMapper**: mapeamento entre entidades e DTOs.
- **Serilog**: observabilidade e logging estruturado.
- **Swagger**: documentação interativa dos endpoints.

### Frontend
- **React 18**: base da interface do usuário.
- **Vite**: bundler e dev server rápido.
- **TypeScript**: tipagem estática para maior segurança.
- **TailwindCSS**: estilização utilitária.
- **React Router 6**: roteamento SPA.
- **Zustand**: gerenciamento de estado global.
- **Axios**: cliente HTTP.
- **Dexie**: abstração sobre IndexedDB para dados offline.
- **vite-plugin-pwa**: suporte PWA e offline-first.

### Banco de Dados
- **SQL Server 2022 (Docker)**: armazenamento relacional principal.

### Infraestrutura
- **Docker & Docker Compose**: conteinerização e orquestração local.
- **GitHub Actions (CI/CD)**: automação de builds, testes e deploys.

### Testes
- **xUnit**: testes automatizados do backend.
- **Vitest + React Testing Library**: cobertura de componentes no frontend.
- **Playwright**: testes ponta a ponta.
- **k6**: testes de performance.

## 📂 Estrutura do Projeto

```
/ProjetoRevendedores
  /frontend
  /backend
  /docker
  /docs
```

- **frontend**: código da aplicação React, incluindo páginas, componentes, stores Zustand e configurações do Vite.
- **backend**: soluções .NET divididas em módulos (Catálogo, Pedidos, Reporting) com Domain, Application, Infrastructure e APIs.
- **docker**: arquivos de conteinerização (Dockerfiles e docker-compose) para ambiente de desenvolvimento e deploy.
- **docs**: documentação complementar como ADRs, arquitetura e especificações OpenAPI.

## ⚙️ Pré-requisitos
- Docker e Docker Compose instalados.
- .NET 6 SDK (opcional para executar os projetos fora dos containers).

## ▶️ Instruções de Execução

Clone o repositório e acesse a pasta do projeto:

```bash
git clone <url>
cd ProjetoRevendedores
```

Suba os containers:

```bash
docker compose up -d
```

Acesse os serviços:

- Backend API: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- Frontend (dev): [http://localhost:5173](http://localhost:5173)
- Frontend (prod): [http://localhost:8080](http://localhost:8080)

## 🧪 Testes

### Backend (xUnit)

```bash
dotnet test
```

### Frontend (Vitest)

```bash
npm run test
```

### E2E (Playwright)

```bash
npx playwright test
```

## 📊 Seed Inicial

O sistema já inicia com setores e produtos de roupas cadastrados (Masculino, Feminino, Infantil e Acessórios), garantindo catálogo completo logo no primeiro acesso.
