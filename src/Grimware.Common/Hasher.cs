using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Grimware
{
    [Guid("C1329C5A-2715-4E49-8B71-69B0AAE9D714")]
    public static class Hasher
    {
        private const int HashMixer = 0xBF;

        private static readonly ConcurrentDictionary<Type, int> TypeHashCodes = new ConcurrentDictionary<Type, int>(32, 16);

        public static int Hash(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeHashCodes.GetOrAdd(type, t => Hash(t.GUID.ToByteArray()));
        }

        public static int Hash(params object[] args)
        {
            return args != null ? Hash(args.Select(o => o?.GetHashCode() ?? 0)) : 0;
        }

        public static int Hash(IEnumerable<int> args)
        {
            return args != null ? Hash(args.ToArray()) : 0;
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
            return args != null ? Hash(args.ToArray()) : 0;
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

            return values.Length > 4 ? CalculateLongHash(values) : CalculateShortHash(values);
        }

        private static int CalculateLongHash(byte[] values)
        {
            var result = values.Length;

            var i = 0;
            for (; i < values.Length - 4; i += 4)
            {
                result = Hash(result, BitConverter.ToInt32(values, i));
            }

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

                return (int)(little ^ big);
            }
        }

        private static int Hash(int i1, int i2)
        {
            unchecked
            {
                return (i1 * HashMixer) ^ i2;
            }
        }

        private static int Hash(int i, long n)
        {
            return Hash(i, Fold(n));
        }
    }
}
