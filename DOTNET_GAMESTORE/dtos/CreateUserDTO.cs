using System.ComponentModel.DataAnnotations;

namespace DOTNET_GAMESTORE.dtos;

public record class CreateUserDTO(int Id, [Required][StringLength(50)] string Name, [Required] string Status, [Required]string Email, [Required]DateOnly JoinDate)
{

}