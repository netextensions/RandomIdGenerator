using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NetExtensions
{
    public class RandomIdGenerator
    {
        private readonly SHA1 _sha1;

        public RandomIdGenerator()
        {
            _sha1 = SHA1.Create();
        }

        public List<long> Generate(int numbers, long greaterThan = 0)
        {
            var randomNumbers = new List<long>(numbers);
            for (var i = 0; i < numbers; i++)
            {
                var number = GetNumber(greaterThan);
                randomNumbers.Add(number);
            }

            return randomNumbers;
        }

        private long GetNumber(long greaterThan)
        {
            while (true)
            {
                var candidate = BitConverter.ToInt64(ComputeHash());
                if (candidate > greaterThan)
                    return candidate;
            }
        }

        private byte[] ComputeHash()
        {
            var bytes = GetRandomBytes();
            return _sha1.ComputeHash(bytes, 0, bytes.Length);
        }

        private byte[] GetRandomBytes()
        {
            return Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
        }
    }
}