using Domain.Entities;
using Domain.Interfaces;

public class PedidoService
{
    private readonly IPedidoRepository _repo;
    private readonly IProdutoRepository _produtoRepo;

    public PedidoService(IPedidoRepository repo, IProdutoRepository produtoRepo )
    {
        _repo = repo;
        _produtoRepo = produtoRepo;
    }

    public async Task Criar(CriarPedidoDTO dto)
    {
        var produto = await _produtoRepo.BuscarPorId(dto.ProdutoId);

        if (produto == null)
            throw new Exception("Produto não encontrado");

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            ProdutoId = dto.ProdutoId,
            Quantidade = dto.Quantidade,
            DataPedido = DateTime.Now,
        };

        pedido.CalcularValorTotal(produto.Valor);
        await _repo.Criar(pedido);
    }
}