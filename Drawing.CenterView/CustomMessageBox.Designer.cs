using System.ComponentModel;

namespace Drawing.CenterView;

partial class CustomMessageBox
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.messageLabel = new System.Windows.Forms.Label();
        this.allButton = new System.Windows.Forms.Button();
        this.fabButton = new System.Windows.Forms.Button();
        this.erectionButton = new System.Windows.Forms.Button();
        this.cancelButton = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // messageLabel
        // 
        this.messageLabel.Location = new System.Drawing.Point(8, 5);
        this.messageLabel.MaximumSize = new System.Drawing.Size(366, 82);
        this.messageLabel.MinimumSize = new System.Drawing.Size(366, 82);
        this.messageLabel.Name = "messageLabel";
        this.messageLabel.Size = new System.Drawing.Size(366, 82);
        this.messageLabel.TabIndex = 0;
        this.messageLabel.Text = "Click \"All\" to center both fabrication and erection drawings.";
        this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // allButton
        // 
        this.allButton.Location = new System.Drawing.Point(9, 105);
        this.allButton.Name = "allButton";
        this.allButton.Size = new System.Drawing.Size(75, 23);
        this.allButton.TabIndex = 1;
        this.allButton.Text = "All";
        this.allButton.UseVisualStyleBackColor = true;
        this.allButton.Click += new System.EventHandler(this.allButton_Click);
        // 
        // fabButton
        // 
        this.fabButton.Location = new System.Drawing.Point(104, 105);
        this.fabButton.Name = "fabButton";
        this.fabButton.Size = new System.Drawing.Size(75, 23);
        this.fabButton.TabIndex = 2;
        this.fabButton.Text = "Fabrication";
        this.fabButton.UseVisualStyleBackColor = true;
        this.fabButton.Click += new System.EventHandler(this.fabButton_Click);
        // 
        // erectionButton
        // 
        this.erectionButton.Location = new System.Drawing.Point(202, 105);
        this.erectionButton.Name = "erectionButton";
        this.erectionButton.Size = new System.Drawing.Size(75, 23);
        this.erectionButton.TabIndex = 3;
        this.erectionButton.Text = "Erection";
        this.erectionButton.UseVisualStyleBackColor = true;
        this.erectionButton.Click += new System.EventHandler(this.erectionButton_Click);
        // 
        // cancelButton
        // 
        this.cancelButton.Location = new System.Drawing.Point(300, 105);
        this.cancelButton.Name = "cancelButton";
        this.cancelButton.Size = new System.Drawing.Size(75, 23);
        this.cancelButton.TabIndex = 4;
        this.cancelButton.Text = "Cancel";
        this.cancelButton.UseVisualStyleBackColor = true;
        this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
        // 
        // CustomMessageBox
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(387, 140);
        this.Controls.Add(this.cancelButton);
        this.Controls.Add(this.erectionButton);
        this.Controls.Add(this.fabButton);
        this.Controls.Add(this.allButton);
        this.Controls.Add(this.messageLabel);
        this.MaximumSize = new System.Drawing.Size(403, 179);
        this.MinimumSize = new System.Drawing.Size(403, 179);
        this.Name = "CustomMessageBox";
        this.Text = "Center All Drawings?";
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Button allButton;
    private System.Windows.Forms.Button fabButton;
    private System.Windows.Forms.Button erectionButton;
    private System.Windows.Forms.Button cancelButton;

    private System.Windows.Forms.Label messageLabel;

    #endregion
}