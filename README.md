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
