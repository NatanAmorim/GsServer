using System.ComponentModel.DataAnnotations;

namespace gs_server.Dtos.Auth;

public class NovaSenhaDto
{
  [Required]
  public required string SenhaAntiga { get; set; }
  [Required]
  public required string SenhaNova { get; set; }
}