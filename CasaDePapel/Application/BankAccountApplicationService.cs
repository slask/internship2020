using System.Collections.Generic;
using CasaDePapel.Controllers;
using CasaDePapel.Controllers.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace CasaDePapel.Application
{
    public class BankAccountApplicationService
    {
        public void Deposit(DepositAmountModel model)
        {
            //TODO: - showcase simple state based testing on domain model
            //get the account
            // use deposit method
            //save the account to DB
            throw new System.NotImplementedException();
        }

        public void WithDraw(object model)
        {
            //TODO: - showcase state based testing + mock verification
            //get account
            //withdrawmoney
            //save
            //call notifyService with the event
        }

        public object Exchange(ExchangeModel model)
        {
            //TODO: - showcase the need for a stub for the exchange rate service
            //get the account
            //get the currency as string
            //pass currency + target currency to rate service
            //after get rate do the conversion by also adding 1% commision
            //create a new account on the fly in the new currency if it does not exist (it will not)
            //save new amount in new account
            //deduct old amount from current account
            //save all
            throw new System.NotImplementedException();
        }

        public void Transfer(TransferMoneyModel model)
        {
            //TODO: - show refactoring strength when having UTs
            // 1. do the transfer here, with 2% commision
            // 2. make the UT
            // 3. then refactor to a domain service and make sure the test passes
            throw new System.NotImplementedException();
        }

        public object ListActiveBankAccounts()
        {
            //TODO: - make sure there is data in DB and showcase testing an ef query with inMemoryDB
            // get list and filter by is acive and userid
            return null;
        }

        public decimal AdminGetBankTotalWorth(int userId)
        {
            //TODO: - showcase how to make the untestable testable and mock deps
            // get all accounts with dapper for perf
            // sum all ammounts and return
            return 0m;
        }
    }
}