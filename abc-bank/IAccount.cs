using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace abc_bank
{
  
    /// <summary>
    /// Interface for Account
    /// </summary>
    public interface IAccount
    {
        #region Properties
        /// <summary>
        /// Gets the Account ID
        /// </summary>
        String Id
        {
            get;
        }

        /// <summary>
        /// Gets Account Type
        /// </summary>
        String AccountType
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deposits Amount in to account
        /// </summary>
        /// <param name="amount">amount to be deposited</param>
        void Deposit(double amount);

        /// <summary>
        /// With draws amount from the account
        /// </summary>
        /// <param name="amount">Amount to with draw</param>
        void Withdraw(double amount);

        /// <summary>
        /// Gets List of all tranasctions
        /// </summary>
        /// <returns>All transactions</returns>
        List<Transaction> GetAllTransactions();

        /// <summary>
        /// Gets current balance in the account
        /// </summary>
        /// <returns>amount</returns>
        double GetCurrentBalance();

        /// <summary>
        /// Calculates interest earned
        /// </summary>
        /// <returns>amount</returns>
        double InterestEarned();
        #endregion
    }
}
