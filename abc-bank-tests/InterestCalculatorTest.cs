using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    #region InterestCalculatorTest
    /// <summary>
    /// Summary description for InterestCalculatorTest
    /// </summary>
    [TestClass]
    public class InterestCalculatorTest
    {

        // Use http://www.pine-grove.com/online-calculators/compound-interest-calculator.htm for expected results
         private static readonly double DOUBLE_DELTA = 1e-2;

        [TestMethod]
        public void TestDailyAccruedInterestWithDays()
        {
           
            // 1 day daily accured iterest
            double interest = InterestCalculator.calculateDailyAccruedInterest(10000, 10, 1);
            Assert.AreEqual(2.74, interest, DOUBLE_DELTA);

            // 30 days / 1 month daily accured iterest
            interest = InterestCalculator.calculateDailyAccruedInterest(10000, 10, 30);
            Assert.AreEqual(82.52, interest, DOUBLE_DELTA);

            // 365 days / 1 year daily accured iterest
           interest = InterestCalculator.calculateDailyAccruedInterest(10000, 10, 365);
           Assert.AreEqual(1051.56, interest, DOUBLE_DELTA);

           // 2 years daily accured iterest
           interest = InterestCalculator.calculateDailyAccruedInterest(10000, 10, 2 * 365);
           Assert.AreEqual(2213.69, interest, DOUBLE_DELTA);


           // Wrong inputs
           interest = InterestCalculator.calculateDailyAccruedInterest(0, 0, 0);
           Assert.AreEqual(0, interest, DOUBLE_DELTA);

           interest = InterestCalculator.calculateDailyAccruedInterest(-1000, -10, -1);
           Assert.AreEqual(0, interest, DOUBLE_DELTA);
        }

        [TestMethod]
        public void TestDailyAccruedInterestWithDates()
        {
            // 1 day daily accured iterest
            double interest = InterestCalculator.calculateAccruedInterest(10000, 10, DateTime.Parse("2015/01/01"), DateTime.Parse("2015/01/02"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(2.74, interest, DOUBLE_DELTA);

            // 30 days / 1 month daily accured iterest
            interest = InterestCalculator.calculateAccruedInterest(10000, 10, DateTime.Parse("2015/01/01"), DateTime.Parse("2015/01/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(82.52, interest, DOUBLE_DELTA);

            // 365 days / 1 year daily accured iterest
            interest = InterestCalculator.calculateAccruedInterest(10000, 10, DateTime.Parse("2015/01/01"), DateTime.Parse("2016/01/01"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(1051.56, interest, DOUBLE_DELTA);

            // 2 years daily accured iterest
            interest = InterestCalculator.calculateAccruedInterest(10000, 10, DateTime.Parse("2015/01/01"), DateTime.Parse("2016/12/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(2213.69, interest, DOUBLE_DELTA);


            // Wrong inputs
            interest = InterestCalculator.calculateAccruedInterest(10000, 10, DateTime.Parse("2015/01/01"), DateTime.Parse("2014/01/01"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(0, interest, DOUBLE_DELTA);
        }


        [TestMethod]
        public void TestDailyAccruedInterestWithSingleTransaction()
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(new Transaction(10000, 10000, TransactionType.Deposit, DateTime.Parse("2015/01/01")));

            // 1 day daily accured iterest
            double interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/01/02"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(2.74, interest, DOUBLE_DELTA);

            // 30 days / 1 month daily accured iterest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/01/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(82.52, interest, DOUBLE_DELTA);

            // 2 years daily accured iterest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2016/12/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(2213.69, interest, DOUBLE_DELTA);


            // Wrong inputs
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2014/01/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(0, interest, DOUBLE_DELTA);
        }

        [TestMethod]
        public void TestDailyAccruedInterestWithMultipleTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(new Transaction(5000, 5000, TransactionType.Deposit, DateTime.Parse("2015/01/01 09:00:00")));
            transactions.Add(new Transaction(2000, 3000, TransactionType.WithDraw, DateTime.Parse("2015/01/01 10:00:00")));
            transactions.Add(new Transaction(7000, 10000, TransactionType.Deposit, DateTime.Parse("2015/01/01 11:00:00")));

            // 1 day daily accured iterest
            double interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/01/02"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(2.74, interest, DOUBLE_DELTA);

            // 30 days / 1 month daily accured iterest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/01/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(82.52, interest, DOUBLE_DELTA);

            // 60 Days interest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/03/02"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(165.72, interest, DOUBLE_DELTA);

            // 90 Days interest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/04/01"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(249.61, interest, DOUBLE_DELTA);

            // Add one more 10K deposit , 30 days after 1st deposit
            transactions.Add(new Transaction(10000, 20000, TransactionType.Deposit, DateTime.Parse("2015/01/31 09:00:00")));

            // 60 Days intetrest (2 months of 10K + 1 month of 10K)
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/03/02"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(165.72 + 82.52, interest, DOUBLE_DELTA);

            // Add one more 10K deposit (2K - 1k + 9k), 60 days after 1st deposit
            transactions.Add(new Transaction(2000, 22000, TransactionType.Deposit, DateTime.Parse("2015/03/02 09:00:00")));
            transactions.Add(new Transaction(1000, 21000, TransactionType.WithDraw, DateTime.Parse("2015/03/02 09:00:00")));
            transactions.Add(new Transaction(9000, 30000, TransactionType.Deposit, DateTime.Parse("2015/03/02 09:00:00")));

            // 60 Days intetrest (new deposit just added shd not be accrued)
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/03/02"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(165.72 + 82.52, interest, DOUBLE_DELTA);

            // 90 Days intetrest (3 months of 10K + 2 month of 10K + 1 month of 10K)
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/04/01"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(249.61 + 165.72 + 82.52, interest, DOUBLE_DELTA);


            // Wrong inputs
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2014/01/31"), InterestType.COMPOUNDED_DAILY);
            Assert.AreEqual(0, interest, DOUBLE_DELTA);
        }



        [TestMethod]
        public void TestSimpleInterestWithDays()
        {
           // 1 day simple interest
            double interest = InterestCalculator.calculateSimpleInterest(10000, 10, 1);
            Assert.AreEqual(2.74, interest, DOUBLE_DELTA);

            // 30 days simple accured interest
            interest = InterestCalculator.calculateSimpleInterest(10000, 10, 30);
            Assert.AreEqual(82.19, interest, DOUBLE_DELTA);

            // 365 days simple interest
            interest = InterestCalculator.calculateSimpleInterest(10000, 10, 365);
            Assert.AreEqual(1000, interest, DOUBLE_DELTA);

            // 2 years simple iterest
            interest = InterestCalculator.calculateSimpleInterest(10000, 10, 2 * 365);
            Assert.AreEqual(2000, interest, DOUBLE_DELTA);

            // Wrong inputs
            interest = InterestCalculator.calculateSimpleInterest(0, 0, 0);
            Assert.AreEqual(0, interest, DOUBLE_DELTA);

            interest = InterestCalculator.calculateDailyAccruedInterest(-1000, -10, -1);
            Assert.AreEqual(0, interest, DOUBLE_DELTA);
        }


        [TestMethod]
        public void TestSimpleInterestWithSingleTransaction()
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(new Transaction(10000, 10000, TransactionType.Deposit, DateTime.Parse("2015/01/01")));

            // 1 day simple iterest
            double interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/01/02"), InterestType.SIMPLE_INTEREST);
            Assert.AreEqual(2.74, interest, DOUBLE_DELTA);

            // 30 days simple iterest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2015/01/31"), InterestType.SIMPLE_INTEREST);
            Assert.AreEqual(82.19, interest, DOUBLE_DELTA);

            // 2 years simple iterest
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2016/12/31"), InterestType.SIMPLE_INTEREST);
            Assert.AreEqual(2000, interest, DOUBLE_DELTA);

            // Wrong inputs
            interest = InterestCalculator.calculateAccruedInterest(transactions, 10, DateTime.Parse("2014/01/31"), InterestType.SIMPLE_INTEREST);
            Assert.AreEqual(0, interest, DOUBLE_DELTA);
        }

    }
    #endregion
}
