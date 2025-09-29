SISTEMA GERENCIAR PEDIDOS

PROMPT MESTRE — Instruções de Código para o Codex

Você deve gerar código completo, funcional e pronto para rodar em produção.
Use .NET 6 no backend, React + Vite + Tailwind no frontend, SQL Server no Docker e siga CQRS + DDD.
Não deixe TODOs. Comente o código em português.

📂 Estrutura do Projeto

Crie a seguinte estrutura de pastas:

/ProjetoRevendedores
  /frontend
    /public
    /src
      /app
      /components
      /pages (Catalogo, Carrinho, Admin, Dashboard, Login)
      /features (catalogo, pedidos, auth, relatorios)
      /state (Zustand)
      /services (api, sync, db)
      /assets/icons
    vite.config.ts
    tailwind.config.ts
    package.json
  /backend
    /Catalogo (Domain, Application, Infrastructure, Api)
    /Pedidos (Domain, Application, Infrastructure, Api)
    /Reporting (Projections, Api)
    /Shared (Contracts, DTOs, BuildingBlocks)
    ProjetoRevendedores.sln
  /docker
    docker-compose.yml
    backend.Dockerfile
    frontend.Dockerfile
  /docs
    ADRs/
    arquitetura.md
    api-openapi.json
  .editorconfig
  .gitignore
  README.md

🖥️ Backend (.NET 6)
1. Crie a Solution

ProjetoRevendedores.sln

Adicione projetos:

Catalogo.Domain, Catalogo.Application, Catalogo.Infrastructure, Catalogo.Api

Pedidos.Domain, Pedidos.Application, Pedidos.Infrastructure, Pedidos.Api

Reporting.Projections, Reporting.Api

Shared

2. Catálogo

Entidades: Produto, Setor.

Regras: preço ≥ 0, ativo/inativo.

Commands: CreateProduct, UpdateProduct, ActivateProduct, DeactivateProduct.

API Endpoints: CRUD de setores e produtos.

Use EF Core 6 para persistência.

Configure Swagger.

3. Pedidos

Entidades: Pedido, PedidoItem.

Status: Rascunho → Fechado.

Commands: CreateOrder, AddItem, RemoveItem, ApplyDiscount, CloseOrder.

API Endpoints:

POST /api/pedidos

POST /api/pedidos/{id}/itens

DELETE /api/pedidos/{id}/itens/{itemId}

POST /api/pedidos/{id}/fechar

4. Relatórios

Projeção: VendasPorDiaSetorView.

Endpoint: GET /api/relatorios/vendas?data=YYYY-MM-DD.

5. Seed Inicial

Implemente CatalogoSeed.Seed(modelBuilder) para criar setores e produtos:

Setores: Masculino, Feminino, Infantil, Acessórios.

Produtos Masculino: Camiseta Branca (49,90), Calça Jeans Slim Azul (139,90), Jaqueta Moletom Cinza (189,90).

Produtos Feminino: Vestido Floral (159,90), Blusa Seda Preta (89,90), Saia Jeans (119,90).

Produtos Infantil: Camiseta Super-Herói (39,90), Legging Rosa (59,90), Jaqueta Jeans (109,90).

Produtos Acessórios: Boné Preto (59,90), Cinto de Couro (79,90), Mochila Casual (149,90).

Use PrecoCentavos como int.
Adicione IconeUrl (ex: /icons/camiseta.png).

6. Infra

Configure SQL Server via Docker.

Crie migrations e aplique dotnet ef database update.

Logs com Serilog.

Validações com FluentValidation.

🌐 Frontend (React + Vite + Tailwind + TS)
1. Configuração

Configure React 18 + TypeScript + Tailwind + Vite.

Configure vite-plugin-pwa para offline.

Configure Zustand para estado global.

2. Páginas

Login (fake auth com role Gestor/Vendedor).

Catálogo: abas por setor, cards com ícone/nome/preço/+–.

Carrinho: lista itens, desconto, observação, botão fechar pedido.

Confirmação: nº do pedido + resumo.

Admin: CRUD de setores/produtos.

Dashboard: tabela de vendas por setor.

3. Offline-First

Use Dexie para IndexedDB.

Salve pedidos em pedidos_local.

Crie fila fila_sync para reprocessar pedidos quando online.

Mostre status: “Sincronizando.../Sincronizado”.

🐳 Docker & Deploy
docker-compose.yml

Serviço sqlserver: mcr.microsoft.com/mssql/server:2022-latest.

Serviço backend: build do .NET 6, expõe 5000.

Serviço frontend: build Vite/Nginx, expõe 5173 (dev) ou 8080 (prod).

Comandos
dotnet ef database drop
dotnet ef database update
docker compose up -d

🧪 Testes

Backend: xUnit + coverlet.

Frontend: Vitest + Testing Library.

E2E: Playwright (fluxo login → catálogo → carrinho → fechar pedido → relatório).

Performance: k6 no endpoint /api/pedidos.

📑 Documentação

README.md com:

Pré-requisitos (Windows + Docker).

Setup e execução (docker compose up -d).

Como resetar o banco (dotnet ef database drop && update).

Swagger/OpenAPI documentando todos endpoints.

Coleção Postman/Bruno pronta.

ADRs registrando principais decisões (CQRS, snapshots de preço, etc.).

👉 Instrução Final para o Codex:
Gere o código completo obedecendo todas estas instruções.
O sistema deve iniciar com:

git clone ...
cd ProjetoRevendedores
docker compose up -d


E o fluxo principal deve funcionar imediatamente:
Login → Catálogo (com produtos de roupas já carregados) → Adicionar itens → Carrinho → Fechar Pedido → Relatório (incluindo offline-first).
