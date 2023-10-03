using gs_server;
using gs_server.Dtos.Produtos;
using Microsoft.AspNetCore.Mvc;
using gs_server.ControlFlow.Produtos;

namespace gs_server.Services.Produtos;

public interface IProdutoService
{
  public Task<Result<IEnumerable<ResponseProdutoDto>, ProdutoErrors>> GetAsync(int Limit, string? Query);
  public Task<Result<ResponseProdutoDto, ProdutoErrors>> FindAsync(int Id);
  public Task<Result<ResponseProdutoDto, ProdutoErrors>> PostAsync(CreateProdutoDto produtoDto);
  public Task<ProdutoErrors?> PutAsync(ResponseProdutoDto produtoDto);
  public Task<ProdutoErrors?> DeleteAsync(int Id);
}