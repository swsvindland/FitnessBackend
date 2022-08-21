using System.Threading.Tasks;
using FitnessRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FitnessApi
{
    public class HelloWorld
    {
        private readonly ISupplementRepository _supplementRepository;

        public HelloWorld(ISupplementRepository supplementRepository)
        {
            _supplementRepository = supplementRepository;
        }
        
        [FunctionName("HelloWorld")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var supplements = await _supplementRepository.GetAllSupplements();

            return new OkObjectResult(supplements);
        }
    }
}
