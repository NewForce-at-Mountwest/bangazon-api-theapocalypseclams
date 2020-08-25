using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class CustomerTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;

        public CustomerTest(DatabaseFixture fixture) => this.fixture = fixture;
        [Fact]
        public async Task Test_Get_All_Customers()
        {

            using (var client = new APIClientProvider().Client)
            {
                /*
                    ARRANGE
                */


                /*
                    ACT
                */
                var response = await client.GetAsync("/api/customers");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customerList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_One_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync($"/api/customers/{fixture.TestCustomer.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Customer singleCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                //Assert.Equal(fixture.TestCoffee.Title, singleCoffee.Title);

            }
        }

        [Fact]
        public async Task Create_One_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Customer newCustomer = new Customer()
                {
                    FirstName = "Test Customer",
                    LastName = "Tester 2",
                    CreationDate = new DateTime(2008, 3, 1, 7, 0, 0),
                    LastActiveDate = new DateTime(2008, 3, 1, 7, 0, 0)
                };

                string jsonCoffee = JsonConvert.SerializeObject(newCustomer);

                // Act
                HttpResponseMessage response = await client.PostAsync("/api/customers",
                    new StringContent(jsonCoffee, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                Customer customerResponse = JsonConvert.DeserializeObject<Customer>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(customerResponse.FirstName, newCustomer.FirstName);

            }
        }

        [Fact]
        public async Task Edit_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                Customer editedCustomer = new Customer()
                {
                    FirstName = "Edited First Name",
                    LastName = "Edited Last Name",
                    CreationDate = new DateTime(2008, 3, 1, 7, 0, 0),
                    LastActiveDate = new DateTime(2008, 3, 1, 7, 0, 0)
                };

                // Act
                string jsonCustomer = JsonConvert.SerializeObject(editedCustomer);
                HttpResponseMessage response = await client.PutAsync($"/api/customers/{fixture.TestCustomer.Id}",
                    new StringContent(jsonCustomer, Encoding.UTF8, "application/json"));

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


            }

        }
    }
}
