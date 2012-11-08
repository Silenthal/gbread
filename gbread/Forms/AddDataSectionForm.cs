namespace GBRead.Forms
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;
    using GBRead.Base;

    public partial class AddDataLabelForm : Form
    {
        private DataLabel editedLabel;
        private LabelEditMode editingMode;
        private LabelContainer labelContainer;

        public AddDataLabelForm(LabelContainer lblContainer, LabelEditMode editMode, DataLabel newPriorLabel = null)
        {
            InitializeComponent();
            labelContainer = lblContainer;
            editingMode = editMode;
            editedLabel = newPriorLabel;
            dataTypeBox.SelectedIndex = 0;
            if (editMode == LabelEditMode.Edit)
            {
                Text = "Edit Data Section";
                if (editedLabel != null)
                {
                    nameBox.Text = editedLabel.Name;
                    offsetBox.Text = editedLabel.Offset.ToString("X");
                    lengthBox.Text = editedLabel.Length.ToString("X");
                    dataTypeBox.SelectedIndex = (int)editedLabel.DSectionType;
                    if (!String.IsNullOrEmpty(editedLabel.Comment))
                    {
                        commentBox.Text = editedLabel.Comment;
                    }
                    dataTemplateBox.Text = TemplateBuilder.TemplateToString(editedLabel.PrintTemplate);
                }
            }
        }

        private void formBox_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                okButton_Click(new object(), EventArgs.Empty);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            bool checkNameCollision = editingMode == LabelEditMode.Add ?
                true :
                !nameBox.Text.Equals(editedLabel.Name, StringComparison.Ordinal);
            int off = -1;
            int len = 0;
            if (!Utility.IsWord(nameBox.Text))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_InvalidName);
            }
            else if (checkNameCollision && labelContainer.SymbolList.ContainsKey(nameBox.Text))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_NameAlreadyDefined);
            }
            else if (!Utility.OffsetStringToInt(offsetBox.Text, out off))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_InvalidOffset);
            }
            else if (!Int32.TryParse(lengthBox.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out len) || len <= 0)
            {
                Error.ShowErrorMessage(ErrorMessage.Label_InvalidLength);
            }
            else
            {
                TemplateBuilder tb = new TemplateBuilder();
                string input = dataTemplateBox.Text;
                bool success = false;
                CompError error = new CompError();
                var f = tb.ValidateTemplate(input, ref error, out success);
                if (!success)
                {
                    Error.ShowErrorMessage(error);
                }
                else
                {
                    if (editingMode == LabelEditMode.Edit)
                    {
                        labelContainer.RemoveDataLabel(editedLabel);
                    }
                    editedLabel = new DataLabel(off, len, nameBox.Text, commentBox.Text, f, (DataSectionType)dataTypeBox.SelectedIndex);
                    labelContainer.AddDataLabel(editedLabel);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}