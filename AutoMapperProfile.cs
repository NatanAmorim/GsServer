using AutoMapper;
using gs_server.Models.Professores;
using gs_server.Dtos.Professores;

namespace gs_server.Models;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<CreateProfessorDto, Professor>();
    CreateMap<Professor, ResponseProfessorDto>();
    // CreateMap<Professor, ResponseDatatableProfessorDto>();
    // CreateMap<Professor, ResponseDropdownProfessorDto>();
  }
}