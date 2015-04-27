using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    #region BankTest
    /// <summary>
    /// Test class for Bank.cs
    /// </summary>
    [TestClass]
    public class BankTest
    {
        #region private fields
        private static readonly double DOUBLE_DELTA = 1e-2;
        #endregion

        #region  TestMethods
        /// <summary>
        /// Tests customer summary
        /// </summary>
        [TestMethod]
        public void CustomerSummary() 
        {
            Bank bank = new Bank();
            Customer john = new Customer("John");
            john.OpenAccount(AccountFactory.CreateAccount(AccountFactory.CHECKING));
            bank.AddCustomer(john);
            Assert.AreEqual("Customer Summary\n - John (1 account)", bank.CustomerSummary());
        }

        /// <summary>
        /// Tests total interest paid
        /// </summary>
        [TestMethod]
        public void CheckingAccount() 
        {
            Bank bank = new Bank();
            IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
            Customer bill = new Customer("Bill").OpenAccount(checkingAccount);
            bank.AddCustomer(bill);

            // Simulate current time so that transactions are recorded with prior dates
            DateProvider.Instance.setSimulatedCurrentTime(DateTime.Now.AddDays(-365));

            checkingAccount.Deposit(10000.0);
            Assert.AreEqual(10, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }

        /// <summary>
        /// Tests TotalInterestPaid with savings Account
        /// </summary>
        [TestMethod]
        public void SavingAccount() 
        {
            Bank bank = new Bank();
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            bank.AddCustomer(new Customer("Bill").OpenAccount(savingsAccount));

            // Simulate current time so that transactions are recorded with prior dates
            DateProvider.Instance.setSimulatedCurrentTime(DateTime.Now.AddDays(-365));

            savingsAccount.Deposit(10000.0);
            Assert.AreEqual(19.02, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }


        /// <summary>
        /// Tests TotalInterestPaid with Maxsavings Account
        /// </summary>
        [TestMethod]
        public void MaxSavingAccount()
        {
            Bank bank = new Bank();
            IAccount maxiAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            bank.AddCustomer(new Customer("Bill").OpenAccount(maxiAccount));

            // Simulate current time so that transactions are recorded with prior dates
            DateProvider.Instance.setSimulatedCurrentTime(DateTime.Now.AddDays(-365));

            maxiAccount.Deposit(10000.0);
            Assert.AreEqual(498.57, bank.TotalInterestPaid(), DOUBLE_DELTA);
        }

        /// <summary>
        /// Tests to get first customer name
        /// </summary>
        [TestMethod]
        public void TestGetFirstCustomer()
        {

            Bank bank = new Bank();
            IAccount maxiAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            bank.AddCustomer(new Customer("Bill").OpenAccount(maxiAccount));
            maxiAccount.Deposit(3000.0);
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            bank.AddCustomer(new Customer("Aureo").OpenAccount(savingsAccount));
            savingsAccount.Deposit(1500.0);
            Assert.AreEqual("Bill", bank.GetFirstCustomer());
        }
        #endregion
    }
    #endregion
}
