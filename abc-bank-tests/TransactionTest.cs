using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    #region TransactionTest
    /// <summary>
    /// Test Class for Transaction
    /// </summary>
    [TestClass]
    public class TransactionTest
    {
        #region TestMethods
        /// <summary>
        /// Tests Creation of transaction instance and its data
        /// </summary>
        [TestMethod]
        public void Transaction()
        {
            Transaction t = new Transaction(500, 4000, TransactionType.Deposit);
            Assert.IsTrue(t.GetType() == typeof(Transaction));
            Assert.IsTrue(t.Balance == 4000);
            Assert.IsTrue(t.TransactionAmount == 500);
        }
        #endregion
    }
    #endregion
}
