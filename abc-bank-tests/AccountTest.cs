using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    #region AccountTest
   /// <summary>
   /// Test Class for Account.cs
   /// </summary>
    [TestClass]
    public class AccountTest
    {
        #region TestMethods
        /// <summary>
        /// Tests creation of an acocunt and verifies number of accounts
        /// </summary>
        [TestMethod]
        public void TestCreateMaxiSavingsAccount()
        {
            IAccount maxiSavingsAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            Customer henry = new Customer("Henry").OpenAccount(maxiSavingsAccount);
            Assert.AreEqual(1, henry.GetNumberOfAccounts());
        }

        /// <summary>
        /// Tests deposit and current balance after deposit
        /// </summary>
        [TestMethod]
        public void TestDepositToSavingsAccount()
        {
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            Customer henry = new Customer("Henry").OpenAccount(savingsAccount);
            savingsAccount.Deposit(1500.0);
            Assert.AreEqual(1500, savingsAccount.GetCurrentBalance());
        }

        /// <summary>
        /// Tests withdrawal and current balance after withdrawal
        /// </summary>
        [TestMethod]
        public void TestWithDrawAmountinCheckingAccount()
        {
            IAccount checkingAccount = AccountFactory.CreateAccount(AccountFactory.CHECKING);
            Customer henry = new Customer("Henry").OpenAccount(checkingAccount);
            checkingAccount.Deposit(1500.0);
            checkingAccount.Withdraw(500.0);
            Assert.AreEqual(1000, checkingAccount.GetCurrentBalance());
        }

        /// <summary>
        /// Tests GetAllTransaction count
        /// </summary>
        [TestMethod]
        public void GetAllTransactionsOfMaxiSavingsAccount()
        {
            IAccount maxiSavingsAccount = AccountFactory.CreateAccount(AccountFactory.MAXI_SAVINGS);
            Customer henry = new Customer("Henry").OpenAccount(maxiSavingsAccount);
            maxiSavingsAccount.Deposit(1500.0);
            maxiSavingsAccount.Withdraw(500.0);
            maxiSavingsAccount.Deposit(500.0);
            Assert.AreEqual(3,maxiSavingsAccount.GetAllTransactions().Count);
           
        }
        
        /// <summary>
        /// Tests data in a transaction are correct or not
        /// </summary>
        [TestMethod]
        public void CheckTransactionFieldsAreCorrect()
        {
            IAccount savingsAccount = AccountFactory.CreateAccount(AccountFactory.SAVINGS);
            Customer henry = new Customer("Henry").OpenAccount(savingsAccount);
            savingsAccount.Deposit(1500.0);
            Transaction trans = new Transaction(1500, 0, TransactionType.Deposit);
            Assert.IsTrue(savingsAccount.GetAllTransactions().Exists(p=>(p.Balance == 1500 && p.TransactionAmount == 1500 && (string.Compare(p.TransactionType.ToString(),TransactionType.Deposit.ToString())==0))));
        }
        #endregion

    }
    #endregion
}
