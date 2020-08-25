using BangazonAPI.Models;
using System;
using System.Data.SqlClient;
namespace TestBangazonAPI
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string ConnectionString = @$"Server=localhost\SQLEXPRESS;Database=BangazonAPI;Trusted_Connection=True;";
        public Product TestProduct { get; set; }
        public Product TestEditProduct { get; set; }
        public PaymentType AddDelTestType { get; set; }
        public PaymentType EditType { get; set; }
        public Customer TestCustomer { get; set; }

        public DatabaseFixture()
        {
            Product newProduct = new Product
            {
                ProductTypeId = 1,
                CustomerId = 1,
                Price = 0.99m,
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

            //This Product is to test the Edit method
            Product newEditProduct = new Product
            {
                ProductTypeId = 2,
                CustomerId = 2,
                Price = 1.99m,
                Title = "Test Product Edit",
                Description = "Test Product Description for Edit",
                Quantity = 1
            };
            //Start of PaymentType
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
                    cmd.CommandText = @$"INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newEditProduct.ProductTypeId}', '{newEditProduct.CustomerId}','{newEditProduct.Price}', '{newEditProduct.Title}','{newEditProduct.Description}', '{newEditProduct.Quantity}')";
                    int newId = (int)cmd.ExecuteScalar();
                    newEditProduct.Id = newId;
                    TestEditProduct = newEditProduct;
                    //PaymentType CommandText
                    cmd.CommandText = @$"INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES ('{newType.AcctNumber}', '{newType.Name}', '{newType.CustomerId}')";
                    newId = (int)cmd.ExecuteScalar();

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
                    cmd.CommandText = @$"DELETE FROM Product WHERE Title='Test Product'";
                    cmd.CommandText = @$"DELETE FROM PaymentType WHERE Name like '%integration test payment type%'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @$"DELETE FROM Customer WHERE LastName ='Customer'";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
