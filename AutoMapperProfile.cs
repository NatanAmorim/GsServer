using AutoMapper;
using gs_server.Models.Professores;
using gs_server.Dtos.Professores;
using gs_server.Models.Usuarios;
using gs_server.Dtos.Usuarios;

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
  }
}