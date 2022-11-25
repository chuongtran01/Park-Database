using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TicketController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult> BuyTicket([FromBody] TicketBuying request)
        {
            DateTime date = DateTime.Parse(request.date);


            string query = @"INSERT INTO amusement_park.ticket_bought(customer_id, ticket_id, price, tickets_bought, entry_date)
                             VALUES (@customer_id, @ticket_id, @price, @tickets_bought, @entry_date)";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    if (request.customer_id == - 1)
                    {
                        myCommand.Parameters.AddWithValue("@customer_id", DBNull.Value);

                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@customer_id", request.customer_id);

                    }


                    myCommand.Parameters.AddWithValue("@ticket_id", 1);
                    myCommand.Parameters.AddWithValue("@price", request.price);
                    myCommand.Parameters.AddWithValue("@tickets_bought", request.tickets);
                    myCommand.Parameters.AddWithValue("@entry_date", date);

                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok("Sucessfully!");
        }

        [Route("{date}")]
        [HttpGet]
        public async Task<ActionResult> GetFull(string date)
        {
            DateTime newDate = DateTime.Parse(date);
            string query = @"SELECT is_full from amusement_park.date
                            WHERE date=@date";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@date", newDate);
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data exist.");
                    }


                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }
    }
}

