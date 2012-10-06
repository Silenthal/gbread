namespace GBRead.Base
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal class InputValidation
    {
        public static bool TryParseOffsetString(string text, out int offset)
        {
            string[] parsed = text.Split(new Char[] { ':' }, 2);
            if (parsed.Length < 2)
            {
                offset = Int32.MinValue;
                return Int32.TryParse(parsed[0], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out offset);
            }
            else
            {
                offset = Int32.MinValue;
                int bank = Int32.MinValue;
                bool success = Int32.TryParse(parsed[0], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out bank);
                if (!success)
                    return false;
                success = Int32.TryParse(parsed[1], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out offset);
                if (!success)
                    return false;
                if (bank < 0 || offset < 0 || offset >= 0x8000)
                {
                    offset = Int32.MinValue;
                    return false;
                }
                if (bank > 0 && (offset < 0x4000))
                {
                    offset = Int32.MinValue;
                    return false;
                }
                offset = ((bank << 14) | (offset & 0x3FFF) & 0xFFFF);
                return true;
            }
        }
    }

    internal class RegularValidation
    {
        private static Regex validLabel = new Regex(@"^[a-z][a-z0-9_]*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex normalInteger = new Regex(@"^([0-9]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex hexInteger = new Regex(@"^([0-9a-z]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex binaryInteger = new Regex(@"^([01]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool TryParseNumber(string inputNumber, out int output)
        {
            if (string.IsNullOrEmpty(inputNumber))
            {
                output = Int32.MinValue;
                return false;
            }
            if (inputNumber[0] == '$' && inputNumber.Length > 1)
            {
                bool success = Int32.TryParse(inputNumber.Substring(1), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out output);
                return success;
            }
            else if (char.ToLower(inputNumber[inputNumber.Length - 1]) == 'h' && inputNumber.Length > 1)
            {
                bool success = Int32.TryParse(inputNumber.Substring(0, inputNumber.Length - 1), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out output);
                return success;
            }
            else if (inputNumber.Length > 2 && inputNumber[0] == '0' && char.ToLower(inputNumber[1]) == 'x')
            {
                bool success = Int32.TryParse(inputNumber.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out output);
                return success;
            }
            else
                return Int32.TryParse(inputNumber, out output);
        }

        public static bool IsWord(string check)
        {
            return validLabel.IsMatch(check);
        }

        public static bool IsLabel(string check)
        {
            if (IsWord(check.Substring(0, check.Length - 1)) && check[check.Length - 1] == ':')
                return true;
            else
                return false;
        }

        public static bool IsNumber(string check)
        {
            if (check.Length == 0)
                return false;
            if (check[0] == '$')
            {
                return hexInteger.IsMatch(check.Substring(1));
            }
            else if (Char.ToLower(check[check.Length - 1]) == 'h')
            {
                return hexInteger.IsMatch(check.Substring(0, check.Length - 1));
            }
            else if (check.Length > 2 && check[0] == '0' && Char.ToLower(check[1]) == 'x')
            {
                return hexInteger.IsMatch(check.Substring(2));
            }
            else if (check[0] == '%')
            {
                return binaryInteger.IsMatch(check.Substring(1));
            }
            else
            {
                int dummy;
                return Int32.TryParse(check, out dummy);
            }
        }
    }
}