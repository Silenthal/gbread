using GBRead.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace GBRead
{
    public class OptionsManager
    {
        public Options options;

        private static string saveFileName = "settings.txt";

        public OptionsManager()
        {
            options = new Options();
        }

        public void LoadOptions()
        {
            try
            {
                using (StreamReader bw = new StreamReader(File.OpenRead(saveFileName)))
                {
                    string line;
                    while ((line = bw.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line[0] == ';')
                        {
                            continue;
                        }
                        string[] f = line.Split(new[] { '=' }, 2);
                        if (f.Length != 2)
                        {
                            continue;
                        }
                        options.SetOption(f[0], f[1]);
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveOptions()
        {
            using (StreamWriter bw = new StreamWriter(File.OpenWrite(saveFileName)))
            {
                bw.WriteLine("[Main]");
                bw.WriteLine(options.GetOptionAsString(options.Main_WordWrapTag));
                bw.WriteLine();
                bw.WriteLine("[Disassembler]");
                bw.WriteLine(options.GetOptionAsString(options.DS_PrintOffsetsTag));
                bw.WriteLine(options.GetOptionAsString(options.DS_PrintBitPatternTag));
                bw.WriteLine(options.GetOptionAsString(options.DS_PrintCommentsTag));
                bw.WriteLine(options.GetOptionAsString(options.DS_PrintedOffsetFormatTag));
                bw.WriteLine(options.GetOptionAsString(options.DS_InstNumFormatTag));
                bw.WriteLine(options.GetOptionAsString(options.DS_HideDefDataTag));
                bw.WriteLine(options.GetOptionAsString(options.DS_GameboyFormatChars));
                bw.WriteLine();
                bw.WriteLine("[Assembler]");
                bw.WriteLine(options.GetOptionAsString(options.AS_GameboyFormatChars));
            }
        }
    }

    public class Options
    {
        #region Public Properties

        public bool MainForm_WordWrap
        {
            get
            {
                return (bool)opts[Main_WordWrapTag];
            }

            set
            {
                opts[Main_WordWrapTag] = value;
            }
        }

        public bool Disassembler_PrintOffsets
        {
            get
            {
                return (bool)opts[DS_PrintOffsetsTag];
            }

            set
            {
                opts[DS_PrintOffsetsTag] = value;
            }
        }

        public bool Disassembler_PrintBitPattern
        {
            get
            {
                return (bool)opts[DS_PrintBitPatternTag];
            }

            set
            {
                opts[DS_PrintBitPatternTag] = value;
            }
        }

        public OffsetFormat Disassembler_PrintedOffsetFormat
        {
            get
            {
                return (OffsetFormat)opts[DS_PrintedOffsetFormatTag];
            }

            set
            {
                opts[DS_PrintedOffsetFormatTag] = value;
            }
        }

        public OffsetFormat Disassembler_InstructionNumberFormat
        {
            get
            {
                return (OffsetFormat)opts[DS_InstNumFormatTag];
            }

            set
            {
                opts[DS_InstNumFormatTag] = value;
            }
        }

        public bool Disassembler_PrintComments
        {
            get
            {
                return (bool)opts[DS_PrintCommentsTag];
            }

            set
            {
                opts[DS_PrintCommentsTag] = value;
            }
        }

        public bool Disassembler_HideDefinedData
        {
            get
            {
                return (bool)opts[DS_HideDefDataTag];
            }

            set
            {
                opts[DS_HideDefDataTag] = value;
            }
        }

        public string Disassembler_GameboyFormatChars
        {
            get
            {
                return (string)opts[DS_GameboyFormatChars];
            }
            set
            {
                opts[DS_GameboyFormatChars] = value;
            }
        }

        public string Assembler_GameboyFormatChars
        {
            get
            {
                return (string)opts[AS_GameboyFormatChars];
            }
            set
            {
                opts[AS_GameboyFormatChars] = value;
            }
        }

        #endregion Public Properties

        #region Tags

        public string Main_WordWrapTag = "bWordWrap";
        public string DS_PrintOffsetsTag = "bPrintOffsets";
        public string DS_PrintBitPatternTag = "bPrintBytes";
        public string DS_PrintedOffsetFormatTag = "eOffsetFormat";
        public string DS_InstNumFormatTag = "eNumberFormat";
        public string DS_PrintCommentsTag = "bPrintComments";
        public string DS_HideDefDataTag = "bHideDefinedData";
        public string DS_GameboyFormatChars = "sDsmGBFormatChars";
        public string AS_GameboyFormatChars = "sAsmGBFormatChars";

        #endregion Tags

        // TODO: separate options for "extended comments" and regular comments.
        private Dictionary<string, object> opts;

        public Options()
        {
            opts = new Dictionary<string, object>()
            {
                {Main_WordWrapTag, true},
                {DS_PrintOffsetsTag, true},
                {DS_PrintBitPatternTag, true},
                {DS_PrintedOffsetFormatTag, OffsetFormat.BankOffset},
                {DS_InstNumFormatTag, OffsetFormat.Hex},
                {DS_PrintCommentsTag, false},
                {DS_HideDefDataTag, false},
                {DS_GameboyFormatChars, "0123"},
                {AS_GameboyFormatChars, "0123"}
            };
        }

        public void SetOption(string option, string value)
        {
            if (opts.ContainsKey(option))
            {
                switch (option[0])
                {
                    case 'b':
                        {
                            bool opt;
                            if (Boolean.TryParse(value, out opt))
                            {
                                opts[option] = opt;
                            }
                        }
                        break;

                    case 'e':
                        {
                            OffsetFormat opt;
                            try
                            {
                                opt = (OffsetFormat)Enum.Parse(typeof(OffsetFormat), value);
                                opts[option] = opt;
                            }
                            catch (ArgumentException)
                            {
                                break;
                            }
                        }
                        break;

                    case 's':
                        {
                            opts[option] = value;
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public string GetOptionAsString(string option)
        {
            if (opts.ContainsKey(option))
            {
                switch (option[0])
                {
                    case 'b':
                        {
                            return option + "=" + ((bool)opts[option]).ToString();
                        }
                    case 'e':
                        {
                            return option + "=" + ((OffsetFormat)opts[option]).ToString();
                        }
                    case 's':
                        {
                            return option + "=" + ((string)opts[option]);
                        }
                    default:
                        return "";
                }
            }
            return "";
        }
    }
}