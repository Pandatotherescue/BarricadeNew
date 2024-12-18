using System;
using System.Linq;

namespace BarricadeNew.Models
{
    public static class PasswordGenerator
    {
        private const string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";

        public static string Generate(int length = 16)
        {
            if (length <= 0)
                throw new ArgumentException("Password length must be greater than zero.", nameof(length));

            var random = new Random();
            return new string(Enumerable.Repeat(ValidChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}