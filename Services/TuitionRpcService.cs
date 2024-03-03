using Grpc.Core;
using gs_server.Protobufs;

namespace gs_server.Services;

public class TuitionRpcService : TuitionService.TuitionServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<TuitionRpcService> _logger;
  public TuitionRpcService(ILogger<TuitionRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetAllTuitionsResponse> GetAll(GetAllTuitionsRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetTuitionByIdResponse> GetById(GetTuitionByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateTuitionResponse> Post(CreateTuitionRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateTuitionResponse> Put(UpdateTuitionRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteTuitionResponse> Delete(DeleteTuitionRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
