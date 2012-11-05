﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GBRead
{
    //Note: work on a proper error class.
    public enum ErrorMessage
    {
        General_NoError,
        DATA_ARGUMENTS_UNRECOGNIZED, 
        DATA_SINGLE_ARG_UNRECOGNIZED, 
        DOUBLE_ARG_UNRECOGNIZED, 
        END_INVALID,
        FILE_TOO_LARGE,
        INCORRECT_ARGUMENT, 
        Build_JROutOfRange, 
        Label_LabelAlreadyDefined, 
        LDD_LDI_HL_A_ONLY, 
        LDHL_SP_OUT_OF_RANGE, 
        NAME_ALREADY_DEFINED, 
        NO_FILE,
        SINGLE_ARG_UNRECOGNIZED, 
        START_AFTER_END,
        START_INVALID,
        START_OR_END_INVALID,
        UNKNOWN_ARGUMENT, 
        UNKNOWN_INSTRUCTION, 
        UNKNOWN_MEMORY_REFERENCE, 
        UNRECOGNIZED_LINE, 
        Label_VariableAlreadyDefined, 
        VARIABLE_NAME_INVALID, 
        VARIABLE_NOT_ASSIGNED_NUMBER, 
        NAME_IS_INVALID, 
        OFFSET_IS_INVALID, 
        LENGTH_IS_INVALID, 
        ROW_LENGTH_IS_INVALID,
        VARIABLE_IS_INVALID,
        NumberOverflow,
        IPS_FileSizeMismatch,
        IPS_FileSizeTooLarge,
        IPS_UnknownError,
        Build_MacroAlreadyDefined,
        Build_NoMacroArgsPresent,
        Build_NotEnoughMacroArgs,
        Build_MacroArgUsedOutsideOfDef,
        Build_MacroDoesNotExist,
        Build_UnknownArgument,
        Build_UnknownLabel,
        Build_UnknownError,
        General_CustomError,
    }

    public struct CompError
    {
        public int lineNumber;
        public int characterNumber;
        public ErrorMessage errorMessage;
        public string extraInfo1;
        public string extraInfo2;
        public string fullLine;
        public CompError(string fLine, int lNum, ErrorMessage cError, string eInfo1 = "", string eInfo2 = "")
        {
            lineNumber = lNum;
            characterNumber = 0;
            errorMessage = cError;
            extraInfo1 = eInfo1;
            extraInfo2 = eInfo2;
            fullLine = fLine;
        }
    }

    public class Error
    {
        public static Dictionary<ErrorMessage, string> ErrorMessages = new Dictionary<ErrorMessage, string>()
        {
            {ErrorMessage.General_NoError,                  "No error."},
            {ErrorMessage.END_INVALID,                      "Please make sure that your end position is a place in the file."}, 
            {ErrorMessage.FILE_TOO_LARGE,                   "The file is too large for a GameBoy file."}, 
            {ErrorMessage.NO_FILE,                          "Please load a file."}, 
            {ErrorMessage.START_AFTER_END,                  "Please make sure that your end position is after the start position."}, 
            {ErrorMessage.START_INVALID,                    "Please make sure that your starting position is a place in the file."}, 
            {ErrorMessage.START_OR_END_INVALID,             "Please make sure to write the start and end positions properly."}, 
            {ErrorMessage.DATA_ARGUMENTS_UNRECOGNIZED,      "The arguments are unrecognized."}, 
            {ErrorMessage.DATA_SINGLE_ARG_UNRECOGNIZED,     "This data argument ({0}) is unrecognized."}, 
            {ErrorMessage.DOUBLE_ARG_UNRECOGNIZED,          "This instruction's arguments({0}, {1}) are not recognized."}, 
            {ErrorMessage.INCORRECT_ARGUMENT,               "This instruction has an incorrect argument."}, 
            {ErrorMessage.Build_JROutOfRange,               "The jump location cannot be reached from this instruction, because it is either more than 127 bytes ahead, or 128 bytes behind, this instruction."}, 
            {ErrorMessage.Label_LabelAlreadyDefined,        "The label {0} is already defined."}, 
            {ErrorMessage.LDD_LDI_HL_A_ONLY,                "The instruction ldi/ldd can only be used with 'a' and '[hl]'"}, 
            {ErrorMessage.LDHL_SP_OUT_OF_RANGE,             "The LDHL SP instruction takes an argument between -128 and 127, or between 0x00 and 0xFF."}, 
            {ErrorMessage.NAME_ALREADY_DEFINED,             "This name is already defined."}, 
            {ErrorMessage.SINGLE_ARG_UNRECOGNIZED,          "This instruction's argument({0}) is not recognized."}, 
            {ErrorMessage.UNKNOWN_ARGUMENT,                 "The instruction's arguments aren't recognized."}, 
            {ErrorMessage.UNKNOWN_INSTRUCTION,              "The instruction used is unknown."}, 
            {ErrorMessage.UNKNOWN_MEMORY_REFERENCE,         "The memory reference ({0}) couldn't be parsed, or refers to an undefined location."}, 
            {ErrorMessage.UNRECOGNIZED_LINE,                "This line could not be recognized as is."}, 
            {ErrorMessage.Label_VariableAlreadyDefined,     "The variable {0} is already defined."}, 
            {ErrorMessage.VARIABLE_NAME_INVALID,            "The name for this variable ({0}) is invalid."}, 
            {ErrorMessage.VARIABLE_NOT_ASSIGNED_NUMBER,     "The number assigned to the variable ({0}) is not valid."}, 
            {ErrorMessage.NAME_IS_INVALID,                  "The name is invalid."}, 
            {ErrorMessage.OFFSET_IS_INVALID,                "The offset is invalid."}, 
            {ErrorMessage.LENGTH_IS_INVALID,                "The length is invalid."}, 
            {ErrorMessage.VARIABLE_IS_INVALID,              "The value is invalid."},
            {ErrorMessage.NumberOverflow,                   "The number on this line is too long."},
            {ErrorMessage.IPS_FileSizeMismatch,             "The two files being patched have different sizes, and are incompatible with IPS."},
            {ErrorMessage.IPS_FileSizeTooLarge,             "The file being patched is too large, and is incompatible with IPS."},
            {ErrorMessage.IPS_UnknownError,                 "An unknown error occured while making the patch."},
            {ErrorMessage.Build_MacroAlreadyDefined,        "A macro with this name has already been defined."},
            {ErrorMessage.Build_NoMacroArgsPresent,         "The macro call wasn't provided with any arguments."},
            {ErrorMessage.Build_NotEnoughMacroArgs,         "There aren't enough macro arguments to evaluate this expression."},
            {ErrorMessage.Build_MacroArgUsedOutsideOfDef,   "Macro arguments can only be used inside of macro definitions."},
            {ErrorMessage.Build_MacroDoesNotExist,          "There is no macro with the name \"{0}\"."},
            {ErrorMessage.Build_UnknownLabel,               "The label \"{0}\" could not be found in this scope."},
            {ErrorMessage.Build_UnknownArgument,            "Unable to evaluate the expression due to an unknown variable."},
            {ErrorMessage.General_CustomError,              "{0}"},
        };
        public static void ShowErrorMessage(ErrorMessage errorOptions)
        {
            MessageBox.Show(ErrorMessages[errorOptions], "Error", MessageBoxButtons.OK);
        }
        public static void ShowErrorMessage(CompError c)
        {
            string errorMessage = "Line " + c.lineNumber + ", Char " + c.characterNumber + ": " + c.fullLine + Environment.NewLine;
            errorMessage += String.Format(ErrorMessages[c.errorMessage], c.extraInfo1, c.extraInfo2);
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
        }
    }
}
