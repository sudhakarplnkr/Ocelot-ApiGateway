namespace EmployeeApi.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET api/employees
        [HttpGet, Route("")]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            return new Employee[] { new Employee("John"), new Employee("Crish") };
        }
    }

    public class Employee
    {
        public Employee(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

}
