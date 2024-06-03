using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class DisciplineRpcService : DisciplineService.DisciplineServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<DisciplineRpcService> _logger;
  public DisciplineRpcService(
      ILogger<DisciplineRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedDisciplinesResponse> GetPaginatedAsync(GetPaginatedDisciplinesRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Discipline).Name,
      request.Cursor
    );

    IQueryable<GetDisciplineByIdResponse> Query;

    if (request.Cursor is null)
    {
      Query = _dbContext.Disciplines
        .Select(Discipline => Discipline.ToGetById());
    }
    else
    {
      Query = _dbContext.Disciplines
        .Where(x => x.DisciplineId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(Discipline => Discipline.ToGetById());
    }

    List<GetDisciplineByIdResponse> Disciplines = await Query
       .Take(20)
       .ToListAsync();

    GetPaginatedDisciplinesResponse response = new();

    response.Disciplines.AddRange(Disciplines);
    response.NextCursor = Disciplines.LastOrDefault()?.DisciplineId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Discipline).Name
    );

    return response;
  }

  public override async Task<GetDisciplineByIdResponse> GetByIdAsync(GetDisciplineByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Discipline).Name,
      request.DisciplineId
    );

    Discipline? Discipline = await _dbContext.Disciplines.FindAsync(request.DisciplineId);

    if (Discipline is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Discipline).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.DisciplineId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Discipline).Name
    );

    return Discipline.ToGetById();
  }

  public override async Task<VoidValue> PostAsync(CreateDisciplineRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Discipline).Name
    );

    Discipline Discipline = Discipline.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Discipline);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Discipline).Name,
      Discipline.DisciplineId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateDisciplineRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Discipline).Name,
      request.DisciplineId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Discipline).Name
    );

    throw new NotImplementedException();

    // TODO
    // Discipline? Discipline = await _dbContext.Disciplines.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Discipline is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Discipline.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateDisciplineResponse();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteDisciplineRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Discipline).Name,
      request.DisciplineId
    );

    Discipline? Discipline = await _dbContext.Disciplines.FindAsync(request.DisciplineId);

    if (Discipline is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Discipline).Name,
        request.DisciplineId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.DisciplineId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Disciplines.Remove(Discipline);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) deleted successfully",
      RequestTracerId,
      typeof(Discipline).Name
    );

    return new VoidValue();
  }
}
