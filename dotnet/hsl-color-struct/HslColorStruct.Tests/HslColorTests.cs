using System;
using NUnit.Framework;
using HslColorStruct;

namespace HslColorStruct.Tests
{
    [TestFixture]
    public class HslColorTests
    {
        [Test]
        public void Constructor_WithValidValues_CreatesColor()
        {
            var c = new HslColor(120, 50, 75);

            Assert.Multiple(() =>
            {
                Assert.That(c.Hue, Is.EqualTo(120));
                Assert.That(c.Saturation, Is.EqualTo(50));
                Assert.That(c.Lightness, Is.EqualTo(75));
            });
        }

        [TestCase(0)]
        [TestCase(360)]
        public void Constructor_HueBoundary_IsValid(int hue)
        {
            var c = new HslColor(hue, 0, 0);
            Assert.That(c.Hue, Is.EqualTo(hue));
        }

        [TestCase(-1)]
        [TestCase(361)]
        public void Constructor_HueOutOfRange_Throws(int hue)
        {
            Assert.Throws<ArgumentException>(() => _ = new HslColor(hue, 0, 0));
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void Constructor_SaturationOutOfRange_Throws(int s)
        {
            Assert.Throws<ArgumentException>(() => _ = new HslColor(0, s, 0));
        }

        [TestCase(-1)]
        [TestCase(101)]
        public void Constructor_LightnessOutOfRange_Throws(int l)
        {
            Assert.Throws<ArgumentException>(() => _ = new HslColor(0, 0, l));
        }

        [Test]
        public void Create_WithValidValues_ReturnsExpected()
        {
            var c = HslColor.Create(10, 20, 30);
            Assert.That(c, Is.EqualTo(new HslColor(10, 20, 30)));
        }

        [Test]
        public void Parse_WithValidString_ReturnsColor()
        {
            var c = HslColor.Parse("120,50,75");
            Assert.That(c, Is.EqualTo(new HslColor(120, 50, 75)));
        }

        [Test]
        public void Parse_WithNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _ = HslColor.Parse(null!));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("120,50")]
        [TestCase("120,50,75,1")]
        [TestCase("a,50,75")]
        [TestCase("120,b,75")]
        [TestCase("120,50,c")]
        [TestCase("361,0,0")]
        [TestCase("0,101,0")]
        [TestCase("0,0,101")]
        [TestCase(" 0,0,0")]
        [TestCase("0,0,0 ")]
        [TestCase("0, 0,0")]
        [TestCase("0,0, 0")]
        [TestCase("0 ,0,0")]
        [TestCase("0,,0")]
        public void Parse_WithInvalidString_Throws(string input)
        {
            Assert.Throws<ArgumentException>(() => _ = HslColor.Parse(input));
        }

        [Test]
        public void TryParse_WithValidString_ReturnsTrue()
        {
            var ok = HslColor.TryParse("120,50,75", out var c);

            Assert.Multiple(() =>
            {
                Assert.That(ok, Is.True);
                Assert.That(c, Is.EqualTo(new HslColor(120, 50, 75)));
            });
        }

        [Test]
        public void TryParse_WithNull_ReturnsFalse()
        {
            var ok = HslColor.TryParse(null!, out var c);

            Assert.Multiple(() =>
            {
                Assert.That(ok, Is.False);
                Assert.That(c, Is.EqualTo(default(HslColor)));
            });
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("120,50")]
        [TestCase("120,50,75,1")]
        [TestCase("a,50,75")]
        [TestCase("361,0,0")]
        [TestCase(" 0,0,0")]
        [TestCase("0,0,0 ")]
        [TestCase("0, 0,0")]
        [TestCase("0,0, 0")]
        [TestCase("0 ,0,0")]
        [TestCase("0,,0")]
        public void TryParse_WithInvalidString_ReturnsFalse(string input)
        {
            var ok = HslColor.TryParse(input, out var c);

            Assert.Multiple(() =>
            {
                Assert.That(ok, Is.False);
                Assert.That(c, Is.EqualTo(default(HslColor)));
            });
        }

        [Test]
        public void TryParse_WithWrongPartsCount_ReturnsFalse()
        {
            Assert.Multiple(() =>
            {
                Assert.That(HslColor.TryParse("0,0", out _), Is.False);
                Assert.That(HslColor.TryParse("0,0,0,0", out _), Is.False);
            });
        }

        [Test]
        public void ToString_Returns_HCommaSCommaL()
        {
            var c = new HslColor(120, 50, 75);
            Assert.That(c.ToString(), Is.EqualTo("120,50,75"));
        }

        [Test]
        public void Equality_WorksForSameValues()
        {
            var a = new HslColor(1, 2, 3);
            var b = new HslColor(1, 2, 3);
            var c = new HslColor(1, 2, 4);

            Assert.Multiple(() =>
            {
                Assert.That(a, Is.EqualTo(b));
                Assert.That(a == b, Is.True);
                Assert.That(a != b, Is.False);

                Assert.That(a, Is.Not.EqualTo(c));
                Assert.That(a == c, Is.False);
                Assert.That(a != c, Is.True);
            });
        }

        [Test]
        public void GetHashCode_EqualObjects_HaveSameHash()
        {
            var a = new HslColor(10, 20, 30);
            var b = new HslColor(10, 20, 30);

            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void GetHashCode_DifferentObjects_AreLikelyDifferent()
        {
            var a = new HslColor(10, 20, 30);
            var b = new HslColor(10, 20, 31);

            Assert.That(a.GetHashCode(), Is.Not.EqualTo(b.GetHashCode()));
        }
    }
}
