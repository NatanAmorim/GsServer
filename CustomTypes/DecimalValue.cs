namespace gs_server.Protobufs;

// Source: https://learn.microsoft.com/en-us/aspnet/core/grpc/protobuf?view=aspnetcore-8.0#decimals
// Source: https://github.com/dotnet-architecture/eBooks/blob/1ed30275281b9060964fcb2a4c363fe7797fe3f3/current/grpc-for-wcf-developers/gRPC-for-WCF-Developers.pdf#decimals
// Source: https://github.com/grpc/grpc-dotnet/issues/424
public partial class DecimalValue
{
  private const decimal NanoFactor = 1_000_000_000;
  public DecimalValue(long units, int nanos)
  {
    Units = units;
    Nanos = nanos;
  }

  public static implicit operator decimal(DecimalValue grpcDecimal)
  {
    return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
  }

  public static implicit operator DecimalValue(decimal value)
  {
    var units = decimal.ToInt64(value);
    var nanos = decimal.ToInt32((value - units) * NanoFactor);
    return new DecimalValue(units, nanos);
  }
}