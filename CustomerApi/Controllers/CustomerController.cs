namespace CustomerApi.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET api/customers
        [Authorize]
        [HttpGet, Route("")]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return new Customer[] { new Customer("John"), new Customer("Crish") };
        }
    }

    public class Customer
    {
        public Customer(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
