using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CasaDePapel.Application;
using CasaDePapel.Controllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CasaDePapel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly BankAccountApplicationService _bankAccountApplicationService;

        public BankAccountController(BankAccountApplicationService bankAccountApplicationService)
        {
            _bankAccountApplicationService = bankAccountApplicationService;
        }

        [HttpPost]
        [Route("deposit")]
        public IActionResult DepositAmount(DepositAmountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bankAccountApplicationService.Deposit(model);

            return Ok();
        }
        
        [HttpPost]
        [Route("transfer")]
        public IActionResult TransferMoney(TransferMoneyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bankAccountApplicationService.Transfer(model);

            return Ok();
        }
        
        [HttpGet]
        [Route("list")]
        public IActionResult GetActiveBankAccountsList(int userId)
        {
       

            return Ok(new List<BankAccountOutputModel>());
        }
        
        [HttpPost]
        [Route("exchange")]
        public IActionResult CurrencyExchange(ExchangeModel model)
        {
            var result = _bankAccountApplicationService.Exchange(model);

            return Ok(result);
        }
    }
}