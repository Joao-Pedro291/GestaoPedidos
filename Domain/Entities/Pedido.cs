using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Pedido
{
    public int PedidoId { get; set; }

    public required int ClienteId { get; set; }

    public required int ProdutoId { get; set; }

    public required int Quantidade { get; set; }
    public decimal ValorTotal { get; private set; }

    public DateTime DataPedido { get; set; } = DateTime.Now;

    public void CalcularValorTotal(decimal valorProduto)
    {
        if(Quantidade <= 0)
        {
            throw new Exception("Quantidade deve ser maior que zero");
        }

        ValorTotal = Quantidade * valorProduto;
    }

}