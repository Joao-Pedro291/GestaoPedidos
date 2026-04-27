namespace GestaoPedidos;

public class Produto
{
    public required int Id { get; set; }
    public required string Nome { get; set; }
    public int Valor { get; set; }
    public int Estoque { get; set; }
}
