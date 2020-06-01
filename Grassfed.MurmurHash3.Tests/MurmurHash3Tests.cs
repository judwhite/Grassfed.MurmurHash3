using System;
using System.Text;
using Grassfed.MurmurHash3;
using NUnit.Framework;

namespace Grassfed.MurmurHash3.Tests
{
    public class MurmurHash3Tests
    {
        private class TestCase
        {
            public string Value { get; set; }
            public string ExpectedHash { get; set; }
            public int Offset { get; set; }
            public int Length { get; set; }
        }

        [Test]
        public void TestComputeHash()
        {
            // Arrange
            var cases = new[]
            {
                new TestCase { Value = "Home grown, grass fed, cage free", ExpectedHash = "0da3518798b74a774f344b82e2315ee0" },
                new TestCase { Value = "The quick brown fox jumps over the lazy dog.", ExpectedHash = "cd99481f9ee902c9695da1a38987b6e7" },
                new TestCase { Value = "", ExpectedHash = "00000000000000000000000000000000" },
                new TestCase { Value = "\0", ExpectedHash = "4610abe56eff5cb551622daa78f83583" },
                new TestCase { Value = "\0\0", ExpectedHash = "3044b81a706c5de818f96bcc37e8a35b" },
                new TestCase { Value = "hello", ExpectedHash = "cbd8a7b341bd9b025b1e906a48ae1d19" },
                new TestCase { Value = "hello\0", ExpectedHash = "084ad4bb6b86b133d402ea7a9c40497b" },
                new TestCase { Value = "hello, world", ExpectedHash = "342fac623a5ebc8e4cdcbc079642414d" },
                new TestCase { Value = "19 Jan 2038 at 3:14:07 AM", ExpectedHash = "b89e5988b737affc664fc2950231b2cb" },
                new TestCase { Value = "The quick brown fox jumps over the lazy dog...", Length = 44, ExpectedHash = "cd99481f9ee902c9695da1a38987b6e7" },
                new TestCase { Value = "  The quick brown fox jumps over the lazy dog.  ", Offset = 2, Length = 44, ExpectedHash = "cd99481f9ee902c9695da1a38987b6e7" },
                new TestCase { Value = "A", ExpectedHash = "035fc2b79a29b17a387df29c46dd9937" },
                new TestCase { Value = "AB", ExpectedHash = "4a75211b3ce4fd780cb062fcc6fd36f1" },
                new TestCase { Value = "ABC", ExpectedHash = "8dbe6477a2f82fde9e2bee4f1c5ba64a" },
                new TestCase { Value = "ABCD", ExpectedHash = "6b12f6cbbad52f195ed5fd4947123e73" },
                new TestCase { Value = "ABCDE", ExpectedHash = "e27529fd9cd6948d2a20972ff65c1afb" },
                new TestCase { Value = "ABCDEF", ExpectedHash = "9174e89a43db6790712240d438c0221b" },
                new TestCase { Value = "ABCDEFG", ExpectedHash = "38af65d501cb4e481108a50231c00291" },
                new TestCase { Value = "ABCDEFGH", ExpectedHash = "a3a725013dbddba86159afa610f16e6e" },
                new TestCase { Value = "ABCDEFGHI", ExpectedHash = "98966aa8e255c53bbf1c6a451d2a9dda" },
                new TestCase { Value = "ABCDEFGHIJ", ExpectedHash = "9859c195fbe27161c6e03b18fb1919ae" },
                new TestCase { Value = "ABCDEFGHIJK", ExpectedHash = "acdbbc62314e9672dbd3146fa77571a0" },
                new TestCase { Value = "ABCDEFGHIJKL", ExpectedHash = "ef9b9fad55dd1d7eb672e5b446bb933e" },
                new TestCase { Value = "ABCDEFGHIJKLM", ExpectedHash = "8a36b1a411d89d5427fe32cd385ba142" },
                new TestCase { Value = "ABCDEFGHIJKLMN", ExpectedHash = "251f5ca561d3c9dd8b9026152da68e1b" },
                new TestCase { Value = "ABCDEFGHIJKLMNO", ExpectedHash = "909232aad85d17662a9f32df8851107b" },
                new TestCase { Value = "ABCDEFGHIJKLMNOP", ExpectedHash = "67d1ccc2efd7ed8f1a19cf5db1beac91" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQ", ExpectedHash = "99f4886afd7ab21110f6c2f976ba0533" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQR", ExpectedHash = "5330c5bef53b49794bde6ebeeb936de9" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRS", ExpectedHash = "e8e78b387b679e9097f036a55bf9f0a3" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRST", ExpectedHash = "dd9a0b5cde565b5cb9a4b6178efec5ab" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRSTU", ExpectedHash = "540c208f064c8a0c62a48fc84d51d2aa" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRSTUV", ExpectedHash = "1ff2da33e738e457eac024a30cb65cc5" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRSTUVW", ExpectedHash = "0dbdb6e58b16f867341c1b4a18464c3a" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRSTUVWX", ExpectedHash = "15c9c335a854f29be459cca1e9d91ff9" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRSTUVWXY", ExpectedHash = "1f4fbb86134e416bae3eca2a132f8dba" },
                new TestCase { Value = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", ExpectedHash = "65e611fed09fced7355e36e45b7fd9e4" },
            };

            var h = new MurmurHash3(0);

            foreach (var c in cases)
            {
                var bytes = Encoding.UTF8.GetBytes(c.Value);

                // Act
                byte[] val;
                if (c.Offset == 0 && c.Length == 0)
                    val = h.ComputeHash(bytes);
                else
                    val = h.ComputeHash(bytes, c.Offset, c.Length);

                // Assert
                var got = ByteArrayToHexString(val);
                if (got != c.ExpectedHash)
                    Assert.Fail("'{0}' want: {1} got: {2}", c.Value, c.ExpectedHash, got);
            }

            var hashWithSeed = new MurmurHash3(38);
            var bytesSeed = Encoding.UTF8.GetBytes("test");
            var computed = hashWithSeed.ComputeHash(bytesSeed);
            var expectedHash = "484389ac868bb47f5eada4a4edfcd9a2";
            var gotSeed = ByteArrayToHexString(computed);
            if (gotSeed != expectedHash)
                Assert.Fail("'{0}' want: {1} got: {2}", "test", expectedHash, gotSeed);
        }

        private static string ByteArrayToHexString(byte[] bytes)
        {
            return string.Concat(Array.ConvertAll(bytes, x => x.ToString("x2")));
        }
    }
}
