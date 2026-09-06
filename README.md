# MeAccount

Aplicacao de gestao financeira pessoal com Angular, ASP.NET Core e PostgreSQL.

## Estrutura

- `MeAccount/`: API ASP.NET Core, EF Core, Identity e JWT.
- `MeAccount-frontend/`: aplicacao Angular servida por nginx.
- `docker-compose.yml`: ambiente local completo.
- `mockups/`: referencias visuais do produto.

## Ambiente local

Requisitos: Docker e Docker Compose.

1. Crie a configuracao local a partir do exemplo:

   ```bash
   cp .env.example .env
   ```

2. Substitua os valores de `POSTGRES_PASSWORD` e `JWT_KEY` no `.env`.

3. Inicie os servicos:

   ```bash
   docker compose up -d --build
   ```

4. Acesse o frontend em `http://localhost:4200` e a saude da API em `http://localhost:5188/health`.

O arquivo `.env`, backups SQL, dependencias e artefatos de build nao sao versionados.

## Verificacao local

```bash
dotnet build --no-restore MeAccount/MeAccount.csproj
npm --prefix MeAccount-frontend run build
docker compose config --quiet
```

## CI/CD

O workflow `.github/workflows/ci-cd.yml` e executado em pull requests e pushes para `main`.

- Compila backend e frontend.
- Valida a configuracao do Docker Compose.
- Sobe um ambiente descartavel com PostgreSQL.
- Testa saude da API e do frontend, cadastro, login e rejeicao de credenciais invalidas.
- Em pushes para `main`, publica imagens de backend e frontend no GitHub Container Registry com as tags `latest` e o SHA do commit.

As imagens geradas sao uma entrega continua. Um deploy automatico para hospedagem pode ser adicionado quando houver um ambiente de destino.
