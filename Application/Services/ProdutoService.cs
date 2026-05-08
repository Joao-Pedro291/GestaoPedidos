using Domain.Entities;
using Domain.Interfaces;

public class ProdutoService
{
    private readonly IProdutoRepository _produtoRepo;

    public ProdutoService(IProdutoRepository produtoRepo)
    {
        _produtoRepo = produtoRepo;
    }

    public async Task Criar(CriarProdutoDTO dto)
    {
        var produto = new Produto
        {
            Nome = dto.Nome,
            Estoque = dto.Estoque,
            Valor = dto.Valor,
            
        };

        await _produtoRepo.Criar(produto);
    }

    public async Task<List<ProdutoListagemDTO>> Listar() 
    {
        var produtos = await _produtoRepo.Listar();

        return produtos.Select(p => new ProdutoListagemDTO
        {
            Id = p.Id,
            Nome = p.Nome,
            Valor = p.Valor,
            Estoque = p.Id,
        }).ToList();
    }
}