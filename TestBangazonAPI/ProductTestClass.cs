using BangazonAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace TestBangazonAPI
{
    [Collection("Database collection")]
    public class ProductTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public ProductTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/product");
                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var productList = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(productList.Count > 0);
            }
        }
        [Fact]
        public async Task Create_One_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Product newProduct = new Product()
                {
                    ProductTypeId = 1,
                    CustomerId = 1,
                    Price = .99m,
                    Title = "Test Product",
                    Description = "This is a test product",
                    Quantity = 1
                };
                string jsonProduct = JsonConvert.SerializeObject(newProduct);
                // Act
                HttpResponseMessage response = await client.PostAsync("/api/product",
                    new StringContent(jsonProduct, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                Product productResponse = JsonConvert.DeserializeObject<Product>(responseBody);
                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(productResponse.Title, newProduct.Title);
            }
        }
        [Fact]
        public async Task Test_One_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync($"/api/product/{fixture.TestProduct.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Product singleProduct = JsonConvert.DeserializeObject<Product>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //Assert.Equal(fixture.TestProduct.Title, singleProduct.Title);
            }
        }
        [Fact]
        public async Task Edit_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Product editedProduct = new Product()
                {
                    ProductTypeId = 2,
                    CustomerId = 2,
                    Price = .99m,
                    Title = "Test Product",
                    Description = "This is a test for an edited product",
                    Quantity = 1
                };
                // Act
                string jsonProduct = JsonConvert.SerializeObject(editedProduct);
                HttpResponseMessage response = await client.PutAsync($"/api/product/{fixture.TestEditProduct.Id}",
                    new StringContent(jsonProduct, Encoding.UTF8, "application/json"));
                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
        [Fact]
        public async Task Delete_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.DeleteAsync($"/api/product/{fixture.TestProduct.Id}");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }
    }
}