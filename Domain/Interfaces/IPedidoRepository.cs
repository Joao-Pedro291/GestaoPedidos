using Domain.Entities;

namespace Domain.Interfaces;

public interface IPedidoRepository
{
    Task<List<Pedido>> Listar();

    Task<Pedido?> BuscarPorId(int id);

    Task Criar(Pedido pedido);

    Task Atualizar(Pedido pedido);

    Task Deletar(int id);
}
