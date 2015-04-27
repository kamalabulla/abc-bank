using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace abc_bank
{
    #region Savings Account
    /// <summary>
    /// Class Savings account 
    /// </summary>
    public class SavingsAccount : Account
    {
        #region Constructor
        /// <summary>
        /// Creates Savings Account Object
        /// </summary>
        /// <param name="accountId">account Id</param>
        public SavingsAccount(string accountId)
            : base(accountId, "Savings Account", 0)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Calculates Interest earned
        /// </summary>
        /// <returns>amount</returns>
        public override double InterestEarned()
        {
            lock (TransactionLock)
            {
                return calculateAccruedInterest(Transactions, DateTime.Now, InterestType.COMPOUNDED_DAILY);
            }
        }

        public static double calculateAccruedInterest(List<Transaction> transactions, DateTime currentDate, InterestType interestType)
        {
            double lastBalance = 0;
            DateTime lastTransactionDate = DateTime.MinValue;
            double totalInterest = 0;

            // Iterate over each transaction
            foreach (Transaction transaction in transactions)
            {
                // Save the transaction date (date part only)
                DateTime transactionDate = transaction.TransactionDate.Date;

                // Consider transactions prior to current date (current date may be changed in unit test simulations)
                if (transactionDate > currentDate.Date)
                {
                    break;
                }

                // Whenever there is a date change in the transaction date, calculate the interest for the balance so far
                if (lastTransactionDate != DateTime.MinValue && transactionDate != lastTransactionDate)
                {
                    // Calculate interest from last transaction to this transaction date (pro-rata as interest rate is in annual percent)
                    // Pass the total interest accrued so far also, so that compounding can be done based on interest type
                    totalInterest += calculateAccruedInterest(lastBalance, totalInterest,
                       lastTransactionDate, transactionDate, interestType);
                }

                // Save last balance and transaction
                lastBalance = transaction.Balance;
                lastTransactionDate = transactionDate;
            }

            // Calculate the interest from the latest transaction's balance to current date
            totalInterest += calculateAccruedInterest(lastBalance, totalInterest, lastTransactionDate, currentDate, interestType);

            return totalInterest;
        }

        private static double calculateAccruedInterest(double lastBalance, double totalInterest, DateTime startingDate, DateTime endingDate, InterestType interestType)
        {
            // If the balance is less than $1000, use 0.1% rate
            if (lastBalance <= 1000)
            {
                return InterestCalculator.calculateAccruedInterest(lastBalance, totalInterest, 0.1, startingDate, endingDate, interestType);
            }

            // Otherwise, apply 0.1% on $1000, and 0.2% on balance over $1000
            double interest = InterestCalculator.calculateAccruedInterest(1000, totalInterest, 0.1, startingDate, endingDate, interestType);
            return interest + InterestCalculator.calculateAccruedInterest((lastBalance - 1000), (totalInterest + interest), 0.2, startingDate, endingDate, interestType);
        }

        #endregion
    }
    #endregion
}