using BangazonAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";
        public Product TestProduct { get; set; }
        public DatabaseFixture()
        {
            Product newProduct = new Product
            {
                ProductTypeId = 1,
                CustomerId = 1,
                Price = 0.99,
                Title = "Test Product",
                Description = "Test Product Description",
                Quantity = 1
            };
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newProduct.ProductTypeId}', '{newProduct.CustomerId}','{newProduct.Price}', '{newProduct.Title}','{newProduct.Description}', '{newProduct.Quantity}')";
                    int newId = (int)cmd.ExecuteScalar();
                    newProduct.Id = newId;
                    TestProduct = newProduct;
                }
            }
        }
        public void Dispose()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"DELETE FROM Product WHERE Title='Test Product'";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}