using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace edfi.sdg.utility
{
    using System.Numerics;

    using edfi.sdg.models;

    public static class IdentifierGenerator
    {
        static readonly BigInteger Base36 = new BigInteger(36);
        static readonly char[] Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static string CreateNew()
        {
            return CreateFromGuid(Guid.NewGuid());
        }

        public static string CreateFromGuid(Guid guid)
        {
            var builder = new StringBuilder();
            // need trailing 0 to not be a negative value
            var bytes = new List<byte>(guid.ToByteArray()) { 0 };
            var bigint = new BigInteger(bytes.ToArray());
            do
            {
                BigInteger digit;
                bigint = BigInteger.DivRem(bigint, Base36, out digit);
                builder.Append(Digits[digit.ToByteArray()[0]]);
            } while (bigint.CompareTo(0) != 0);
            return builder.ToString();
        }

        public static Guid ToGuid(string identifier)
        {
            var bytes = new byte[16];
            var bigint = new BigInteger(0);
            foreach (var digit in identifier.ToCharArray().Reverse())
            {
                bigint = BigInteger.Multiply(bigint, Base36);
                bigint = BigInteger.Add(bigint,  new BigInteger(digit - (char.IsNumber(digit) ?'0': 55)));
            }
            // remove trailing 0 if it exists
            Array.Copy(bigint.ToByteArray(), bytes, 16);
            return new Guid(bytes);
        }
    }
}
