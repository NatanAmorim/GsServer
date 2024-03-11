using System.Security.Claims;
using Grpc.Core;
using gs_server.Models;
using gs_server.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace gs_server.Services;

[Authorize]
public class UserRpcService : UserService.UserServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<UserRpcService> _logger;
  public UserRpcService(ILogger<UserRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedUsersResponse> GetPaginated(GetPaginatedUsersRequest request, ServerCallContext context)
  {
    _logger.LogInformation("Listing Users");
    IQueryable<GetUserByIdResponse> Query = _dbContext.Users.Select(
      User => new GetUserByIdResponse
      {
        UserId = User.UserId,
        Email = User.Email,
        Role = User.Role,
      }
    );

    List<GetUserByIdResponse> Users = [];

    // If cursor is bigger than the size of the collection you will get the following error
    // ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    // My solution (hack) will be on Front-end:
    // if (response.collection.count < 20) don't make GetPaginated request anymore.
    Users = await Query
      .Where(x => x.UserId > request.Cursor)
      .Take(20)
      .ToListAsync();

    GetPaginatedUsersResponse response = new();

    response.Users.AddRange(Users);
    // Id of the last element of the list same as `Users[Users.Count - 1].Id`
    response.NextCursor = Users[^1].UserId;

    _logger.LogInformation("Users have been listed successfully");
    return response;
  }

  public override async Task<GetUserByIdResponse> GetById(GetUserByIdRequest request, ServerCallContext context)
  {
    _logger.LogInformation(
      "Searching for User with ID {Id}",
      request.UserId
    );
    UserModel? User = await _dbContext.Users.FindAsync(request.UserId);

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
    return new GetUserByIdResponse
    {
      UserId = User.UserId,
      Email = User.Email,
      Role = User.Role,
    };
  }

  public override async Task<UpdateUserResponse> Put(UpdateUserRequest request, ServerCallContext context)
  {
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation("Updating User with ID {Id}", UserId);
    UserModel? User = await _dbContext.Users.FindAsync(UserId);

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

  public override async Task<DeleteUserResponse> Delete(DeleteUserRequest request, ServerCallContext context)
  {
    _logger.LogInformation("Deleting User with ID {Id}", request.UserId);
    UserModel? User = await _dbContext.Users.FindAsync(request.UserId);

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
