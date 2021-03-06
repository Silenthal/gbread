﻿using GBRead.Base;
using GBRead.Base.Annotation;
using System;
using System.Windows.Forms;

namespace GBRead.Forms
{
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
                    commentBox.Text = labelContainer.Comments[offset];
                }
                Text = "Edit Comment";
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int off = -1;
            if (!Utility.OffsetStringToInt(offsetBox.Text, out off))
            {
                Error.ShowErrorMessage(ErrorMessage.Label_InvalidOffset);
            }
            labelContainer.AddComment(off, commentBox.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void fetchCommentButton_Click(object sender, EventArgs e)
        {
            int off = -1;
            if (Utility.OffsetStringToInt(offsetBox.Text, out off) && labelContainer.Comments.ContainsKey(off))
            {
                commentBox.Text = labelContainer.Comments[off];
            }
        }
    }
}