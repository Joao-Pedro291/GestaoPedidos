namespace GestaoPedidos;

public class Pedido
{
    public required int PedidoId { get; set; }
    public required int ClienteId { get; set; }
    public required int ProdutoId { get; set; }
    public required int Quantidade { get; set; }
    public required decimal ValorTotal { get; set; }
    public DateTime DataPedido { get; set; }

}