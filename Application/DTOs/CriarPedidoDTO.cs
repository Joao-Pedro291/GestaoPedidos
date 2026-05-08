public class CriarPedidoDTO
{
    public required int ClienteId { get; set; }

    public required int ProdutoId { get; set; }

    public required int Quantidade { get; set; }

    public required decimal ValorTotal { get; set; }

    public DateTime DataPedido { get; set; } = DateTime.Now;
}