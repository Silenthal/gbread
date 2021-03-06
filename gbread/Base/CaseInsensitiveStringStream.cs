﻿using Antlr.Runtime;
using System;

namespace GBRead.Base
{
    /// <summary>
    /// An extension of <see cref="ANTLRStringStream"/> that allows for case insensitivity.
    /// </summary>
    public class CaseInsensitiveStringStream : ANTLRStringStream
    {
        public CaseInsensitiveStringStream(char[] data, int numberOfActualCharsInArray)
            : base(data, numberOfActualCharsInArray)
        {
        }

        public CaseInsensitiveStringStream()
        {
        }

        public CaseInsensitiveStringStream(string input)
            : base(input)
        {
        }

        public override int LA(int i)
        {
            if (i == 0)
            {
                return 0;
            }

            if (i < 0)
            {
                i++;
            }

            if (((p + i) - 1) >= n)
            {
                return (int)CharStreamConstants.EndOfFile;
            }

            return Char.ToLowerInvariant(data[(p + i) - 1]);
        }
    }
}