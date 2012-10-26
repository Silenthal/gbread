namespace GBRead.Forms
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using GBRead.Base;

    public partial class InsertBinaryForm : Form
    {
        public BinFile pre;
        public byte[] insFile;
        public int offsetPos;
        public bool readyToLoad;

        public InsertBinaryForm(BinFile preTemp)
        {
            InitializeComponent();
            pre = preTemp;
        }

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fs = new FileInfo(openFileDialog1.FileName);
                if (fs.Length > pre.Length)
                {
                    MessageBox.Show("The file selected is bigger than the main file.", "Error", MessageBoxButtons.OK);
                    readyToLoad = false;
                }
                else
                {
                    insFile = File.ReadAllBytes(openFileDialog1.FileName);
                    fileNameLabel.Text = String.Format("File Name: {0}", openFileDialog1.SafeFileName);
                    fileSizeLabel.Text = String.Format("File Size: 0x{0:X} byte(s)", insFile.Length);
                    readyToLoad = true;
                }
            }
        }

        private void insertFileButton_Click(object sender, EventArgs e)
        {
            if (!readyToLoad)
            {
                MessageBox.Show("Load a file first.", "Error", MessageBoxButtons.OK);
            }
            else
            {
                int offsetPos;
                bool success = InputValidation.TryParseOffsetString(offsetBox.Text, out offsetPos);
                if (!success)
                {
                    MessageBox.Show("There was a problem reading your entered offset.P  Please check it and try again.", "Error", MessageBoxButtons.OK);
                }
                else if (offsetPos >= pre.Length)
                {
                    MessageBox.Show("The offset is greater than the size of the loaded file.", "Error", MessageBoxButtons.OK);
                }
                else if (offsetPos + insFile.Length >= pre.Length)
                {
                    MessageBox.Show("The inserted file cannot be placed at this position.", "Error", MessageBoxButtons.OK);
                }
                else
                {
                    string insertionMessage = String.Format("Inserting a 0x{0:X} byte file at position {1:X}.", insFile.Length, offsetPos);
                    int endOff = offsetPos + insFile.Length - 1;
                    if (((endOff >> 14) & 0xFF) != ((offsetPos >> 14) & 0xFF))
                        insertionMessage += Environment.NewLine + "Note: This file may cross over the boundary of the offset's bank." + Environment.NewLine;
                    insertionMessage += "Are you sure you want to insert?";
                    if (MessageBox.Show(insertionMessage, "Confirm Insertion", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        pre.ModifyFile(offsetPos, insFile);
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
        }
    }
}