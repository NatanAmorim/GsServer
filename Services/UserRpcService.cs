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

    IQueryable<GetUserByIdResponse> Query = _dbContext.Users.Select(
      User => User.ToGetById()
    );

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    List<GetUserByIdResponse> Users = await Query
      .Where(x => x.UserId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
      .Take(20)
      .ToListAsync();

    GetPaginatedUsersResponse response = new();

    response.Users.AddRange(Users);
    if (Users.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Users[^1].UserId;
    }

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

  public override async Task<UpdateUserResponse> PutAsync(UpdateUserRequest request, ServerCallContext context)
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

    return new UpdateUserResponse();
  }

  public override async Task<DeleteUserResponse> DeleteAsync(DeleteUserRequest request, ServerCallContext context)
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

    return new DeleteUserResponse();
  }
}
