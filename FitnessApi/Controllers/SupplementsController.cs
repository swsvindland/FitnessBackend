using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers
{
    public sealed class SupplementsController
    {
        private readonly ISupplementService _supplementService;
        private readonly IAuthService _authService;
        private readonly ILogger<SupplementsController> _logger;

        public SupplementsController(ISupplementService supplementService, IAuthService authService,
            ILogger<SupplementsController> logger)
        {
            _supplementService = supplementService;
            _authService = authService;
            _logger = logger;
        }

        [Function("GetAllSupplements")]
        public async Task<IActionResult> GetAllSupplements(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req)
        {
            var authed = _authService.CheckAuth(req);

            if (authed == false)
            {
                return new UnauthorizedResult();
            }

            var supplements = await _supplementService.GetAllSupplements();

            return new OkObjectResult(supplements);
        }

        [Function("GetUserSupplements")]
        public async Task<IActionResult> GetUserSupplements(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req)
        {
            var authed = _authService.CheckAuth(req);

            if (authed == false)
            {
                return new UnauthorizedResult();
            }

            var userId = Guid.Parse(req.Query["userId"]);

            var userSupplements = await _supplementService.GetUserSupplements(userId);

            return new OkObjectResult(userSupplements);
        }

        [Function("UpdateUserSupplements")]
        public async Task<IActionResult> UpdateUserSupplements(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req)
        {
            var authed = _authService.CheckAuth(req);

            if (authed == false)
            {
                return new UnauthorizedResult();
            }

            string requestBody;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<UpdateUserSupplement>(requestBody);

            await _supplementService.UpdateUserSupplement(data);

            return new OkObjectResult(true);
        }

        [Function("GetUserSupplementActivity")]
        public async Task<IActionResult> GetUserSupplementActivity(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req)
        {
            var authed = _authService.CheckAuth(req);

            if (authed == false)
            {
                return new UnauthorizedResult();
            }

            var userId = Guid.Parse(req.Query["userId"]);
            var userSupplementId = long.Parse(req.Query["userSupplementId"]);
            var date = req.Query["date"];
            var time = Enum.Parse<SupplementTimes>(req.Query["time"]);

            var userSupplements =
                await _supplementService.GetUserSupplementActivity(userId, userSupplementId, date, time);

            return new OkObjectResult(userSupplements);
        }

        [Function("ToggleUserSupplementActivity")]
        public async Task<IActionResult> ToggleUserSupplementActivity(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req)
        {
            var authed = _authService.CheckAuth(req);

            if (authed == false)
            {
                return new UnauthorizedResult();
            }

            string requestBody;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var data = JsonConvert.DeserializeObject<UpdateUserSupplementActivity>(requestBody);

            await _supplementService.ToggleUserSupplementActivity(data);

            return new OkObjectResult(true);
        }
    }
}