using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GBRead.Base;

namespace GBRead
{
	public class OptionsManager
	{
		public Options options;

		private static byte saveFileVersion = 4;

		private static byte[] saveFileHeader = new byte[4]
		{
			(byte)'G', 
			(byte)'B', 
			(byte)'O', 
			saveFileVersion
		};

		public OptionsManager()
		{
			options = new Options();
		}

		public void LoadOptions()
		{
			string saveFileName = "options.dat";
			try
			{
				using (Stream bw = File.OpenRead(saveFileName))
				{
					BinaryFormatter bf = new BinaryFormatter();
					if (bw.Length < saveFileHeader.Length) return;
					byte[] header = new byte[saveFileHeader.Length];
					bw.Read(header, 0, header.Length);
					switch (header[3])
					{
						case 4:
							options = (Options)bf.Deserialize(bw);
							break;
						default:
							break;
					}
				}
			}
			catch { }
		}

		public void SaveOptions()
		{
			string saveFileName = "options.dat";
			using (Stream bw = File.OpenWrite(saveFileName))
			{
				BinaryFormatter bf = new BinaryFormatter();
				bw.Write(saveFileHeader, 0, saveFileHeader.Length);
				bf.Serialize(bw, options);
			}
		}
	}

	[Serializable()]
	public class Options : ISerializable
	{
		public bool MainForm_WordWrap { get; set; }
		public bool MainForm_HighlightComments { get; set; }
		public bool MainForm_HighlightSyntax { get; set; }
		public bool MainForm_HighlightNumbers { get; set; }
		public bool MainForm_HighlightOffsets { get; set; }
		public bool MainForm_HighlightKeywords { get; set; }
		public bool MainForm_HighlightLabels { get; set; }
		public bool MainForm_HighlightRegisters { get; set; }

		public bool Disassembler_PrintOffsets { get; set; }
		public bool Disassembler_PrintBitPattern { get; set; }
		public OffsetFormat Disassembler_PrintedOffsetFormat { get; set; }
		public OffsetFormat Disassembler_InstructionNumberFormat { get; set; }
		public bool Disassembler_PrintComments { get; set; }
		public bool Disassembler_HideDefinedFunctions { get; set; }
		public bool Disassembler_HideDefinedData { get; set; }

		private string MF_WWString = "MF_WW";
		private string MF_HighlightCommentsString = "MF_HighlightComments";
		private string MF_HighlightSyntaxString = "MF_HighlightSyntax";
		private string MF_HighlightNumbersString = "MF_HighlightNumbers";
		private string MF_HighlightOffsetsString = "MF_HighlightOffsets";
		private string MF_HighlightKeywordsString = "MF_HighlightKeywords";
		private string MF_HighlightLabelsString = "MF_HighlightLabels";
		private string MF_HighlightRegistersString = "MF_HighlightRegisters";


		private string DSMPrintOffsetsString = "DSM_PrintOffsets";
		private string DSMPrintBitPatternString = "DSM_PrintBP";
		private string DSMPrintedOffsetFormatString = "DSM_PrintedOffFormat";
		private string DSMInstNumFormatString = "DSM_InstNumFormat";
		private string DSMPrintCommentsString = "DSM_PrintComments";
		private string DSMHideDefFuncsString = "DSM_HideDefFuncs";
		private string DSMHideDefDataString = "DSM_HideDefData";

		public Options()
		{
			MainForm_WordWrap = false;
			MainForm_HighlightComments = true;
			MainForm_HighlightSyntax = false;
			MainForm_HighlightNumbers = false;
			MainForm_HighlightOffsets = false;
			MainForm_HighlightKeywords = true;
			MainForm_HighlightLabels = true;
			MainForm_HighlightRegisters = false;

			Disassembler_PrintOffsets = true;
			Disassembler_PrintBitPattern = true;
			Disassembler_PrintedOffsetFormat = OffsetFormat.BBOO;
			Disassembler_InstructionNumberFormat = OffsetFormat.Hex;
			Disassembler_PrintComments = false;
			Disassembler_HideDefinedFunctions = false;
			Disassembler_HideDefinedData = false;
		}

		public Options(SerializationInfo info, StreamingContext context)
		{
			MainForm_WordWrap = info.GetBoolean(MF_WWString);
			MainForm_HighlightComments = info.GetBoolean(MF_HighlightCommentsString);
			MainForm_HighlightSyntax = info.GetBoolean(MF_HighlightSyntaxString);
			MainForm_HighlightNumbers = info.GetBoolean(MF_HighlightNumbersString);
			MainForm_HighlightOffsets = info.GetBoolean(MF_HighlightOffsetsString);
			MainForm_HighlightKeywords = info.GetBoolean(MF_HighlightKeywordsString);
			MainForm_HighlightLabels = info.GetBoolean(MF_HighlightLabelsString);
			MainForm_HighlightRegisters = info.GetBoolean(MF_HighlightRegistersString);

			Disassembler_PrintOffsets = info.GetBoolean(DSMPrintOffsetsString);
			Disassembler_PrintBitPattern = info.GetBoolean(DSMPrintBitPatternString);
			Disassembler_PrintedOffsetFormat = (OffsetFormat)info.GetValue(DSMPrintedOffsetFormatString, typeof(OffsetFormat));
			Disassembler_InstructionNumberFormat = (OffsetFormat)info.GetValue(DSMInstNumFormatString, typeof(OffsetFormat));
			Disassembler_PrintComments = info.GetBoolean(DSMPrintCommentsString);
			Disassembler_HideDefinedFunctions = info.GetBoolean(DSMHideDefFuncsString);
			Disassembler_HideDefinedData = info.GetBoolean(DSMHideDefDataString);
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(MF_WWString, MainForm_WordWrap);
			info.AddValue(MF_HighlightCommentsString, MainForm_HighlightComments);
			info.AddValue(MF_HighlightSyntaxString, MainForm_HighlightSyntax);
			info.AddValue(MF_HighlightNumbersString, MainForm_HighlightNumbers);
			info.AddValue(MF_HighlightOffsetsString, MainForm_HighlightOffsets);
			info.AddValue(MF_HighlightKeywordsString, MainForm_HighlightKeywords);
			info.AddValue(MF_HighlightLabelsString, MainForm_HighlightLabels);
			info.AddValue(MF_HighlightRegistersString, MainForm_HighlightRegisters);

			info.AddValue(DSMPrintOffsetsString, Disassembler_PrintOffsets);
			info.AddValue(DSMPrintBitPatternString, Disassembler_PrintBitPattern);
			info.AddValue(DSMPrintedOffsetFormatString, Disassembler_PrintedOffsetFormat);
			info.AddValue(DSMInstNumFormatString, Disassembler_InstructionNumberFormat);
			info.AddValue(DSMPrintCommentsString, Disassembler_PrintComments);
			info.AddValue(DSMHideDefFuncsString, Disassembler_HideDefinedFunctions);
			info.AddValue(DSMHideDefDataString, Disassembler_HideDefinedData);
		}
	}
}