namespace CatalogoJogos.Models;

public class RelatorioEstoqueDto
{
    public string Plataforma { get; set; } = string.Empty;
    public int TotalTitulos { get; set; }
    public decimal ValorTotalInventario { get; set; }
}
