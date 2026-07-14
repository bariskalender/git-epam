using System;
using System.Linq;
using NUnit.Framework;

namespace ContiguousSubstrings.Tests
{
    [TestFixture]
    public class SequencesTests
    {
        [TestCase("1", 1, new[] { "1" })]
        [TestCase("12", 1, new[] { "1", "2" })]
        [TestCase("35", 2, new[] { "35" })]
        [TestCase("9142", 2, new[] { "91", "14", "42" })]
        [TestCase("777777", 3, new[] { "777", "777", "777", "777" })]
        [TestCase("918493904243", 5, new[] { "91849", "18493", "84939", "49390", "93904", "39042", "90424", "04243" })]
        public void GetSubstrings_ValidInput_ReturnsExpected(string numbers, int length, string[] expected)
        {
            var actual = ContiguousSubstrings.Sequences.GetSubstrings(numbers, length).ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetSubstrings_Null_Throws()
        {
            Assert.Throws<ArgumentException>(() => ContiguousSubstrings.Sequences.GetSubstrings(null!, 1).ToArray());
        }

        [Test]
        public void GetSubstrings_Empty_Throws()
        {
            Assert.Throws<ArgumentException>(() => ContiguousSubstrings.Sequences.GetSubstrings("", 1).ToArray());
        }

        [Test]
        public void GetSubstrings_Whitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() => ContiguousSubstrings.Sequences.GetSubstrings("   ", 1).ToArray());
        }

        [TestCase("123a")]
        [TestCase("12 3")]
        [TestCase("12-3")]
        public void GetSubstrings_NonDigit_Throws(string numbers)
        {
            Assert.Throws<ArgumentException>(() => ContiguousSubstrings.Sequences.GetSubstrings(numbers, 1).ToArray());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSubstrings_LengthInvalid_Throws(int length)
        {
            Assert.Throws<ArgumentException>(() => ContiguousSubstrings.Sequences.GetSubstrings("12345", length).ToArray());
        }

        [Test]
        public void GetSubstrings_LengthTooLarge_Throws()
        {
            Assert.Throws<ArgumentException>(() => ContiguousSubstrings.Sequences.GetSubstrings("12345", 6).ToArray());
        }
    }
}
