// © 2021 Tuukka Junnikkala

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FavoriteRankerLibrary.Logic;

namespace FavoriteRankerLibrary.UnitTests.Tests
{
    [TestClass]
    public class RankerHelperTests
    {
        [TestMethod]
        public void GetOpposite_InputBetter_ReturnsWorse()
        {
            const Relation input = Relation.Better;
            const Relation expected = Relation.Worse;

            var actual = input.GetOpposite();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetOpposite_InputWorse_ReturnsBetter()
        {
            const Relation input = Relation.Worse;
            const Relation expected = Relation.Better;

            var actual = input.GetOpposite();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetOpposite_InputNone_ReturnsNone()
        {
            const Relation input = Relation.None;
            const Relation expected = Relation.None;

            var actual = input.GetOpposite();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LimitNameLength_InputLongString_ReturnsMaxLengthString()
        {
            string expected = "A";
            while (expected.Length < RankerLogic.MaximumNameLength)
            {
                expected += "A";
            }
            string input = $"   {expected}   B   {expected}   ";

            string actual = RankerHelper.LimitNameLength(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
