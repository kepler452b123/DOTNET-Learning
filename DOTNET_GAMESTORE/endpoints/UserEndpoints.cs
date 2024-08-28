using Dapper;
using DOTNET_GAMESTORE.dtos;
using DOTNET_GAMESTORE.db_models;
using DOTNET_GAMESTORE.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;

namespace DOTNET_GAMESTORE.endpoints;

public static class UserEndpoints{
    public static RouteGroupBuilder GetEndpoints(this WebApplication app ){
        var group = app.MapGroup("users");

        group.MapGet("/", async (SqlPipeline sqlpipe) => {
            using var connection = sqlpipe.Create();

            const string getALL = "SELECT * FROM Users;";

            var users = await connection.QueryAsync<User>(getALL);

            return Results.Ok(users);
        });

        //Use WithName as we have to reference the route elsewhere
        group.MapGet("/{id}", async (int id, SqlPipeline sqlpipe) => {
            using var connection = sqlpipe.Create();
            string getId = "SELECT * FROM Users WHERE Users.Id = @UserId";

            //Use QuerySingleOrDefaultAsync as it returns null if nothing is found. Otherwise, even if a record doesn't exist, it would return
            //OK with a blank payload
            var user = await connection.QuerySingleOrDefaultAsync<User>(getId, new {UserId = id});

            return user is null ? Results.NotFound(): Results.Ok(user);
            }
            ).WithName("GetUser");

        group.MapPost("/", async (CreateUserDTO newUser, SqlPipeline sqlpipe) => 
        {
            using var connection = sqlpipe.Create();

            string getCount = "SELECT ISNULL(MAX(Id), 0) FROM Users";

            var lastId = (await connection.QueryAsync<int>(getCount)).FirstOrDefault();

            UserDTO user = new (lastId + 1, newUser.Name, newUser.Status, newUser.Email, newUser.JoinDate );

            
            string insert = "INSERT INTO Users (Name, Status, Email, JoinDate) VALUES ( @Name, @Status, @Email, @JoinDate )";
            await connection.ExecuteScalarAsync(insert, new {Name = user.Name, Status = user.Status, Email = newUser.Email, JoinDate = newUser.JoinDate.ToDateTime(new TimeOnly(0,0))});
            //Give the name of the route, the parameters for the route, and the object. Required due to REST principles of giving the location of the new
            //resource
            return Results.CreatedAtRoute("GetUser", new {id = user.Id}, user);
        }
        );

        group.MapPut("/{id}", async (UpdateUserDTO newUser, int id, SqlPipeline sqlpipe) => 
            {
                using var connection = sqlpipe.Create();

                string find = "SELECT * FROM Users WHERE Users.Id = @Id";
                var index = (await connection.QueryAsync<int>(find, new {Id = id})).FirstOrDefault();
                //Check if index is 0 because that will be the default value returned for an int if an entry is not found
                if (index == 0){
                    return Results.NotFound();
                }

                string update = "UPDATE Users SET Name = @Name, Status = @Status, Email = @Email, JoinDate = @JoinDate WHERE Users.Id = @Id";
                UserDTO user = new (id, newUser.Name, newUser.Status, newUser.Email, newUser.JoinDate);
                //Use FindIndex instead of games[id], as we don't know if the indices correspond directly to the IDs
                
                await connection.ExecuteScalarAsync(update, new {Name = user.Name, Status = user.Status, Email = user.Email, JoinDate = user.JoinDate.ToDateTime(new TimeOnly(0,0)), Id = id});
                return Results.NoContent();
            }
        );

        group.MapDelete("/{id}", async (int id, SqlPipeline sqlpipe) => {
            using var connection = sqlpipe.Create();

            string find = "SELECT * FROM Users WHERE Users.Id = @Id";
            var index = (await connection.QueryAsync<int>(find, new {Id = id})).FirstOrDefault();

            //Check if index is 0 because that will be the default value returned for an int if an entry is not found
            if (index == 0){
                return Results.NotFound();
            }

            string delete = "DELETE FROM Users WHERE Users.Id = @Id";
            await connection.ExecuteScalarAsync(delete, new {Id = id});

            return Results.NoContent();
        });


        return group;
    }
}
