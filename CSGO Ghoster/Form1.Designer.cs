namespace CSGO_Ghoster
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.startRemoteGhostingButton = new System.Windows.Forms.Button();
            this.startGhostLocalButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.toolTipsGhoster = new System.Windows.Forms.ToolTip(this.components);
            this.replaceMapButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.MapImage = new System.Windows.Forms.PictureBox();
            this.GhosterWindowInformationLabel = new System.Windows.Forms.Label();
            this.DebugBox = new System.Windows.Forms.RichTextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MapImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // ipTextBox
            // 
            this.ipTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ipTextBox.Location = new System.Drawing.Point(0, 0);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(107, 20);
            this.ipTextBox.TabIndex = 0;
            this.ipTextBox.TextChanged += new System.EventHandler(this.ipTextBox_TextChanged);
            // 
            // startRemoteGhostingButton
            // 
            this.startRemoteGhostingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startRemoteGhostingButton.Enabled = false;
            this.startRemoteGhostingButton.Location = new System.Drawing.Point(115, -2);
            this.startRemoteGhostingButton.Name = "startRemoteGhostingButton";
            this.startRemoteGhostingButton.Size = new System.Drawing.Size(75, 23);
            this.startRemoteGhostingButton.TabIndex = 1;
            this.startRemoteGhostingButton.Text = "Ghost IP";
            this.toolTipsGhoster.SetToolTip(this.startRemoteGhostingButton, "Ghost another match");
            this.startRemoteGhostingButton.UseVisualStyleBackColor = true;
            this.startRemoteGhostingButton.Click += new System.EventHandler(this.startRemoteGhostingButton_Click);
            // 
            // startGhostLocalButton
            // 
            this.startGhostLocalButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startGhostLocalButton.Location = new System.Drawing.Point(196, -3);
            this.startGhostLocalButton.Name = "startGhostLocalButton";
            this.startGhostLocalButton.Size = new System.Drawing.Size(86, 23);
            this.startGhostLocalButton.TabIndex = 2;
            this.startGhostLocalButton.Text = "Connect Local";
            this.toolTipsGhoster.SetToolTip(this.startGhostLocalButton, "Ghost your own match");
            this.startGhostLocalButton.UseVisualStyleBackColor = true;
            this.startGhostLocalButton.Click += new System.EventHandler(this.startGhostLocalButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusLabel.Location = new System.Drawing.Point(0, 251);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(115, 13);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "Status: Not Connected";
            this.toolTipsGhoster.SetToolTip(this.statusLabel, "CS:GO Ghoster Status");
            // 
            // replaceMapButton
            // 
            this.replaceMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceMapButton.Location = new System.Drawing.Point(196, 241);
            this.replaceMapButton.Name = "replaceMapButton";
            this.replaceMapButton.Size = new System.Drawing.Size(66, 23);
            this.replaceMapButton.TabIndex = 7;
            this.replaceMapButton.Text = "Use Map";
            this.toolTipsGhoster.SetToolTip(this.replaceMapButton, "Resize and replace the old map");
            this.replaceMapButton.UseVisualStyleBackColor = true;
            this.replaceMapButton.Click += new System.EventHandler(this.replaceMapButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsButton.BackgroundImage = global::CSGO_Ghoster.Properties.Resources._489749_Gear_512;
            this.settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.settingsButton.Location = new System.Drawing.Point(259, 241);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(25, 23);
            this.settingsButton.TabIndex = 8;
            this.toolTipsGhoster.SetToolTip(this.settingsButton, "Settings");
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // MapImage
            // 
            this.MapImage.BackgroundImage = global::CSGO_Ghoster.Properties.Resources.ghost_chicken;
            this.MapImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.MapImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapImage.Location = new System.Drawing.Point(0, 0);
            this.MapImage.Name = "MapImage";
            this.MapImage.Size = new System.Drawing.Size(284, 264);
            this.MapImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.MapImage.TabIndex = 4;
            this.MapImage.TabStop = false;
            // 
            // GhosterWindowInformationLabel
            // 
            this.GhosterWindowInformationLabel.AutoSize = true;
            this.GhosterWindowInformationLabel.BackColor = System.Drawing.SystemColors.WindowText;
            this.GhosterWindowInformationLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.GhosterWindowInformationLabel.Location = new System.Drawing.Point(112, 121);
            this.GhosterWindowInformationLabel.Name = "GhosterWindowInformationLabel";
            this.GhosterWindowInformationLabel.Padding = new System.Windows.Forms.Padding(5);
            this.GhosterWindowInformationLabel.Size = new System.Drawing.Size(67, 49);
            this.GhosterWindowInformationLabel.TabIndex = 9;
            this.GhosterWindowInformationLabel.Text = "Map Size: \r\nTop: \r\nLeft: ";
            this.GhosterWindowInformationLabel.Visible = false;
            this.GhosterWindowInformationLabel.Click += new System.EventHandler(this.GhosterWindowInformationLabel_Click);
            this.GhosterWindowInformationLabel.MouseHover += new System.EventHandler(this.GhosterWindowInformationLabel_MouseHover);
            // 
            // DebugBox
            // 
            this.DebugBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugBox.Location = new System.Drawing.Point(10, 38);
            this.DebugBox.Name = "DebugBox";
            this.DebugBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.DebugBox.Size = new System.Drawing.Size(262, 197);
            this.DebugBox.TabIndex = 10;
            this.DebugBox.Text = "Debugger:";
            this.DebugBox.Click += new System.EventHandler(this.DebugBox_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.GhosterWindowInformationLabel);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.replaceMapButton);
            this.Controls.Add(this.ipTextBox);
            this.Controls.Add(this.startRemoteGhostingButton);
            this.Controls.Add(this.startGhostLocalButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.DebugBox);
            this.Controls.Add(this.MapImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(165, 165);
            this.Name = "Form1";
            this.Text = "CS:GO Ghoster";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.MapImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox ipTextBox;
        public System.Windows.Forms.Button startRemoteGhostingButton;
        public System.Windows.Forms.Button startGhostLocalButton;
        public System.Windows.Forms.PictureBox MapImage;
        public System.Windows.Forms.Label statusLabel;
        public System.Windows.Forms.ToolTip toolTipsGhoster;
        public System.Windows.Forms.Button replaceMapButton;
        public System.Windows.Forms.Button settingsButton;
        public System.Windows.Forms.Label GhosterWindowInformationLabel;
        public System.Windows.Forms.RichTextBox DebugBox;
        public System.Windows.Forms.BindingSource bindingSource1;
    }
}

