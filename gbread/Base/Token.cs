namespace GBRead.Base
{
    using System.Text;

    public enum TokenSubtype
    {
        Unknown,

        // Special keywords...
        db,
        dw,
        equ
    }

    /// <summary>
    /// Enumerates the types of tokens that can exist.
    /// </summary>
    public enum TokenType
    {
        Unknown,
        CharacterEscape,
        Comma,
        Comment,
        EndOfLine,
        Equal,
        Identifier,
        Keyword,
        LocalLabel,
        GlobalLabel,
        ExportLabel,
        MemMapEnd,
        MemMapStart,
        Number,
        Operator_Plus,
        Operator_Minus,
        Operator_Mult,
        Operator_Div,
        Special,
        Keyword_adc,
        Keyword_add,
        Keyword_and,
        Keyword_bit,
        Keyword_call,
        Keyword_ccf,
        Keyword_cb,
        Keyword_cp,
        Keyword_cpl,
        Keyword_ei,
        Keyword_daa,
        Keyword_db,
        Keyword_ddw,
        Keyword_dq,
        Keyword_dw,
        Keyword_dec,
        Keyword_di,
        Keyword_halt,
        Keyword_inc,
        Keyword_jp,
        Keyword_jr,
        Keyword_ld,
        Keyword_ldi,
        Keyword_ldd,
        Keyword_ldhl,
        Keyword_nop,
        Keyword_or,
        Keyword_pop,
        Keyword_push,
        Keyword_res,
        Keyword_ret,
        Keyword_reti,
        Keyword_rl,
        Keyword_rla,
        Keyword_rlc,
        Keyword_rlca,
        Keyword_rot,
        Keyword_rr,
        Keyword_rra,
        Keyword_rrc,
        Keyword_rrca,
        Keyword_rst,
        Keyword_sbc,
        Keyword_scf,
        Keyword_set,
        Keyword_sla,
        Keyword_sra,
        Keyword_srl,
        Keyword_stop,
        Keyword_sub,
        Keyword_swap,
        Keyword_xor,

        Reg_a,
        Reg_b,
        Reg_c,
        Reg_d,
        Reg_e,
        Reg_h,
        Reg_l,
        RD_af,
        RD_bc,
        RD_de,
        RD_hl,
        RD_sp,
        CC_nc,
        CC_nz,
        CC_z,

    }

    /// <summary>
    /// Defines a string of characters, representing
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The line number where the token came from, if there is one.
        /// </summary>
        public int Line = 0;

        /// <summary>
        /// The starting index in the original string containing the token, if there is one.
        /// </summary>
        public int StartingIndex = 0;

        /// <summary>
        /// The string of characters representing the entire token.
        /// </summary>
        public StringBuilder Lexeme = new StringBuilder();

        /// <summary>
        /// The type of token.
        /// </summary>
        public TokenType Type = TokenType.Unknown;
    }
}