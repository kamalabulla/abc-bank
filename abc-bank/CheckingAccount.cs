using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace abc_bank
{
    /// <summary>
    /// Checking Account Class 
    /// </summary>
    public class CheckingAccount : Account
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountId">account id</param>
        public CheckingAccount(string accountId) : base(accountId, "Checking Account", 0)
        {       
        }

        public CheckingAccount(String accountId, double currentbal, List<Transaction> transactions)
            : base(accountId, "Checking Account", currentbal, transactions)
        {
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Caculates interest earned in the checking account
        /// </summary>
        /// <returns>amount</returns>
        public override double InterestEarned()
        {
            lock (TransactionLock)
            {
                // Use 0.1% rate accrued daily
                return InterestCalculator.calculateAccruedInterest(Transactions, 0.1, DateTime.Now, InterestType.COMPOUNDED_DAILY);
            }
        }
        #endregion

    }
 
}
