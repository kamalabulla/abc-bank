using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    #region Date Provider
    /// <summary>
    /// Class Date Provider
    /// </summary>
    public class DateProvider
    {
        #region Private Fields     
        private static readonly Lazy<DateProvider> lazy = new Lazy<DateProvider>(() => new DateProvider());

        private Nullable<DateTime> simulatedCurrentTime = null;
        private readonly Object syncLock = new Object();
        #endregion

        #region Constructor
        private DateProvider()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Instance
        /// </summary>
        public static DateProvider Instance 
        { 
            get
            {
                return lazy.Value;
            } 
        }

        /// <summary>
        /// Gets Current DateTime
        /// </summary>
        public DateTime Now
        {
            get
            {
                // TODO Expensive, but works for now, optimize it later
                lock (syncLock)
                {
                    // If there is a simulated curent time, return it. (used in unit testing)
                    if (simulatedCurrentTime != null)
                    {
                        return (DateTime) simulatedCurrentTime;
                    }
                }

                return DateTime.Now;
            }
        }

        /// <summary>
        /// Set the simulated curren time for using testing
        /// </summary>
        /// <param name="simulatedCurrentTime"></param>
        public void setSimulatedCurrentTime(Nullable<DateTime> simulatedCurrentTime)
        {
            lock (syncLock)
            {
                this.simulatedCurrentTime = simulatedCurrentTime;
            }
        }

        #endregion
    }
    #endregion
}
