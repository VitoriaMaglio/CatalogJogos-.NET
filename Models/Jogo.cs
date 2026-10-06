using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogoJogos.Models;

public class Jogo
{
    // Guarda como ObjectId no Mongo, mas na API trabalhamos com string
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("titulo")]
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    [BsonElement("plataforma")]
    [Required(ErrorMessage = "A plataforma é obrigatória.")]
    public string Plataforma { get; set; } = string.Empty;

    [BsonElement("genero")]
    [Required(ErrorMessage = "O gênero é obrigatório.")]
    public string Genero { get; set; } = string.Empty;

    // Decimal128 é o tipo do Mongo que preserva precisão decimal (dinheiro)
    [BsonElement("preco")]
    [BsonRepresentation(BsonType.Decimal128)]
    [Range(0, double.MaxValue, ErrorMessage = "O preço não pode ser negativo.")]
    public decimal Preco { get; set; }

    [BsonElement("anoLancamento")]
    [Range(1950, 2100, ErrorMessage = "Ano de lançamento inválido.")]
    public int AnoLancamento { get; set; }

    [BsonElement("estoque")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }
}
