using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickVsMortyGame
{
    public class FairRandomResult
    {
        public int MortyValue { get; set; }
        public int RickValue { get; set; }
        public int FinalValue { get; set; }
        public byte[] SecretKey { get; set; } = Array.Empty<byte>();
        public string Hmac { get; set; } = string.Empty;
    }
    public class FairRandomGenerator
    {
        private const int KeySize = 32;

        public string GenerateHmac(int mortyValue, int range)
        {
            var key = GenerateSecretKey();
            return ComputeHmac(mortyValue, key, range);
        }

        public FairRandomResult GenerateFairRandom(int mortyValue, int rickValue, int range, byte[] secretKey)
        {
            var finalValue = (mortyValue + rickValue) % range;

            return new FairRandomResult
            {
                MortyValue = mortyValue,
                RickValue = rickValue,
                FinalValue = finalValue,
                SecretKey = secretKey
            };
        }

        public byte[] GenerateSecretKey()
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var key = new byte[KeySize];
            rng.GetBytes(key);
            return key;
        }

        private string ComputeHmac(int value, byte[] key, int range)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(key);
            var valueBytes = System.Text.Encoding.UTF8.GetBytes($"{value}:{range}");
            var hash = hmac.ComputeHash(valueBytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

    }
}
