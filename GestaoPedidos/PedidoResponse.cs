namespace GestaoPedidos;

public class PedidoResponse
{

    public int IdPedido { get; set; }
    public string NomeCliente { get; set; }
    public string NomeProduto { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataPedido { get; set; }
}
