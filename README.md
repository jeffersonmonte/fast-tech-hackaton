# FastTech Foods

Esta é a plataforma de gerenciamento de atendimento e pedidos da FastTech Foods.

## Arquitetura

A plataforma é baseada em uma arquitetura de microsserviços, com os seguintes serviços:

- **Auth Service:** Gerencia a autenticação de funcionários e clientes.
- **Menu Service:** Gerencia o cardápio de produtos.
- **Order Service:** Gerencia os pedidos dos clientes.
- **Kitchen Service:** Gerencia o preparo dos pedidos na cozinha.
- **Search Service:** Fornece funcionalidades de busca de produtos.

## Como Executar

Para executar a solução completa, você precisará de:

- Docker
- Kubernetes (Minikube, Kind, ou um cluster em nuvem)
- .NET 8 SDK

### 1. Construir e Publicar as Imagens Docker

Execute o seguinte comando na raiz do repositório para construir e publicar as imagens Docker de cada serviço:

```bash
# Faça login no seu registry de contêineres
docker login

# Construa e publique as imagens
docker build -t <seu-registry>/fasttech-autenticacao-api:latest -f autenticacao-api/FastTech.Autenticacao/dockerfile autenticacao-api/FastTech.Autenticacao
docker push <seu-registry>/fasttech-autenticacao-api:latest

docker build -t <seu-registry>/fasttech-catalogo-api:latest -f catalogo-api/FastTech.Catalogo/dockerfile catalogo-api/FastTech.Catalogo
docker push <seu-registry>/fasttech-catalogo-api:latest

docker build -t <seu-registry>/fasttech-pedido-api:latest -f pedido-api/FastTech.Pedido/FastTech.Pedido.Api/Dockerfile pedido-api/FastTech.Pedido
docker push <seu-registry>/fasttech-pedido-api:latest

docker build -t <seu-registry>/fasttech-kitchen-api:latest -f kitchen-api/FastTech.Kitchen/dockerfile kitchen-api/FastTech.Kitchen
docker push <seu-registry>/fasttech-kitchen-api:latest
```

### 2. Fazer o Deploy no Kubernetes

Depois de publicar as imagens, você pode fazer o deploy dos serviços no Kubernetes:

```bash
# Crie o secret de TLS (substitua pelos seus próprios arquivos de certificado e chave)
kubectl create secret tls fasttech-tls --key tls.key --cert tls.crt

# Aplique os manifestos do Kubernetes
kubectl apply -f iac/
```

### 3. Testar a Aplicação

Depois que os serviços estiverem em execução, você pode testar a aplicação acessando os endpoints através do Ingress.

## CI/CD

O repositório está configurado com um pipeline de CI/CD usando o GitHub Actions. O pipeline é acionado a cada push para a branch `main` e irá automaticamente construir, testar, publicar as imagens Docker e fazer o deploy no Kubernetes.
=======
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
