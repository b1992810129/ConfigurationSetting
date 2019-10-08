namespace ConfigurationSetting
{
    partial class Form1
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
            this.funcTabControl = new System.Windows.Forms.TabControl();
            this.verSettingTabPage = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ASE_D2DB_eP5 = new System.Windows.Forms.TabPage();
            this.label_subcmd = new System.Windows.Forms.Label();
            this.subCmdBox = new System.Windows.Forms.TextBox();
            this.buttonNextStep = new System.Windows.Forms.Button();
            this.msgBox = new System.Windows.Forms.TextBox();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.ASE_D2DB_eScan420 = new System.Windows.Forms.TabPage();
            this.ASE_D2DB_eScan600 = new System.Windows.Forms.TabPage();
            this.SN2_5XP_D2DB_eP5 = new System.Windows.Forms.TabPage();
            this.SN2_5XP_D2DB_eCan600 = new System.Windows.Forms.TabPage();
            this.SN2_5XP = new System.Windows.Forms.TabPage();
            this.WinMappingTabPage = new System.Windows.Forms.TabPage();
            this.parameditctrTabPage = new System.Windows.Forms.TabPage();
            this.FWTabPage = new System.Windows.Forms.TabPage();
            this.SuperNovaTabPage = new System.Windows.Forms.TabPage();
            this.funcTabControl.SuspendLayout();
            this.verSettingTabPage.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.ASE_D2DB_eP5.SuspendLayout();
            this.SuspendLayout();
            // 
            // funcTabControl
            // 
            this.funcTabControl.Controls.Add(this.verSettingTabPage);
            this.funcTabControl.Controls.Add(this.WinMappingTabPage);
            this.funcTabControl.Controls.Add(this.parameditctrTabPage);
            this.funcTabControl.Controls.Add(this.FWTabPage);
            this.funcTabControl.Controls.Add(this.SuperNovaTabPage);
            this.funcTabControl.Location = new System.Drawing.Point(29, 25);
            this.funcTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.funcTabControl.Multiline = true;
            this.funcTabControl.Name = "funcTabControl";
            this.funcTabControl.SelectedIndex = 0;
            this.funcTabControl.Size = new System.Drawing.Size(937, 334);
            this.funcTabControl.TabIndex = 0;
            // 
            // verSettingTabPage
            // 
            this.verSettingTabPage.Controls.Add(this.tabControl1);
            this.verSettingTabPage.Location = new System.Drawing.Point(4, 40);
            this.verSettingTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.verSettingTabPage.Name = "verSettingTabPage";
            this.verSettingTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.verSettingTabPage.Size = new System.Drawing.Size(929, 290);
            this.verSettingTabPage.TabIndex = 0;
            this.verSettingTabPage.Text = "Version Setting Upgrade";
            this.verSettingTabPage.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ASE_D2DB_eP5);
            this.tabControl1.Controls.Add(this.ASE_D2DB_eScan420);
            this.tabControl1.Controls.Add(this.ASE_D2DB_eScan600);
            this.tabControl1.Controls.Add(this.SN2_5XP_D2DB_eP5);
            this.tabControl1.Controls.Add(this.SN2_5XP_D2DB_eCan600);
            this.tabControl1.Controls.Add(this.SN2_5XP);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(924, 287);
            this.tabControl1.TabIndex = 0;
            // 
            // ASE_D2DB_eP5
            // 
            this.ASE_D2DB_eP5.Controls.Add(this.label_subcmd);
            this.ASE_D2DB_eP5.Controls.Add(this.subCmdBox);
            this.ASE_D2DB_eP5.Controls.Add(this.buttonNextStep);
            this.ASE_D2DB_eP5.Controls.Add(this.msgBox);
            this.ASE_D2DB_eP5.Controls.Add(this.buttonSetting);
            this.ASE_D2DB_eP5.Location = new System.Drawing.Point(4, 22);
            this.ASE_D2DB_eP5.Name = "ASE_D2DB_eP5";
            this.ASE_D2DB_eP5.Padding = new System.Windows.Forms.Padding(3);
            this.ASE_D2DB_eP5.Size = new System.Drawing.Size(916, 261);
            this.ASE_D2DB_eP5.TabIndex = 0;
            this.ASE_D2DB_eP5.Text = "ASE_D2DB-eP5";
            this.ASE_D2DB_eP5.UseVisualStyleBackColor = true;
            // 
            // label_subcmd
            // 
            this.label_subcmd.AutoSize = true;
            this.label_subcmd.Location = new System.Drawing.Point(12, 116);
            this.label_subcmd.Name = "label_subcmd";
            this.label_subcmd.Size = new System.Drawing.Size(71, 13);
            this.label_subcmd.TabIndex = 9;
            this.label_subcmd.Text = "subCommand";
            // 
            // subCmdBox
            // 
            this.subCmdBox.Location = new System.Drawing.Point(15, 132);
            this.subCmdBox.Multiline = true;
            this.subCmdBox.Name = "subCmdBox";
            this.subCmdBox.Size = new System.Drawing.Size(132, 59);
            this.subCmdBox.TabIndex = 8;
            // 
            // buttonNextStep
            // 
            this.buttonNextStep.Enabled = false;
            this.buttonNextStep.Location = new System.Drawing.Point(15, 57);
            this.buttonNextStep.Name = "buttonNextStep";
            this.buttonNextStep.Size = new System.Drawing.Size(75, 23);
            this.buttonNextStep.TabIndex = 7;
            this.buttonNextStep.Text = "next step";
            this.buttonNextStep.UseVisualStyleBackColor = true;
            // 
            // msgBox
            // 
            this.msgBox.Location = new System.Drawing.Point(153, 16);
            this.msgBox.Multiline = true;
            this.msgBox.Name = "msgBox";
            this.msgBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.msgBox.Size = new System.Drawing.Size(752, 229);
            this.msgBox.TabIndex = 6;
            // 
            // buttonSetting
            // 
            this.buttonSetting.Location = new System.Drawing.Point(15, 16);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(75, 23);
            this.buttonSetting.TabIndex = 5;
            this.buttonSetting.Text = "Setting";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Visible = false;
            // 
            // ASE_D2DB_eScan420
            // 
            this.ASE_D2DB_eScan420.Location = new System.Drawing.Point(4, 22);
            this.ASE_D2DB_eScan420.Name = "ASE_D2DB_eScan420";
            this.ASE_D2DB_eScan420.Padding = new System.Windows.Forms.Padding(3);
            this.ASE_D2DB_eScan420.Size = new System.Drawing.Size(916, 261);
            this.ASE_D2DB_eScan420.TabIndex = 1;
            this.ASE_D2DB_eScan420.Text = "ASE_D2DB-eScan420";
            this.ASE_D2DB_eScan420.UseVisualStyleBackColor = true;
            // 
            // ASE_D2DB_eScan600
            // 
            this.ASE_D2DB_eScan600.Location = new System.Drawing.Point(4, 22);
            this.ASE_D2DB_eScan600.Name = "ASE_D2DB_eScan600";
            this.ASE_D2DB_eScan600.Size = new System.Drawing.Size(916, 261);
            this.ASE_D2DB_eScan600.TabIndex = 2;
            this.ASE_D2DB_eScan600.Text = "ASE_D2DB-eScan600";
            this.ASE_D2DB_eScan600.UseVisualStyleBackColor = true;
            // 
            // SN2_5XP_D2DB_eP5
            // 
            this.SN2_5XP_D2DB_eP5.Location = new System.Drawing.Point(4, 22);
            this.SN2_5XP_D2DB_eP5.Name = "SN2_5XP_D2DB_eP5";
            this.SN2_5XP_D2DB_eP5.Size = new System.Drawing.Size(916, 261);
            this.SN2_5XP_D2DB_eP5.TabIndex = 3;
            this.SN2_5XP_D2DB_eP5.Text = "SN2.5XP_D2DB-eP5";
            this.SN2_5XP_D2DB_eP5.UseVisualStyleBackColor = true;
            // 
            // SN2_5XP_D2DB_eCan600
            // 
            this.SN2_5XP_D2DB_eCan600.Location = new System.Drawing.Point(4, 22);
            this.SN2_5XP_D2DB_eCan600.Name = "SN2_5XP_D2DB_eCan600";
            this.SN2_5XP_D2DB_eCan600.Size = new System.Drawing.Size(916, 261);
            this.SN2_5XP_D2DB_eCan600.TabIndex = 4;
            this.SN2_5XP_D2DB_eCan600.Text = "SN2.5XP_D2DB-eCan600";
            this.SN2_5XP_D2DB_eCan600.UseVisualStyleBackColor = true;
            // 
            // SN2_5XP
            // 
            this.SN2_5XP.Location = new System.Drawing.Point(4, 22);
            this.SN2_5XP.Name = "SN2_5XP";
            this.SN2_5XP.Size = new System.Drawing.Size(916, 261);
            this.SN2_5XP.TabIndex = 5;
            this.SN2_5XP.Text = "SN2.5XP";
            this.SN2_5XP.UseVisualStyleBackColor = true;
            // 
            // WinMappingTabPage
            // 
            this.WinMappingTabPage.Location = new System.Drawing.Point(4, 40);
            this.WinMappingTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.WinMappingTabPage.Name = "WinMappingTabPage";
            this.WinMappingTabPage.Padding = new System.Windows.Forms.Padding(2);
            this.WinMappingTabPage.Size = new System.Drawing.Size(929, 290);
            this.WinMappingTabPage.TabIndex = 1;
            this.WinMappingTabPage.Text = "All Windows Environment Mapping";
            this.WinMappingTabPage.UseVisualStyleBackColor = true;
            // 
            // parameditctrTabPage
            // 
            this.parameditctrTabPage.Location = new System.Drawing.Point(4, 40);
            this.parameditctrTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.parameditctrTabPage.Name = "parameditctrTabPage";
            this.parameditctrTabPage.Size = new System.Drawing.Size(929, 290);
            this.parameditctrTabPage.TabIndex = 2;
            this.parameditctrTabPage.Text = "Parameditctr.xml update to eScan";
            this.parameditctrTabPage.UseVisualStyleBackColor = true;
            // 
            // FWTabPage
            // 
            this.FWTabPage.Location = new System.Drawing.Point(4, 40);
            this.FWTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.FWTabPage.Name = "FWTabPage";
            this.FWTabPage.Size = new System.Drawing.Size(929, 290);
            this.FWTabPage.TabIndex = 3;
            this.FWTabPage.Text = "FW ASE_D2DB offline Configuration change";
            this.FWTabPage.UseVisualStyleBackColor = true;
            // 
            // SuperNovaTabPage
            // 
            this.SuperNovaTabPage.Location = new System.Drawing.Point(4, 40);
            this.SuperNovaTabPage.Margin = new System.Windows.Forms.Padding(2);
            this.SuperNovaTabPage.Name = "SuperNovaTabPage";
            this.SuperNovaTabPage.Size = new System.Drawing.Size(929, 290);
            this.SuperNovaTabPage.TabIndex = 4;
            this.SuperNovaTabPage.Text = "SuperNovaGUI_D2DB offline Configuration change";
            this.SuperNovaTabPage.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 377);
            this.Controls.Add(this.funcTabControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.funcTabControl.ResumeLayout(false);
            this.verSettingTabPage.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ASE_D2DB_eP5.ResumeLayout(false);
            this.ASE_D2DB_eP5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl funcTabControl;
        private System.Windows.Forms.TabPage verSettingTabPage;
        private System.Windows.Forms.TabPage WinMappingTabPage;
        private System.Windows.Forms.TabPage parameditctrTabPage;
        private System.Windows.Forms.TabPage FWTabPage;
        private System.Windows.Forms.TabPage SuperNovaTabPage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ASE_D2DB_eP5;
        private System.Windows.Forms.Label label_subcmd;
        private System.Windows.Forms.TextBox subCmdBox;
        private System.Windows.Forms.Button buttonNextStep;
        private System.Windows.Forms.TextBox msgBox;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.TabPage ASE_D2DB_eScan420;
        private System.Windows.Forms.TabPage ASE_D2DB_eScan600;
        private System.Windows.Forms.TabPage SN2_5XP_D2DB_eP5;
        private System.Windows.Forms.TabPage SN2_5XP_D2DB_eCan600;
        private System.Windows.Forms.TabPage SN2_5XP;
    }
}

