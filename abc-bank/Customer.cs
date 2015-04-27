using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    /// <summary>
    /// Class Customer
    /// </summary>
    public class Customer
    {
        #region Private Fields
        private String name;
        private Dictionary<String, IAccount> accounts;
        private Object syncLock = new Object();
        #endregion
        
        #region Constructor
        /// <summary>
        /// Creates Customer object
        /// </summary>
        /// <param name="name">
        /// Name of the Customer
        /// </param>
        public Customer(String name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Customer name cannot be empty");
            }

            this.name = name;
            this.accounts = new Dictionary<String, IAccount>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets Name of the Customer.
        /// </summary>
        /// <returns></returns>
        public String Name  
        {
            get
            {
                return name;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens an account for the customer
        /// </summary>
        /// <param name="account">
        /// Account details
        /// </param>
        /// <returns>
        /// The Customer 
        /// </returns>
        public Customer OpenAccount(IAccount account)
        {
            lock (syncLock ) 
            {
                // Make sure that there is no existing account with the same id
                if (accounts.ContainsKey(account.Id))
                {
                    throw new Exception("There is already an account opened with the id: " + account.Id);
                }
               
                accounts.Add(account.Id, account);
                return this;
            }
        }
        /// <summary>
        /// Gets the number of accounts of a customer
        /// </summary>
        /// <returns>
        /// Number of accounts
        /// </returns>
        public int GetNumberOfAccounts()
        {
            lock (syncLock)
            {
                return accounts.Count;
            }
        }
        /// <summary>
        /// Gives the total interest earned on all account
        /// </summary>
        /// <returns>
        /// Returns total interest earned 
        /// </returns>
        public double TotalInterestEarned() 
        {
             lock (syncLock ) 
             {
                double total = 0;
                foreach (IAccount a in accounts.Values)
                {
                    total += a.InterestEarned();
                }
                return total;
            }
        }
        /// <summary>
        /// Gets the statemnt for all accounts
        /// </summary>
        /// <returns>
        /// Returns Statemnt
        /// </returns>
        public String GetStatement() 
        {
             String statement = null;
            statement = "Statement for " + name + "\n";
            lock (syncLock)
            {
                double total = 0.0;
                foreach (IAccount a in accounts.Values)
                {
                    double currentBalance;
                    // Compute the statement and current balance in one go to get a consistent state
                    statement += "\n" + statementForAccount(a, out currentBalance) + "\n";
                    total += currentBalance;
                }

                statement += "\nTotal In All Accounts " + ToDollars(total);
            }
            return statement;
        }
        /// <summary>
        /// Transfers Amount between to accounts of a customer
        /// </summary>
        /// <param name="amount">Amount to be transfered</param>
        /// <param name="fromAccountId">From account id</param>
        /// <param name="toAccountId">To account id</param>
        public void TransferAmount(double amount, String fromAccountId, String toAccountId)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero");
            }

            lock (syncLock)
            {
                IAccount fromAccount = validateAndGetAccount(fromAccountId);
                IAccount toAccount = validateAndGetAccount(toAccountId);
                
                // Withdraw the amunt from the source account
                fromAccount.Withdraw(amount);

                try
                {
                    // Deposit the amount in the target account
                    toAccount.Deposit(amount);
                }
                catch (Exception e)
                {
                    // If the desposit fails in the target account, depost it back in the source account
                    fromAccount.Deposit(amount);
                    throw new Exception("Could not deposit the amount in the account: " + toAccountId + ", Reason: " + e.Message);
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gives statemnt for  each account
        /// </summary>
        /// <param name="account">account details</param>
        /// <returns>The statemnt</returns>
        private String statementForAccount(IAccount account, out double currentBalance) 
        {
            String s = account.AccountType + "\n";

            // Now total up all the transactions
            double total = 0.0;

            foreach (Transaction t in account.GetAllTransactions())
            {
                s += "  " + (t.TransactionAmount < 0 ? "withdrawal" : "deposit") + " " + ToDollars(t.TransactionAmount) + "\n";
                total += t.TransactionAmount;
            }

            s += "Total " + ToDollars(total);
            currentBalance = total;
            return s;
        }

        /// <summary>
        /// Converts double to string comma seperated with two decimals.
        /// </summary>
        /// <param name="d">amount to be comnverted</param>
        /// <returns>string format of double</returns>
        private String ToDollars(double d)
        {
            return String.Format("${0:0,00.00}", Math.Abs(d));
        }

        /// <summary>
        /// Validates and Gets account give account Id
        /// </summary>
        /// <param name="accountId">account ID</param>
        /// <returns>Account details</returns>
        private IAccount validateAndGetAccount(String accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                throw new ArgumentException("AccountId cannot be null");
            }

            lock (syncLock)
            {
                if (!accounts.ContainsKey(accountId))
                {
                    throw new ArgumentException("Cannot find the account with the id: " + accountId);
                }

                return accounts[accountId];
            }
        }
        #endregion

    }
}
