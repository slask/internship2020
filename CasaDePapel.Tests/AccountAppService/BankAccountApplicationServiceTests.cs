using System.Collections.Generic;
using System.Linq;
using CasaDePapel.Application;
using CasaDePapel.Controllers;
using CasaDePapel.DataAccess;
using CasaDePapel.Domain;
using CasaDePapel.Infrastructure;
using CasaDePapel.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CasaDePapel.Tests.AccountAppService
{
    [TestClass]
    public class BankAccountApplicationServiceTests
    {
        private BankAccountApplicationService _sut;
        private BankContext _context;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<CurrencyRateService> _currencyRateServiceMock;
        private Mock<IQueryDatabaseGateway> _dbGateway;

        [TestInitialize]
        public void BeforeEeach()
        {
            _context = GetNewInMemoryDatabase("TestDb");
            _context.Database.EnsureCreated();
            _notificationServiceMock = new Mock<INotificationService>();
            _currencyRateServiceMock = new Mock<CurrencyRateService>();
            _dbGateway = new Mock<IQueryDatabaseGateway>();
            _sut = new BankAccountApplicationService(_context, _notificationServiceMock.Object,
                _currencyRateServiceMock.Object, null, _dbGateway.Object);
        }

        [TestCleanup]
        public void AfterEach()
        {
            _context.Database.EnsureDeleted();
        }
        
        public static BankContext GetNewInMemoryDatabase(string dbName)
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;
            var context = new BankContext(options);
            return context;
        }

        [TestMethod]
        public void Withdraw_WhenAmountLessThanBalance_BalanceDecreasesWithTheAmount()
        {
            //arrange
            _context.Accounts.Add(new Account() {Balance = 12, Id = 1});
            _context.SaveChanges();

            //act
            _sut.Withdraw(new WithdrawAmountModel()
            {
                Amount = 5,
                Id = 1
            });
            
            //assert
            _context.Accounts.Find(1).Balance.Should().Be(7);
        }

        [TestMethod]
        public void Withdraw_SendsNotificationToInterestedPartiesWheneverMoneyAreWithdrawnSuccessfully_WithTheAccountNumberAndRemainingAmount()
        {
            //arrange
            _context.Accounts.Add(new Account()
            {
                Balance = 5, Id = 1,
                Iban = "RO11BTRL123"
            });
            _context.SaveChanges();

            //act
            _sut.Withdraw(new WithdrawAmountModel()
            {
                Amount = 3,
                Id = 1
            });

            //assert
            _notificationServiceMock.Verify(s => s.MoneyWithdrawn("RO11BTRL123", 2), Times.Once);
        }
        
        //TODO: another test for withdrawn, if money insufficient

        [TestMethod]
        public void
            Exchange_WhenTargetCurrencyAccountExists_ConvertMoney_AndIncreaseBalanceOfTargetAccountWithProperAmount()
        {
            //arrange
            _context.Accounts.Add(CreateDefaultAccount("RO11BTRL",20));
            _context.Accounts.Add(new Account()
            {
                Balance = 0, Id = 2,
                OwnerId = 1,
                Iban = "RO12BTRL",
                Currency = "USD"
            });
            _context.SaveChanges();

            _currencyRateServiceMock.Setup(c => c.GetRate(It.IsAny<string>(), It.IsAny<string>())).Returns(1.1m);
            
            //act
            var result = _sut.Exchange(new ExchangeModel()
            {
                Amount = 10,
                AccountNr = "RO11BTRL",
                UserId = 1,
                TargetCurrency = "USD"
            });
            
            //assert
            result.Amount.Should().Be(11);
        }
        
        [TestMethod]
        public void
            Exchange_WhenTargetCurrencyAccountDoesNotExists_ConversionCreatesTheTargetAutomatically()
        {
            //arrange
            _context.Accounts.Add(CreateDefaultAccount("RO11BTRL",22));
            _context.SaveChanges();
            _currencyRateServiceMock.Setup(c => c.GetRate(It.IsAny<string>(), It.IsAny<string>())).Returns(1.1m);
            
            //act
            var result = _sut.Exchange(new ExchangeModel()
            {
                Amount = 10,
                AccountNr = "RO11BTRL",
                UserId = 1,
                TargetCurrency = "USD"
            });
            
            //assert
            result.Should().BeEquivalentTo(new BankAccountOutputModel()
            {
                Amount = 11,
                Currency = "USD",
                Type = "Current",
                AccountNr = "RO100BTRL123XX"
            });
            _context.Accounts.Any(x => x.Currency == "USD").Should().BeTrue();
            //OR
            //result.Currency.Should().BeEquivalentTo(n )Be("USD");
            //result.AccountNr.Should().Be("RO100BTRL123XX");
        }

        [TestMethod]
        public void Transfer_IncreasesTargetBalanceByAmount_AndDecreasesSourceBalanceByAmountPlus2PErcentCommission()
        {
            //arrange
            _context.Accounts.Add(
            new Account()
            {
                Balance = 200, 
                Id = 1,
                OwnerId = 1,
                Iban = "RO10",
                Currency = "RON"
            });
            _context.Accounts.Add(new Account()
            {
                Balance = 10, 
                Id = 2,
                OwnerId = 1,
                Iban = "RO11",
                Currency = "RON",
                IsActive = true
            });
            _context.SaveChanges();
            
            //act
            _sut.Transfer(new TransferMoneyModel()
            {
                Amount = 100,
                IbanFrom = "RO10",
                IbanTo = "RO11"
            });

            _context.Accounts.FirstOrDefault(a => a.Iban == "RO11").Balance.Should().Be(110);
            _context.Accounts.FirstOrDefault(a => a.Iban == "RO10").Balance.Should().Be(98);
        }

        [TestMethod]
        public void GetAllIsSummingCorrectly()
        {
            //arrange
            _dbGateway.Setup(_ => _.GetAllAccounts()).Returns(new List<Account>()
            {
                new Account()
                {
                    Balance = 1,
                },
                new Account()
                {
                    Balance = 2,
                },
                new Account()
                {
                    Balance = 3,
                }
            });
            
            //act
            var sum = _sut.AdminGetBankTotalWorth();

            sum.Should().Be(6);
        } 
        
        
        private static Account CreateDefaultAccount( string iban = "RO11BTRL", decimal initialAmount = 0)
        {
            return new Account()
            {
                Balance = initialAmount, 
                Id = 1,
                OwnerId = 1,
                Iban = iban,
                Currency = "RON"
            };
        }
    }
}