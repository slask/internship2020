using System;
using CasaDePapel.Application;
using CasaDePapel.Controllers.Models;
using Microsoft.AspNetCore.Mvc;

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
        [Route("withdraw")]
        public IActionResult WithdrawAmount(WithdrawAmountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bankAccountApplicationService.Withdraw(model);

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
            if (userId <= 0)
            {
                throw new ArgumentException($"{nameof(userId)} is not valid");
            }
            
            var accounts = _bankAccountApplicationService.ListActiveBankAccounts(userId);
            return Ok(accounts);
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