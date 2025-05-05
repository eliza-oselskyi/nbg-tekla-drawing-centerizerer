using System;
using System.Windows.Forms;

namespace Drawing.CenterView;

public partial class CustomMessageBox : Form
{
    public CustomMessageBox()
    {
        InitializeComponent();
    }

    public static DialogResult ShowPrompt()
    {
        var form = new CustomMessageBox();
     return form.ShowDialog();
    }

    private void allButton_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.OK;
    }

    private void fabButton_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Yes;
    }

    private void erectionButton_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.No;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
    }
}

public enum CustomMessageBoxButtons
{
    All,
    Fabrication,
    Erection,
    Cancel
}