using System.Collections.Generic;
using System.IO;

namespace GBRead.Patch
{
    public class IPS
    {
        private MemoryStream ms = new MemoryStream();

        private struct PatchRecord
        {
            public int offset;
            public int length;
            public bool isRLE;

            public PatchRecord(int off, int len, bool rle = false)
            {
                offset = off;
                length = len;
                isRLE = rle;
            }
        }

        private void ClearStream()
        {
            ms.Seek(0, SeekOrigin.Begin);
            ms.SetLength(0);
        }

        public ErrorMessage GenerateIPS(byte[] source, byte[] modified, bool optimize = false)
        {
            if (source.Length != modified.Length)
            {
                return ErrorMessage.IPS_FileSizeMismatch;
            }
            else if (source.Length > 0xFFFFFF)
            {
                return ErrorMessage.IPS_FileSizeTooLarge;
            }
            ClearStream();
            WriteChar('P');
            WriteChar('A');
            WriteChar('T');
            WriteChar('C');
            WriteChar('H');
            var patches = new List<PatchRecord>();
            int curPos = 0;
            while (curPos < source.Length)
            {
                while (curPos < source.Length && source[curPos] == modified[curPos])
                {
                    curPos++;
                }
                if (curPos == source.Length)
                {
                    break;
                }
                PatchRecord p = new PatchRecord(curPos, 1);
                while (curPos < source.Length && source[curPos] != modified[curPos])
                {
                    curPos++;
                    p.length++;
                }
                patches.Add(p);
            }
            patches = ConstrainRecords(SplitRecords(CombineRecords(patches), modified));
            foreach (PatchRecord p in patches)
            {
                if (!WritePatchRecord(p, modified))
                {
                    return ErrorMessage.IPS_UnknownError;
                }
            }
            WriteChar('E');
            WriteChar('O');
            WriteChar('F');
            return ErrorMessage.General_NoError;
        }

        private bool WritePatchRecord(PatchRecord p, byte[] modified)
        {
            if (p.offset < 0 || p.length <= 0 || p.offset + p.length > modified.Length || p.length > 0xFFFF)
            {
                return false;
            }
            if (p.isRLE)
            {
                Write24(p.offset);
                Write16(0);
                Write16(p.length);
                Write8(modified[p.offset]);
            }
            else
            {
                Write24(p.offset);
                Write16(p.length);
                for (int i = 0; i < p.length; i++)
                {
                    Write8(modified[p.offset + i]);
                }
            }
            return true;
        }

        private List<PatchRecord> CombineRecords(List<PatchRecord> inRecords)
        {
            var ret = new List<PatchRecord>();
            if (inRecords.Count == 0)
            {
                return ret;
            }
            PatchRecord cache = inRecords[0];
            for (int i = 1; i < inRecords.Count; i++)
            {
                // If the non-changed region inbetween two records is greater than 5, then a combined
                // patch with the length of that region plus the length of the two records saves
                // space in the final patch.
                int diff = inRecords[i].offset - (cache.offset + cache.length);
                if (diff > 5)
                {
                    int newLen = inRecords[i].length + cache.length + diff;
                    cache = new PatchRecord(inRecords[i].offset, newLen);
                }
                else
                {
                    ret.Add(cache);
                    cache = inRecords[i];
                }
            }
            return ret;
        }

        private List<PatchRecord> SplitRecords(List<PatchRecord> inRecords, byte[] modified)
        {
            var ret = new List<PatchRecord>();
            if (inRecords.Count == 0)
            {
                return ret;
            }
            var cache = new Stack<PatchRecord>();
            for (int i = 1; i < inRecords.Count; i++)
            {
                cache.Push(inRecords[i]);
                while (cache.Count > 0)
                {
                    PatchRecord start = cache.Pop();
                    if (start.isRLE)
                    {
                        ret.Add(start);
                    }
                    else
                    {
                        PatchRecord split = GetLargestContiguousRegion(start, modified);
                        if (split.offset == start.offset)
                        {
                            // Case A: the size of the RLE region is equivalent to the
                            // entire region. Split if the length of the run is > 3.
                            if (split.length == start.length)
                            {
                                if (split.length > 3)
                                {
                                    ret.Add(split);
                                }
                                else
                                {
                                    ret.Add(start);
                                }
                            }
                            // Case B: the RLE region is at the beginning, and less than
                            // the size of the entire region. Split if the length of the
                            // run is > 8.
                            else
                            {
                                if (split.length > 8)
                                {
                                    start.offset += split.length;
                                    cache.Push(start);
                                    ret.Add(split);
                                }
                                else
                                {
                                    ret.Add(start);
                                }
                            }
                        }
                        // Case C: the RLE region is at the end, and less than the size
                        // of the region. Split if the length of the run is > 8.
                        else if (split.offset + split.length == start.offset + start.length)
                        {
                            if (split.length > 8)
                            {
                                start.length += split.length;
                                cache.Push(split);
                                cache.Push(start);
                            }
                            else
                            {
                                ret.Add(start);
                            }
                        }
                        // Case D: the RLE region is in the middle of the whole region.
                        // Split into 3 if the length of the run is > 13.
                        else
                        {
                            if (split.length > 13)
                            {
                                PatchRecord end = new PatchRecord();
                                end.offset = split.offset + split.length;
                                end.length = start.length - split.length;
                                start.length -= split.length + end.length;
                                cache.Push(end);
                                cache.Push(split);
                                cache.Push(start);
                            }
                            else
                            {
                                ret.Add(start);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private List<PatchRecord> ConstrainRecords(List<PatchRecord> inRecords)
        {
            var ret = new List<PatchRecord>();
            if (inRecords.Count == 0)
            {
                return ret;
            }
            for (int i = 0; i < inRecords.Count; i++)
            {
                PatchRecord current = inRecords[i];
                while (current.length > 0xFFFF)
                {
                    PatchRecord split = new PatchRecord();
                    split.isRLE = current.isRLE;
                    split.offset = current.offset;
                    split.length = 0xFFFF;
                    current.offset += 0xFFFF;
                    current.length -= 0xFFFF;
                    ret.Add(split);
                    if (current.isRLE)
                    {
                        current.isRLE = current.length >= 3;
                    }
                }
                ret.Add(current);
            }
            return ret;
        }

        private PatchRecord GetLargestContiguousRegion(PatchRecord p, byte[] modified)
        {
            if (p.offset < 0 || p.length < 0 || p.offset + p.length > modified.Length)
            {
                return new PatchRecord();
            }
            PatchRecord highest = new PatchRecord() { isRLE = true };
            PatchRecord current = new PatchRecord() { isRLE = true };
            int curr = p.offset;
            while (curr < p.offset + p.length)
            {
                current.offset = curr;
                current.length = 1;
                while (modified[curr + 1] == modified[curr])
                {
                    current.length++;
                    curr++;
                }
                if (current.length > highest.length)
                {
                    highest = current;
                }
                curr++;
            }
            return highest;
        }

        public byte[] GetIPS()
        {
            return ms.ToArray();
        }

        private void WriteChar(char c)
        {
            ms.WriteByte((byte)c);
        }

        private void Write8(int x)
        {
            ms.WriteByte((byte)x);
        }

        private void Write16(int x)
        {
            ms.WriteByte((byte)x);
        }

        private void Write24(int x)
        {
            ms.WriteByte((byte)x);
            ms.WriteByte((byte)(x >> 8));
            ms.WriteByte((byte)(x >> 16));
        }
    }
}