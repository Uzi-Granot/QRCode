namespace QRCodeEncoder
	{
	partial class SaveImage
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
			this.label6 = new System.Windows.Forms.Label();
			this.ImageFormatComboBox = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.QuietZoneTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ModuleSizeTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SaveToPngButton = new System.Windows.Forms.Button();
			this.SaveImageButton = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(14, 124);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(87, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Image Format";
			// 
			// ImageFormatComboBox
			// 
			this.ImageFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ImageFormatComboBox.FormattingEnabled = true;
			this.ImageFormatComboBox.Location = new System.Drawing.Point(107, 121);
			this.ImageFormatComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.ImageFormatComboBox.Name = "ImageFormatComboBox";
			this.ImageFormatComboBox.Size = new System.Drawing.Size(106, 24);
			this.ImageFormatComboBox.TabIndex = 6;
			this.ImageFormatComboBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnImageFileFormat);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(29, 257);
			this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(169, 32);
			this.button1.TabIndex = 9;
			this.button1.Text = "Save Special";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnSaveSpecial);
			// 
			// QuietZoneTextBox
			// 
			this.QuietZoneTextBox.Location = new System.Drawing.Point(135, 88);
			this.QuietZoneTextBox.Name = "QuietZoneTextBox";
			this.QuietZoneTextBox.Size = new System.Drawing.Size(42, 22);
			this.QuietZoneTextBox.TabIndex = 4;
			this.QuietZoneTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Quiet zone (pix)";
			// 
			// ModuleSizeTextBox
			// 
			this.ModuleSizeTextBox.Location = new System.Drawing.Point(135, 55);
			this.ModuleSizeTextBox.Name = "ModuleSizeTextBox";
			this.ModuleSizeTextBox.Size = new System.Drawing.Size(42, 22);
			this.ModuleSizeTextBox.TabIndex = 2;
			this.ModuleSizeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 58);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Module size (pix)";
			// 
			// SaveToPngButton
			// 
			this.SaveToPngButton.Location = new System.Drawing.Point(29, 169);
			this.SaveToPngButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.SaveToPngButton.Name = "SaveToPngButton";
			this.SaveToPngButton.Size = new System.Drawing.Size(169, 32);
			this.SaveToPngButton.TabIndex = 7;
			this.SaveToPngButton.Text = "Save Compressed PNG";
			this.SaveToPngButton.UseVisualStyleBackColor = true;
			this.SaveToPngButton.Click += new System.EventHandler(this.OnSavePng);
			// 
			// SaveImageButton
			// 
			this.SaveImageButton.Location = new System.Drawing.Point(29, 213);
			this.SaveImageButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.SaveImageButton.Name = "SaveImageButton";
			this.SaveImageButton.Size = new System.Drawing.Size(169, 32);
			this.SaveImageButton.TabIndex = 8;
			this.SaveImageButton.Text = "Save Bitmap Image";
			this.SaveImageButton.UseVisualStyleBackColor = true;
			this.SaveImageButton.Click += new System.EventHandler(this.OnSaveBitmap);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(29, 301);
			this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(169, 32);
			this.button2.TabIndex = 10;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnCancel);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(28, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(170, 19);
			this.label3.TabIndex = 0;
			this.label3.Text = "Save QR Code Image";
			// 
			// SaveImage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(226, 352);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.ImageFormatComboBox);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.QuietZoneTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ModuleSizeTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SaveToPngButton);
			this.Controls.Add(this.SaveImageButton);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SaveImage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SaveImage";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

			}

		#endregion

		private Label label6;
		private ComboBox ImageFormatComboBox;
		private Button button1;
		private TextBox QuietZoneTextBox;
		private Label label2;
		private TextBox ModuleSizeTextBox;
		private Label label1;
		private Button SaveToPngButton;
		private Button SaveImageButton;
		private Button button2;
		private Label label3;
		}
	}