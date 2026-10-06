# Catálogo de Jogos – .NET 8 + MongoDB

## Como rodar
1. Subir o MongoDB:
   `docker run -d -p 27017:27017 --name mongo-local mongo`
2. Na pasta do projeto: `dotnet run`
3. Abrir http://localhost:5080/swagger

## Endpoints
| Método | Rota | Retorno |
|---|---|---|
| POST | /api/jogos | 201 |
| GET | /api/jogos | 200 |
| GET | /api/jogos/{id} | 200 / 400 / 404 |
| PUT | /api/jogos/{id} | 200 / 400 / 404 |
| DELETE | /api/jogos/{id} | 204 / 400 / 404 |
| GET | /api/jogos/busca?plataforma=X&precoMaximo=Y | 200 |
| GET | /api/jogos/relatorio-estoque | 200 |
