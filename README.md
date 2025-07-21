# 🛒 FastTech - Sistema de Pedidos

Este projeto é composto por três microserviços principais: `Catálogo`, `Autenticação` e `Pedidos`, integrados com `RabbitMQ` para mensageria e banco de dados MySQL para persistência.

---

## 📦 Estrutura

```
.
├── catalogo-api/
│   └── FastTech.Catalogo/
├── autenticacao-api/
│   └── FastTech.Autenticacao/
├── pedido-api/
│   └── FastTech.Pedido/
└── docker-compose.yml
```

---

## 🚀 Subindo a aplicação com Docker

### ✅ Pré-requisitos

- Docker e Docker Compose instalados
- .NET SDK 8.0+ (para builds locais, se necessário)

### 🧱 Build e execução

```bash
docker compose up --build
```

Isso irá:

- Criar e iniciar os containers de banco de dados
- Executar as migrations de cada serviço
- Subir as APIs com os devidos vínculos

---

## 🌐 Endpoints disponíveis

| Serviço      | Porta Local | Porta Interna | URL Base                                               |
| ------------ | ----------- | ------------- | ------------------------------------------------------ |
| Catálogo     | `8081`      | `8080`        | http://localhost:8081                                  |
| Autenticação | `8082`      | `8080`        | http://localhost:8082                                  |
| Pedidos      | `8083`      | `8080`        | http://localhost:8083                                  |
| RabbitMQ UI  | `15672`     | `15672`       | http://localhost:15672 (usuário: guest / senha: guest) |

---

## 🗃️ Bancos de Dados

| Serviço      | Container        | Porta Local | Banco de Dados          |
| ------------ | ---------------- | ----------- | ----------------------- |
| Catálogo     | `catalogo-mysql` | `3310`      | `fasttech_catalogo`     |
| Autenticação | `auth-mysql`     | `3311`      | `fasttech_autenticacao` |
| Pedidos      | `pedido-mysql`   | `3312`      | `fasttech_pedido`       |

Todos usam:

- Usuário: `root`
- Senha: `123456`

---

## 🧪 Rodando testes localmente

```bash
dotnet test ./catalogo-api/FastTech.Catalogo/FastTech.Catalogo.Application.Test
dotnet test ./autenticacao-api/FastTech.Autenticacao/FastTech.Autenticacao.Application.Test
dotnet test ./pedido-api/FastTech.Pedido/FastTech.Pedido.Application.Test
```

---

## 🔐 Autenticação

- O serviço de autenticação expõe um endpoint `/api/usuarios/login`
- Retorna um JWT que deve ser usado nas demais APIs
- Header padrão:

```http
Authorization: Bearer <seu_token>
```

---

## 📤 RabbitMQ

- RabbitMQ está disponível na porta `15672` (UI)
- As aplicações se comunicam via filas para eventos como: criação de pedido, notificação de usuário, etc.

---

## 🛠️ Ambiente

Essas variáveis de ambiente estão configuradas por padrão:

```env
ASPNETCORE_ENVIRONMENT=Production
CONNECTION_STRING=Server=container-db;Database=db_name;User=root;Password=123456;
RABBITMQ_HOST=rabbitmq
```

---

## 📄 Migrations

Os containers `*-migrations` são executados uma vez para aplicar as migrations automaticamente assim que os bancos estiverem saudáveis.

---

## 🧹 Limpando o ambiente

```bash
docker compose down -v --remove-orphans
```

---

## 🧪 Teste de carga com K6 (exemplo)

```bash
k6 run load-test.js
```