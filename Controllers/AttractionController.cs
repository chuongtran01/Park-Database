using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AttractionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        [HttpGet]
        public async Task<ActionResult> GetAttractions()
        {
            string query = @"SELECT * FROM amusement_park.attraction";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                        
                    }
                    else
                    {
                        return BadRequest("No Attraction.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> GetAttractions(int id)
        {
            string query = @"DELETE  FROM amusement_park.attraction
                             WHERE attraction_id = @id";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("id", id);
                    myReader = myCommand.ExecuteReader();

                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok("Successfully!");
        }


        [HttpPost]
        public async Task<ActionResult> AddAttraction([FromBody] AttractionAdd request)
        {
            string query = @"INSERT INTO amusement_park.attraction (name, type, description, location, min_height, start_time, end_time, breakdown_nums)
                             VALUES (@name, @type, @description, @location, @min_height, @start_time, @end_time, @breakdown_nums)";

            int location = Convert.ToInt16(request.location);

            double min_height = Convert.ToDouble(request.min_height);

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@name", request.name);
                    myCommand.Parameters.AddWithValue("@type", request.type);
                    myCommand.Parameters.AddWithValue("@description", request.description);
                    myCommand.Parameters.AddWithValue("@location", location);
                    myCommand.Parameters.AddWithValue("@min_height", min_height);
                    if (request.start_time == null)
                    {
                        myCommand.Parameters.AddWithValue("@start_time", DBNull.Value);

                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@start_time", request.start_time);

                    }

                    if (request.end_time == null)
                    {
                        myCommand.Parameters.AddWithValue("@end_time", DBNull.Value);

                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@end_time", request.end_time);


                    }
                    myCommand.Parameters.AddWithValue("@breakdown_nums", request.breakdown_num);

                    myReader = myCommand.ExecuteReader();


                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok("Successfully!");
        }

        [Route("update/{id}")]
        [HttpPost]
        public async Task<ActionResult> UpdateAttraction([FromBody] AttractionUpdate request, int id)
        {
            DateTime start = DateTime.Parse(request.start_time);
            DateTime end = DateTime.Parse(request.end_time);


            string query = @"UPDATE amusement_park.attraction
                             SET name = @name, description = @description, location = @location, min_height = @min_height, start_time = @start_time, end_time = @end_time 
                             WHERE attraction_id = @id";

            int location = Convert.ToInt16(request.location);

            double min_height = Convert.ToDouble(request.min_height);

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@name", request.name);
                    myCommand.Parameters.AddWithValue("@description", request.description);
                    myCommand.Parameters.AddWithValue("@location", location);
                    myCommand.Parameters.AddWithValue("@min_height", min_height);
                    if (request.start_time == null)
                    {
                        myCommand.Parameters.AddWithValue("@start_time", DBNull.Value);

                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@start_time", start);

                    }

                    if (request.end_time == null)
                    {
                        myCommand.Parameters.AddWithValue("@end_time", DBNull.Value);

                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@end_time", end);


                    }


                    myReader = myCommand.ExecuteReader();


                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok("Successfully!");
        }

        [Route("rides")]
        [HttpGet]
        public async Task<ActionResult> GetRides()
        {
            string query = @"SELECT * FROM amusement_park.rides";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No Attraction.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("shops")]
        [HttpGet]
        public async Task<ActionResult> GetShops()
        {
            string query = @"SELECT * FROM amusement_park.shops";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No Attraction.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("shopreportpastmonth")]
        [HttpGet]
        public async Task<ActionResult> GetShopReport()
        {
            string query = @"SELECT * FROM amusement_park.shopreportpastmonth";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No Attraction.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("ridereportpastmonth")]
        [HttpGet]
        public async Task<ActionResult> GetRideReport()
        {
            string query = @"SELECT * FROM amusement_park.ridereportpastmonth";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No Attraction.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("breakdown/{month}/{year}")]
        [HttpGet]
        public async Task<ActionResult> GetBreakdownDate(string month, string year)
        {
            string query = @"SELECT r.ride_id, a.name, a.breakdown_nums, r.breakdown_desc, r.breakdown_date, r.resolved, r.maintainer_id, r.breakdown_id
                             FROM amusement_park.ride_breakdown as r, amusement_park.attraction as a
                             WHERE r.ride_id = a.attraction_id AND monthname(r.breakdown_date) = @month AND YEAR(r.breakdown_date) = @year";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@month", month);
                    myCommand.Parameters.AddWithValue("@year", year);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No Breakdown found.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("breakdown")]
        [HttpGet]
        public async Task<ActionResult> GetBreakdowns()
        {
            string query = @"SELECT r.ride_id, a.name, a.breakdown_nums, r.breakdown_desc, r.breakdown_date, r.resolved, r.maintainer_id, r.breakdown_id
                             FROM amusement_park.ride_breakdown as r, amusement_park.attraction as a
                             WHERE r.ride_id = a.attraction_id";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No Breakdown found.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("popular/month/{month}/{year}")]
        [HttpGet]
        public async Task<ActionResult> GetMostPopularMonth(int year, string month)
        {
            string query = @"SELECT most_popular_ride FROM date
                            WHERE (EXTRACT(YEAR FROM date) = @year) AND (monthname(date) = @month)
                            GROUP BY most_popular_ride
                            ORDER BY COUNT(most_popular_ride) DESC
                            LIMIT 1";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@year", year);
                    myCommand.Parameters.AddWithValue("@month", month);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }

        [Route("popular/{start}/{end}")]
        [HttpGet]
        public async Task<ActionResult> GetMostPopularMonth(string start, string end)
        {
            DateTime start_time = DateTime.Parse(start);
            DateTime end_time = DateTime.Parse(end);

            string query = @"SELECT most_popular_ride FROM date
                            WHERE (date between DATE(@start_time) and DATE(@end_time))
                            GROUP BY most_popular_ride
                            ORDER BY COUNT(most_popular_ride) DESC
                            LIMIT 1";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@start_time", start_time);
                    myCommand.Parameters.AddWithValue("@end_time", end_time);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);

                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }


        [Route("breakdown/update")]
        [HttpPost]
        public async Task<ActionResult> BreakdownUpdate([FromBody] BreakdownUpdate request)
        {
            int id = Convert.ToInt16(request.breakdown_id);

            string query = @"UPDATE amusement_park.ride_breakdown
                             SET resolved = @resolved
                             WHERE breakdown_id = @breakdown_id";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@resolved", request.resolved);
                    myCommand.Parameters.AddWithValue("breakdown_id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok("Successfully!");
        }

        [Route("breakdown")]
        [HttpPost]
        public async Task<ActionResult> BreakdownAdd([FromBody] BreakdownAdd request)
        {
            int ride_id = Convert.ToInt16(request.ride_id);
            int maintainer_id = Convert.ToInt16(request.maintainer_id);
            DateTime date = DateTime.Parse(request.breakdown_date);

            string query = @"INSERT INTO amusement_park.ride_breakdown (ride_id, maintainer_id, breakdown_date, breakdown_desc, resolved)
                             VALUES (@ride_id, @maintainer_id, @breakdown_date, @breakdown_desc, @resolved)";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@ride_id", ride_id);
                    myCommand.Parameters.AddWithValue("maintainer_id", maintainer_id);
                    myCommand.Parameters.AddWithValue("breakdown_date", date);
                    myCommand.Parameters.AddWithValue("breakdown_desc", request.breakdown_desc);
                    myCommand.Parameters.AddWithValue("resolved", Convert.ToByte(0));


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok("Successfully!");
        }

        [Route("usage/{dateRequest}")]
        [HttpGet]
        public async Task<ActionResult> BreakdownAdd(string dateRequest)
        {

            DateTime date = DateTime.Parse(dateRequest);

            string query = @"select a.name, u.attraction_id, a.type, u.uses, u.revenue from amusement_park.usage_per_day as u
                            inner join amusement_park.attraction as a on u.attraction_id = a.attraction_id
                            where u.date_id = @date
                            order by a.type";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@date", date);

                    

                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                    }
                    else
                    {
                        return BadRequest("No data found.");
                    }
                    myReader.Close();
                    mycon.Close();
                }
            }

            return Ok(table);
        }
    }
}

