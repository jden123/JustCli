using System;
using NUnit.Framework;

namespace JustCli.Tests
{
    [TestFixture]
    public class ValueConverterTests
    {
        [TestCase("1", 1)]
        public void ConverterShouldConvertToInt(string stringValue, int resultValue)
        {
            var converter = new ValueConverter();

            var value = converter.ConvertFromString(stringValue, typeof (int));

            Assert.IsInstanceOf<int>(value);
            Assert.AreEqual(resultValue, value);
        }

        [TestCase("1", 1)]
        [TestCase("2.5", 2.5)]
        public void ConverterShouldConvertToDouble(string stringValue, double resultValue)
        {
            var converter = new ValueConverter();

            var value = converter.ConvertFromString(stringValue, typeof (double));

            Assert.IsInstanceOf<double>(value);
            Assert.AreEqual(resultValue, value);
        }

        [TestCase("2015-12-11", "2015-12-11")]
        [TestCase("2015-12-11 12:00", "2015-12-11 12:00")]
        public void ConverterShouldConvertToDateTime(string stringValue, DateTime resultValue)
        {
            var converter = new ValueConverter();

            var value = converter.ConvertFromString(stringValue, typeof(DateTime));

            Assert.IsInstanceOf<DateTime>(value);
            Assert.AreEqual(resultValue, value);
        }

        [TestCase("false", false)]
        [TestCase("true", true)]
        [TestCase("False", false)]
        [TestCase("True", true)]
        public void ConverterShouldConvertToBoolean(string stringValue, bool resultValue)
        {
            var converter = new ValueConverter();

            var value = converter.ConvertFromString(stringValue, typeof(bool));

            Assert.IsInstanceOf<bool>(value);
            Assert.AreEqual(resultValue, value);
        }
    }
}
