using Domain.Entities;

namespace Domain.Interfaces;

public interface IClienteRepository
{
    Task<List<Cliente>> Listar();

    Task<Cliente?> BuscarPorId(int id);

    Task Criar(Cliente cliente);

    Task Atualizar(Cliente cliente);

    Task Deletar(int id);
}
