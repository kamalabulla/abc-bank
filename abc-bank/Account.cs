using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    #region Account
    /// <summary>
    /// Abstract account class
    /// </summary>
    public abstract class Account : IAccount
    {
        #region Private Fields
        private string id;
        private string type;
        private double currentBalance = 0;
        private List<Transaction> transactions;
        private readonly Object transactionLock = new Object();
        #endregion

        #region Constructor
        /// <summary>
        /// Account Constructor
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="acntType">account type</param>
        /// <param name="currentbal">amount to be deposited</param>
        public Account(String accountId, String acntType, double currentbal) : this(accountId, acntType, currentbal, new List<Transaction>())
        {
        }

        /// <summary>
        /// Account  Full Constructor
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="acntType">account type</param>
        /// <param name="currentbal">amount to be deposited</param>
        /// <param name="transactions">all the transactions</param>
        public Account(String accountId, String acntType, double currentbal, List<Transaction> transactions)
        {
            id = accountId;
            type = acntType;
            currentBalance = currentbal;
            this.transactions = transactions;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Account Id
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Accunt Type
        /// </summary>
        public string AccountType
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Give transacion lock to sub classes so that they can use transactions in a thread-safe manner
        /// </summary>
        protected Object TransactionLock 
        {
            get
            { 
                return transactionLock;
            }
        }

        /// <summary>
        /// Give transacions access to sub classes so that they can use transactions directly
        /// </summary>
        protected List<Transaction> Transactions 
        {
            get 
            { 
                return transactions;
            } 
        }

        #endregion

        #region Public Methods
        /// <summary>
        ///Deposit amount in Account 
        /// </summary>
        /// <param name="amount">amount to be deposited</param>
        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than zero");
            }
            else
            {
                // Update bothe current balance and transactions in a single trasaction lock
                lock (transactionLock)
                {
                    currentBalance += amount;
                    transactions.Add(new Transaction(amount, currentBalance,TransactionType.Deposit));
                }
            }
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <returns>List of Transaction</returns>
        public List<Transaction> GetAllTransactions()
        {
            // TODO use thread safe enumerable read only collections from .net framework 4.5
            // I do not have the latest framework in my personal dev environment
            lock (transactionLock)
            {
                // For now, create a new list so that user can not modify transactions
                return new List<Transaction>(transactions);
            }
        }

        /// <summary>
        /// Get current balance
        /// </summary>
        /// <returns>current balance in the account</returns>
        public double GetCurrentBalance()
        {
            lock (transactionLock)
            {
                return currentBalance;
            }
        }

        /// <summary>
        /// Withdraws amount from the account
        /// </summary>
        /// <param name="amount">amount to withdraw</param>
        public void Withdraw(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than zero");
            }

            lock (transactionLock)
            {
                // Make sure to have sufficient funds before withdrawl
                if (amount > currentBalance)
                {
                    throw new ArgumentException(string.Format("insufficient funds: current balance is: {0}", currentBalance));
                }
                else
                {
                    currentBalance -= amount;
                    transactions.Add(new Transaction(-amount, currentBalance, TransactionType.WithDraw));
                }
            }
        }

        /// <summary>
        /// interest earned - to be implemented by repsective accounts based on the interest rates
        /// </summary>
        /// <returns></returns>
        public abstract double InterestEarned();
        #endregion
    }
    #endregion
}



