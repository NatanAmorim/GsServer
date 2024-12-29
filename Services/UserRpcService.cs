using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class UserRpcService : UserService.UserServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<UserRpcService> _logger;
  public UserRpcService(
     ILogger<UserRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedUsersResponse> GetPaginatedAsync(GetPaginatedUsersRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(User).Name,
      request.Cursor
    );

    IQueryable<GetUserByIdResponse> Query;

    if (request.Cursor is null || request.Cursor == string.Empty)
    {
      Query = _dbContext.Users
        .Select(User => User.ToGetById());
    }
    else
    {
      Query = _dbContext.Users
        .Where(x => x.UserId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(User => User.ToGetById());
    }

    List<GetUserByIdResponse> Users = await Query
      .Take(20)
      .AsNoTracking()
      .ToListAsync();

    GetPaginatedUsersResponse response = new();

    response.Users.AddRange(Users);
    response.NextCursor = Users.LastOrDefault()?.UserId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(User).Name
    );

    return response;
  }

  public override async Task<GetUserByIdResponse> GetByIdAsync(GetUserByIdRequest request, ServerCallContext context)
  {
    _logger.LogInformation(
      "Searching for User with ID {Id}",
      request.UserId
    );
    User? User = await _dbContext.Users.FindAsync(request.UserId);

    if (User is null)
    {
      _logger.LogWarning(
        "Error search User request, no User with ID {Id}",
        request.UserId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao procurar usuário, nenhum usuário com ID {request.UserId}"
      ));
    }

    _logger.LogInformation(
      "User with ID {Id} found successfully",
      request.UserId
    );

    return User.ToGetById();
  }

  public override async Task<VoidValue> PutAsync(UpdateUserRequest request, ServerCallContext context)
  {
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation("Updating User with ID {Id}", UserId);
    User? User = await _dbContext.Users.FindAsync(UserId);

    if (User is null)
    {
      _logger.LogWarning(
        "Error while updating User, no User with ID {Id}",
        UserId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao atualizar usuário, nenhum usuário com ID {UserId}"
      ));
    }

    // Attach the entity to the context in the modified state
    _dbContext.Users.Attach(User);

    User.Email = request.Email;

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "User {Id} updated successfully",
      UserId
    );

    return new VoidValue();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteUserRequest request, ServerCallContext context)
  {
    _logger.LogInformation("Deleting User with ID {Id}", request.UserId);
    User? User = await _dbContext.Users.FindAsync(request.UserId);

    if (User is null)
    {
      _logger.LogWarning(
        "Error in delete User request, no User with ID {Id}",
        request.UserId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover usuário, nenhum usuário com ID {request.UserId}"
      ));
    }

    _dbContext.Users.Remove(User);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "User deleted successfully ID {Id}",
          request.UserId
        );

    return new VoidValue();
  }
}
