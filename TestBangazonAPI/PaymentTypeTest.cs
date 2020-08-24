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
    public class PaymentTypeTest : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        public PaymentTypeTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

   
        [Fact]
        public async Task Test_Get_All_PaymentTypes()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                var response = await client.GetAsync("/api/PaymentType");

                // Act
                string responseBody = await response.Content.ReadAsStringAsync();
                var typeList = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(typeList.Count > 0);
            }
        }
        [Fact]
        public async Task Create_One_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                PaymentType newType = new PaymentType()
                {
                    AcctNumber = "5432109876",
                    Name = "integration test payment type",
                    CustomerId = 2
                };
                string jsonPaymentType = JsonConvert.SerializeObject(newType);

                // Act
                HttpResponseMessage response = await client.PostAsync("/api/PaymentType",
                    new StringContent(jsonPaymentType, Encoding.UTF8, "application/json"));
                string responseBody = await response.Content.ReadAsStringAsync();
                PaymentType paymentTypeResponse = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                // Assert
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(paymentTypeResponse.Name, newType.Name);
            }
        }

 
        [Fact]
        public async Task Test_One_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync($"/api/paymenttype/{fixture.AddDelTestType.Id}");
                string responseBody = await response.Content.ReadAsStringAsync();
                PaymentType singleType = JsonConvert.DeserializeObject<PaymentType>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                
            }
        }
        [Fact]
        public async Task Edit_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                // Arrange
                PaymentType editedType = new PaymentType()
                {
                    AcctNumber = "6789012345",
                    Name = "EDITED integration test payment type",
                    CustomerId = 2
                };
                // Act
                string jsonPaymentType = JsonConvert.SerializeObject(editedType);
                HttpResponseMessage response = await client.PutAsync($"/api/paymenttype/{fixture.EditType.Id}",
                    new StringContent(jsonPaymentType, Encoding.UTF8, "application/json"));
                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
            GET section
            Verify that the PUT operation was successful
        */
                var getEditedType = await client.GetAsync($"/api/paymenttype/{fixture.EditType.Id}");
                getEditedType.EnsureSuccessStatusCode();

                string getEditedTypeBody = await getEditedType.Content.ReadAsStringAsync();
                PaymentType editType = JsonConvert.DeserializeObject<PaymentType>(getEditedTypeBody);

                Assert.Equal(HttpStatusCode.OK, getEditedType.StatusCode);
                Assert.Equal(editedType.Name, editType.Name);

            }
        }

    
        [Fact]
        public async Task Delete_PaymentType()
        {
            using (var client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.DeleteAsync($"/api/paymentType/{fixture.AddDelTestType.Id}");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

     }
}

