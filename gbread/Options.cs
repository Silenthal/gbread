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

		private static byte saveFileVersion = 5;

		private static string saveFileName = "options.dat";

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
			try
			{
				using (Stream bw = File.OpenRead(saveFileName))
				{
					if (!bw.CanRead) return;
					BinaryFormatter bf = new BinaryFormatter();
					if (bw.Length < saveFileHeader.Length) return;
					byte[] header = new byte[saveFileHeader.Length];
					bw.Read(header, 0, header.Length);
					switch (header[3])
					{
						case 4:
						case 5:
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
		#region Public Properties

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

		#endregion Public Properties

		#region Tags

		private string MF_WWTag = "MF_WW";
		private string MF_HighlightCommentsTag = "MF_HighlightComments";
		private string MF_HighlightSyntaxTag = "MF_HighlightSyntax";
		private string MF_HighlightNumbersTag = "MF_HighlightNumbers";
		private string MF_HighlightOffsetsTag = "MF_HighlightOffsets";
		private string MF_HighlightKeywordsTag = "MF_HighlightKeywords";
		private string MF_HighlightLabelsTag = "MF_HighlightLabels";
		private string MF_HighlightRegistersTag = "MF_HighlightRegisters";

		private string DSMPrintOffsetsTag = "DSM_PrintOffsets";
		private string DSMPrintBitPatternTag = "DSM_PrintBP";
		private string DSMPrintedOffsetFormatTag = "DSM_PrintedOffFormat";
		private string DSMInstNumFormatTag = "DSM_InstNumFormat";
		private string DSMPrintCommentsTag = "DSM_PrintComments";
		private string DSMHideDefDataTag = "DSM_HideDefData";

		#endregion Tags

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
			Disassembler_HideDefinedData = false;
		}

		public Options(SerializationInfo info, StreamingContext context)
		{
			MainForm_WordWrap = info.GetBoolean(MF_WWTag);
			MainForm_HighlightComments = info.GetBoolean(MF_HighlightCommentsTag);
			MainForm_HighlightSyntax = info.GetBoolean(MF_HighlightSyntaxTag);
			MainForm_HighlightNumbers = info.GetBoolean(MF_HighlightNumbersTag);
			MainForm_HighlightOffsets = info.GetBoolean(MF_HighlightOffsetsTag);
			MainForm_HighlightKeywords = info.GetBoolean(MF_HighlightKeywordsTag);
			MainForm_HighlightLabels = info.GetBoolean(MF_HighlightLabelsTag);
			MainForm_HighlightRegisters = info.GetBoolean(MF_HighlightRegistersTag);

			Disassembler_PrintOffsets = info.GetBoolean(DSMPrintOffsetsTag);
			Disassembler_PrintBitPattern = info.GetBoolean(DSMPrintBitPatternTag);
			Disassembler_PrintedOffsetFormat = (OffsetFormat)info.GetValue(DSMPrintedOffsetFormatTag, typeof(OffsetFormat));
			Disassembler_InstructionNumberFormat = (OffsetFormat)info.GetValue(DSMInstNumFormatTag, typeof(OffsetFormat));
			Disassembler_PrintComments = info.GetBoolean(DSMPrintCommentsTag);
			Disassembler_HideDefinedData = info.GetBoolean(DSMHideDefDataTag);
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(MF_WWTag, MainForm_WordWrap);
			info.AddValue(MF_HighlightCommentsTag, MainForm_HighlightComments);
			info.AddValue(MF_HighlightSyntaxTag, MainForm_HighlightSyntax);
			info.AddValue(MF_HighlightNumbersTag, MainForm_HighlightNumbers);
			info.AddValue(MF_HighlightOffsetsTag, MainForm_HighlightOffsets);
			info.AddValue(MF_HighlightKeywordsTag, MainForm_HighlightKeywords);
			info.AddValue(MF_HighlightLabelsTag, MainForm_HighlightLabels);
			info.AddValue(MF_HighlightRegistersTag, MainForm_HighlightRegisters);

			info.AddValue(DSMPrintOffsetsTag, Disassembler_PrintOffsets);
			info.AddValue(DSMPrintBitPatternTag, Disassembler_PrintBitPattern);
			info.AddValue(DSMPrintedOffsetFormatTag, Disassembler_PrintedOffsetFormat);
			info.AddValue(DSMInstNumFormatTag, Disassembler_InstructionNumberFormat);
			info.AddValue(DSMPrintCommentsTag, Disassembler_PrintComments);
			info.AddValue(DSMHideDefDataTag, Disassembler_HideDefinedData);
		}
	}
}