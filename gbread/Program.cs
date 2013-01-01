namespace GBRead
{
    using System;
    using System.Windows.Forms;
    using GBRead.Base;
    using GBRead.Forms;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            OptionsManager opm = new OptionsManager();
            opm.LoadOptions();

            BinFile bFile = new BinFile();
            LabelContainer lContainer = new LabelContainer();
            Disassembler dsembler = new Disassembler(bFile, lContainer);
            Assembler asmbler = new Assembler(lContainer);
            MainForm mainForm = new MainForm(bFile, dsembler, asmbler, lContainer);

            mainForm.GetOptions(opm.options);
            dsembler.GetOptions(opm.options);
            asmbler.GetOptions(opm.options);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm);

            mainForm.SetOptions(opm.options);
            dsembler.SetOptions(opm.options);
            asmbler.SetOptions(opm.options);

            opm.SaveOptions();
        }
    }
}