using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            return Ok(await _context.customer.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Customer>>> GetCustomer(int id)
        {
            var temp = await _context.customer.Where(h => h.customer_id == id).FirstOrDefaultAsync();

            if (temp == null)
            {
                return BadRequest("Customer not existed!");
            }


            return Ok(temp.email);
        }


        [Route("signup")]
        [HttpPost]
        public async Task<ActionResult<Customer>> CustomerSignUp(CustomerSignUp newCustomer)
        {
            var oldCustomer = await _context.customer.Where(h => h.email == newCustomer.email).FirstOrDefaultAsync();

            if (oldCustomer != null)
            {
                return BadRequest("Duplicate Email");
            }

            string[] chars = newCustomer.height.Split("'");

            double newHeight = Convert.ToDouble(chars[0]) + (Convert.ToDouble(chars[1]) / 12);

            var createdCustomer = new Customer
            {
                fname = newCustomer.fname,
                lname = newCustomer.lname,
                email = newCustomer.email,
                password = newCustomer.password,
                height = newHeight,
                tickets_bought = 0,
                DOB = new DateTime(2001, 12, 12)
            };

            _context.customer.Add(createdCustomer);
            await _context.SaveChangesAsync();

            return Ok(newCustomer);
        }

        [Route("signin")]
        [HttpPost]
        public async Task<ActionResult<Customer>> CustomerSignIn(CustomerLogin request)
        {
            var temp = await _context.customer.Where(h => h.email == request.email).FirstOrDefaultAsync();

            if (temp == null)
            {
                return BadRequest("Email not existed or password is incorrect.");
            }

            return Ok(temp);
        }

        [HttpPut]
        public async Task<ActionResult<List<Customer>>> ModifyCustomer(Customer request)
        {
            var dbCustomer = await _context.customer.FirstOrDefaultAsync(h => h.email == request.email);

            if (dbCustomer == null)
            {
                return BadRequest("Customer not existed");
            }

            dbCustomer.password = request.password;
            dbCustomer.height = request.height;
            dbCustomer.DOB = request.DOB;

            await _context.SaveChangesAsync();

            return Ok(await _context.customer.ToListAsync());
        }

        
    }
}

