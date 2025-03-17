using FUST.ECommerce.Data;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql;

public class CategoryService
{
    private readonly string _connectionString;

    public CategoryService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = new List<Category>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("SELECT * FROM Categories", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description")
                        });
                    }
                }
            }
        }
        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        Category? category = null;
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("SELECT * FROM Categories WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        category = new Category
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description")
                        };
                    }
                }
            }
        }
        return category;
    }

    public async Task AddCategoryAsync(Category category)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)", connection))
            {
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Description", category.Description);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Description", category.Description);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteCategoryAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("DELETE FROM Categories WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
