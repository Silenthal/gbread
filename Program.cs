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

			BinFile cs = new GBBinFile();
			LabelContainer lc = new LabelContainer();
			Disassembler dc = new Disassembler(cs, lc);
			Assembler ac = new Assembler(lc);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm mf = new MainForm(cs, dc, ac, lc);
			
			mf.GetOptions(opm.options);
			lc.GetOptions(opm.options);
			dc.GetOptions(opm.options);
			
			Application.Run(mf);
			
			mf.SetOptions(ref opm.options);
			dc.SetOptions(ref opm.options);
			lc.SetOptions(ref opm.options);
			
			opm.SaveOptions();
		}
	}
}
