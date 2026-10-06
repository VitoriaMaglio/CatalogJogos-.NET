using CatalogoJogos.Models;
using CatalogoJogos.Repositories;

namespace CatalogoJogos.Services;

public class JogoService
{
    private readonly IJogoRepository _repository;

    public JogoService(IJogoRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Jogo>> ObterTodosAsync() => _repository.ObterTodosAsync();

    public Task<Jogo?> ObterPorIdAsync(string id) => _repository.ObterPorIdAsync(id);

    public async Task<Jogo> CriarAsync(Jogo jogo)
    {
        jogo.Id = null; // garante que o Mongo gere um novo ObjectId
        await _repository.CriarAsync(jogo);
        return jogo;
    }

    public async Task<Jogo?> AtualizarAsync(string id, Jogo jogo)
    {
        jogo.Id = id; // o id da rota manda
        var atualizado = await _repository.AtualizarAsync(id, jogo);
        return atualizado ? jogo : null;
    }

    public Task<bool> RemoverAsync(string id) => _repository.RemoverAsync(id);

    public Task<List<Jogo>> BuscarAsync(string plataforma, decimal precoMaximo) =>
        _repository.BuscarPorPlataformaEPrecoAsync(plataforma, precoMaximo);

    public Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync() =>
        _repository.RelatorioEstoqueAsync();
}
