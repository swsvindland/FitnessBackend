using System.Threading.Tasks;
using FitnessRepository;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FitnessApi
{
    public class SupplementsController
    {
        private readonly ISupplementService _supplementService;

        public SupplementsController(ISupplementService supplementService)
        {
            _supplementService = supplementService;
        }
        
        [FunctionName("GetAllSupplements")]
        public async Task<IActionResult> GetAllSupplements(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var supplements = await _supplementService.GetAllSupplements();

            return new OkObjectResult(supplements);
        }
    }
}
