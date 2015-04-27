using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace abc_bank
{
    #region MaxiSavings Account
    /// <summary>
    /// Class MaxiSavings Account
    /// </summary>
    public class MaxiSavingsAccount : Account
    {
        #region Constructor
        /// <summary>
        /// Creates MaxiSavingsAccount
        /// </summary>
        /// <param name="accountId">Account Id</param>
        public MaxiSavingsAccount(string accountId) : base(accountId, "Maxi Savings Account", 0)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Interest Earned
        /// </summary>
        /// <returns>Amount</returns>
        public override double InterestEarned()
        {
            lock (TransactionLock)
            {
                return calculateAccruedInterest(Transactions, DateTime.Now.Date, InterestType.COMPOUNDED_DAILY);
            }

            // Old Code
            //double amount = GetCurrentBalance();
            //if (amount <= 1000)
            //    return amount * 0.02;
            //if (amount <= 2000)
            //    return 20 + (amount - 1000) * 0.05;
            //return 70 + (amount - 2000) * 0.1;
        }
        #endregion

        private static double calculateAccruedInterest(List<Transaction> transactions, 
            DateTime currentDate, InterestType interestType)
        {
            double lastBalance = 0;
            DateTime lastTransactionDate = DateTime.MinValue;
            double totalInterest = 0;
            DateTime lastWithdrawlDate = DateTime.MinValue;

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
                    // Calculate interest from last transaction to this transaction date (considers 10-day withdrawsl, etc)
                    // Pass the total interest accrued so far also, so that compounding can be done based on interest type
                    // Pass last withdrawl date
                    totalInterest += calculateAccruedInterest(lastBalance, totalInterest,
                         lastTransactionDate, transactionDate, lastWithdrawlDate, interestType);
                }


                // Save last balance and transaction
                lastBalance = transaction.Balance;
                lastTransactionDate = transactionDate;


                // Maintain last withdrawl date (to be used in interest rate calculations)
                if (transaction.TransactionType == TransactionType.WithDraw)
                {
                    lastWithdrawlDate = transactionDate;
                }
            }

            // Calculate the interest from the latest transaction's balance to current date
            totalInterest += calculateAccruedInterest(lastBalance, totalInterest,
                         lastTransactionDate, DateTime.Now.Date, lastWithdrawlDate, interestType);


            return totalInterest;
        }

        private static double calculateAccruedInterest(double lastBalance, double totalInterest, DateTime startingDate, DateTime endingDate, DateTime lastWithdrawlDate, InterestType interestType)
        {
            // If there are no wothdraws, set the last withdrawl to starting date
            if (lastWithdrawlDate == DateTime.MinValue)
            {
                lastWithdrawlDate = startingDate;
            }

            // Calculate number of days since last withdrawl to the starting transaction date
            int daysSinceLastWithdrawl = startingDate.Subtract(lastWithdrawlDate).Days;

            // If there are no withdralws in the last 10 days, use 5% rate
            if (daysSinceLastWithdrawl > 10)
            {
                return InterestCalculator.calculateAccruedInterest(lastBalance, totalInterest, 5, startingDate, endingDate, interestType);
            }

            // If the span is more, we need to use different rates based on withdrawl date (like amount based rate slabs)
            DateTime nextInterestRateUpgradeDate = lastWithdrawlDate.AddDays(10);

            if (nextInterestRateUpgradeDate < endingDate)
            {
                // use 0.1% rate until 10-day withdrawl rule breaks, and use 5% after that date
                double interest = InterestCalculator.calculateAccruedInterest(lastBalance, totalInterest, 0.1, startingDate, nextInterestRateUpgradeDate, interestType);
                return interest + InterestCalculator.calculateAccruedInterest(lastBalance, totalInterest + interest, 5, nextInterestRateUpgradeDate, endingDate, interestType);
            }
            else
            {
                // Use 0.1% rate if there are no withdrawls in the past 10 days
                return InterestCalculator.calculateAccruedInterest(lastBalance, totalInterest, 0.1, startingDate, endingDate, interestType);
            }        
        }
    }
    #endregion

    
}
