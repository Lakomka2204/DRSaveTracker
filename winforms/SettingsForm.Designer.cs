namespace DRSaveTracker
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            hideOnCloseCB = new CheckBox();
            startupCB = new CheckBox();
            settingsBindingSource = new BindingSource(components);
            groupBox2 = new GroupBox();
            rmNameCB = new CheckBox();
            label3 = new Label();
            label4 = new Label();
            verLabel = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)settingsBindingSource).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(hideOnCloseCB);
            groupBox1.Controls.Add(startupCB);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1122, 200);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Program";
            // 
            // hideOnCloseCB
            // 
            hideOnCloseCB.AutoSize = true;
            hideOnCloseCB.Location = new Point(32, 113);
            hideOnCloseCB.Name = "hideOnCloseCB";
            hideOnCloseCB.Size = new Size(515, 36);
            hideOnCloseCB.TabIndex = 1;
            hideOnCloseCB.Text = "Hide program in tray when closing window?";
            hideOnCloseCB.UseVisualStyleBackColor = true;
            // 
            // startupCB
            // 
            startupCB.AutoSize = true;
            startupCB.Location = new Point(32, 54);
            startupCB.Name = "startupCB";
            startupCB.Size = new Size(296, 36);
            startupCB.TabIndex = 0;
            startupCB.Text = "Run on system startup?";
            startupCB.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(rmNameCB);
            groupBox2.Location = new Point(12, 218);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1122, 200);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Game";
            // 
            // rmNameCB
            // 
            rmNameCB.AutoSize = true;
            rmNameCB.Location = new Point(32, 61);
            rmNameCB.Name = "rmNameCB";
            rmNameCB.Size = new Size(503, 36);
            rmNameCB.TabIndex = 0;
            rmNameCB.Text = "Fetch room names instead of numeric IDs?";
            rmNameCB.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(12, 517);
            label3.Name = "label3";
            label3.Size = new Size(193, 32);
            label3.TabIndex = 2;
            label3.Text = "Author: Lakomka\r\n";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(795, 517);
            label4.Name = "label4";
            label4.Size = new Size(339, 32);
            label4.TabIndex = 3;
            label4.Text = "Close window to save changes";
            label4.TextAlign = ContentAlignment.TopRight;
            // 
            // verLabel
            // 
            verLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            verLabel.AutoSize = true;
            verLabel.Location = new Point(12, 485);
            verLabel.Name = "verLabel";
            verLabel.Size = new Size(195, 32);
            verLabel.TabIndex = 4;
            verLabel.Text = "Version: a.b.c.123";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1146, 558);
            Controls.Add(verLabel);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            Load += SettingsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)settingsBindingSource).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private CheckBox startupCB;
        private GroupBox groupBox2;
        private CheckBox rmNameCB;
        private Label label3;
        private Label label4;
        private BindingSource settingsBindingSource;
        private CheckBox hideOnCloseCB;
        private Label verLabel;
    }
}