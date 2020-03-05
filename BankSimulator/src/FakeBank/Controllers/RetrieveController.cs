using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeBank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FakeBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RetrieveController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post([FromBody]RetrieveModel retrieveModel)
        {
            if (DateTime.Today > 
                new DateTime(retrieveModel.CardExpireYear, retrieveModel.CardExpireMonth, 
                    DateTime.Today.Day))
                return BadRequest("EXPIRED");

            if(retrieveModel.CardNumber == 99999)
                return BadRequest("INVALID");

            return Ok(new RetrieveResponseModel()
            {
                Id = Guid.NewGuid()
            });
        }
    }
}
