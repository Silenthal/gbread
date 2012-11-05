//Copyright © 2012 Silenthal

//This file is part of LibGBasm.
//LibGBasm is free software: you can redistribute it and/or modify
//it under the terms of the GNU Lesser General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//LibGBasm is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU Lesser General Public License for more details.

//You should have received a copy of the GNU Lesser General Public License
//along with LibGBasm.  If not, see <http://www.gnu.org/licenses/>.
namespace LibGBasm
{
    using System;

    public class GBASM
    {
        /// <summary>
        /// Given a binary file and the appropriate offsets, constructs a GBInstructionUnit containing information about the instruction and its arguments.
        /// </summary>
        /// <param name="baseFile">The file to read from.</param>
        /// <param name="org">The origin address of the file.</param>
        /// <param name="offset">The offset into the file to read from.</param>
        /// <param name="outputInstruction">The GBInstruction to write the information to.</param>
        /// <returns>The success of the operation. Returns false if the information fetching went wrong for any reason
        /// </returns>
        public static bool GetInstruction(byte[] baseFile, int org, int offset, ref GBInstruction outputInstruction)
        {
            outputInstruction = new GBInstruction();
            if (baseFile == null || offset > baseFile.Length - 1)
                return false;

            // Holds the "actual" address of the instruction, in case the array provided isn't the full file.
            int address = org + offset;
            byte inst = baseFile[offset];
            if (inst == 0xCB)
            {
                // If the inst is a CB instruction, it's a 2-byte one, and a different table is used.
                if (offset > baseFile.Length - 2)
                {
                    // In case the inst is part of a bigger file, interpret as a DB instruction instead.
                    // Prior behavior: return false
                    return GBASM.CreateDBInstruction(baseFile, org, offset, ref outputInstruction);
                }
                outputInstruction = GBInstructions.CBInstructionUnitTable[baseFile[offset + 1]];
            }
            else
            {
                outputInstruction = GBInstructions.InstructionUnitTable[baseFile[offset]];
            }

            // If the instruction gotten is too big, then return a DB/DW instruction, as default.
            if (offset + outputInstruction.InstSize > baseFile.Length)
            {
                int retSize = baseFile.Length - offset;
                switch (retSize)
                {
                    case 1:
                        {
                            return GBASM.CreateDBInstruction(baseFile, org, offset, ref outputInstruction);
                        }

                    case 2:
                        {
                            return GBASM.CreateDWInstruction(baseFile, org, offset, ref outputInstruction);
                        }

                    default:
                        {
                            return false;
                        }
                }
            }

            // Adjust the gotten inst's bank and address manually
            outputInstruction.Bank = (byte)(address >> 14);
            outputInstruction.Address = (ushort)(address & 0x3FFF);
            if (address > 0x4000)
            {
                outputInstruction.Address += 0x4000;
            }

            // Finally, adjust the values of the arguments if they are dependant on the other bytes
            // in the instruction.
            if (outputInstruction.InstSize == 1 && outputInstruction.InstType == InstructionType.db)
            {
                outputInstruction.Arg1.NumArg = baseFile[offset];
            }
            else if (outputInstruction.InstSize == 2 && inst != 0xCB)
            {
                if (outputInstruction.ArgCount == 1)
                {
                    if (outputInstruction.InstType == InstructionType.jr)
                    {
                        //jr nn
                        int modifier = (baseFile[offset + 1] < 0x80 ? baseFile[offset + 1] : -(0x100 - baseFile[offset + 1]));
                        ushort newAddress = (ushort)(outputInstruction.Address + 2 + modifier);
                        outputInstruction.Arg1.NumArg = newAddress;
                    }
                    else
                    {
                        //and, or, sub, cp, xor nn
                        outputInstruction.Arg1.NumArg = baseFile[offset + 1];
                    }
                }
                else if (outputInstruction.ArgCount == 2)
                {
                    if (outputInstruction.InstType == InstructionType.jr)
                    {
                        //jr nn
                        int modifier = (baseFile[offset + 1] < 0x80 ? baseFile[offset + 1] : -(0x100 - baseFile[offset + 1]));
                        ushort newAddress = (ushort)(outputInstruction.Address + 2 + modifier);
                        outputInstruction.Arg2.NumArg = newAddress;
                    }
                    else if (outputInstruction.Arg1.ArgType == GBArgumentType.MemMapWord)
                    {
                        outputInstruction.Arg1.NumArg = (ushort)(0xFF00 + baseFile[offset + 1]);
                    }
                    else if (outputInstruction.Arg2.ArgType == GBArgumentType.MemMapWord)
                    {
                        outputInstruction.Arg2.NumArg = (ushort)(0xFF00 + baseFile[offset + 1]);
                    }
                    else
                    {
                        outputInstruction.Arg2.NumArg = baseFile[offset + 1];
                    }
                }
            }
            else if (outputInstruction.InstSize == 3)
            {
                if (outputInstruction.ArgCount == 1 && outputInstruction.Arg1.ArgType == GBArgumentType.Word)
                {
                    //jp nnnn, call nnnn
                    outputInstruction.Arg1.NumArg = BitConverter.ToUInt16(baseFile, offset + 1);
                }
                else if (outputInstruction.ArgCount == 2)
                {
                    if (outputInstruction.Arg1.ArgType == GBArgumentType.MemMapWord)
                    {
                        outputInstruction.Arg1.NumArg = BitConverter.ToUInt16(baseFile, offset + 1);
                    }
                    else if (outputInstruction.Arg2.ArgType == GBArgumentType.Word ||
                        outputInstruction.Arg2.ArgType == GBArgumentType.MemMapWord)
                    {
                        outputInstruction.Arg2.NumArg = BitConverter.ToUInt16(baseFile, offset + 1);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Creates a byte-sized data instruction.
        /// </summary>
        /// <param name="baseFile">The file to read from.</param>
        /// <param name="org">The origin address of the file.</param>
        /// <param name="offset">The offset into the file to read from.</param>
        /// <param name="outputInstruction">The GBInstruction to write the information to.</param>
        /// <returns>Returns true if the creation was successful, and false otherwise.
        /// </returns>
        public static bool CreateDBInstruction(byte[] baseFile, int org, int offset, ref GBInstruction outputInstruction)
        {
            outputInstruction = new GBInstruction();
            if (baseFile == null || offset > baseFile.Length - 1)
                return false;
            int address = org + offset;
            outputInstruction.InstSize = 1;
            outputInstruction.InstType = InstructionType.db;
            outputInstruction.Bank = (byte)(address >> 14);
            outputInstruction.Address = (ushort)(address & 0x3FFF);
            outputInstruction.ArgCount = 1;
            outputInstruction.Arg1.ArgType = GBArgumentType.Byte;
            outputInstruction.Arg1.NumArg = baseFile[offset];
            if (address > 0x4000)
            {
                outputInstruction.Address += 0x4000;
            }
            return true;
        }

        /// <summary>
        /// Creates a word-sized data instruction.
        /// </summary>
        /// <param name="baseFile">The file to read from.</param>
        /// <param name="org">The origin address of the file.</param>
        /// <param name="offset">The offset into the file to read from.</param>
        /// <param name="outputInstruction">The GBInstruction to write the information to.</param>
        /// <returns>Returns true if the creation was successful, and false otherwise.
        /// </returns>
        public static bool CreateDWInstruction(byte[] baseFile, int org, int offset, ref GBInstruction outputInstruction)
        {
            outputInstruction = new GBInstruction();
            if (baseFile == null || offset > baseFile.Length - 2)
                return false;
            int address = org + offset;
            outputInstruction.InstSize = 2;
            outputInstruction.InstType = InstructionType.dw;
            outputInstruction.Bank = (byte)(address >> 14);
            outputInstruction.Address = (ushort)(address & 0x3FFF);
            outputInstruction.ArgCount = 1;
            outputInstruction.Arg1.ArgType = GBArgumentType.Word;
            outputInstruction.Arg1.NumArg = BitConverter.ToUInt16(baseFile, offset);
            if (address > 0x4000)
            {
                outputInstruction.Address += 0x4000;
            }
            return true;
        }
    }
}