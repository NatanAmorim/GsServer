using System.Security.Claims;
using Grpc.Core;
using gs_server.Models;
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

  public override async Task<GetAllUsersResponse> GetAll(GetAllUsersRequest request, ServerCallContext context)
  {
    _logger.LogInformation("Listing Users");
    List<User> Users =
      await _dbContext.Users
      .ToListAsync();

    GetAllUsersResponse response = new();

    foreach (User User in Users)
    {
      response.Users.Add(new GetUserByIdResponse()
      {
        Id = User.Id,
        Email = User.Email,
        Role = User.Role,
      });
    }

    _logger.LogInformation("Users have been listed successfully");
    return response;
  }

  public override async Task<GetUserByIdResponse> GetById(GetUserByIdRequest request, ServerCallContext context)
  {
    _logger.LogInformation(
      "Searching for User with ID {Id}",
      request.Id
    );
    User? User = await _dbContext.Users.FindAsync(request.Id);

    if (User is null)
    {
      _logger.LogWarning(
        "Error search User request, no User with ID {Id}",
        request.Id
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao procurar usuário, nenhum usuário com ID {request.Id}"
      ));
    }

    _logger.LogInformation(
      "User with ID {Id} found successfully",
      request.Id
    );
    return new GetUserByIdResponse()
    {
      Id = User.Id,
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

  public override async Task<DeleteUserResponse> Delete(DeleteUserRequest request, ServerCallContext context)
  {
    _logger.LogInformation("Deleting User with ID {Id}", request.Id);
    User? User = await _dbContext.Users.FindAsync(request.Id);

    if (User is null)
    {
      _logger.LogWarning(
        "Error in delete User request, no User with ID {Id}",
        request.Id
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover usuário, nenhum usuário com ID {request.Id}"
      ));
    }

    _dbContext.Users.Remove(User);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "User deleted successfully ID {Id}",
      request.Id
    );

    return new DeleteUserResponse();
  }
}
