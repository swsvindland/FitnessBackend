using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers
{
    public sealed class SupplementsController
    {
        private readonly ISupplementService _supplementService;

        public SupplementsController(ISupplementService supplementService)
        {
            _supplementService = supplementService;
        }
        
        [FunctionName("GetAllSupplements")]
        public async Task<IActionResult> GetAllSupplements(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var supplements = await _supplementService.GetAllSupplements();

            return new OkObjectResult(supplements);
        }
        
        [FunctionName("GetUserSupplements")]
        public async Task<IActionResult> GetUserSupplements(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var userId = Guid.Parse(req.Query["userId"]);
            
            var userSupplements = await _supplementService.GetUserSupplements(userId);

            return new OkObjectResult(userSupplements);
        }
        
        [FunctionName("UpdateUserSupplements")]
        public async Task<IActionResult> UpdateUserSupplements(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody;
            using (var streamReader =  new  StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var data = JsonConvert.DeserializeObject<UpdateUserSupplement>(requestBody);
            
            await _supplementService.UpdateUserSupplement(data);

            return new OkObjectResult(true);
        }
        
        [FunctionName("GetUserSupplementActivity")]
        public async Task<IActionResult> GetUserSupplementActivity(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var userId = Guid.Parse(req.Query["userId"]);
            var userSupplementId = long.Parse(req.Query["userSupplementId"]);
            var date = req.Query["date"];
            var time = Enum.Parse<SupplementTimes>(req.Query["time"]);
            
            var userSupplements = await _supplementService.GetUserSupplementActivity(userId, userSupplementId, date, time);

            return new OkObjectResult(userSupplements);
        }
        
        [FunctionName("ToggleUserSupplementActivity")]
        public async Task<IActionResult> ToggleUserSupplementActivity(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody;
            using (var streamReader =  new  StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var data = JsonConvert.DeserializeObject<UpdateUserSupplementActivity>(requestBody);
            
            await _supplementService.ToggleUserSupplementActivity(data);

            return new OkObjectResult(true);
        }
    }
}
