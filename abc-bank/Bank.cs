using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    #region Bank
    /// <summary>
     /// Class Bank   
     /// </summary>
    public class Bank
    {
        #region Private Fields 
        private List<Customer> customers;
        private Object syncLock = new Object();
        #endregion

        #region Constructor
        /// <summary>
        /// Creates Bank object
        /// </summary>
        public Bank()
        {
            customers = new List<Customer>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds new customer to the bank
        /// </summary>
        /// <param name="customer">Customer</param>
        public void AddCustomer(Customer customer)
        {
            lock (syncLock)
            {
                // Validate customer name to make sure that there is no existing customer with the same name
                if (customers.Exists(p => String.Compare(p.Name, customer.Name, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    throw new ArgumentException("There is already a customer with the same name: " + customer.Name);
                }

                customers.Add(customer);
            }
        }
        
        /// <summary>
        /// Gets Customer Summary
        /// </summary>
        /// <returns>returns customer aand account details</returns>
        public String CustomerSummary()
        {
            String summary = "Customer Summary";

            lock (syncLock)
            {
                foreach (Customer c in customers)
                {
                    summary += "\n - " + c.Name + " (" + format(c.GetNumberOfAccounts(), "account") + ")";
                }
            }

            return summary;
        }
        
        /// <summary>
        /// Calculates total Interest paid to all customers
        /// </summary>
        /// <returns>total value </returns>
        public double TotalInterestPaid() 
        {
            double total = 0;

            lock (syncLock)
            {
                foreach (Customer c in customers)
                {
                    total += c.TotalInterestEarned();
                }
            }

            return total;
        }

        /// <summary>
        ///  Gets first cusotmer in the bank
        /// </summary>
        /// <returns>First customer name</returns>
        public string GetFirstCustomer()
        {
            lock (syncLock)
            {
                // If there are no customers yet, return null
                if (customers.Count < 1)
                {
                    return null;
                }

                // Get the first customer name
                return customers[0].Name;
            }
        }
        #endregion

        #region Private Methods
        //Make sure correct plural of word is created based on the number passed in:
        //If number passed in is 1 just return the word otherwise add an 's' at the end
        private String format(int number, String word)
        {
            return number + " " + (number == 1 ? word : word + "s");
        }
        #endregion
    }
    #endregion
}
