using System;
using System.Windows.Forms;
using GBRead.Base;
using GBRead.Forms;

namespace GBRead
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			OptionsManager opm = new OptionsManager();
			opm.LoadOptions();

			BinFile bFile = new BinFile();
			LabelContainer lContainer = new LabelContainer();
			Disassembler dsembler = new Disassembler(bFile, lContainer);
			Assembler asmbler = new Assembler(lContainer);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm mainForm = new MainForm(bFile, dsembler, asmbler, lContainer);
			
			mainForm.GetOptions(opm.options);
			dsembler.GetOptions(opm.options);
			
			Application.Run(mainForm);
			
			mainForm.SetOptions(ref opm.options);
			dsembler.SetOptions(ref opm.options);
			
			opm.SaveOptions();
		}
	}
}
