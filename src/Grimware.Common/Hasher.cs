using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grimware
{
    public static class Hasher
    {
        private const int _HashMixer = 0xBF;

        public static int Hash(IEnumerable<string> args)
        {
            return args != null
                ? Hash(args as string[] ?? args.ToArray())
                : 0;
        }

        public static int Hash(params string[] args)
        {
            return args != null
                ? Hash(args.Select(Hash))
                : 0;
        }

        public static int Hash(IEnumerable<int> args)
        {
            return args != null
                ? Hash(args as int[] ?? args.ToArray())
                : 0;
        }

        public static int Hash(params int[] args)
        {
            if (args == null)
                return 0;

            var result = args.Length;

            if (result != 0)
                result = args.Aggregate(result, Hash);

            return result;
        }

        public static int Hash(IEnumerable<long> args)
        {
            return args != null
                ? Hash(args as long[] ?? args.ToArray())
                : 0;
        }

        public static int Hash(params long[] args)
        {
            if (args == null)
                return 0;

            var result = args.Length;

            if (result != 0)
                result = args.Aggregate(result, Hash);

            return result;
        }

        public static int Hash(byte[] values)
        {
            if (values == null)
                return 0;

            return
                values.Length > 4
                    ? CalculateLongHash(values)
                    : CalculateShortHash(values);
        }

        private static int CalculateLongHash(byte[] values)
        {
            var result = values.Length;

            var i = 0;
            for (; i < values.Length - 4; i += 4) result = Hash(result, BitConverter.ToInt32(values, i));

            if (i < values.Length)
                result = Hash(result, BitConverter.ToInt32(values, values.Length - 4));

            return result;
        }

        private static int CalculateShortHash(IReadOnlyCollection<byte> values)
        {
            return values.Aggregate(values.Count, (n, b) => Hash(n, b));
        }

        private static int Fold(long n)
        {
            unchecked
            {
                var little = n & 0xFFFFFFFF;
                var big = (n >> 32) & 0xFFFFFFFF;

                return (int) (little ^ big);
            }
        }

        private static int Hash(int i1, int i2)
        {
            unchecked
            {
                return (i1 * _HashMixer) ^ i2;
            }
        }

        private static int Hash(int i, long n)
        {
            return Hash(i, Fold(n));
        }

        private static int Hash(string arg)
        {
            return arg != null
                ? Hash(Encoding.Unicode.GetBytes(arg))
                : 0;
        }
    }
}