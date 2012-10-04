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
	/// <summary>
	/// Represents the type of instruction.
	/// </summary>
	public enum InstructionType { adc, add, and, bit, call, ccf, cb, cp, cpl, db, dw, ei, daa, dec, di, halt, inc, jp, jr, ld, ldi, ldd, ldhl, nop, or, pop, push, res, ret, reti, rl, rla, rlc, rlca, rot, rr, rra, rrc, rrca, rst, sbc, scf, set, sla, sra, srl, stop, sub, swap, xor }

	/// <summary>
	/// The type of a GBArgument. Depending on the type of argument, the values within are interpreted differently.
	/// </summary>
	public enum GBArgumentType { None, Bit, Byte, Word, MemMapWord, Conditional, RegisterSingle, MemMapRegisterSingle, RegisterDouble, MemMapRegisterDouble }

	/// <summary>
	/// Represents a single register.
	/// </summary>
	public enum GBRegisterSingle { a, b, c, d, e, h, l }

	//Represents a register pair.
	public enum GBRegisterDouble { af, bc, de, hl, sp }

	/// <summary>
	/// Represents a conditional statement.
	/// </summary>
	public enum GBConditional { nz, z, nc, c }

	/// <summary>
	/// Represents a single GB instruction.
	/// </summary>
	public struct GBInstruction
	{
		/// <summary>
		/// The bank containing the instruction.
		/// </summary>
		public byte Bank;
		/// <summary>
		/// The address of the instruction.
		/// </summary>
		public ushort Address;
		/// <summary>
		/// The size of the argument, in bytes.
		/// </summary>
		public int InstSize;
		/// <summary>
		/// The type of instruction.
		/// </summary>
		public InstructionType InstType;
		/// <summary>
		/// The number of arguments to this instruction.
		/// </summary>
		public int ArgCount;
		/// <summary>
		/// The first argument of the instruction.
		/// </summary>
		public GBArgument Arg1;
		/// <summary>
		/// The second argument of the instruction.
		/// </summary>
		public GBArgument Arg2;
	}

	/// <summary>
	/// Represents an argument to a GB instruction.
	/// </summary>
	public struct GBArgument
	{
		/// <summary>
		/// The type of argument.
		/// </summary>
		public GBArgumentType ArgType;
		/// <summary>
		/// The number value of the argument.
		/// </summary>
		public ushort NumArg;
		/// <summary>
		/// The RegisterSingle value of the argument, if it has one.
		/// </summary>
		public GBRegisterSingle RegSingleArg;
		/// <summary>
		/// The RegisterDouble value of the argument, if it has one.
		/// </summary>
		public GBRegisterDouble RegDoubleArg;
		/// <summary>
		/// The Conditional value of the argument, if it has one.
		/// </summary>
		public GBConditional CondArg;
	}
}