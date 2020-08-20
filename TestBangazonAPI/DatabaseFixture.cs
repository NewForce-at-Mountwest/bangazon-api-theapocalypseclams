using BangazonAPI.Models;
using System;
using System.Data.SqlClient;

namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {

        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";

        public Customer TestCustomer { get; set; }

        public DatabaseFixture()
        {

            Customer newCustomer = new Customer
            {
                FirstName = "Test",
                LastName = "Customer",
                CreationDate = new DateTime(2008, 3, 1, 7, 0, 0),
                LastActiveDate = new DateTime(2008, 3, 1, 7, 0, 0)
            };

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newCustomer.FirstName}', '{newCustomer.LastName}', '{newCustomer.CreationDate}', '{newCustomer.LastActiveDate}')";


                    int newId = (int)cmd.ExecuteScalar();

                    newCustomer.Id = newId;

                    TestCustomer = newCustomer;
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
                    cmd.CommandText = @$"DELETE FROM Customer WHERE LastName ='Customer'";

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
