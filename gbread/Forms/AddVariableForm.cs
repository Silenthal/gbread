using GBRead.Base;
using GBRead.Base.Annotation;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace GBRead.Forms
{
    public partial class AddVarLabelForm : Form
    {
        private VarLabel editedLabel;
        private LabelEditMode editingMode;
        private LabelContainer labelContainer;

        public AddVarLabelForm(LabelContainer lblContainer, LabelEditMode editMode, VarLabel newPriorLabel = null)
        {
            InitializeComponent();
            labelContainer = lblContainer;
            editingMode = editMode;
            editedLabel = newPriorLabel;
            if (editMode == LabelEditMode.Edit)
            {
                Text = "Edit Variable";
                if (editedLabel != null)
                {
                    nameBox.Text = editedLabel.Name;
                    offsetBox.Text = editedLabel.Value.ToString("X");
                    if (!String.IsNullOrEmpty(editedLabel.Comment))
                    {
                        commentBox.Text = editedLabel.Comment;
                    }
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
            int val = 0;
            Symbol sym = new Symbol()
            {
                Name = nameBox.Text
            };
            if (!Utility.IsWord(nameBox.Text))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_InvalidName);
            }
            else if (checkNameCollision && labelContainer.IsSymbolDefined(sym))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_NameAlreadyDefined);
            }
            else if (!Int32.TryParse(offsetBox.Text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out val))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_InvalidVariable);
            }
            else
            {
                if (editingMode == LabelEditMode.Edit)
                {
                    labelContainer.RemoveVarLabel(editedLabel);
                }
                editedLabel = new VarLabel(val, nameBox.Text, commentBox.Text);
                labelContainer.AddVarLabel(editedLabel);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}