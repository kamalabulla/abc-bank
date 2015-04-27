using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    /// <summary>
    /// Enum Transaction Type
    /// </summary>
    public enum TransactionType
    {
        Deposit = 1,
        WithDraw = 2,
    }

    /// <summary>
    /// Class Transaction
    /// </summary>
    public class Transaction
    {
        #region Private Fields
        private double transactionAmount;
        private double balance;
        private DateTime transactionDate;
        private TransactionType transactionType;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates Transaction object
        /// </summary>
        /// <param name="amount">amount to be updated in trasaction</param>
        /// <param name="balance">total balance in the account</param>
        /// <param name="transType">transaction type</param>
        public Transaction(double amount, double balance, TransactionType transType) : this(amount, balance, transType, DateProvider.Instance.Now)
        {
        }

        /// <summary>
        /// Creates Transaction object
        /// </summary>
        /// <param name="amount">amount to be updated in trasaction</param>
        /// <param name="balance">total balance in the account</param>
        /// <param name="transType">transaction type</param>
        /// <param name="transactionDate">transaction date</param>
        public Transaction(double amount, double balance, TransactionType transType, DateTime transactionDate)
        {
            this.transactionAmount = amount;
            this.balance = balance;
            this.transactionDate = transactionDate;
            this.transactionType = transType;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Transaction Amount
        /// </summary>
        public double TransactionAmount
        {
            get
            {
                return transactionAmount;
            }
        }
       
        /// <summary>
        /// Total Balance
        /// </summary>
        public double Balance
        {
            get
            {
                return balance;
            }
        }

        /// <summary>
        /// Transaction Date
        /// </summary>
        public DateTime TransactionDate
        {
            get
            {
                return transactionDate;
            }
        }

        /// <summary>
        /// Transacion Type
        /// </summary>
        public TransactionType TransactionType
        {
            get
            {
                return transactionType;
            }
        }

        #endregion

    }
}
