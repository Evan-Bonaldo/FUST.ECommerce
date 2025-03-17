using FUST.ECommerce.Data;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql;

public class ProductService
{
    private readonly string _connectionString;

    public ProductService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var products = new List<Product>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("SELECT * FROM Products", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            Price = reader.GetDecimal("Price"),
                            CategoryId = reader.GetInt32("CategoryId")
                        });
                    }
                }
            }
        }
        return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        Product? product = null;
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("SELECT * FROM Products WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            Price = reader.GetDecimal("Price"),
                            CategoryId = reader.GetInt32("CategoryId")
                        };
                    }
                }
            }
        }
        return product;
    }

    public async Task AddProductAsync(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("INSERT INTO Products (Name, Description, Price, CategoryId) VALUES (@Name, @Description, @Price, @CategoryId)", connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateProductAsync(Product product)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, CategoryId = @CategoryId WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("DELETE FROM Products WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
