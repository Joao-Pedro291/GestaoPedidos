using Domain.Entities;

namespace Domain.Interfaces;

public interface IProdutoRepository
{
    Task<List<Produto>> Listar();

    Task<Produto?> BuscarPorId(int id);

    Task Criar(Produto produto);

    Task Atualizar(Produto produto);

    Task Deletar(int id);
}
