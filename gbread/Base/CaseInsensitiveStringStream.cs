namespace GBRead.Base
{
    using System;
    using Antlr.Runtime;

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

        // Only the lookahead is converted to lowercase. The original case is preserved in the stream.
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

            return Char.ToLowerInvariant(data[(p + i) - 1]); // This is how "case insensitive" is defined, i.e., could also use a special culture...
        }
    }
}