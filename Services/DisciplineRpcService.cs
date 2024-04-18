// using System.Security.Claims;
// using AutoMapper;
// using Grpc.Core;
// using GsServer.Models;
// using GsServer.Protobufs;

// namespace GsServer.Services;

// public class DisciplineRpcService : DisciplineService.DisciplineServiceBase
// {
//   private readonly DatabaseContext _dbContext;
//   private readonly ILogger<DisciplineRpcService> _logger;
//   private readonly IMapper _mapper;
//   public DisciplineRpcService(
//       ILogger<DisciplineRpcService> logger,
//       DatabaseContext dbContext,
//       IMapper mapper
//     )
//   {
//     _logger = logger;
//     _dbContext = dbContext;
//     _mapper = mapper;
//   }

//   public override async Task<GetPaginatedDisciplinesResponse> GetPaginatedAsync(GetPaginatedDisciplinesRequest request, ServerCallContext context)
//   {
//     string RequestTracerId = context.GetHttpContext().TraceIdentifier;
//     int UserId = int.Parse(
//       context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
//     );
//     _logger.LogInformation(
//       "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
//       RequestTracerId,
//       UserId,
//       typeof(Discipline).Name,
//       request.Cursor
//     );

//     IQueryable<GetDisciplineByIdResponse> Query = _dbContext.Disciplines.Select(
//       Discipline => _mapper.Map<GetDisciplineByIdResponse>(Discipline)
//     );

//     // TODO
//     // IQueryable<GetDisciplineByIdResponse> Query = _dbContext.Disciplines.Select(
//     //   Discipline => new GetDisciplineByIdResponse
//     //   {
//     //     TODO
//     //   }
//     // );

//     List<GetDisciplineByIdResponse> Disciplines = [];

//     /// If cursor is bigger than the size of the collection you will get the following error
//     /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
//     Disciplines = await Query
//       .Where(x => x.DisciplinePk > request.Cursor)
//       .Take(20)
//       .ToListAsync();

//     GetPaginatedDisciplinesResponse response = new();

//     response.Disciplines.AddRange(Disciplines);
//     if (Disciplines.Count < 20)
//     {
//       /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
//       response.NextCursor = null;
//     }
//     else
//     {
//       /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
//       response.NextCursor = Disciplines[^1].DisciplinePk;
//     }

//     _logger.LogInformation(
//       "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
//       RequestTracerId,
//       typeof(Discipline).Name
//     );
//     return response;
//   }

//   public override async Task<GetDisciplineByIdResponse> GetByIdAsync(GetDisciplineByIdRequest request, ServerCallContext context)
//   {
//     string RequestTracerId = context.GetHttpContext().TraceIdentifier;
//     int UserId = int.Parse(
//       context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
//     );

//     _logger.LogInformation(
//       "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
//       RequestTracerId,
//       UserId,
//       typeof(Discipline).Name,
//       request.DisciplinePk
//     );

//     Discipline? Discipline = await _dbContext.Disciplines.FindAsync(request.DisciplinePk);

//     if (Discipline is null)
//     {
//       _logger.LogWarning(
//         "({TraceIdentifier}) record ({RecordType}) not found",
//         RequestTracerId,
//         typeof(Discipline).Name
//       );
//       throw new RpcException(new Status(
//         StatusCode.NotFound, $"Nenhum produto com ID {request.DisciplinePk}"
//       ));
//     }

//     _logger.LogInformation(
//       "({TraceIdentifier}) record ({RecordType}) accessed successfully",
//       RequestTracerId,
//       typeof(Discipline).Name
//     );
//     // Create a mapping between the two enums
//     Dictionary<int, CSharpDayOfWeek> protobufToCSharpMapping = new Dictionary<int, CSharpDayOfWeek>
//     {
//       { Protobufs.DayOfWeek.SUNDAY, DayOfWeek.Sunday },
//       { ProtobufDayOfWeek.MONDAY, DayOfWeek.Monday },
//       { ProtobufDayOfWeek.TUESDAY, DayOfWeek.Tuesday },
//       { ProtobufDayOfWeek.WEDNESDAY, DayOfWeek.Wednesday },
//       { ProtobufDayOfWeek.THURSDAY, DayOfWeek.Thursday },
//       { ProtobufDayOfWeek.FRIDAY, DayOfWeek.Friday },
//       { ProtobufDayOfWeek.SATURDAY, DayOfWeek.Saturday }
//     };
//     List<DayOfWeek> csharp_list = [];
//     List<Protobufs.TimeOfDay> protobuf_list = [];
//     foreach (var day in Discipline.ClassDays)
//     {
//       csharp_list.Add((DayOfWeek)day);
//     }

//     return new GetDisciplineByIdResponse
//     {
//       DisciplinePk = Discipline.DisciplinePk,
//       Name = Discipline.Name,
//       TuitionPrice = Discipline.TuitionPrice,
//       InstructorFk = Discipline.InstructorFk,
//       StartTime = new()
//       {
//         Hour = Discipline.StartTime.Hour,
//         Minute = Discipline.StartTime.Minute,
//       },
//       EndTime = new()
//       {
//         Hour = Discipline.EndTime.Hour,
//         Minute = Discipline.EndTime.Minute,
//       },
//       ClassDays = {
//         Discipline.ClassDays.Select(
//           ClassDay => ClassDay.
//         ).ToList(),
//       },
//       IsActive = Discipline.IsActive,
//     };
//   }

//   public override async Task<CreateDisciplineResponse> PostAsync(CreateDisciplineRequest request, ServerCallContext context)
//   {
//     string RequestTracerId = context.GetHttpContext().TraceIdentifier;
//     int UserId = int.Parse(
//       context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
//     );

//     _logger.LogInformation(
//       "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
//       RequestTracerId,
//       UserId,
//       typeof(Discipline).Name
//     );



//     var Discipline = new Discipline
//     {
//       Name = request.Name,
//       TuitionPrice = request.TuitionPrice,
//       InstructorFk = request.InstructorFk,
//       StartTime = new(
//         request.EndTime.Hour,
//         request.EndTime.Minute
//       ),
//       EndTime = new(
//         request.EndTime.Hour,
//         request.EndTime.Minute
//       ),
//       ClassDays = request.ClassDays,
//       CreatedBy = UserId,
//     };

//     await _dbContext.AddAsync(Discipline);
//     await _dbContext.SaveChangesAsync();

//     _logger.LogInformation(
//       "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
//       RequestTracerId,
//       typeof(Discipline).Name,
//       Discipline.DisciplinePk
//     );

//     return new CreateDisciplineResponse();
//   }

//   public override Task<UpdateDisciplineResponse> PutAsync(UpdateDisciplineRequest request, ServerCallContext context)
//   {
//     string RequestTracerId = context.GetHttpContext().TraceIdentifier;
//     int UserId = int.Parse(
//       context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
//     );
//     _logger.LogInformation(
//       "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
//       RequestTracerId,
//       UserId,
//       typeof(Discipline).Name,
//       request.DisciplinePk
//     );

//     _logger.LogInformation(
//       "({TraceIdentifier}) record ({RecordType}) updated successfully",
//       RequestTracerId,
//       typeof(Discipline).Name
//     );

//     throw new NotImplementedException();

//     // TODO
//     // if (request.Id <= 0)
//     //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

//     // Discipline? Discipline = await _dbContext.Disciplines.FirstOrDefaultAsync(x => x.Id == request.Id);
//     // if (Discipline is null)
//     // {
//     //   throw new RpcException(new Status(
//     //     StatusCode.NotFound, $"registro não encontrado"
//     //   ));
//     // }

//     // Discipline.Name = request.Name;
//     // // TODO Add Another fields

//     // await _dbContext.SaveChangesAsync();
//     // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
//     // return new UpdateDisciplineResponse();
//   }

//   public override async Task<DeleteDisciplineResponse> DeleteAsync(DeleteDisciplineRequest request, ServerCallContext context)
//   {
//     string RequestTracerId = context.GetHttpContext().TraceIdentifier;
//     int UserId = int.Parse(
//       context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
//     );
//     _logger.LogInformation(
//       "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
//       RequestTracerId,
//       UserId,
//       typeof(Discipline).Name,
//       request.DisciplinePk
//     );

//     Discipline? Discipline = await _dbContext.Disciplines.FindAsync(request.DisciplinePk);

//     if (Discipline is null)
//     {
//       _logger.LogWarning(
//         "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
//         RequestTracerId,
//         typeof(Discipline).Name,
//         request.DisciplinePk
//       );
//       throw new RpcException(new Status(
//         StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.DisciplinePk}"
//       ));
//     }

//     /// TODO check if record is being used before deleting it use something like PK or FK

//     _dbContext.Disciplines.Remove(Discipline);
//     await _dbContext.SaveChangesAsync();

//     _logger.LogInformation(
//       "({TraceIdentifier}) record ({RecordType}) deleted successfully",
//       RequestTracerId,
//       typeof(Discipline).Name
//     );

//     return new DeleteDisciplineResponse();
//   }
// }
