using System;
using CasaDePapel.Domain;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CasaDePapel.Tests.Domain
{
    [TestClass]
    public class AccountTests
    {
        private Account _account;

        [TestInitialize]
        public void BeforeEach()
        {
            _account = new Account() {Balance = 3};
        }
        
        [TestMethod]
        public void Deposit_WhenPositiveAmountProvided_ThenBalanceIncreasesWithTheAmount()
        {
            //act
            _account.Deposit(6);
            
            //assert
            _account.Balance.Should().Be(9);
        }
        
        [TestMethod]
        public void Deposit_WhenNegativeAmount_AnExceptionIsThrown()
        {
            //act
            //assert
            _account.Invoking(a => a.Deposit(-4)).Should().Throw<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Deposit_WhenZeroAmount_BalanceRemainsUnchanged()
        {
            //act
            _account.Deposit(0);

            //assert
            Assert.AreEqual(3, _account.Balance);
        }
        
    }
}