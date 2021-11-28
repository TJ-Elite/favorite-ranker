// © 2021 Tuukka Junnikkala

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FavoriteRankerLibrary.Logic;
using FavoriteRankerLibrary.UnitTests.Mocking;

namespace FavoriteRankerLibrary.UnitTests.Tests
{
    [TestClass]
    public class RankerLogicTests
    {
        /// <summary>
        /// Tests that the Run method throws an exception if it's called while passing in a null reference.
        /// </summary>
        [TestMethod]
        public void Run_ArgumentIsNull_ThrowsArgumentNullException()
        {
            IUserInterface nullInterface = null;

            Assert.ThrowsException<ArgumentNullException>(() => RankerLogic.Run(nullInterface));
        }

        [TestMethod]
        public void Run_Numbers1Through100AreRanked_ProducesTheCorrectRanking()
        {
            const uint totalEntries = 100;
            if (totalEntries > RankerLogic.MaximumEntries)
            {
                throw new Exception();
            }
            var ui = new MockInterface(totalEntries);

            RankerLogic.Run(ui);

            Assert.IsFalse(ui.TestFailed);
        }
    }
}
