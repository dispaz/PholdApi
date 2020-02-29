using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PholdApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PholdController : ControllerBase
    {
        private readonly ILogger<PholdController> _logger;

        public PholdController(ILogger<PholdController> logger)
        {
            _logger = logger;
        }

       
    }
}
