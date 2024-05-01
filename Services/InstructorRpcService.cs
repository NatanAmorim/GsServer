using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class InstructorRpcService : InstructorService.InstructorServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<InstructorRpcService> _logger;
  private readonly IMapper _mapper;
  public InstructorRpcService(
      ILogger<InstructorRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public override async Task<GetPaginatedInstructorsResponse> GetPaginatedAsync(GetPaginatedInstructorsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Instructor).Name,
      request.Cursor
    );

    IQueryable<GetInstructorByIdResponse> Query = _dbContext.Instructors.Select(
      Instructor => _mapper.Map<GetInstructorByIdResponse>(Instructor)
    );

    List<GetInstructorByIdResponse> Instructors = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Instructors = await Query
      .Where(x => x.InstructorId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
      .Take(20)
      .ToListAsync();

    GetPaginatedInstructorsResponse response = new();

    response.Instructors.AddRange(Instructors);
    if (Instructors.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Instructors[^1].InstructorId;
    }

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Instructor).Name
    );
    return response;
  }

  public override async Task<GetInstructorByIdResponse> GetByIdAsync(GetInstructorByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Instructor).Name,
      request.InstructorId
    );

    Instructor? Instructor = await _dbContext.Instructors.FindAsync(request.InstructorId);

    if (Instructor is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Instructor).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.InstructorId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Instructor).Name
    );

    return _mapper.Map<GetInstructorByIdResponse>(Instructor);
  }

  public override async Task<CreateInstructorResponse> PostAsync(CreateInstructorRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Instructor).Name
    );

    Instructor Instructor = _mapper.Map<Instructor>(request);
    Instructor.CreatedBy = Ulid.Parse(UserId);

    // TODO
    // var Instructor = new Instructor
    // {
    //   Person = new Person
    //   {
    //     Name = request.Person.Name,
    //     MobilePhoneNumber = request.Person.MobilePhoneNumber,
    //     BirthDate = request.Person.BirthDate,
    //     Cpf = request.Person.Cpf,
    //     Cin = request.Person.Cin,
    //     CreatedBy = UserId,
    //   },
    //   CreatedBy = UserId,
    // };

    await _dbContext.AddAsync(Instructor);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Instructor).Name,
      Instructor.InstructorId
    );

    return new CreateInstructorResponse();
  }

  public override Task<UpdateInstructorResponse> PutAsync(UpdateInstructorRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Instructor).Name,
      request.InstructorId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Instructor).Name
    );

    throw new NotImplementedException();

    // TODO
    // InstructorModel? Instructor = await _dbContext.Instructors.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Instructor is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Instructor.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateInstructorResponse();
  }

  public override async Task<DeleteInstructorResponse> DeleteAsync(DeleteInstructorRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Instructor).Name,
        request.InstructorId
      );

    Instructor? Instructor = await _dbContext.Instructors.FindAsync(request.InstructorId);

    if (Instructor is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Instructor).Name,
        request.InstructorId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.InstructorId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Instructors.Remove(Instructor);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Instructor).Name
        );

    return new DeleteInstructorResponse();
  }
}
