namespace GBRead.Base
{
    using System.Collections.Generic;
    using System.IO;

    
    /// <summary>
    /// A class to generate machine code targeting the GB/C.
    /// </summary>
    public class CodeGenerator
    {
        public delegate void ArithmeticFuncDelegate(int arg);

        public delegate void BitFunctionDelegate(int bit, string arg);

        private MemoryStream ms;

        public long Position
        {
            get
            {
                return ms.Position;
            }
        }

        public CodeGenerator()
        {
            ms = new MemoryStream();
        }

        public void ClearStream()
        {
            ms.Seek(0, SeekOrigin.Begin);
            ms.SetLength(0);
        }

        public void Seek(long offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            ms.Seek(offset, origin);
        }

        public byte[] StreamToArray()
        {
            return ms.ToArray();
        }

        public void EmitByte(int num)
        {
            ms.WriteByte((byte)num);
        }

        public void EmitWord(int num)
        {
            ms.WriteByte((byte)num);
            ms.WriteByte((byte)(num >> 8));
        }

        public void EmitDWord(int num)
        {
            ms.WriteByte((byte)num);
            ms.WriteByte((byte)(num >> 8));
            ms.WriteByte((byte)(num >> 16));
            ms.WriteByte((byte)(num >> 24));
        }

        public void EmitQWord(long num)
        {
            ms.WriteByte((byte)num);
            ms.WriteByte((byte)(num >> 8));
            ms.WriteByte((byte)(num >> 16));
            ms.WriteByte((byte)(num >> 24));
            ms.WriteByte((byte)(num >> 32));
            ms.WriteByte((byte)(num >> 40));
            ms.WriteByte((byte)(num >> 48));
            ms.WriteByte((byte)(num >> 56));
        }

        #region ADC

        public void EmitAdcN(int arg)
        {
            EmitByte(0xCE);
            EmitByte(arg);
        }

        public void EmitAdcR(string reg)
        {
            EmitByte(0x88 + regSingleBytes[reg]);
        }

        #endregion ADC

        #region ADD

        public void EmitAddN(int arg)
        {
            EmitByte(0xC6);
            EmitByte(arg);
        }

        public void EmitAddR(string reg)
        {
            EmitByte(0x80 + regSingleBytes[reg]);
        }

        public void EmitAddRR(string reg)
        {
            EmitByte(0x09 + regDoubleBytes[reg]);
        }

        public void EmitAddSPN(int arg)
        {
            EmitByte(0xE8);
            EmitByte(arg);
        }

        #endregion ADD

        #region AND

        public void EmitAndN(int arg)
        {
            EmitByte(0xE6);
            EmitByte(arg);
        }

        public void EmitAndR(string reg)
        {
            EmitByte(0xA0 + regSingleBytes[reg]);
        }

        #endregion AND

        #region BIT

        public void EmitBitXR(int bit, string reg)
        {
            bit &= 7;
            EmitByte(0xCB);
            EmitByte(0x40 + (bit * 8) + regSingleBytes[reg]);
        }

        #endregion BIT

        #region CALL

        public void EmitCallN(int arg)
        {
            EmitByte(0xCD);
            EmitWord(arg);
        }

        public void EmitCallCCN(string cond, int arg)
        {
            EmitByte(0xC4 + conditionBytes[cond]);
            EmitWord(arg);
        }

        #endregion CALL

        #region CCF

        public void EmitCCF()
        {
            EmitByte(0x3F);
        }

        #endregion CCF

        #region CP

        public void EmitCpN(int arg)
        {
            EmitByte(0xFE);
            EmitByte(arg);
        }

        public void EmitCpR(string reg)
        {
            EmitByte(0xB8 + regSingleBytes[reg]);
        }

        #endregion CP

        #region CPL

        public void EmitCPL()
        {
            EmitByte(0x2F);
        }

        #endregion CPL

        #region DAA

        public void EmitDAA()
        {
            EmitByte(0x2F);
        }

        #endregion DAA

        #region DEC

        public void EmitDecR(string reg)
        {
            EmitByte(0x05 + (8 * regSingleBytes[reg]));
        }

        public void EmitDecRR(string reg)
        {
            EmitByte(0x0B + regDoubleBytes[reg]);
        }

        #endregion DEC

        #region DI

        public void EmitDI()
        {
            EmitByte(0xF3);
        }

        #endregion DI

        #region EI

        public void EmitEI()
        {
            EmitByte(0xFB);
        }

        #endregion EI

        #region HALT

        public void EmitHalt()
        {
            EmitByte(0x76);
        }

        #endregion HALT

        #region INC

        public void EmitIncR(string reg)
        {
            EmitByte(0x04 + (8 * regSingleBytes[reg]));
        }

        public void EmitIncRR(string reg)
        {
            EmitByte(0x03 + regDoubleBytes[reg]);
        }

        #endregion INC

        #region JP

        public void EmitJpHL()
        {
            EmitByte(0xE9);
        }

        public void EmitJpN(int arg)
        {
            EmitByte(0xC3);
            EmitWord(arg);
        }

        public void EmitJpCCN(string cond, int arg)
        {
            EmitByte(0xC2 + conditionBytes[cond]);
            EmitWord(arg);
        }

        #endregion JP

        #region JR

        public void EmitJr(int arg)
        {
            EmitByte(0x18);
            EmitByte(arg);
        }

        public void EmitJrCCN(string cond, int arg)
        {
            EmitByte(0x20 + conditionBytes[cond]);
            EmitByte(arg);
        }

        #endregion JR

        #region LDHL

        public void EmitLdHLSP(int arg)
        {
            EmitByte(0xF8);
            EmitByte(arg);
        }

        #endregion LDHL

        #region LDIO

        public void EmitLdioAN(int arg)
        {
            EmitByte(0xF0);
            EmitByte(arg);
        }

        public void EmitLdioNA(int arg)
        {
            EmitByte(0xE0);
            EmitByte(arg);
        }

        #endregion LDIO

        #region LDI

        public void EmitLdiHLA()
        {
            EmitByte(0x22);
        }

        public void EmitLdiAHL()
        {
            EmitByte(0x2A);
        }

        #endregion LDI

        #region LDD

        public void EmitLddAHL()
        {
            EmitByte(0x3A);
        }

        public void EmitLddHLA()
        {
            EmitByte(0x32);
        }

        #endregion LDD

        #region LD

        public void EmitLdRegReg(string reg1, string reg2)
        {
            EmitByte(0x40 + (8 * regSingleBytes[reg1]) + regSingleBytes[reg2]);
        }

        public void EmitLdACRef()
        {
            EmitByte(0xF2);
        }

        public void EmitLdABCRef()
        {
            EmitByte(0x0A);
        }

        public void EmitLdADERef()
        {
            EmitByte(0x1A);
        }

        public void EmitLdANRef(int arg)
        {
            EmitByte(0xFA);
            EmitWord(arg);
        }

        public void EmitLdRN(string reg, int arg)
        {
            EmitByte(0x06 + (8 * regSingleBytes[reg]));
            EmitByte(arg);
        }

        public void EmitLdRRN(string reg, int arg)
        {
            EmitByte(0x01 + regDoubleBytes[reg]);
        }

        public void EmitLdSPHL()
        {
            EmitByte(0xF9);
        }

        public void EmitLdCRefA()
        {
            EmitByte(0xE2);
        }

        public void EmitLdBCRefA()
        {
            EmitByte(0x02);
        }

        public void EmitLdDERefA()
        {
            EmitByte(0x12);
        }

        public void EmitLdNRefA(int arg)
        {
            EmitByte(0xEA);
            EmitWord(arg);
        }

        public void EmitLdNRefSP(int arg)
        {
            EmitByte(0x08);
            EmitWord(arg);
        }

        #endregion LD

        #region NOP

        public void EmitNop()
        {
            EmitByte(0);
        }

        #endregion NOP

        #region OR

        public void EmitOrN(int arg)
        {
            EmitByte(0xF6);
            EmitByte(arg);
        }

        public void EmitOrR(string reg)
        {
            EmitByte(0xB0 + regSingleBytes[reg]);
        }

        #endregion OR

        #region POP

        public void EmitPopRR(string reg)
        {
            EmitByte(0xC1 + regDoubleBytes[reg]);
        }

        #endregion POP

        #region PUSH

        public void EmitPushRR(string reg)
        {
            EmitByte(0xC5 + regDoubleBytes[reg]);
        }

        #endregion PUSH

        #region RES

        public void EmitResXR(int bit, string reg)
        {
            bit &= 7;
            EmitByte(0xCB);
            EmitByte(0x80 + (bit * 8) + regSingleBytes[reg]);
        }

        #endregion RES

        #region RET

        public void EmitRet()
        {
            EmitByte(0xC9);
        }

        public void EmitRetCC(string cond)
        {
            EmitByte(0xC0 + conditionBytes[cond]);
        }

        #endregion RET

        #region RETI

        public void EmitReti()
        {
            EmitByte(0xD9);
        }

        #endregion RETI

        #region RLA

        public void EmitRla()
        {
            EmitByte(0x17);
        }

        #endregion RLA

        #region RL

        public void EmitRl(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x10 + regSingleBytes[reg]);
        }

        #endregion RL

        #region RLCA

        public void EmitRlca()
        {
            EmitByte(0x07);
        }

        #endregion RLCA

        #region RLC

        public void EmitRlc(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x00 + regSingleBytes[reg]);
        }

        #endregion RLC

        #region RRA

        public void EmitRra()
        {
            EmitByte(0x1F);
        }

        #endregion RRA

        #region RR

        public void EmitRr(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x18 + regSingleBytes[reg]);
        }

        #endregion RR

        #region RRCA

        public void EmitRrca()
        {
            EmitByte(0x0F);
        }

        #endregion RRCA

        #region RRC

        public void EmitRrc(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x08 + regSingleBytes[reg]);
        }

        #endregion RRC

        #region RST

        public void EmitRst(int arg)
        {
            arg &= 0x38;
            EmitByte(0xC7 + arg);
        }

        #endregion RST

        #region SBC

        public void EmitSbcN(int arg)
        {
            EmitByte(0xDE);
            EmitByte(arg);
        }

        public void EmitSbcR(string reg)
        {
            EmitByte(0x98 + regSingleBytes[reg]);
        }

        #endregion SBC

        #region SCF

        public void EmitScf()
        {
            EmitByte(0x37);
        }

        #endregion SCF

        #region SET

        public void EmitSetXR(int bit, string reg)
        {
            bit &= 7;
            EmitByte(0xC0 + (bit * 8) + regSingleBytes[reg]);
        }

        #endregion SET

        #region SLA

        public void EmitSla(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x20 + regSingleBytes[reg]);
        }

        #endregion SLA

        #region SRA

        public void EmitSra(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x28 + regSingleBytes[reg]);
        }

        #endregion SRA

        #region SRL

        public void EmitSrl(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x38 + regSingleBytes[reg]);
        }

        #endregion SRL

        #region STOP

        public void EmitStop()
        {
            EmitByte(0x10);

            //EmitByte(0x00);
        }

        #endregion STOP

        #region SUB

        public void EmitSubN(int arg)
        {
            EmitByte(0xD6);
            EmitByte(arg);
        }

        public void EmitSubR(string reg)
        {
            EmitByte(0x90 + regSingleBytes[reg]);
        }

        #endregion SUB

        #region SWAP

        public void EmitSwapR(string reg)
        {
            EmitByte(0xCB);
            EmitByte(0x30 + regSingleBytes[reg]);
        }

        #endregion SWAP

        #region XOR

        public void EmitXorN(int arg)
        {
            EmitByte(0xEE);
            EmitByte(arg);
        }

        public void EmitXorR(string reg)
        {
            EmitByte(0xA8 + regSingleBytes[reg]);
        }

        #endregion XOR

        #region Token Values

        private static Dictionary<string, byte> regSingleBytes = new Dictionary<string, byte>
        {
            {"b", 0},
            {"c", 1},
            {"d", 2},
            {"e", 3},
            {"h", 4},
            {"l", 5},
            {"RR_REF_HL", 6},
            {"a", 7}
        };

        private static Dictionary<string, byte> conditionBytes = new Dictionary<string, byte>
        {
            {"nz", 0},
            {"z", 8},
            {"nc", 16},
            {"c", 24}
        };

        private static Dictionary<string, byte> regDoubleBytes = new Dictionary<string, byte>
        {
            {"bc", 0},
            {"de", 16},
            {"hl", 32},
            {"af", 48},
            {"sp", 48}
        };

        #endregion Token Values
    }
}