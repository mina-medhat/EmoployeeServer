using EmoployeeServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmoployeeServer.Controllers
{
    [Authorize]
    public class EmployeeController : ApiController
    {
        private ApplicationDbContext _context;
        public EmployeeController()
        {
            _context = new ApplicationDbContext();
        }

        // GET api/Employee
        public IEnumerable<Employee> Get()
        {
            return _context.Employees.ToList();
        }

        // GET api/Employee/hiringDate
        [Route("api/employee/{hiringDate}")]
        public IHttpActionResult Get(DateTime hiringDate)
        {
            var date = hiringDate.Date;
            var employees = _context.Employees.Where(e => DbFunctions.TruncateTime(e.HiringDate) == date).ToList();
            if (employees.Count > 0)
                return Ok(employees);
            else
                return NotFound();
        }

        // GET api/Employee/id
        public IHttpActionResult Get(int id)
        {
            var employee = _context.Employees.SingleOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        // POST api/Employee
        [HttpPost]
        public IHttpActionResult CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return Created(new Uri(Request.RequestUri + "/" + employee.Id), employee);
        }
    }
}
