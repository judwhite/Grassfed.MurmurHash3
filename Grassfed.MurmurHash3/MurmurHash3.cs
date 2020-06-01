using System;

namespace Grassfed.MurmurHash3
{
    /// <summary>
    /// <para>
    ///     MurmurHash3 x64 128-bit variant.
    /// </para>
    /// <para>
    ///     Project home: https://github.com/judwhite/Grassfed.MurmurHash3
    /// </para>
    /// <para>
    ///     See https://github.com/aappleby/smhasher/wiki/MurmurHash3 for more information. Port of 
    ///     https://github.com/aappleby/smhasher/blob/61a0530f28277f2e850bfc39600ce61d02b518de/src/MurmurHash3.cpp#L255
    /// </para>
    /// </summary>
    public class MurmurHash3
    {
        /// <summary>
        /// todo write this
        /// </summary>
        /// <param name="seed">todo write this</param>
        public MurmurHash3(ulong seed = 0)
        {
            Seed = seed;
        }

        /// <summary>Gets the size, in bits, of the computed hash code.</summary>
        /// <returns>The size, in bits, of the computed hash code.</returns>
        public int HashSize => 128;

        /// <summary>
        /// todo write this
        /// </summary>
        public ulong Seed { get; }

        /// <summary>Computes the hash value for the specified byte array.</summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="buffer" /> is null.</exception>
        public byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }

        /// <summary>Computes the hash value for the specified region of the specified byte array.</summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <param name="offset">The offset into the byte array from which to begin using data.</param>
        /// <param name="count">The number of bytes in the array to use as data.</param>
        /// <returns>The computed hash code.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="count" /> is an invalid value.-or-<paramref name="buffer" /> length is invalid.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="buffer" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="offset" /> is out of range. This parameter requires a non-negative number.</exception>
        public unsafe byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "offset must be >= 0");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "count must be >= 0");
            if (offset + count > buffer.Length)
                throw new ArgumentException($"offset ({offset}) + count ({count}) exceed buffer length ({buffer.Length})");

            const ulong c1 = 0x87c37b91114253d5;
            const ulong c2 = 0x4cf5ad432745937f;

            int nblocks = count / 16;

            ulong h1 = Seed;
            ulong h2 = Seed;

            // body
            fixed (byte* pbuffer = buffer)
            {
                byte* pinput = pbuffer + offset;
                ulong* body = (ulong*)pinput;

                ulong k1;
                ulong k2;

                for (int i = 0; i < nblocks; i++)
                {
                    k1 = body[i * 2];
                    k2 = body[i * 2 + 1];

                    k1 *= c1;
                    k1 = (k1 << 31) | (k1 >> (64 - 31)); // ROTL64(k1, 31);
                    k1 *= c2;
                    h1 ^= k1;

                    h1 = (h1 << 27) | (h1 >> (64 - 27)); // ROTL64(h1, 27);
                    h1 += h2;
                    h1 = h1 * 5 + 0x52dce729;

                    k2 *= c2;
                    k2 = (k2 << 33) | (k2 >> (64 - 33)); // ROTL64(k2, 33);
                    k2 *= c1;
                    h2 ^= k2;

                    h2 = (h2 << 31) | (h2 >> (64 - 31)); // ROTL64(h2, 31);
                    h2 += h1;
                    h2 = h2 * 5 + 0x38495ab5;
                }

                // tail

                k1 = 0;
                k2 = 0;

                byte* tail = pinput + nblocks * 16;
                switch (count & 15)
                {
                    case 15:
                        k2 ^= (ulong)tail[14] << 48;
                        goto case 14;
                    case 14:
                        k2 ^= (ulong)tail[13] << 40;
                        goto case 13;
                    case 13:
                        k2 ^= (ulong)tail[12] << 32;
                        goto case 12;
                    case 12:
                        k2 ^= (ulong)tail[11] << 24;
                        goto case 11;
                    case 11:
                        k2 ^= (ulong)tail[10] << 16;
                        goto case 10;
                    case 10:
                        k2 ^= (ulong)tail[9] << 8;
                        goto case 9;
                    case 9:
                        k2 ^= tail[8];
                        k2 *= c2;
                        k2 = (k2 << 33) | (k2 >> (64 - 33)); // ROTL64(k2, 33);
                        k2 *= c1;
                        h2 ^= k2;
                        goto case 8;
                    case 8:
                        k1 ^= (ulong)tail[7] << 56;
                        goto case 7;
                    case 7:
                        k1 ^= (ulong)tail[6] << 48;
                        goto case 6;
                    case 6:
                        k1 ^= (ulong)tail[5] << 40;
                        goto case 5;
                    case 5:
                        k1 ^= (ulong)tail[4] << 32;
                        goto case 4;
                    case 4:
                        k1 ^= (ulong)tail[3] << 24;
                        goto case 3;
                    case 3:
                        k1 ^= (ulong)tail[2] << 16;
                        goto case 2;
                    case 2:
                        k1 ^= (ulong)tail[1] << 8;
                        goto case 1;
                    case 1:
                        k1 ^= tail[0];
                        k1 *= c1;
                        k1 = (k1 << 31) | (k1 >> (64 - 31)); // ROTL64(k1, 31);
                        k1 *= c2;
                        h1 ^= k1;
                        break;
                }
            }

            // finalization
            h1 ^= (ulong)count;
            h2 ^= (ulong)count;

            h1 += h2;
            h2 += h1;

            h1 = FMix64(h1);
            h2 = FMix64(h2);

            h1 += h2;
            h2 += h1;

            var ret = new byte[16];
            fixed (byte* pret = ret)
            {
                var ulpret = (ulong*)pret;

                ulpret[0] = Reverse(h1);
                ulpret[1] = Reverse(h2);
            }
            return ret;
        }

        private static ulong FMix64(ulong k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;
            return k;
        }

        private static ulong Reverse(ulong value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                    (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                    (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                    (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }
    }
}
