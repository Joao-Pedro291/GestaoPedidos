using Domain.Interfaces;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IPedidoRepository _pedidoRepository;

    public ClienteService(IClienteRepository clienteRepository, IPedidoRepository pedidoRepository)
    {
        _clienteRepository = clienteRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task Criar(CriarClienteDTO dto)
    {
        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Email = dto.Email,
        };

        await _clienteRepository.Criar(cliente);
    }

    public async Task<Cliente> Atualizar(Cliente cliente) 
    {
        var clienteExistente = await _clienteRepository.BuscarPorId(cliente.Id);

        if (clienteExistente == null) 
        {
            return null;
        }

        clienteExistente.Nome = cliente.Nome;
        clienteExistente.Email = cliente.Email;

        await _clienteRepository.Atualizar(clienteExistente);

        return (clienteExistente);
    }

    public async Task<List<Cliente>> Listar()
    {
        return await _clienteRepository.Listar();
    }

    public async Task<Cliente> BuscarPorId(int id)
    {
        var cliente = await _clienteRepository.BuscarPorId(id);

        if (cliente == null)
        {
            return null;
        }

        return new Cliente
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
        };
    }

    public async Task<(bool Sucesso, string? Erro)> Deletar(int id)
    {
        var cliente = await _clienteRepository.BuscarPorId(id);

        if (cliente == null)
        {
            return (false, "Cliente não encontrado");
        }

        var possuiPedidos = await _pedidoRepository.ExistePedidoComCliente(id);

        if (possuiPedidos)
        {
            return (false, "Cliente está vinculado a pedidos");
        }

        await _clienteRepository.Deletar(id);

        return (true, null);
    }
}
