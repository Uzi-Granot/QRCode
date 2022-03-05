namespace QRCodeDecoder
{
	partial class QRCodeDecoderDemo
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QRCodeDecoderDemo));
			this.DecodedDataLabel = new System.Windows.Forms.Label();
			this.ImageFileButton = new System.Windows.Forms.Button();
			this.HeaderLabel = new System.Windows.Forms.Label();
			this.DataTextBox = new System.Windows.Forms.TextBox();
			this.ECIAssignLabel = new System.Windows.Forms.Label();
			this.ECIValueLabel = new System.Windows.Forms.Label();
			this.VideoCameraButton = new System.Windows.Forms.Button();
			this.GoToUrlButton = new System.Windows.Forms.Button();
			this.ViewingPanel = new System.Windows.Forms.Panel();
			this.QRCodeDimensionLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ErrorCodeLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// DecodedDataLabel
			// 
			this.DecodedDataLabel.AutoSize = true;
			this.DecodedDataLabel.Location = new System.Drawing.Point(7, 420);
			this.DecodedDataLabel.Name = "DecodedDataLabel";
			this.DecodedDataLabel.Size = new System.Drawing.Size(87, 16);
			this.DecodedDataLabel.TabIndex = 10;
			this.DecodedDataLabel.Text = "Decoded data";
			// 
			// ImageFileButton
			// 
			this.ImageFileButton.Location = new System.Drawing.Point(7, 61);
			this.ImageFileButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.ImageFileButton.Name = "ImageFileButton";
			this.ImageFileButton.Size = new System.Drawing.Size(100, 36);
			this.ImageFileButton.TabIndex = 1;
			this.ImageFileButton.Text = "Image File";
			this.ImageFileButton.UseVisualStyleBackColor = true;
			this.ImageFileButton.Click += new System.EventHandler(this.OnLoadImage);
			// 
			// HeaderLabel
			// 
			this.HeaderLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.HeaderLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.HeaderLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.HeaderLabel.Location = new System.Drawing.Point(210, 11);
			this.HeaderLabel.Name = "HeaderLabel";
			this.HeaderLabel.Size = new System.Drawing.Size(344, 32);
			this.HeaderLabel.TabIndex = 0;
			this.HeaderLabel.Text = "QR Code Decoder";
			this.HeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DataTextBox
			// 
			this.DataTextBox.AcceptsReturn = true;
			this.DataTextBox.BackColor = System.Drawing.SystemColors.Info;
			this.DataTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DataTextBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.DataTextBox.Location = new System.Drawing.Point(8, 440);
			this.DataTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.DataTextBox.Multiline = true;
			this.DataTextBox.Name = "DataTextBox";
			this.DataTextBox.ReadOnly = true;
			this.DataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.DataTextBox.Size = new System.Drawing.Size(743, 101);
			this.DataTextBox.TabIndex = 11;
			this.DataTextBox.TabStop = false;
			this.DataTextBox.Text = "\r\n";
			// 
			// ECIAssignLabel
			// 
			this.ECIAssignLabel.AutoSize = true;
			this.ECIAssignLabel.Location = new System.Drawing.Point(7, 273);
			this.ECIAssignLabel.Name = "ECIAssignLabel";
			this.ECIAssignLabel.Size = new System.Drawing.Size(64, 16);
			this.ECIAssignLabel.TabIndex = 7;
			this.ECIAssignLabel.Text = "ECI Value";
			// 
			// ECIValueLabel
			// 
			this.ECIValueLabel.BackColor = System.Drawing.SystemColors.Info;
			this.ECIValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ECIValueLabel.Location = new System.Drawing.Point(7, 289);
			this.ECIValueLabel.Name = "ECIValueLabel";
			this.ECIValueLabel.Size = new System.Drawing.Size(67, 20);
			this.ECIValueLabel.TabIndex = 8;
			this.ECIValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// VideoCameraButton
			// 
			this.VideoCameraButton.Location = new System.Drawing.Point(7, 105);
			this.VideoCameraButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.VideoCameraButton.Name = "VideoCameraButton";
			this.VideoCameraButton.Size = new System.Drawing.Size(100, 36);
			this.VideoCameraButton.TabIndex = 2;
			this.VideoCameraButton.Text = "Video Camera";
			this.VideoCameraButton.UseVisualStyleBackColor = true;
			this.VideoCameraButton.Click += new System.EventHandler(this.OnVideoCamera);
			// 
			// GoToUrlButton
			// 
			this.GoToUrlButton.Location = new System.Drawing.Point(7, 380);
			this.GoToUrlButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.GoToUrlButton.Name = "GoToUrlButton";
			this.GoToUrlButton.Size = new System.Drawing.Size(100, 36);
			this.GoToUrlButton.TabIndex = 9;
			this.GoToUrlButton.Text = "Go to URL";
			this.GoToUrlButton.UseVisualStyleBackColor = true;
			this.GoToUrlButton.Click += new System.EventHandler(this.OnGoToUrl);
			// 
			// ViewingPanel
			// 
			this.ViewingPanel.Location = new System.Drawing.Point(114, 61);
			this.ViewingPanel.Name = "ViewingPanel";
			this.ViewingPanel.Size = new System.Drawing.Size(640, 360);
			this.ViewingPanel.TabIndex = 12;
			this.ViewingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnViewingPanelPaint);
			// 
			// QRCodeDimensionLabel
			// 
			this.QRCodeDimensionLabel.BackColor = System.Drawing.SystemColors.Info;
			this.QRCodeDimensionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.QRCodeDimensionLabel.Location = new System.Drawing.Point(8, 204);
			this.QRCodeDimensionLabel.Name = "QRCodeDimensionLabel";
			this.QRCodeDimensionLabel.Size = new System.Drawing.Size(67, 20);
			this.QRCodeDimensionLabel.TabIndex = 4;
			this.QRCodeDimensionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 186);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Dimension";
			// 
			// ErrorCodeLabel
			// 
			this.ErrorCodeLabel.BackColor = System.Drawing.SystemColors.Info;
			this.ErrorCodeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ErrorCodeLabel.Location = new System.Drawing.Point(7, 247);
			this.ErrorCodeLabel.Name = "ErrorCodeLabel";
			this.ErrorCodeLabel.Size = new System.Drawing.Size(67, 20);
			this.ErrorCodeLabel.TabIndex = 6;
			this.ErrorCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 229);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Error code";
			// 
			// QRCodeDecoderDemo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.ClientSize = new System.Drawing.Size(764, 561);
			this.Controls.Add(this.ErrorCodeLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.QRCodeDimensionLabel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ViewingPanel);
			this.Controls.Add(this.GoToUrlButton);
			this.Controls.Add(this.VideoCameraButton);
			this.Controls.Add(this.ECIValueLabel);
			this.Controls.Add(this.ECIAssignLabel);
			this.Controls.Add(this.ImageFileButton);
			this.Controls.Add(this.DataTextBox);
			this.Controls.Add(this.DecodedDataLabel);
			this.Controls.Add(this.HeaderLabel);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimumSize = new System.Drawing.Size(780, 600);
			this.Name = "QRCodeDecoderDemo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this.Resize += new System.EventHandler(this.OnResize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label DecodedDataLabel;
		private System.Windows.Forms.Button ImageFileButton;
		private System.Windows.Forms.Label HeaderLabel;
		private System.Windows.Forms.TextBox DataTextBox;
		private System.Windows.Forms.Label ECIAssignLabel;
		private System.Windows.Forms.Label ECIValueLabel;
		private System.Windows.Forms.Button VideoCameraButton;
		private System.Windows.Forms.Button GoToUrlButton;
		private System.Windows.Forms.Panel ViewingPanel;
		private System.Windows.Forms.Label QRCodeDimensionLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label ErrorCodeLabel;
		private System.Windows.Forms.Label label3;
		}
}

