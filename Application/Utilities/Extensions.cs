using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public static class Extensions
    {
        public static bool HasUpperCase(string value)
        {
            return value.Any(char.IsUpper);
        }

        public static bool HasLowerCase(string value)
        {
            return value.Any(char.IsLower);
        }

        public static bool HasNumeric(string value)
        {
            return value.Any(char.IsDigit);
        }

        public static bool HasSpecialCharacter(string value)
        {
            var specialCharacters = "!@#$%^&*(),.?\":{}|<>";
            return value.Any(c => specialCharacters.Contains(c));
        }
        public static bool IsValidLength(string value)
        {
            return value.Length >= 8;
        }
    }
}
