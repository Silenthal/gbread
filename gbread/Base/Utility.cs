namespace GBRead.Base
{
    using System;
    /// <summary>
    /// Contains utility methods used elsewhere in the program.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Return an actual address, based on a given bank and offset.
        /// </summary>
        /// <param name="bank">The bank of the original address.</param>
        /// <param name="address">The offset of the original address.</param>
        /// <returns>The address as it would be in a binary file.</returns>
        public static int GetRealAddress(byte bank, ushort address)
        {
            if (address < 0x4000 || bank == 0)
            {
                return address;
            }
            return (bank * 0x4000) + (address - 0x4000);
        }

        public static int NumStringToInt(string check)
        {
            check = check.ToLower();
            if (check.StartsWith("0x"))
            {
                return Convert.ToInt32(check, 16);
            }
            else if (check.StartsWith("$"))
            {
                check = check.Substring(1);
                return Convert.ToInt32(check, 16);
            }
            else if (check.StartsWith("0o"))
            {
                check = check.Substring(2);
                return Convert.ToInt32(check, 8);
            }
            else if (check.StartsWith("&"))
            {
                check = check.Substring(1);
                return Convert.ToInt32(check, 8);
            }
            else if (check.StartsWith("0b"))
            {
                check = check.Substring(2);
                return Convert.ToInt32(check, 2);
            }
            else if (check.StartsWith("%"))
            {
                check = check.Substring(1);
                return Convert.ToInt32(check, 2);
            }
            else
            {
                return Convert.ToInt32(check);
            }
        }
    }
}
