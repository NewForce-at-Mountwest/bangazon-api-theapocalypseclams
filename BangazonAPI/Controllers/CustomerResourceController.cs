using BangazonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomersController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Customer";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Customer> Customers = new List<Customer>();

                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        };

                        Customers.Add(customer);
                    }
                    reader.Close();

                    return Ok(Customers);
                }
            }
        }

        //[HttpGet("{id}", Name = "GetCoffee")]
        //public async Task<IActionResult> Get([FromRoute] int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                SELECT
        //                    Id, Title, BeanType
        //                FROM Coffee
        //                WHERE Id = @id";
        //            cmd.Parameters.Add(new SqlParameter("@id", id));
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            Coffee coffee = null;

        //            if (reader.Read())
        //            {
        //                coffee = new Coffee
        //                {
        //                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                    Title = reader.GetString(reader.GetOrdinal("Title")),
        //                    BeanType = reader.GetString(reader.GetOrdinal("BeanType"))
        //                };
        //            }
        //            reader.Close();

        //            return Ok(coffee);
        //        }
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Coffee coffee)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"INSERT INTO Coffee (Title, BeanType)
        //                                OUTPUT INSERTED.Id
        //                                VALUES (@title, @beanType)";
        //            cmd.Parameters.Add(new SqlParameter("@title", coffee.Title));
        //            cmd.Parameters.Add(new SqlParameter("@beanType", coffee.BeanType));

        //            int newId = (int)cmd.ExecuteScalar();
        //            coffee.Id = newId;
        //            return CreatedAtRoute("GetCoffee", new { id = newId }, coffee);
        //        }
        //    }
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Coffee coffee)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"UPDATE Coffee
        //                                    SET Title = @title,
        //                                        BeanType = @beanType
        //                                    WHERE Id = @id";
        //                cmd.Parameters.Add(new SqlParameter("@title", coffee.Title));
        //                cmd.Parameters.Add(new SqlParameter("@beanType", coffee.BeanType));
        //                cmd.Parameters.Add(new SqlParameter("@id", id));

        //                int rowsAffected = cmd.ExecuteNonQuery();
        //                if (rowsAffected > 0)
        //                {
        //                    return new StatusCodeResult(StatusCodes.Status204NoContent);
        //                }
        //                throw new Exception("No rows affected");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (!CoffeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete([FromRoute] int id)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"DELETE FROM Coffee WHERE Id = @id";
        //                cmd.Parameters.Add(new SqlParameter("@id", id));

        //                int rowsAffected = cmd.ExecuteNonQuery();
        //                if (rowsAffected > 0)
        //                {
        //                    return new StatusCodeResult(StatusCodes.Status204NoContent);
        //                }
        //                throw new Exception("No rows affected");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (!CoffeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private bool CoffeeExists(int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                SELECT Id, Title, BeanType
        //                FROM Coffee
        //                WHERE Id = @id";
        //            cmd.Parameters.Add(new SqlParameter("@id", id));

        //            SqlDataReader reader = cmd.ExecuteReader();
        //            return reader.Read();
        //        }
        //    }
        //}
    }
}
