﻿using System;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentType payment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PaymentType (AcctNumber, Name, CustomerId)
                                        OUTPUT INSERTED.Id
                                        VALUES (@acctNum, @name, @custId)";
                    cmd.Parameters.Add(new SqlParameter("@acctNum", payment.AcctNumber));
                    cmd.Parameters.Add(new SqlParameter("@name", payment.Name));
                    cmd.Parameters.Add(new SqlParameter("@custId", payment.CustomerId));

                    int newId = (int)cmd.ExecuteScalar();
                    payment.Id = newId;
                    return CreatedAtRoute("GetPaymentType", new { id = newId }, payment);
                }
            }
        }

        //PUT method needs ID to update and New information to change
        //204NoContent means we aren't getting anything back
        //Not Found is a 404 Status
        //PaymentTypeExists is a method WE wrote to check to see if the PaymentType id is there or not - returns boolean

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PaymentType payment)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE PaymentType
                                            SET AcctNumber = @acctNum,
                                                Name = @name,
                                                CustomerId = @custId
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@acctNum", payment.AcctNumber));
                        cmd.Parameters.Add(new SqlParameter("@name", payment.Name));
                        cmd.Parameters.Add(new SqlParameter("@custId", payment.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!PaymentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM PaymentType WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!PaymentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool PaymentTypeExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, AcctNumber, Name, CustomerId
                        FROM PaymentType
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}