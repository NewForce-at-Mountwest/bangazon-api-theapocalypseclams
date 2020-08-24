using BangazonAPI.Models;
using System;
using System.Data.SqlClient;

namespace TestBangazonAPI
{

    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";
        public PaymentType AddDelTestType { get; set; }
        public PaymentType EditType { get; set; }

        public DatabaseFixture()
        {
            PaymentType newType = new PaymentType
            {
                AcctNumber = "9876543210",
                Name = "integration test payment type",
                CustomerId = 2
            };

            PaymentType editType = new PaymentType
            {
                AcctNumber = "6789012345",
                Name = "integration test payment type",
                CustomerId = 2
            };

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @$"INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newType.AcctNumber}', '{newType.Name}', '{newType.CustomerId}')";
                    int newId = (int)cmd.ExecuteScalar();

                    newType.Id = newId;

                    AddDelTestType = newType;

                    cmd.CommandText = @$"INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{editType.AcctNumber}', '{editType.Name}', '{editType.CustomerId}')";
                    int newEditId = (int)cmd.ExecuteScalar();

                    editType.Id = newEditId;

                    EditType = editType;

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
                    cmd.CommandText = @$"DELETE FROM PaymentType WHERE Name like '%integration test payment type%'";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
