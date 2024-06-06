using System.Security.Claims;
using Bogus;
using Bogus.Extensions.Brazil;
using Grpc.Core;
using GsServer.Models;

using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class FakerRpcService : FakerService.FakerServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<FakerRpcService> _logger;
  public FakerRpcService(ILogger<FakerRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GenerateFakeCustomersResponse> GenerateFakeCustomersAsync(GenerateFakeCustomersRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    Faker<Models.Person> PersonFaker = new Faker<Models.Person>("pt_BR")
      .RuleFor(x => x.PersonId, Faker => Ulid.NewUlid())
      .RuleFor(x => x.FullName, Faker => Faker.Name.FullName())
      .RuleFor(x => x.MobilePhoneNumber, Faker => Faker.Phone.PhoneNumber("(##) #####-####"))
      .RuleFor(x => x.BirthDate, Faker => Faker.Date.Past(Faker.Random.Number(21, 70), new DateTime(2025, 1, 1)).ToString("dd/MM/yyyy"))
      .RuleFor(x => x.Cpf, Faker => Faker.Person.Cpf())
      .RuleFor(x => x.CreatedBy, Ulid.Parse(UserId));

    Faker<Models.Dependent> DependentFaker = new Faker<Models.Dependent>("pt_BR")
      .RuleFor(x => x.DependentId, Faker => Ulid.NewUlid())
      .RuleFor(x => x.FullName, Faker => Faker.Name.FullName())
      .RuleFor(x => x.BirthDate, Faker => Faker.Date.Past(Faker.Random.Number(4, 16), new DateTime(2025, 1, 1)).ToString("dd/MM/yyyy"))
      .RuleFor(x => x.CreatedBy, Ulid.Parse(UserId));

    Faker<Customer> CustomerFaker = new Faker<Customer>("pt_BR")
      .RuleFor(x => x.CustomerId, Faker => Ulid.NewUlid())
      .RuleFor(x => x.Person, Faker => PersonFaker.Generate(1).FirstOrDefault())
      .RuleFor(x => x.Dependents, Faker => DependentFaker.Generate(Faker.Random.Number(0, 3)))
      .RuleFor(x => x.BillingAddress, Faker => Faker.Address.FullAddress())
      .RuleFor(x => x.AdditionalInformation, Faker => "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris ac mauris ac odio ornare congue vitae at ipsum. Donec semper pulvinar libero sed viverra. Maecenas viverra, massa a pellentesque varius, dui nisi elementum metus, nec placerat.")
      .RuleFor(x => x.CreatedBy, Ulid.Parse(UserId));

    List<Customer> Customers = CustomerFaker.Generate(request.Quantity);

    _dbContext.Customers.AddRange(Customers);
    await _dbContext.SaveChangesAsync();

    return new GenerateFakeCustomersResponse
    {
      Customers = { Customers.Select(Customer => Customer.ToGetById()) },
    };
  }
}