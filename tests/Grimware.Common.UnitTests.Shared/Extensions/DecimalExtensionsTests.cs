using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Grimware.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimware.Common.UnitTests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void ToDouble()
        {
            Decimal.MinValue.ToDouble().Should().Be((double)Decimal.MinValue);
            (-101.101M).ToDouble().Should().Be(-101.101D);
            (-1M).ToDouble().Should().Be(-1D);
            0M.ToDouble().Should().Be(0D);
            1M.ToDouble().Should().Be(1D);
            101.101M.ToDouble().Should().Be(101.101D);
            Decimal.MaxValue.ToDouble().Should().Be((double)Decimal.MaxValue);
        }

        [TestMethod]
        public void ToDouble_Nullable()
        {
            ((decimal?)null).ToDouble().Should().BeNull();
            ((decimal?)Decimal.MinValue).ToDouble().Should().Be((double)Decimal.MinValue);
            ((decimal?)-101.101M).ToDouble().Should().Be(-101.101);
            ((decimal?)-1M).ToDouble().Should().Be(-1);
            ((decimal?)0M).ToDouble().Should().Be(0);
            ((decimal?)1M).ToDouble().Should().Be(1);
            ((decimal?)101.101M).ToDouble().Should().Be(101.101);
            ((decimal?)Decimal.MaxValue).ToDouble().Should().Be((double)Decimal.MaxValue);
        }

        [TestMethod]
        public void ToInt16()
        {
            ((decimal)Int16.MinValue).ToInt16().Should().Be(Int16.MinValue);
            (-101.101M).ToInt16().Should().Be(-101);
            (-1M).ToInt16().Should().Be(-1);
            0M.ToInt16().Should().Be(0);
            1M.ToInt16().Should().Be(1);
            101.101M.ToInt16().Should().Be(101);
            ((decimal)Int16.MaxValue).ToInt16().Should().Be(Int16.MaxValue);
        }

        [TestMethod]
        public void ToInt16_Nullable()
        {
            ((decimal?)null).ToInt16().Should().BeNull();
            ((decimal?)Int16.MinValue).ToInt16().Should().Be(Int16.MinValue);
            ((decimal?)-101.101M).ToInt16().Should().Be(-101);
            ((decimal?)-1M).ToInt16().Should().Be(-1);
            ((decimal?)0M).ToInt16().Should().Be(0);
            ((decimal?)1M).ToInt16().Should().Be(1);
            ((decimal?)101.101M).ToInt16().Should().Be(101);
            ((decimal?)Int16.MaxValue).ToInt16().Should().Be(Int16.MaxValue);
        }


        [TestMethod]
        public void ToInt32()
        {
            ((decimal)Int32.MinValue).ToInt32().Should().Be(Int32.MinValue);
            (-101.101M).ToInt32().Should().Be(-101);
            (-1M).ToInt32().Should().Be(-1);
            0M.ToInt32().Should().Be(0);
            1M.ToInt32().Should().Be(1);
            101.101M.ToInt32().Should().Be(101);
            ((decimal)Int32.MaxValue).ToInt32().Should().Be(Int32.MaxValue);
        }

        [TestMethod]
        public void ToInt32_Nullable()
        {
            ((decimal?)null).ToInt32().Should().BeNull();
            ((decimal?)Int32.MinValue).ToInt32().Should().Be(Int32.MinValue);
            ((decimal?)-101.101M).ToInt32().Should().Be(-101);
            ((decimal?)-1M).ToInt32().Should().Be(-1);
            ((decimal?)0M).ToInt32().Should().Be(0);
            ((decimal?)1M).ToInt32().Should().Be(1);
            ((decimal?)101.101M).ToInt32().Should().Be(101);
            ((decimal?)Int32.MaxValue).ToInt32().Should().Be(Int32.MaxValue);
        }


        [TestMethod]
        public void ToInt64()
        {
            ((decimal)Int64.MinValue).ToInt64().Should().Be(Int64.MinValue);
            (-101.101M).ToInt64().Should().Be(-101);
            (-1M).ToInt64().Should().Be(-1);
            0M.ToInt64().Should().Be(0);
            1M.ToInt64().Should().Be(1);
            101.101M.ToInt64().Should().Be(101);
            ((decimal)Int64.MaxValue).ToInt64().Should().Be(Int64.MaxValue);
        }

        [TestMethod]
        public void ToInt64_Nullable()
        {
            ((decimal?)null).ToInt64().Should().BeNull();
            ((decimal?)Int64.MinValue).ToInt64().Should().Be(Int64.MinValue);
            ((decimal?)-101.101M).ToInt64().Should().Be(-101);
            ((decimal?)-1M).ToInt64().Should().Be(-1);
            ((decimal?)0M).ToInt64().Should().Be(0);
            ((decimal?)1M).ToInt64().Should().Be(1);
            ((decimal?)101.101M).ToInt64().Should().Be(101);
            ((decimal?)Int64.MaxValue).ToInt64().Should().Be(Int64.MaxValue);
        }

        [TestMethod]
        public void ToSingle()
        {
            Decimal.MinValue.ToSingle().Should().Be((float)Decimal.MinValue);
            (-101.101M).ToSingle().Should().Be((float)-101.101);
            (-1M).ToSingle().Should().Be(-1);
            0M.ToSingle().Should().Be(0);
            1M.ToSingle().Should().Be(1);
            101.101M.ToSingle().Should().Be((float)101.101);
            Decimal.MaxValue.ToSingle().Should().Be((float)Decimal.MaxValue);
        }

        [TestMethod]
        public void ToSingle_Nullable()
        {
            ((decimal?)null).ToSingle().Should().BeNull();
            ((decimal?)Decimal.MinValue).ToSingle().Should().Be((float)Decimal.MinValue);
            ((decimal?)-101.101M).ToSingle().Should().Be((float)-101.101);
            ((decimal?)-1M).ToSingle().Should().Be(-1);
            ((decimal?)0M).ToSingle().Should().Be(0);
            ((decimal?)1M).ToSingle().Should().Be(1);
            ((decimal?)101.101M).ToSingle().Should().Be((float)101.101);
            ((decimal?)Decimal.MaxValue).ToSingle().Should().Be((float)Decimal.MaxValue);
        }

        [TestMethod]
        public void ToUInt16()
        {
            ((decimal)UInt16.MinValue).ToUInt16().Should().Be(UInt16.MinValue);
            0M.ToUInt16().Should().Be(0);
            1M.ToUInt16().Should().Be(1);
            101.101M.ToUInt16().Should().Be(101);
            ((decimal)UInt16.MaxValue).ToUInt16().Should().Be(UInt16.MaxValue);
        }

        [TestMethod]
        public void ToUInt16_Nullable()
        {
            ((decimal?)null).ToUInt16().Should().BeNull();
            ((decimal?)UInt16.MinValue).ToUInt16().Should().Be(UInt16.MinValue);
            ((decimal?)0M).ToUInt16().Should().Be(0);
            ((decimal?)1M).ToUInt16().Should().Be(1);
            ((decimal?)101.101M).ToUInt16().Should().Be(101);
            ((decimal?)UInt16.MaxValue).ToUInt16().Should().Be(UInt16.MaxValue);
        }

        [TestMethod]
        public void ToUInt32()
        {
            ((decimal)UInt32.MinValue).ToUInt32().Should().Be(UInt32.MinValue);
            0M.ToUInt32().Should().Be(0);
            1M.ToUInt32().Should().Be(1);
            101.101M.ToUInt32().Should().Be(101);
            ((decimal)UInt32.MaxValue).ToUInt32().Should().Be(UInt32.MaxValue);
        }

        [TestMethod]
        public void ToUInt32_Nullable()
        {
            ((decimal?)null).ToUInt32().Should().BeNull();
            ((decimal?)UInt32.MinValue).ToUInt32().Should().Be(UInt32.MinValue);
            ((decimal?)0M).ToUInt32().Should().Be(0);
            ((decimal?)1M).ToUInt32().Should().Be(1);
            ((decimal?)101.101M).ToUInt32().Should().Be(101);
            ((decimal?)UInt32.MaxValue).ToUInt32().Should().Be(UInt32.MaxValue);
        }

        [TestMethod]
        public void ToUInt64()
        {
            ((decimal)UInt64.MinValue).ToUInt64().Should().Be(UInt64.MinValue);
            0M.ToUInt64().Should().Be(0);
            1M.ToUInt64().Should().Be(1);
            101.101M.ToUInt64().Should().Be(101);
            ((decimal)UInt64.MaxValue).ToUInt64().Should().Be(UInt64.MaxValue);
        }

        [TestMethod]
        public void ToUInt64_Nullable()
        {
            ((decimal?)null).ToUInt64().Should().BeNull();
            ((decimal?)UInt64.MinValue).ToUInt64().Should().Be(UInt64.MinValue);
            ((decimal?)0M).ToUInt64().Should().Be(0);
            ((decimal?)1M).ToUInt64().Should().Be(1);
            ((decimal?)101.101M).ToUInt64().Should().Be(101);
            ((decimal?)UInt64.MaxValue).ToUInt64().Should().Be(UInt64.MaxValue);
        }
    }
}
