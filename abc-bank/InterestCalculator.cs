using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace abc_bank
{
    // Interest type
    public enum InterestType
    {
        SIMPLE_INTEREST, 
        COMPOUNDED_DAILY
        // We can add more later
    }

    public class InterestCalculator
    {
        /// <summary>
        /// Calculates the accrued interest basedon the given parameters
        /// </summary>
        /// <param name="transactions">account's transactions</param>
        /// <param name="annualInterestPercentRate">annual interest rate in percent</param>
        /// <param name="currentDate">current date</param>
        /// <param name="interestType">interest type</param>
        /// <returns>the interest calculated</returns>
        public static double calculateAccruedInterest(List<Transaction> transactions, double annualInterestPercentRate, DateTime currentDate, InterestType interestType)
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
                        annualInterestPercentRate, lastTransactionDate, transactionDate, interestType);
                }

                // Save last balance and transaction
                lastBalance = transaction.Balance;
                lastTransactionDate = transactionDate;
            }

            // Calculate the interest from the latest transaction's balance to current date
            totalInterest += calculateAccruedInterest(lastBalance, totalInterest,
                annualInterestPercentRate, lastTransactionDate, currentDate, interestType);


            return totalInterest;
        }

        public static double calculateAccruedInterest(double principal, double prevInterest, double annualInterestPercentRate, DateTime startingDate, DateTime endingDate, InterestType interestType)
        {
            double amount = principal;

            // Add interest to principal for non simple interests, this logic will need change when we add more types later
            if (interestType != InterestType.SIMPLE_INTEREST)
            {
                amount += prevInterest;
            }

            // Compute the number of days between starting and ending transaction dates
            // This is used to get the pro-rata interest rate
            int daysElapsed = endingDate.Date.Subtract(startingDate.Date).Days;

            // Call appropriate interest calculaiton method based on the interest type
            switch (interestType)
            {
                case InterestType.SIMPLE_INTEREST:
                    return calculateSimpleInterest(amount, annualInterestPercentRate, daysElapsed);
                case InterestType.COMPOUNDED_DAILY:
                    return calculateDailyAccruedInterest(amount, annualInterestPercentRate, daysElapsed);
                default:
                    throw new ArgumentException("Unknown interest type: " + interestType);
            }         
        }

        public static double calculateAccruedInterest(double principal, double annualInterestPercentRate, DateTime startingDate, DateTime endingDate, InterestType interestType)
        {
            return calculateAccruedInterest(principal, 0, annualInterestPercentRate, startingDate, endingDate, interestType);
        }

        public static double calculateDailyAccruedInterest(double principal, double annualInterestPercentRate, int noOfDays)
        {
            // Basic validations
            if (principal <= 0 || annualInterestPercentRate <= 0 || noOfDays <= 0)
            {
                return 0;
            }

            // Calculate interest based on daily compouding/accruing (see http://www.mathwarehouse.com/compound-interest/formula-calculate.php)
            double annualInterestRateRatio = annualInterestPercentRate / 100;
            return (principal * Math.Pow((1 + (annualInterestRateRatio / 365)), noOfDays)) - principal;
        }

        public static double calculateSimpleInterest(double principal, double annualInterestPercentRate, int noOfDays)
        {
            // Basic validations
            if (principal <= 0 || annualInterestPercentRate <= 0 || noOfDays <= 0)
            {
                return 0;
            }

            double annualInterestRateRatio = annualInterestPercentRate / 100;
            return principal * (annualInterestRateRatio) * noOfDays / 365;
        }
    }
}
