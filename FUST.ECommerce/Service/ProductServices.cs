using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FUST.ECommerce.Service
{
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
                using (var command = new MySqlCommand("SELECT * FROM prodotti", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("nome"),
                                Description = reader.IsDBNull("descrizione") ? null : reader.GetString("descrizione"),
                                Price = reader.GetDecimal("prezzo"),
                                CategoryId = reader.GetInt32("categoria_id")
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
                using (var command = new MySqlCommand("SELECT * FROM prodotti WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("nome"),
                                Description = reader.IsDBNull("descrizione") ? null : reader.GetString("descrizione"),
                                Price = reader.GetDecimal("prezzo"),
                                CategoryId = reader.GetInt32("categoria_id")
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
                using (var command = new MySqlCommand("INSERT INTO prodotti (nome, descrizione, prezzo, categoria_id) VALUES (@Nome, @Descrizione, @Prezzo, @CategoriaId)", connection))
                {
                    command.Parameters.AddWithValue("@Nome", product.Name);
                    command.Parameters.AddWithValue("@Descrizione", product.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Prezzo", product.Price);
                    command.Parameters.AddWithValue("@CategoriaId", product.CategoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("UPDATE prodotti SET nome = @Nome, descrizione = @Descrizione, prezzo = @Prezzo, categoria_id = @CategoriaId WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue("@Nome", product.Name);
                    command.Parameters.AddWithValue("@Descrizione", product.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Prezzo", product.Price);
                    command.Parameters.AddWithValue("@CategoriaId", product.CategoryId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("DELETE FROM prodotti WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
