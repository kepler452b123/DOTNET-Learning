using System.ComponentModel.DataAnnotations;

namespace DOTNET_GAMESTORE.db_models;

public class User{
    public int Id { get; set;}
    public string Name { get; set; }
    public string Status { get; set; }
    public string Email { get; set; }
    public DateTime JoinDate { get; set; }

    

}