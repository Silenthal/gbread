namespace GBRead.Base
{
    using System;
    /// <summary>
    /// Contains utility methods used elsewhere in the program.
    /// </summary>
    public class Utility
    {
        public static int GetRealAddress(byte bank, ushort address)
        {
            if (address < 0x4000 || bank == 0)
            {
                return address;
            }
            return (bank * 0x4000) + (address - 0x4000);
        }

        public static int GetPCAddress(int address)
        {
            if (address < 0x4000)
            {
                return address;
            }
            else
            {
                return (address & 0x3FFF) | 0x4000;
            }
        }

        public static bool NumStringToInt(string check, out long result)
        {
            check = check.ToLower();
            result = 0;
            if (check.StartsWith("0x"))
            {
                try
                {
                    result = Convert.ToInt64(check, 16);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            else if (check.StartsWith("$"))
            {
                check = check.Substring(1);
                try
                {
                    result = Convert.ToInt64(check, 16);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            else if (check.StartsWith("0o"))
            {
                check = check.Substring(2);
                try
                {
                    result = Convert.ToInt64(check, 8);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            else if (check.StartsWith("&"))
            {
                check = check.Substring(1);
                try
                {
                    result = Convert.ToInt64(check, 8);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            else if (check.StartsWith("0b"))
            {
                check = check.Substring(2);
                try
                {
                    result = Convert.ToInt64(check, 2);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            else if (check.StartsWith("%"))
            {
                check = check.Substring(1);
                try
                {
                    result = Convert.ToInt64(check, 2);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    result = Convert.ToInt64(check);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
