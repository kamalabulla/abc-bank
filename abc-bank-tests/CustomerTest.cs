using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    #region CustomerTest
    /// <summary>
    /// Test Class for Customer Test
    /// </summary>
    [TestClass]
    public class CustomerTest
    {
        private static readonly double DOUBLE_DELTA = 1e-2;

        #region TestMethods
        /// <summary>
        /// Tests generate statement
        /// </summary>
        [TestMethod]
        public void TestGenerateStatement()
        {
            IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            IAccount maxisavingsAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            Customer henry = new Customer("Henry").OpenAccount(checkingAccount).OpenAccount(savingsAccount).OpenAccount(maxisavingsAccount);
            checkingAccount.Deposit(100.0);
            savingsAccount.Deposit(4000.0);
            savingsAccount.Withdraw(200.0);
            maxisavingsAccount.Deposit(200.0);
            Assert.AreEqual("Statement for Henry\n" +
                    "\n" +
                    "Checking Account\n" +
                    "  deposit $100.00\n" +
                    "Total $100.00\n" +
                    "\n" +
                    "Savings Account\n" +
                    "  deposit $4,000.00\n" +
                    "  withdrawal $200.00\n" +
                    "Total $3,800.00\n" +
                    "\n" +
                    "Maxi Savings Account\n" +
                    "  deposit $200.00\n" +
                    "Total $200.00\n" +
                    "\n" +
                    "Total In All Accounts $4,100.00", henry.GetStatement());
        }

        /// <summary>
        /// Test adding single acoount
        /// </summary>
        [TestMethod]
        public void TestOneAccount()
        {
            Customer oscar = new Customer("Oscar").OpenAccount(AccountFactory.CreateAccount(AccountFactory.SAVINGS));
            Assert.AreEqual(1, oscar.GetNumberOfAccounts());
        }

        /// <summary>
        /// Test adding two accounts 
        /// </summary>
        [TestMethod]
        public void TestTwoAccount()
        {
            Customer oscar = new Customer("Oscar")
                 .OpenAccount(AccountFactory.CreateAccount(AccountFactory.SAVINGS));
            oscar.OpenAccount(AccountFactory.CreateAccount(AccountFactory.CHECKING));
            Assert.AreEqual(2, oscar.GetNumberOfAccounts());
        }

        /// <summary>
        /// Test adding all three types of accounts
        /// </summary>
        [TestMethod]
        public void TestThreeAccounts()
        {
            Customer oscar = new Customer("Oscar")
                    .OpenAccount(AccountFactory.CreateAccount(AccountFactory.SAVINGS));
            oscar.OpenAccount(AccountFactory.CreateAccount(AccountFactory.CHECKING));
            oscar.OpenAccount(AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS));
            Assert.AreEqual(3, oscar.GetNumberOfAccounts());
        }
    
        /// <summary>
        /// Test to give total interest on all the accounts of a customer
        /// </summary>
        [TestMethod]
        public void TestTotalInterestEarned()
        {
            IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            IAccount maxiSavingsAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            Customer oscar = new Customer("Oscar").OpenAccount(checkingAccount).OpenAccount(savingsAccount).OpenAccount(maxiSavingsAccount);

            // Simulate current time so that transactions are recorded with prior dates
            DateProvider.Instance.setSimulatedCurrentTime(DateTime.Now.AddDays(-365));

            checkingAccount.Deposit(10000);
            savingsAccount.Deposit(10000);
            maxiSavingsAccount.Deposit(10000);
            Assert.AreEqual(527.59, oscar.TotalInterestEarned(), DOUBLE_DELTA);
        }

        /// <summary>
        /// Test transfering amount with insufficient funds
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestTransferAmountInsufficientFundsFail()
        {
                IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
                IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
                IAccount maxiSavingsAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
                Customer oscar = new Customer("Oscar").OpenAccount(checkingAccount).OpenAccount(savingsAccount).OpenAccount(maxiSavingsAccount);
                checkingAccount.Deposit(1500.0);
                savingsAccount.Deposit(1500.0);
                maxiSavingsAccount.Deposit(1500.0);
                oscar.TransferAmount(2000, checkingAccount.Id, savingsAccount.Id);
        }

        /// <summary>
        /// Tests transfering amount with sufficient funds
        /// </summary>
        [TestMethod]
        public void TestTransferAmountSuccess()
        {
            IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            IAccount maxiSavingsAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            Customer oscar = new Customer("Oscar").OpenAccount(checkingAccount).OpenAccount(savingsAccount).OpenAccount(maxiSavingsAccount);
            checkingAccount.Deposit(1500.0);
            savingsAccount.Deposit(1500.0);
            maxiSavingsAccount.Deposit(1500.0);

            oscar.TransferAmount(500, checkingAccount.Id, savingsAccount.Id);

            Assert.AreEqual(1000, checkingAccount.GetCurrentBalance());
            Assert.AreEqual(2000, savingsAccount.GetCurrentBalance());
            Assert.AreEqual(1500.0, maxiSavingsAccount.GetCurrentBalance());
        }

        /// <summary>
        /// Test to get exception while transfering amount with invalid account id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestTransferAmountSendingInvalidAccountId()
        {
            IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            Customer oscar = new Customer("Oscar").OpenAccount(checkingAccount).OpenAccount(savingsAccount);
            checkingAccount.Deposit(1500.0);
            savingsAccount.Deposit(1500.0);
            oscar.TransferAmount(500, "18", savingsAccount.Id);
        }
        #endregion
    }
    #endregion
}
