using APIRestaurante.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRestaurante.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var customers = await _customerService.GetAllAsync(pageNumber, pageSize);
            return Ok(new { customers });
        }
    }
}
