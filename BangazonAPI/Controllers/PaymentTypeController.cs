using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BangazonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        // GET: api/<PaymentTypeController>

        //Following code allows access to SQL database

        private readonly IConfiguration _config;

        public PaymentTypeController(IConfiguration config)
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
                    cmd.CommandText = @"SELECT Id, AcctNumber AS AccountNumber, Name AS PaymentType, CustomerId FROM PaymentType";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<PaymentType> paymentTypes = new List<PaymentType>();

                    while (reader.Read())
                    {

                        if (!paymentTypes.Any(p => p.Id == reader.GetInt32(reader.GetOrdinal("Id"))))
                        {
                            PaymentType payment = new PaymentType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                AcctNumber = reader.GetString(reader.GetOrdinal("AccountNumber")),
                                Name = reader.GetString(reader.GetOrdinal("PaymentType")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                            };

                            paymentTypes.Add(payment);

                        }
                    }

                    reader.Close();
                    return Ok(paymentTypes);

                }
            }
        }

        // GET api/<PaymentTypeController>/2
        [HttpGet("{id}", Name = "GetPaymentType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, AcctNumber AS AccountNumber, Name AS PaymentType, CustomerId FROM PaymentType
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    PaymentType paymentType = null;

                    if (reader.Read())
                    {
                        paymentType = new PaymentType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            AcctNumber = reader.GetString(reader.GetOrdinal("AccountNumber")),
                            Name = reader.GetString(reader.GetOrdinal("PaymentType")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                        };
                    }
                    reader.Close();

                    return Ok(paymentType);
                }
            }
        }

        // POST api/<PaymentTypeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PaymentTypeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PaymentTypeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
