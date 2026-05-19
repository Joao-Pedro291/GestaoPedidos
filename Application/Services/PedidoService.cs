using Domain.Entities;
using Domain.Interfaces;
using System.Runtime.CompilerServices;

public class PedidoService
{
    private readonly IPedidoRepository _repo;
    private readonly IProdutoRepository _produtoRepo;
    private readonly IClienteRepository _clienteRepo;

    public PedidoService(IPedidoRepository repo, IProdutoRepository produtoRepo, IClienteRepository clienteRepo )
    {
        _repo = repo;
        _produtoRepo = produtoRepo;
        _clienteRepo = clienteRepo;
    }

    public async Task Criar(CriarPedidoDTO dto)
    {
        var produto = await _produtoRepo.BuscarPorId(dto.ProdutoId);

        if (produto == null)
            throw new Exception("Produto não encontrado");

        var cliente = await _clienteRepo.BuscarPorId(dto.ClienteId);

        if (cliente == null)
            throw new Exception("Cliente não encontrado");

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

    public async Task<Pedido?> Atualizar(Pedido pedido)
    {
        var produto = await _produtoRepo.BuscarPorId(pedido.ProdutoId);

        if (produto == null)
            throw new Exception("Produto não encontrado");


        var pedidoExistente = await _repo.BuscarPorId(pedido.PedidoId);

        if (pedidoExistente == null)
        {
            return null;
        }

        pedidoExistente.PedidoId = pedido.PedidoId;
        pedidoExistente.ClienteId = pedido.ClienteId;
        pedidoExistente.Quantidade = pedido.Quantidade;
        pedidoExistente.DataPedido = DateTime.Now;

        pedidoExistente.CalcularValorTotal(produto.Valor);
        await _repo.Atualizar(pedidoExistente);

        return (pedidoExistente);
    }

    public async Task<List<Pedido>> Listar()
    {
        return await _repo.Listar();
    }

    public async Task<Pedido?> BuscarPorId(int id)
    {
        var pedido = await _repo.BuscarPorId(id);

        if (pedido == null)
            return null;

        return pedido;

    }

    public async Task<(bool Sucesso, string? Erro)> Deletar(int id)
    {
        var produto = await _repo.BuscarPorId(id);

        if (produto == null)
        {
            return (false, "Produto não encontrado");
        }

        await _repo.Deletar(id);

        return (true, null);
    }
}