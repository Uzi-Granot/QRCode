/////////////////////////////////////////////////////////////////////
//
//	QR Code Encoder Library
//
//	QR Code Encoder demo/test application.
//
//	Author: Uzi Granot
//	Original Version: 1.0.0
//
//	Date: June 30, 2018
//
//	Copyright (C) 2018-2022 Uzi Granot. All Rights Reserved
//
//	QR Code Library C# class library and the attached test/demo
//  applications are free software.
//	Software developed by this author is licensed under CPOL 1.02.
//	Some portions of the QRCodeVideoDecoder are licensed under GNU Lesser
//	General Public License v3.0.
//
//	The main points of CPOL 1.02 subject to the terms of the License are:
//
//	Source Code and Executable Files can be used in commercial applications;
//	Source Code and Executable Files can be redistributed; and
//	Source Code can be modified to create derivative works.
//	No claim of suitability, guarantee, or any warranty whatsoever is
//	provided. The software is provided "as-is".
//	The Article accompanying the Work may not be distributed or republished
//	without the Author's consent
//
//	For full version history please look at QREncode.cs
/////////////////////////////////////////////////////////////////////

using QRCodeEncoderLibrary;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Text;

namespace QRCodeEncoder
	{
	/// <summary>
	/// Test QR Code Encoder
	/// </summary>
	public partial class QRCodeEncoderDemo : Form
		{
		private bool[,] QRCodeMatrix;
		private ErrorCorrection ErrCorrection;
		private int ModuleSize;
		private int QuietZone;
		private ImageFormat FileFormat;
		private Bitmap QRCodeImage;
		private Rectangle QRCodeImageArea = new();

		/// <summary>
		/// Constructor
		/// </summary>
		public QRCodeEncoderDemo()
			{
			InitializeComponent();
			return;
			}

		/// <summary>
		/// Test program initialization
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		private void OnLoad(object sender, EventArgs e)
			{
			// program title
			Text = "QRCodeEncoderDemo - " + QREncoder.VersionNumber + " \u00a9 2018-2022 Uzi Granot. All rights reserved.";

#if DEBUG
			// change current directory to Work directory if exits
			string CurDir = Environment.CurrentDirectory;
			int Index = CurDir.IndexOf("bin\\Debug");
			if(Index > 0)
				{
				string WorkDir = string.Concat(CurDir.AsSpan(0, Index), "\\Work");
				if(Directory.Exists(WorkDir)) Environment.CurrentDirectory = WorkDir;
				}
#endif

			// load error correction combo box
			ErrorCorrectionComboBox.Items.Add("L (7%)");
			ErrorCorrectionComboBox.Items.Add("M (15%)");
			ErrorCorrectionComboBox.Items.Add("Q (25%)");
			ErrorCorrectionComboBox.Items.Add("H (30%)");
			ErrorCorrectionComboBox.SelectedIndex = 1;

			// initial image constants
			ModuleSize = 2;
			QuietZone = 8;
			FileFormat = ImageFormat.Jpeg;

			// set initial screen
			DataTextBox.Text = Text + "\r\n" +
				"The QR Code libraries allows your program to create (encode) QR Code image or, read (decode) an\r\n" +
				"image containing one or more QR Codes. The attached source code is made of two solutions, a\r\n" +
				"QR Code encoder solution and a QR Code decoder solution. The software was upgraded to VS 2022\r\n" +
				".NET 6.0 The source code is written in C#. It is an open-source code.";

			// enable create and disable save
			EnableButtons(true);

			// force resize
			OnResize(sender, e);
			return;
			}

		/// <summary>
		/// Create QR Code image
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		private void OnEncode(object sender, EventArgs e)
			{
			// get error correction code
			ErrCorrection = (ErrorCorrection) ErrorCorrectionComboBox.SelectedIndex;

			// get eci value
			int EciValue = -1;
			string EciStr = ECIValueTextBox.Text.Trim();
			if(!string.IsNullOrWhiteSpace(EciStr))
				{
				if(!int.TryParse(EciStr, out EciValue) || EciValue < 0 || EciValue > 999999)
					{
					MessageBox.Show("ECI value must be blank or 0-999999.");
					return;
					}
				}

			// get data for QR Code
			string Data = DataTextBox.Text.Trim();
			if(Data.Length == 0)
				{
				MessageBox.Show("Data must not be empty.");
				return;
				}

			// disable buttons
			EnableButtons(false);

			// encoding try-catch block
			try
				{
				// create encoder object
				QREncoder Encoder = new();

				// set error correction code
				Encoder.ErrorCorrection = ErrCorrection;

				// set ECI value
				Encoder.ECIAssignValue = EciValue;

				// multi segment
				// note: double vertical line "||" is treated as in text |
				if(SeparatorCheckBox.Checked && Data.Contains('|'))
					{
					List<string> Segments = new();
					StringBuilder Str = new();

					// split the text into segments
					for(int Index = 0; Index < Data.Length; Index++)
						{
						char Chr = Data[Index];
						if(Chr != '|')
							{
							Str.Append(Chr);
							continue;
							}
						if(Index + 1 < Data.Length && Data[Index + 1] == '|')
							{
							Str.Append('|');
							Index++;
							continue;
							}
						if (Str.Length != 0)
							{
							Segments.Add(Str.ToString());
							Str.Length = 0;
							}
						}

					// last segment
					if (Str.Length != 0)
						{
						Segments.Add(Str.ToString());
						}

					// encode multi-segments text case
					QRCodeMatrix = Encoder.Encode(Segments.ToArray());
					}

				// single segment
				else
					{
					// encode data
					QRCodeMatrix = Encoder.Encode(Data);
					}

				// display RR code matrix dimension
				DimensionLabel.Text = QRCodeMatrix.GetLength(0).ToString();

				// convert QR code matrix to Bitmap
				// create bitmap image object
				QRSaveBitmapImage BitmapImage = new(QRCodeMatrix);

				// module size in pixels
				BitmapImage.ModuleSize = ModuleSize;

				// quiet zone in pixels
				BitmapImage.QuietZone = QuietZone;

				// create bitmap image
				QRCodeImage = BitmapImage.CreateQRCodeBitmap();
				}

			catch (Exception Ex)
				{
				DimensionLabel.Text = null;
				QRCodeImage = null;
				MessageBox.Show("Encoding exception.\r\n" + Ex.Message);
				}

			// enable buttons
			EnableButtons(true);

			// repaint panel
			Invalidate();
			return;
			}

		/// <summary>
		/// Save image
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		private void OnSaveImage(object sender, EventArgs e)
			{
			// save image dialog
			SaveImage SaveImageDialog = new();
			SaveImageDialog.ModuleSize = ModuleSize;
			SaveImageDialog.QuietZone = QuietZone;
			SaveImageDialog.FileFormat = FileFormat;

			// display dialog
			if(SaveImageDialog.ShowDialog() != DialogResult.OK) return;

			// get result
			ModuleSize = SaveImageDialog.ModuleSize;
			QuietZone = SaveImageDialog.QuietZone;
			FileFormat = SaveImageDialog.FileFormat;

			// switch based on typw of save
			switch (SaveImageDialog.SaveIndex)
				{
				case SaveType.Png:
					SavePNG();
					break;

				case SaveType.Bitmap:
					SaveBitmap();
					break;

				case SaveType.Special:
					SaveSpecial();
					break;
				}
			return;
			}

		/////////////////////////////////////////////////////////////////////
		// save barcode image
		/////////////////////////////////////////////////////////////////////

		private void SavePNG()
			{
			// save file name dialog box
			string FileName = SaveFileName(ImageFormat.Png);
			if(FileName == null) return;

			// save image as png file
			QRSavePngImage PngImage = new(QRCodeMatrix);
			PngImage.ModuleSize = ModuleSize;
			PngImage.QuietZone = QuietZone;
			PngImage.SaveQRCodeToPngFile(FileName);

			// start image editor
			Process Proc = new();
			Proc.StartInfo = new ProcessStartInfo(FileName) { UseShellExecute = true };
			Proc.Start();
			return;
			}

		/////////////////////////////////////////////////////////////////////
		// save barcode image
		/////////////////////////////////////////////////////////////////////

		private void SaveBitmap()
			{
			// save file name dialog box
			string FileName = SaveFileName(FileFormat);
			if (FileName == null) return;

			// save image as png file
			QRSaveBitmapImage BitmapImage = new(QRCodeMatrix);
			BitmapImage.ModuleSize = ModuleSize;
			BitmapImage.QuietZone = QuietZone;
			BitmapImage.SaveQRCodeToImageFile(FileName, FileFormat);

			// start image editor
			Process Proc = new();
			Proc.StartInfo = new ProcessStartInfo(FileName) { UseShellExecute = true };
			Proc.Start();
			return;
			}

		/// <summary>
		/// Save image
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		private void SaveSpecial()
			{
			SaveImageSpecial Dialog = new();
			Dialog.QRCodeMatrix = QRCodeMatrix;
			Dialog.QRCodeBitmapImage = QRCodeImage;
			Dialog.ModuleSize = ModuleSize;
			Dialog.QuietZone = QuietZone;
			Dialog.FileFormat = FileFormat;
			Dialog.ShowDialog(this);
			return;
			}

		/// <summary>
		/// Save file name
		/// </summary>
		/// <param name="ImageFormat">Image format</param>
		/// <returns>File Name</returns>
		internal static string SaveFileName
				(
				ImageFormat ImageFormat
				)
			{
			// save file dialog box
			SaveFileDialog Dialog = new();
			Dialog.AddExtension = true;
			Dialog.Filter = string.Format("{0} Image|*.{1}", ImageFormat.ToString(), ImageFormat.ToString().ToLower());
			Dialog.Title = "Save QR Code Image";
			Dialog.InitialDirectory = Directory.GetCurrentDirectory();
			Dialog.RestoreDirectory = true;
			Dialog.FileName = string.Format("QRCodeImage.{0}", ImageFormat.ToString().ToLower());
			if (Dialog.ShowDialog() == DialogResult.OK) return Dialog.FileName;
			return Dialog.ShowDialog() == DialogResult.OK ? Dialog.FileName : null;
			}

		/// <summary>
		/// Paint QR Code image
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		private void OnPaint(object sender, PaintEventArgs e)
			{
			// no image
			if(QRCodeImage == null) return;

			// image height to preserve aspect ratio
			int ImageHeight = (QRCodeImageArea.Width * QRCodeImage.Height) / QRCodeImage.Width;
			int ImageWidth;
			if(ImageHeight <= QRCodeImageArea.Height)
				{
				ImageWidth = QRCodeImageArea.Width;
				}
			else
				{
				ImageWidth = (QRCodeImageArea.Height * QRCodeImage.Width) / QRCodeImage.Height;
				ImageHeight = QRCodeImageArea.Height;
				}

			// calculate position
			int ImageX = QRCodeImageArea.X + (QRCodeImageArea.Width - ImageWidth) / 2;
			int ImageY = QRCodeImageArea.Y + (QRCodeImageArea.Height - ImageHeight) / 2;
			e.Graphics.DrawImage(QRCodeImage, new Rectangle(ImageX, ImageY, ImageWidth, ImageHeight));
			return;
			}

		/// <summary>
		/// Enabled buttons
		/// </summary>
		private void EnableButtons
				(
				bool Enabled
				)
			{
			EncodeButton.Enabled = Enabled;
			SaveImageButton.Enabled = QRCodeImage != null && Enabled;
			return;
			}

		/// <summary>
		/// Resize frame
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event arguments</param>
		private void OnResize(object sender, EventArgs e)
			{
			if(ClientSize.Width == 0) return;

			// center header label
			HeaderLabel.Left = (ClientSize.Width - HeaderLabel.Width) / 2;

			// put data text box at the bottom of client area
			DataTextBox.Top = ClientSize.Height - DataTextBox.Height - 8;
			DataTextBox.Width = ClientSize.Width - 2 * DataTextBox.Left;

			// put data label above text box
			DataLabel.Top = DataTextBox.Top - DataLabel.Height - 3;

			// put separator check box above and to the right of the text box
			SeparatorCheckBox.Top = DataTextBox.Top - SeparatorCheckBox.Height - 3;
			SeparatorCheckBox.Left = DataTextBox.Right - SeparatorCheckBox.Width;

			// put buttons half way between header and data text
			ButtonsGroupBox.Top = (DataLabel.Top + HeaderLabel.Top - ButtonsGroupBox.Height) / 2;

			// image area
			QRCodeImageArea.X = ButtonsGroupBox.Right + 4;
			QRCodeImageArea.Y = HeaderLabel.Bottom + 4;
			QRCodeImageArea.Width = ClientSize.Width - QRCodeImageArea.X - 4;
			QRCodeImageArea.Height = DataLabel.Top - QRCodeImageArea.Y - 4;

			// force re-paint
			Invalidate();
			return;
			}
		}
	}
