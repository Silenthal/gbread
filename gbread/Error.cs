using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GBRead
{
    // TODO: work on a proper error class.
    public enum ErrorMessage
    {
        Build_JROutOfRange,
        Build_MacroAlreadyDefined,
        Build_MacroArgUsedOutsideOfDef,
        Build_MacroDoesNotExist,
        Build_NoMacroArgsPresent,
        Build_NotEnoughMacroArgs,
        Build_NumberOverflow,
        Build_UnknownArgument,
        Build_UnknownError,
        Build_UnknownLabel,
        Disassembly_EndInvalid,
        Disassembly_StartAfterEnd,
        Disassembly_StartInvalid,
        Disassembly_StartOrEndInvalid,
        General_CustomError,
        General_NoError,
        General_NoFileLoaded,
        IPS_FileSizeMismatch,
        IPS_FileSizeTooLarge,
        IPS_UnknownError,
        Label_InvalidLength,
        Label_InvalidName,
        Label_InvalidOffset,
        Label_InvalidVariable,
        Label_LabelAlreadyDefined,
        Label_NameAlreadyDefined,
        Label_VariableAlreadyDefined,
        Template_LessThanOne,
        Template_StringLenInvalid,
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
            {ErrorMessage.Build_JROutOfRange,               "The jump location cannot be reached from this instruction, because it is either more than 127 bytes ahead, or 128 bytes behind, this instruction."},
            {ErrorMessage.Build_MacroAlreadyDefined,        "A macro with this name has already been defined."},
            {ErrorMessage.Build_MacroArgUsedOutsideOfDef,   "Macro arguments can only be used inside of macro definitions."},
            {ErrorMessage.Build_MacroDoesNotExist,          "There is no macro with the name \"{0}\"."},
            {ErrorMessage.Build_NoMacroArgsPresent,         "The macro call wasn't provided with any arguments."},
            {ErrorMessage.Build_NotEnoughMacroArgs,         "There aren't enough macro arguments to evaluate this expression."},
            {ErrorMessage.Build_NumberOverflow,             "The number on this line is too long."},
            {ErrorMessage.Build_UnknownArgument,            "Unable to evaluate the expression due to an unknown variable."},
            {ErrorMessage.Build_UnknownLabel,               "The label \"{0}\" could not be found in this scope."},
            {ErrorMessage.Disassembly_EndInvalid,           "Please make sure that your end position is a place in the file."},
            {ErrorMessage.Disassembly_StartAfterEnd,        "Please make sure that your end position is after the start position."},
            {ErrorMessage.Disassembly_StartInvalid,         "Please make sure that your starting position is a place in the file."},
            {ErrorMessage.Disassembly_StartOrEndInvalid,    "Please make sure to write the start and end positions properly."},
            {ErrorMessage.General_CustomError,              "{0}"},
            {ErrorMessage.General_NoError,                  "No error."},
            {ErrorMessage.General_NoFileLoaded,             "Please load a file."},
            {ErrorMessage.IPS_FileSizeMismatch,             "The two files being patched have different sizes, and are incompatible with IPS."},
            {ErrorMessage.IPS_FileSizeTooLarge,             "The file being patched is too large, and is incompatible with IPS."},
            {ErrorMessage.IPS_UnknownError,                 "An unknown error occured while making the patch."},
            {ErrorMessage.Label_InvalidLength,              "The length is invalid."},
            {ErrorMessage.Label_InvalidName,                "The name is invalid."},
            {ErrorMessage.Label_InvalidOffset,              "The offset is invalid."},
            {ErrorMessage.Label_InvalidVariable,            "The value is invalid."},
            {ErrorMessage.Label_LabelAlreadyDefined,        "The label {0} is already defined."},
            {ErrorMessage.Label_NameAlreadyDefined,         "This name is already defined."},
            {ErrorMessage.Label_VariableAlreadyDefined,     "The variable {0} is already defined."},
            {ErrorMessage.Template_LessThanOne,             "The size of an argument can't be less than 1."},
            {ErrorMessage.Template_StringLenInvalid,        "The string size must be at least 1."},
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