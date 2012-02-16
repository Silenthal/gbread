using System;
using System.Windows.Forms;

namespace GBRead
{
    //Note: work on a proper error class.
    public enum ErrorMessage
    {
        NO_FILE, 
        FILE_TOO_LARGE, 
        START_OR_END_INVALID, 
        START_INVALID, 
        END_INVALID, 
        START_AFTER_END, 
    }

    public enum CompilerErrorMessage
    {
        UNRECOGNIZED_LINE, 
        INCORRECT_ARGUMENT, 
        LDHL_SP_OUT_OF_RANGE, 
        VARIABLE_ALREADY_DEFINED, 
        VARIABLE_NOT_ASSIGNED_NUMBER, 
        UNKNOWN_INSTRUCTION, 
        UNKNOWN_MEMORY_REFERENCE, 
        UNKNOWN_ARGUMENT, 
        JR_OUT_OF_RANGE, 
        SINGLE_ARG_UNRECOGNIZED, 
        DOUBLE_ARG_UNRECOGNIZED, 
        LDD_LDI_HL_A_ONLY, 
        VARIABLE_NAME_INVALID, 
        LABEL_ALREADY_DEFINED, 
        DATA_ARGUMENTS_UNRECOGNIZED, 
        DATA_SINGLE_ARG_UNRECOGNIZED
    }

    public struct CompError
    {
        public int lineNumber;
        public CompilerErrorMessage errorMessage;
        public string extraInfo1;
        public string extraInfo2;
        public string fullLine;
        public CompError(string fLine, int lNum, CompilerErrorMessage cError, string eInfo1 = "", string eInfo2 = "")
        {
            lineNumber = lNum;
            errorMessage = cError;
            extraInfo1 = eInfo1;
            extraInfo2 = eInfo2;
            fullLine = fLine;
        }
    }

    public class Error
    {
        public static void ShowErrorMessage(ErrorMessage errorOptions)
        {
            switch (errorOptions)
            {
                case ErrorMessage.NO_FILE:
                    MessageBox.Show("Please load a file.", "Error", MessageBoxButtons.OK);
                    break;
                case ErrorMessage.FILE_TOO_LARGE:
                    MessageBox.Show("The file is too large for a GameBoy file.", "Error", MessageBoxButtons.OK);
                    break;
                case ErrorMessage.START_OR_END_INVALID:
                    MessageBox.Show("Please make sure to write the start and end positions properly.", "Error", MessageBoxButtons.OK);
                    break;
                case ErrorMessage.START_INVALID:
                    MessageBox.Show("Please make sure that your starting position is a place in the file.", "Error", MessageBoxButtons.OK);
                    break;
                case ErrorMessage.END_INVALID:
                    MessageBox.Show("Please make sure that your end position is a place in the file.", "Error", MessageBoxButtons.OK);
                    break;
                case ErrorMessage.START_AFTER_END:
                    MessageBox.Show("Please make sure that your end position is after the start position.", "Error", MessageBoxButtons.OK);
                    break;
                default:
                    MessageBox.Show("Undefined error.", "Error", MessageBoxButtons.OK);
                    break;
            }
        }
        public static void ShowErrorMessage(CompError c)
        {
            string errorMessage = "Line " + c.lineNumber + ": " + c.fullLine + Environment.NewLine;
            switch (c.errorMessage)
            {
                case CompilerErrorMessage.UNRECOGNIZED_LINE:
                    errorMessage += "This line could not be recognized as is.";
                    break;
                case CompilerErrorMessage.INCORRECT_ARGUMENT:
                    errorMessage += "This instruction has an incorrect argument.";
                    break;
                case CompilerErrorMessage.LDHL_SP_OUT_OF_RANGE:
                    errorMessage += "The LDHL SP instruction takes an argument between -128 and 127, or between 0x00 and 0xFF.";
                    break;
                case CompilerErrorMessage.VARIABLE_ALREADY_DEFINED:
                    errorMessage += "The variable " + c.extraInfo1 + " is already defined.";
                    break;
                case CompilerErrorMessage.LABEL_ALREADY_DEFINED:
                    errorMessage += "The label " + c.extraInfo1 + " is already defined.";
                    break;
                case CompilerErrorMessage.VARIABLE_NOT_ASSIGNED_NUMBER:
                    errorMessage += "The number assigned to the variable (" + c.extraInfo1 + ") is not valid.";
                    break;
                case CompilerErrorMessage.UNKNOWN_INSTRUCTION:
                    errorMessage += "The instruction used is unknown.";
                    break;
                case CompilerErrorMessage.UNKNOWN_ARGUMENT:
                    errorMessage += "The instruction's arguments aren't recognized.";
                    break;
                case CompilerErrorMessage.UNKNOWN_MEMORY_REFERENCE:
                    errorMessage += "The memory reference (" + c.extraInfo1 + ") couldn't be parsed, or refers to an undefined location.";
                    break;
                case CompilerErrorMessage.JR_OUT_OF_RANGE:
                    errorMessage += "The jump location cannot be reached from this instruction, because it is either more than 127 bytes ahead, or 128 bytes behind, this instruction.";
                    break;
                case CompilerErrorMessage.SINGLE_ARG_UNRECOGNIZED:
                    errorMessage += "This instruction's argument(" + c.extraInfo1 + ") is not recognized.";
                    break;
                case CompilerErrorMessage.DOUBLE_ARG_UNRECOGNIZED:
                    errorMessage += "This instruction's arguments(" + c.extraInfo1 + ", " + c.extraInfo2 + ") are not recognized.";
                    break;
                case CompilerErrorMessage.LDD_LDI_HL_A_ONLY:
                    errorMessage += "The instruction ldi/ldd can only be used with 'a' and '[hl]'";
                    break;
                case CompilerErrorMessage.VARIABLE_NAME_INVALID:
                    errorMessage += "The name for this variable (" + c.extraInfo1 + ") is invalid.";
                    break;
                case CompilerErrorMessage.DATA_ARGUMENTS_UNRECOGNIZED:
                    errorMessage += "The arguments are unrecognized.";
                    break;
                case CompilerErrorMessage.DATA_SINGLE_ARG_UNRECOGNIZED:
                    errorMessage += "This data argument (" + c.extraInfo1 + ") is unrecognized.";
                    break;
                default:
                    break;
            }
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
        }
    }
}
