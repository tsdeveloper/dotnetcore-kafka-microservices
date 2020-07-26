using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commom.Helper;
using Commom.Model;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ProducerConfig config;
        public HelloWorldController(ProducerConfig config)
        {
            this.config = config;

        }
        // POST api/values
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]HelloWorldRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Serialize 
            string serializedHelloWorld = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: HelloWorldController => Post => Recieved a new purchase helloWorld:");
            Console.WriteLine(serializedHelloWorld);
            Console.WriteLine("=========");

            var producer = new ProducerWrapper(this.config,"hello-requests");
            await producer.writeMessage(serializedHelloWorld);

            return Created("TransactionId", "Your helloWorld is in progress");
        }
    }
}