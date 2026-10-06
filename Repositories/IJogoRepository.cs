using CatalogoJogos.Models;

namespace CatalogoJogos.Repositories;

public interface IJogoRepository
{
    Task<List<Jogo>> ObterTodosAsync();
    Task<Jogo?> ObterPorIdAsync(string id);
    Task CriarAsync(Jogo jogo);
    Task<bool> AtualizarAsync(string id, Jogo jogo);
    Task<bool> RemoverAsync(string id);
    Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo);
    Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync();
}
