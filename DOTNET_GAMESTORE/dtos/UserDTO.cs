namespace DOTNET_GAMESTORE.dtos;

public record class UserDTO(
    int Id, 
    string Name, 
    string Status, 
    string Email, 
    DateOnly JoinDate)
{

}