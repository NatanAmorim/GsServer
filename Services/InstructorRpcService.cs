using Grpc.Core;
using GsServer.Protobufs;

namespace GsServer.Services;

public class InstructorRpcService : InstructorService.InstructorServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<InstructorRpcService> _logger;
  public InstructorRpcService(ILogger<InstructorRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetPaginatedInstructorsResponse> GetPaginated(GetPaginatedInstructorsRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetInstructorByIdResponse> GetById(GetInstructorByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetAllInstructorsOptionsResponse> GetAllOptions(GetAllInstructorsOptionsRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateInstructorResponse> Post(CreateInstructorRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateInstructorResponse> Put(UpdateInstructorRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteInstructorResponse> Delete(DeleteInstructorRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
