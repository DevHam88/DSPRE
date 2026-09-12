namespace DSPRE.Editors
{
    partial class TrainerClassPresentationEditor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.trainerClassLabel = new System.Windows.Forms.Label();
            this.presentationGroupBox = new System.Windows.Forms.GroupBox();
            this.style2TimingUpDown = new System.Windows.Forms.NumericUpDown();
            this.style2TimingLabel = new System.Windows.Forms.Label();
            this.style1MotionUpDown = new System.Windows.Forms.NumericUpDown();
            this.style1MotionLabel = new System.Windows.Forms.Label();
            this.savedRivalComboBox = new System.Windows.Forms.ComboBox();
            this.savedRivalLabel = new System.Windows.Forms.Label();
            this.trainerNameComboBox = new DSPRE.InputComboBox();
            this.trainerNameLabel = new System.Windows.Forms.Label();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.styleLabel = new System.Windows.Forms.Label();
            this.assetsGroupBox = new System.Windows.Forms.GroupBox();
            this.assetsGrid = new System.Windows.Forms.DataGridView();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.presentationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.style2TimingUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.style1MotionUpDown)).BeginInit();
            this.assetsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetsGrid)).BeginInit();
            this.SuspendLayout();
            //
            // trainerClassLabel
            //
            this.trainerClassLabel.AutoSize = true;
            this.trainerClassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.trainerClassLabel.Location = new System.Drawing.Point(12, 12);
            this.trainerClassLabel.Name = "trainerClassLabel";
            this.trainerClassLabel.Size = new System.Drawing.Size(79, 13);
            this.trainerClassLabel.TabIndex = 0;
            this.trainerClassLabel.Text = "Trainer class";
            //
            // presentationGroupBox
            //
            this.presentationGroupBox.Controls.Add(this.style2TimingUpDown);
            this.presentationGroupBox.Controls.Add(this.style2TimingLabel);
            this.presentationGroupBox.Controls.Add(this.style1MotionUpDown);
            this.presentationGroupBox.Controls.Add(this.style1MotionLabel);
            this.presentationGroupBox.Controls.Add(this.savedRivalComboBox);
            this.presentationGroupBox.Controls.Add(this.savedRivalLabel);
            this.presentationGroupBox.Controls.Add(this.trainerNameComboBox);
            this.presentationGroupBox.Controls.Add(this.trainerNameLabel);
            this.presentationGroupBox.Controls.Add(this.styleComboBox);
            this.presentationGroupBox.Controls.Add(this.styleLabel);
            this.presentationGroupBox.Location = new System.Drawing.Point(12, 34);
            this.presentationGroupBox.Name = "presentationGroupBox";
            this.presentationGroupBox.Size = new System.Drawing.Size(720, 126);
            this.presentationGroupBox.TabIndex = 1;
            this.presentationGroupBox.TabStop = false;
            this.presentationGroupBox.Text = "Presentation";
            //
            // styleComboBox
            //
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Location = new System.Drawing.Point(15, 36);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(260, 21);
            this.styleComboBox.TabIndex = 1;
            this.styleComboBox.SelectedIndexChanged += new System.EventHandler(this.styleComboBox_SelectedIndexChanged);
            //
            // styleLabel
            //
            this.styleLabel.AutoSize = true;
            this.styleLabel.Location = new System.Drawing.Point(12, 20);
            this.styleLabel.Name = "styleLabel";
            this.styleLabel.Size = new System.Drawing.Size(48, 13);
            this.styleLabel.TabIndex = 0;
            this.styleLabel.Text = "VS Style";
            //
            // trainerNameComboBox
            //
            this.trainerNameComboBox.FormattingEnabled = true;
            this.trainerNameComboBox.Location = new System.Drawing.Point(296, 36);
            this.trainerNameComboBox.Name = "trainerNameComboBox";
            this.trainerNameComboBox.Size = new System.Drawing.Size(407, 21);
            this.trainerNameComboBox.TabIndex = 3;
            this.trainerNameComboBox.SelectedIndexChanged += new System.EventHandler(this.MarkDirty);
            this.trainerNameComboBox.TextChanged += new System.EventHandler(this.MarkDirty);
            //
            // trainerNameLabel
            //
            this.trainerNameLabel.AutoSize = true;
            this.trainerNameLabel.Location = new System.Drawing.Point(293, 20);
            this.trainerNameLabel.Name = "trainerNameLabel";
            this.trainerNameLabel.Size = new System.Drawing.Size(129, 13);
            this.trainerNameLabel.TabIndex = 2;
            this.trainerNameLabel.Text = "Trainer-name message ID";
            //
            // savedRivalComboBox
            //
            this.savedRivalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savedRivalComboBox.FormattingEnabled = true;
            this.savedRivalComboBox.Items.AddRange(new object[] {
            "0 - Static trainer name",
            "1 - Saved rival name"});
            this.savedRivalComboBox.Location = new System.Drawing.Point(15, 89);
            this.savedRivalComboBox.Name = "savedRivalComboBox";
            this.savedRivalComboBox.Size = new System.Drawing.Size(260, 21);
            this.savedRivalComboBox.TabIndex = 5;
            this.savedRivalComboBox.SelectedIndexChanged += new System.EventHandler(this.savedRivalComboBox_SelectedIndexChanged);
            //
            // savedRivalLabel
            //
            this.savedRivalLabel.AutoSize = true;
            this.savedRivalLabel.Location = new System.Drawing.Point(12, 73);
            this.savedRivalLabel.Name = "savedRivalLabel";
            this.savedRivalLabel.Size = new System.Drawing.Size(105, 13);
            this.savedRivalLabel.TabIndex = 4;
            this.savedRivalLabel.Text = "Style 1 name source";
            //
            // style1MotionUpDown
            //
            this.style1MotionUpDown.Hexadecimal = true;
            this.style1MotionUpDown.Location = new System.Drawing.Point(296, 89);
            this.style1MotionUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.style1MotionUpDown.Name = "style1MotionUpDown";
            this.style1MotionUpDown.Size = new System.Drawing.Size(180, 20);
            this.style1MotionUpDown.TabIndex = 7;
            this.toolTip.SetToolTip(this.style1MotionUpDown, "Displayed in hexadecimal.");
            this.style1MotionUpDown.ValueChanged += new System.EventHandler(this.MarkDirty);
            //
            // style1MotionLabel
            //
            this.style1MotionLabel.AutoSize = true;
            this.style1MotionLabel.Location = new System.Drawing.Point(293, 73);
            this.style1MotionLabel.Name = "style1MotionLabel";
            this.style1MotionLabel.Size = new System.Drawing.Size(119, 13);
            this.style1MotionLabel.TabIndex = 6;
            this.style1MotionLabel.Text = "Style 1 position/motion";
            //
            // style2TimingUpDown
            //
            this.style2TimingUpDown.Location = new System.Drawing.Point(500, 89);
            this.style2TimingUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.style2TimingUpDown.Name = "style2TimingUpDown";
            this.style2TimingUpDown.Size = new System.Drawing.Size(120, 20);
            this.style2TimingUpDown.TabIndex = 9;
            this.style2TimingUpDown.ValueChanged += new System.EventHandler(this.MarkDirty);
            //
            // style2TimingLabel
            //
            this.style2TimingLabel.AutoSize = true;
            this.style2TimingLabel.Location = new System.Drawing.Point(497, 73);
            this.style2TimingLabel.Name = "style2TimingLabel";
            this.style2TimingLabel.Size = new System.Drawing.Size(76, 13);
            this.style2TimingLabel.TabIndex = 8;
            this.style2TimingLabel.Text = "Style 2 timing";
            //
            // assetsGroupBox
            //
            this.assetsGroupBox.Controls.Add(this.assetsGrid);
            this.assetsGroupBox.Location = new System.Drawing.Point(12, 166);
            this.assetsGroupBox.Name = "assetsGroupBox";
            this.assetsGroupBox.Size = new System.Drawing.Size(720, 174);
            this.assetsGroupBox.TabIndex = 2;
            this.assetsGroupBox.TabStop = false;
            this.assetsGroupBox.Text = "Asset Members (/a/1/0/9)";
            //
            // assetsGrid
            //
            this.assetsGrid.AllowUserToAddRows = false;
            this.assetsGrid.AllowUserToDeleteRows = false;
            this.assetsGrid.AllowUserToResizeColumns = false;
            this.assetsGrid.AllowUserToResizeRows = false;
            this.assetsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assetsGrid.Location = new System.Drawing.Point(12, 22);
            this.assetsGrid.MultiSelect = false;
            this.assetsGrid.Name = "assetsGrid";
            this.assetsGrid.RowHeadersWidth = 82;
            this.assetsGrid.RowTemplate.Height = 32;
            this.assetsGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.assetsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.assetsGrid.Size = new System.Drawing.Size(696, 138);
            this.assetsGrid.TabIndex = 0;
            this.assetsGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.assetsGrid_CellValidating);
            this.assetsGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.assetsGrid_CellValueChanged);
            //
            // saveButton
            //
            this.saveButton.Location = new System.Drawing.Point(576, 353);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 28);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            //
            // cancelButton
            //
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(657, 353);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 28);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            //
            // TrainerClassPresentationEditor
            //
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(744, 393);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.assetsGroupBox);
            this.Controls.Add(this.presentationGroupBox);
            this.Controls.Add(this.trainerClassLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrainerClassPresentationEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trainer Class Presentation Metadata";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrainerClassPresentationEditor_FormClosing);
            this.presentationGroupBox.ResumeLayout(false);
            this.presentationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.style2TimingUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.style1MotionUpDown)).EndInit();
            this.assetsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.assetsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label trainerClassLabel;
        private System.Windows.Forms.GroupBox presentationGroupBox;
        private System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.Label styleLabel;
        private DSPRE.InputComboBox trainerNameComboBox;
        private System.Windows.Forms.Label trainerNameLabel;
        private System.Windows.Forms.ComboBox savedRivalComboBox;
        private System.Windows.Forms.Label savedRivalLabel;
        private System.Windows.Forms.NumericUpDown style1MotionUpDown;
        private System.Windows.Forms.Label style1MotionLabel;
        private System.Windows.Forms.NumericUpDown style2TimingUpDown;
        private System.Windows.Forms.Label style2TimingLabel;
        private System.Windows.Forms.GroupBox assetsGroupBox;
        private System.Windows.Forms.DataGridView assetsGrid;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
