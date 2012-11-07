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
        private static Regex validLabel = new Regex(@"^[a-z][a-z0-9_]*$", RegexOptions.IgnoreCase);

        public static bool IsWord(string check)
        {
            return validLabel.IsMatch(check);
        }
    }
}