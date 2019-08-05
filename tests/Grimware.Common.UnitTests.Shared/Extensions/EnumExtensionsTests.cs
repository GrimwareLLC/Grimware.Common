using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void Add()
        {

        }

        [Flags]
        private enum TestFlags
        {
            None = 0,

            BitOne = 1,
            BitTwo = 2,
            BitThree = 4,
            BitFour = 8,
            BitFive = 16,
            BitSix = 32,
            BitSeven = 64,
            BitEight = 128,

            All = 0xFF,
        }
    }
}
