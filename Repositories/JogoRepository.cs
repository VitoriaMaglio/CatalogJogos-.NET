using CatalogoJogos.Models;
using CatalogoJogos.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogoJogos.Repositories;

public class JogoRepository : IJogoRepository
{
    private readonly IMongoCollection<Jogo> _jogos;

    // IMongoDatabase vem da injeção de dependência (configurada no Program.cs)
    public JogoRepository(IMongoDatabase database, IOptions<MongoDbSettings> options)
    {
        _jogos = database.GetCollection<Jogo>(options.Value.JogosCollectionName);
    }

    // ---------- CRUD ----------

    public async Task<List<Jogo>> ObterTodosAsync() =>
        await _jogos.Find(_ => true).ToListAsync();

    public async Task<Jogo?> ObterPorIdAsync(string id) =>
        await _jogos.Find(j => j.Id == id).FirstOrDefaultAsync();

    public async Task CriarAsync(Jogo jogo) =>
        await _jogos.InsertOneAsync(jogo); // o driver preenche jogo.Id após inserir

    public async Task<bool> AtualizarAsync(string id, Jogo jogo)
    {
        var resultado = await _jogos.ReplaceOneAsync(j => j.Id == id, jogo);
        return resultado.MatchedCount > 0;
    }

    public async Task<bool> RemoverAsync(string id)
    {
        var resultado = await _jogos.DeleteOneAsync(j => j.Id == id);
        return resultado.DeletedCount > 0;
    }

    // ---------- DESAFIO 1: filtro com Builders ----------

    public async Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo)
    {
        var builder = Builders<Jogo>.Filter;

        // plataforma == X  E  preco <= precoMaximo
        var filtro = builder.And(
            builder.Eq(j => j.Plataforma, plataforma),
            builder.Lte(j => j.Preco, precoMaximo)
        );

        return await _jogos.Find(filtro).ToListAsync();
    }

    // ---------- DESAFIO 2: pipeline de agregação ----------

    public async Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync()
    {
        // Equivalente ao Mongo shell:
        // db.jogos.aggregate([
        //   { $group: {
        //       _id: "$plataforma",
        //       totalTitulos: { $sum: 1 },
        //       valorTotalInventario: { $sum: { $multiply: ["$preco", "$estoque"] } }
        //   }},
        //   { $sort: { _id: 1 } }
        // ])
        var grupo = new BsonDocument
        {
            { "_id", "$plataforma" },
            { "totalTitulos", new BsonDocument("$sum", 1) },
            { "valorTotalInventario", new BsonDocument("$sum",
                new BsonDocument("$multiply", new BsonArray { "$preco", "$estoque" })) }
        };

        var docs = await _jogos.Aggregate()
            .Group(grupo)
            .Sort(new BsonDocument("_id", 1))
            .ToListAsync();

        return docs.Select(d => new RelatorioEstoqueDto
        {
            Plataforma = d["_id"].AsString,
            TotalTitulos = d["totalTitulos"].ToInt32(),
            ValorTotalInventario = d["valorTotalInventario"].ToDecimal()
        }).ToList();
    }
}
