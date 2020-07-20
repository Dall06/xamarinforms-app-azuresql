using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_db_azuresql.Driver.Domain;
using api_db_azuresql.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_db_azuresql.Driver.Infraestructure
{
    [Route("[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        // GET: api/<DriverController>n
        [HttpGet]
        public List<DriverModel> Get()
        {
            return new DriverModel().GetAll();
        }

        // GET api/<DriverController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DriverController>
        [HttpPost]
        public ApiResponse Post([FromBody] DriverModel value)
        {
            return value.Insert();
        }

        // PUT api/<DriverController>/5
        [HttpPut] //("{id}")
        public ApiResponse Put([FromBody] DriverModel value)
        {
            return value.Update();
        }

        // DELETE api/<DriverController>/5
        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            return new DriverModel().Delete(id);
        }
    }
}
