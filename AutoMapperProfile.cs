using AutoMapper;
using gs_server.Models.Professores;
using gs_server.Dtos.Professores;
using gs_server.Models.Usuarios;
using gs_server.Dtos.Usuarios;
using gs_server.Models.Produtos;
using gs_server.Dtos.Produtos;

namespace gs_server.Models;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<CreateUsuarioDto, Usuario>();
    CreateMap<Usuario, ResponseUsuarioDto>();
    CreateMap<Usuario, ResponseLeanUsuarioDto>();

    CreateMap<CreateProfessorDto, Professor>();
    CreateMap<Professor, ResponseProfessorDto>();
    CreateMap<Professor, ResponseLeanProfessorDto>();

    CreateMap<CreateProdutoDto, Produto>();
    CreateMap<Produto, ResponseProdutoDto>();
    CreateMap<CreateProdutoVarianteDto, ProdutoVariante>();
    CreateMap<ProdutoVariante, ResponseProdutoVarianteDto>();
  }
}