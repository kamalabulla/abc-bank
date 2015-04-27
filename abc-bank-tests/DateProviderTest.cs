using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using abc_bank;

namespace abc_bank_tests
{
    #region DateProviderTest
    /// <summary>
    /// Summary description for DateProviderTest
    /// </summary>
    [TestClass]
    public class DateProviderTest
    {
        #region TestMethods
        /// <summary>
        /// Tests date and time
        /// </summary>
        [TestMethod]
        public void TestGetCurrentDateTime()
        {
            // This may fail due to OS returning different times at different times
            // Assert.AreEqual(DateTime.Now, DateProvider.Instance.Now);

             double actualMillis = (DateProvider.Instance.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            double expectedMillis = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

            double difference = Math.Abs(expectedMillis - actualMillis);

            // Should not be off by more than 10 seconds, unless the computation itself takes that long
            Assert.IsTrue(difference < 10000);        
        }

        [TestMethod]
        public void TestGetCurrentDateTimeBySimulatedTime()
        {
             // Set the simulation date time
            DateTime simulatedDateTime = DateTime.Parse("2015/01/01 09:00:00");
            DateProvider.Instance.setSimulatedCurrentTime(simulatedDateTime);

            Assert.AreEqual(simulatedDateTime, DateProvider.Instance.Now);

            // Reset to null
            DateProvider.Instance.setSimulatedCurrentTime(null);

            double actualMillis = (DateProvider.Instance.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            double expectedMillis = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

            double difference = Math.Abs(expectedMillis - actualMillis);

            // Should not be off by more than 10 seconds, unless the computation itself takes that long
            Assert.IsTrue(difference < 10000);       
        }

        #endregion
    }
    #endregion
}
