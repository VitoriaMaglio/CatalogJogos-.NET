using CatalogoJogos.Models;
using CatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CatalogoJogos.Controllers;

[ApiController]
[Route("api/jogos")]
public class JogosController : ControllerBase
{
    private readonly JogoService _service;

    public JogosController(JogoService service)
    {
        _service = service;
    }

    // POST /api/jogos -> 201 | 400 (validação automática do [ApiController])
    [HttpPost]
    public async Task<ActionResult<Jogo>> Criar([FromBody] Jogo jogo)
    {
        var criado = await _service.CriarAsync(jogo);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    // GET /api/jogos -> 200
    [HttpGet]
    public async Task<ActionResult<List<Jogo>>> ObterTodos() =>
        Ok(await _service.ObterTodosAsync());

    // GET /api/jogos/{id} -> 200 | 400 | 404
    [HttpGet("{id}")]
    public async Task<ActionResult<Jogo>> ObterPorId(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Id inválido.");

        var jogo = await _service.ObterPorIdAsync(id);
        return jogo is null ? NotFound() : Ok(jogo);
    }

    // PUT /api/jogos/{id} -> 200 | 400 | 404
    [HttpPut("{id}")]
    public async Task<ActionResult<Jogo>> Atualizar(string id, [FromBody] Jogo jogo)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Id inválido.");

        var atualizado = await _service.AtualizarAsync(id, jogo);
        return atualizado is null ? NotFound() : Ok(atualizado);
    }

    // DELETE /api/jogos/{id} -> 204 | 400 | 404
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return BadRequest("Id inválido.");

        var removido = await _service.RemoverAsync(id);
        return removido ? NoContent() : NotFound();
    }

    // GET /api/jogos/busca?plataforma=PlayStation 3&precoMaximo=100
    [HttpGet("busca")]
    public async Task<ActionResult<List<Jogo>>> Buscar([FromQuery] string plataforma, [FromQuery] decimal precoMaximo)
    {
        if (string.IsNullOrWhiteSpace(plataforma))
            return BadRequest("Informe a plataforma.");

        return Ok(await _service.BuscarAsync(plataforma, precoMaximo));
    }

    // GET /api/jogos/relatorio-estoque
    [HttpGet("relatorio-estoque")]
    public async Task<ActionResult<List<RelatorioEstoqueDto>>> RelatorioEstoque() =>
        Ok(await _service.RelatorioEstoqueAsync());
}
