namespace GBRead.Forms
{
    using System;
    using System.Windows.Forms;
    using GBRead.Base;

    public partial class AddCommentForm : Form
    {
        private LabelContainer labelContainer;

        public AddCommentForm(LabelContainer lblContainer, LabelEditMode editMode, int offset = -1)
        {
            InitializeComponent();
            labelContainer = lblContainer;
            if (editMode == LabelEditMode.Edit)
            {
                if (labelContainer.Comments.ContainsKey(offset))
                {
                    textBox2.Text = labelContainer.Comments[offset];
                }
                Text = "Edit Comment";
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int off = -1;
            if (!InputValidation.TryParseOffsetString(offsetBox.Text, out off))
            {
                Error.ShowErrorMessage(ErrorMessage.OFFSET_IS_INVALID);
            }
            labelContainer.AddComment(off, textBox2.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void fetchCommentButton_Click(object sender, EventArgs e)
        {
            int off = -1;
            if (InputValidation.TryParseOffsetString(offsetBox.Text, out off) && labelContainer.Comments.ContainsKey(off))
            {
                textBox2.Text = labelContainer.Comments[off];
            }
        }
    }
}