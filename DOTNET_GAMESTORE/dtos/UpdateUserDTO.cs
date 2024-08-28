using System.ComponentModel.DataAnnotations;
namespace DOTNET_GAMESTORE.dtos;

public record class UpdateUserDTO([Required][StringLength(50)] string Name, [Required] string Status, [Required] string Email, [Required] DateOnly JoinDate)
{

}