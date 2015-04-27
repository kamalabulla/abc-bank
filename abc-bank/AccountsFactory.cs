using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace abc_bank
{
    /// <summary>
    /// Class Accounts Factory - creates account given type
    /// </summary>
    public class AccountFactory
    {
        #region Private Fields
        public const int CHECKING = 0;
        public const int SAVINGS = 1;
        public const int MAXI_SAVINGS = 2;
        private static int accountIdCounter = 1;

        private static readonly Object idLock = new Object();

        #endregion

        #region Constructor

        /// <summary>
        /// Create Account
        /// </summary>
        /// <param name="accountType">account type</param>
        /// <returns>Account</returns>
        public static IAccount CreateAccount(int accountType)
        {
            // Create new account id in a thread safe manner
            String newAccountId = "";
            lock (idLock)
            {
                accountIdCounter = accountIdCounter + 1;
                newAccountId = "Account-" + accountIdCounter.ToString();
            }
            
            switch (accountType)
            {
                case CHECKING: return new CheckingAccount(newAccountId);
                case SAVINGS: return new SavingsAccount(newAccountId);
                case MAXI_SAVINGS: return new MaxiSavingsAccount(newAccountId);
                default:
                    throw new ArgumentException("Unknown account type: " + accountType);
            }
        }
        #endregion
    }
}
