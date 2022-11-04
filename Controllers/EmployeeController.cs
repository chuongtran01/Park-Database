using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace amusement_park.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            return Ok(await _context.employee.ToListAsync());
        }

        [Route("signin")]
        [HttpPost]
        public async Task<ActionResult<Employee>> EmployeeLogin(EmployeeLogin request)
        {

            var temp = await _context.employee.Where(h => h.username == request.username && h.password == request.password).FirstOrDefaultAsync();

            if (temp == null)
            {
                return BadRequest("Email not existed or password is incorrect.");
            }

            return Ok(temp);
        }

        [Route("position")]
        [HttpGet]
        public async Task<ActionResult<Employee>> EmployeePosition(EmployeeLogin request)
        {
            return Ok("Updated!");
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<Employee>> EmployeeDelete(int id)
        {

            var existed = await _context.employee.Where(h => h.employee_id == id).FirstOrDefaultAsync();

            if (existed == null)
            {
                return BadRequest("Employee not existed");
            }

            _context.employee.Remove(existed);
            await _context.SaveChangesAsync();

            return Ok("Successfully deleted");
        }

        [Route("update")]
        [HttpPost]
        public async Task<ActionResult<List<Employee>>> UpdateEmployee(EmployeeUpdate request)
        {
            int id = Convert.ToInt32(request.employee_id);
            var existed = await _context.employee.Where(h => h.employee_id == id).FirstOrDefaultAsync();

            if (existed == null)
            {
                return BadRequest("Employee not existed!");
            }

            existed.username = request.newUsername;
            await _context.SaveChangesAsync();

            return Ok(existed);
        }

        [HttpPost]
        public async Task<ActionResult<List<Employee>>> AddEmployee(Employee newEmployee)
        {
            var oldEmployee = await _context.employee.Where(h => h.username == newEmployee.username).FirstOrDefaultAsync();

            if (oldEmployee != null)
            {
                return BadRequest("Duplicate Email");
            }

            Convert.ToInt32(newEmployee.employee_id);
            Convert.ToInt32(newEmployee.supervisor_id);

            _context.employee.Add(newEmployee);
            await _context.SaveChangesAsync();


            return Ok(newEmployee);
        }

        
    }
}

