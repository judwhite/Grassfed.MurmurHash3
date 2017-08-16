using System;
using System.Diagnostics;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Grassfed.MurmurHash3.Tests
{
    public class Benchmarks
    {
        [Test]
        public void Benchmark_NoOp()
        {
            Benchmark(b => null, 8192);
        }

        [Test]
        public void Benchmark_MurmurHash3_x64_128_512()
        {
            var h = new MurmurHash3();
            Benchmark(h.ComputeHash, 512);
        }

        [Test]
        public void Benchmark_CRC32_512()
        {
            var h = new CRC32();
            Benchmark(h.ComputeHash, 512);
        }

        [Test]
        public void Benchmark_MD5_512()
        {
            using (var h = new MD5CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 512);
            }
        }

        [Test]
        public void Benchmark_SHA1_512()
        {
            using (var h = new SHA1CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 512);
            }
        }

        [Test]
        public void Benchmark_SHA256_512()
        {
            using (var h = new SHA256CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 512);
            }
        }

        [Test]
        public void Benchmark_MurmurHash3_x64_128_4096()
        {
            var h = new MurmurHash3();
            Benchmark(h.ComputeHash, 4096);
        }

        [Test]
        public void Benchmark_CRC32_4096()
        {
            var h = new CRC32();
            Benchmark(h.ComputeHash, 4096);
        }

        [Test]
        public void Benchmark_MD5_4096()
        {
            using (var h = new MD5CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 4096);
            }
        }

        [Test]
        public void Benchmark_SHA1_4096()
        {
            using (var h = new SHA1CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 4096);
            }
        }

        [Test]
        public void Benchmark_SHA256_4096()
        {
            using (var h = new SHA256CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 4096);
            }
        }

        [Test]
        public void Benchmark_MurmurHash3_x64_128_8192()
        {
            var h = new MurmurHash3();
            Benchmark(h.ComputeHash, 8192);
        }

        [Test]
        public void Benchmark_CRC32_8192()
        {
            var h = new CRC32();
            Benchmark(h.ComputeHash, 8192);
        }

        [Test]
        public void Benchmark_MD5_8192()
        {
            using (var h = new MD5CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 8192);
            }
        }

        [Test]
        public void Benchmark_SHA1_8192()
        {
            using (var h = new SHA1CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 8192);
            }
        }

        [Test]
        public void Benchmark_SHA256_8192()
        {
            using (var h = new SHA256CryptoServiceProvider())
            {
                Benchmark(h.ComputeHash, 8192);
            }
        }

        private void Benchmark(Func<byte[], byte[]> f, int size)
        {
            const int iterations = 100000;
            var b = new byte[size];

            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                f(b);
            }
            stopwatch.Stop();

            Console.WriteLine("{0} ns/op", stopwatch.Elapsed.TotalMilliseconds * 1000000 / iterations);
            Console.WriteLine("{0} MB/s", size * iterations / 1024.0 / 1024.0 / stopwatch.Elapsed.TotalSeconds);
        }
    }
}
