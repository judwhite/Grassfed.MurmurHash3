using System;

namespace Grassfed.MurmurHash3.Tests
{
    public class CRC32
    {
        private static readonly uint[] _crc32Table = new uint[256];

        static CRC32()
        {
            unchecked
            {
                // This is the official polynomial used by CRC32 in PKZip.
                // Often the polynomial is shown reversed as 0x04C11DB7.
                const uint dwPolynomial = 0xEDB88320;

                for (uint i = 0; i < 256; i++)
                {
                    uint dwCrc = i;
                    for (uint j = 8; j > 0; j--)
                    {
                        if ((dwCrc & 1) == 1)
                            dwCrc = (dwCrc >> 1) ^ dwPolynomial;
                        else
                            dwCrc >>= 1;
                    }
                    _crc32Table[i] = dwCrc;
                }
            }
        }

        public unsafe byte[] ComputeHash(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            uint crc32Result = 0xFFFFFFFF;

            foreach (byte b in data)
            {
                crc32Result = (crc32Result >> 8) ^ _crc32Table[b ^ (crc32Result & 0x000000FF)];
            }

            crc32Result = ~crc32Result;

            var ret = new byte[4];
            fixed (byte* pret = ret)
            {
                var ulpret = (ulong*)pret;

                ulpret[0] = Reverse(crc32Result);
            }
            return ret;
        }

        private static uint Reverse(uint value)
        {
            return (value & 0x000000FF) << 24 | (value & 0x0000FF00) << 8 |
                   (value & 0x00FF0000) >> 8 | (value & 0xFF000000) >> 24;
        }

    }
}
