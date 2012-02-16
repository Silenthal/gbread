using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GBRead
{
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
				if (!success) return false;
				success = Int32.TryParse(parsed[1], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out offset);
				if (!success) return false;
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
		private Regex normalInteger = new Regex(@"^([0-9]+)$");
		private Regex hexInteger2 = new Regex(@"^(\$|0x)([0-9a-f]+)$");
		private Regex hexInteger3 = new Regex(@"^([0-9a-f]+)h$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public RegularValidation()
		{

		}

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
			else return Int32.TryParse(inputNumber, out output);
		}

		public static bool IsWord(string check)
		{
			if (string.IsNullOrEmpty(check)) return false;
			if (!Char.IsLetter(check[0])) return false;
			if (check.Length == 1)
			{
				return true;
			}
			else
			{
				foreach (char s in check.Substring(1))
				{
					if (!(Char.IsLetterOrDigit(s) || s.Equals('_'))) return false;
				}
				return true;
			}
		}
	}
}
