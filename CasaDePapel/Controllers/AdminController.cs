using CasaDePapel.Application;
using Microsoft.AspNetCore.Mvc;

namespace CasaDePapel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly BankAccountApplicationService _bankAccountApplicationService;

        public AdminController(BankAccountApplicationService bankAccountApplicationService)
        {
            _bankAccountApplicationService = bankAccountApplicationService;
        }
        
        [Route("worth")]
        public IActionResult GetBankWorth()
        {
            return Ok(_bankAccountApplicationService.AdminGetBankTotalWorth());
        }
    }
}