/////////////////////////////////////////////////////////////////////
//
//	QR Code Encoder Library
//
//	QR Code encoder save image screen.
//
//	Author: Uzi Granot
//	Original Version: 1.0
//	Date: June 30, 2018
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

using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace QRCodeEncoder
	{
	/// <summary>
	/// Error spot control
	/// </summary>
	public enum ErrorSpotControl
		{
		/// <summary>
		/// No errors
		/// </summary>
		None,

		/// <summary>
		/// White spots
		/// </summary>
		White,

		/// <summary>
		/// Black spots
		/// </summary>
		Black,

		/// <summary>
		/// White and black spots
		/// </summary>
		Alternate,
		}

	/// <summary>
	/// Save QR code as an image file
	/// </summary>
	public partial class SaveImageSpecial : Form
		{
		#pragma warning disable CS0649
		internal bool[,] QRCodeMatrix;
		internal Bitmap QRCodeBitmapImage;
		internal int ModuleSize;
		internal int QuietZone;
		internal ImageFormat FileFormat;
		#pragma warning disable CS0649

		private int QRCodeDimension;
		private int QRCodeImageDimension;
		private Bitmap BGImageBitmap;

		/// <summary>
		/// Save image constructor
		/// </summary>
		public SaveImageSpecial()
			{
			// initialize component
			InitializeComponent();
			return;
			}

		private void OnLoad(object sender, EventArgs e)
			{
			QRCodeDimension = QRCodeMatrix.GetLength(0);
			QRCodeImageDimension = QRCodeDimension * ModuleSize + 2 * QuietZone;

			// display initial values
			QRCodeDimensionLabel.Text = QRCodeDimension.ToString();
			ModuleSizeLabel.Text = ModuleSize.ToString();
			QuietZoneLabel.Text = QuietZone.ToString();
			QRImageDimensionLabel.Text = QRCodeImageDimension.ToString();
			FileFormatLabel.Text = FileFormat.ToString();
			int BGImageSide = (int) (1.6 * QRCodeImageDimension);
			BrushWidthTextBox.Text = BGImageSide.ToString();
			BrushHeightTextBox.Text = BGImageSide.ToString();
			CameraDistanceTextBox.Text = (2 * BGImageSide).ToString();

			// load hatch style combo box
			for(int Index = -1; Index < 53; Index++)
				HatchStyleComboBox.Items.Add(Index);
			HatchStyleComboBox.SelectedIndex = 0;

			BrushColorButton.BackColor = Color.LightSkyBlue;
			ImageRotationTextBox.Text = "0";
			CameraViewRotationTextBox.Text = "0";
			CameraViewRotationTextBox.Text = "0";

			// set none radio button
			NoneRadioButton.Checked = true;

			// set none error radio button
			ErrorNoneRadioButton.Checked = true;
			ErrorDiameterTextBox.Text = (ModuleSize * 2).ToString();
			ErrorNumberTextBox.Text = "10";
			return;
			}

		private void OnImageFileFormat
				(
				object sender,
				ListControlConvertEventArgs e
				)
			{
			ImageFormat Format = (ImageFormat) e.ListItem;
			e.Value = string.Format("{0} (*.{1})", Format.ToString(), Format.ToString().ToLower());
			return;
			}

		// brush hatch style format
		private void OnHatchStyleFormat(object sender, ListControlConvertEventArgs e)
			{
			e.Value = (int) e.ListItem < 0 ? "Solid" : ((HatchStyle) e.ListItem).ToString();
			return;
			}

		// user changed brush background area size
		private void OnBrushSizeChanged(object sender, EventArgs e)
			{
			if(int.TryParse(BrushWidthTextBox.Text, out int BrushWidth) && BrushWidth > 0 && BrushWidth <= 10000 &&
				int.TryParse(BrushHeightTextBox.Text, out int BrushHeight) && BrushHeight > 0 && BrushHeight <= 10000)
				{
				QRCodePosXTextBox.Text = (BrushWidth / 2).ToString();
				QRCodePosYTextBox.Text = (BrushHeight / 2).ToString();
				}
			return;
			}

		// background radio button checked
		private void OnBackgroundRadioButton(object sender, EventArgs e)
			{
			// radio button changed from off to on
			if(((RadioButton) sender).Checked)
				{
				// enable/disable controls
				EnableControls();
				}
			return;
			}

		private void EnableControls()
			{
			// background image
			ImageBrowseButton.Enabled = ImageRadioButton.Checked;
			ImageFileNameTextBox.Enabled = ImageRadioButton.Checked;

			// background brush
			BrushColorButton.Enabled = BrushRadioButton.Checked;
			HatchStyleComboBox.Enabled = BrushRadioButton.Checked;
			BrushWidthTextBox.Enabled = BrushRadioButton.Checked;
			BrushHeightTextBox.Enabled = BrushRadioButton.Checked;

			// background image or brush
			QRCodePosXTextBox.Enabled = !NoneRadioButton.Checked;
			QRCodePosYTextBox.Enabled = !NoneRadioButton.Checked;
			ImageRotationTextBox.Enabled = !NoneRadioButton.Checked;
			CameraDistanceTextBox.Enabled = !NoneRadioButton.Checked;
			CameraViewRotationTextBox.Enabled = !NoneRadioButton.Checked;
			return;
			}

		private void OnImageBrowse(object sender, EventArgs e)
			{
			// open file dialog box
			OpenFileDialog Dialog = new()
				{
				Filter = "Image Files(*.PNG;*.JPG;*.JPEG;*.BMP;*.GIF)|*.PNG;*.JPG;*.JPEG;*.BMP;*.GIF|All files (*.*)|*.*",
				Title = "Load Background Image",
				InitialDirectory = Directory.GetCurrentDirectory(),
				RestoreDirectory = true
				};

			// user pressed OK
			if (Dialog.ShowDialog() == DialogResult.OK) ImageFileNameTextBox.Text = Dialog.FileName;
			return;
			}

		// background image file name changed
		private void ImageFileNameChanged(object sender, EventArgs e)
			{
			string FileName = ImageFileNameTextBox.Text.Trim();
			if(File.Exists(FileName))
				{
				BGImageBitmap = new Bitmap(FileName);
				ImageBGWidthLabel.Text = BGImageBitmap.Width.ToString();
				ImageBGHeightLabel.Text = BGImageBitmap.Height.ToString();
				QRCodePosXTextBox.Text = (BGImageBitmap.Width / 2).ToString();
				QRCodePosYTextBox.Text = (BGImageBitmap.Height / 2).ToString();
				;
				}
			else
				{
				BGImageBitmap = null;
				ImageBGWidthLabel.Text = string.Empty;
				ImageBGHeightLabel.Text = string.Empty;
				QRCodePosXTextBox.Text = string.Empty;
				QRCodePosYTextBox.Text = string.Empty;
				}
			return;
			}

		// select color
		private void OnSelectColor(object sender, EventArgs e)
			{
			ColorDialog Dialog = new();
			Dialog.FullOpen = true;
			if(Dialog.ShowDialog(this) == DialogResult.OK)
				BrushColorButton.BackColor = Dialog.Color;
			return;
			}

		// add error spots
		private void OnErrorRadioButton(object sender, EventArgs e)
			{
			// radio button changed from off to on
			if(((RadioButton) sender).Checked)
				{
				// enable/disable controls
				ErrorDiameterTextBox.Enabled = !ErrorNoneRadioButton.Checked;
				ErrorNumberTextBox.Enabled = !ErrorNoneRadioButton.Checked;
				}
			return;
			}

		private void OnSaveClick(object sender, EventArgs e)
			{
			// reset some variables
			int CameraDistance = 0;
			int CameraRotation = 0;
			int OutputImageWidth = 0;
			int OutputImageHeight = 0;
			int QRCodePosX = 0;
			int QRCodePosY = 0;
			int ViewXRotation = 0;
			ErrorSpotControl ErrorControl = ErrorSpotControl.None;
			int ErrorDiameter = 0;
			int ErrorSpots = 0;

			// display qr code over image made with a brush
			if(BrushRadioButton.Checked)
				{
				// area width
				if(!int.TryParse(BrushWidthTextBox.Text.Trim(), out OutputImageWidth) ||
					OutputImageWidth <= 0 || OutputImageWidth >= 100000)
					{
					MessageBox.Show("Brush background width is invalid");
					return;
					}

				// area width
				if(!int.TryParse(BrushHeightTextBox.Text.Trim(), out OutputImageHeight) ||
					OutputImageHeight <= 0 || OutputImageHeight >= 100000)
					{
					MessageBox.Show("Brush background height is invalid");
					return;
					}
				}

			// display qr code over an image
			if(ImageRadioButton.Checked)
				{
				// image must be defined
				if(this.BGImageBitmap == null)
					{
					MessageBox.Show("Background image must be defined");
					return;
					}

				OutputImageWidth = this.BGImageBitmap.Width;
				OutputImageHeight = this.BGImageBitmap.Height;
				}

			if(!NoneRadioButton.Checked)
				{
				// QR code position X
				if(!int.TryParse(QRCodePosXTextBox.Text.Trim(), out QRCodePosX) || QRCodePosX <= 0 || QRCodePosX >= OutputImageWidth)
					{
					MessageBox.Show("QR code position X must be within image width");
					return;
					}

				// QR code position Y
				if(!int.TryParse(QRCodePosYTextBox.Text.Trim(), out QRCodePosY) || QRCodePosY <= 0 || QRCodePosY >= OutputImageHeight)
					{
					MessageBox.Show("QR code position Y must be within image height");
					return;
					}

				// rotation
				if(!int.TryParse(ImageRotationTextBox.Text.Trim(), out CameraRotation) || CameraRotation < -360 || CameraRotation > 360)
					{
					MessageBox.Show("Rotation must be -360 to 360");
					return;
					}

				// camera distance
				if(!int.TryParse(CameraDistanceTextBox.Text.Trim(), out CameraDistance) || CameraDistance < 10 * ModuleSize)
					{
					MessageBox.Show("Camera distance is invalid");
					return;
					}

				// Axis X Rotation
				if(!int.TryParse(CameraViewRotationTextBox.Text.Trim(), out ViewXRotation) || ViewXRotation > 160 || ViewXRotation < -160)
					{
					MessageBox.Show("View X rotation invalid");
					return;
					}
				}

			// error
			if(!ErrorNoneRadioButton.Checked)
				{
				if(ErrorWhiteRadioButton.Checked)
					ErrorControl = ErrorSpotControl.White;
				else if(ErrorBlackRadioButton.Checked)
					ErrorControl = ErrorSpotControl.Black;
				else
					ErrorControl = ErrorSpotControl.Alternate;

				int MaxSpotDiameter = QRCodeImageDimension / 8;
				if(!int.TryParse(ErrorDiameterTextBox.Text.Trim(), out ErrorDiameter) ||
					ErrorDiameter <= 0 || ErrorDiameter > MaxSpotDiameter)
					{
					MessageBox.Show("Error diameter is invalid");
					return;
					}

				if(!int.TryParse(ErrorNumberTextBox.Text.Trim(), out ErrorSpots) ||
					ErrorSpots <= 0 || ErrorSpots > 100)
					{
					MessageBox.Show("Number of error spots is invalid");
					return;
					}
				}

			// output bitmap
			Bitmap OutputBitmap;

			// display QR Code image by itself
			if(NoneRadioButton.Checked)
				{
				OutputBitmap = QRCodeBitmapImage;
				}

			else
				{
				if(ImageRadioButton.Checked)
					{
					OutputBitmap = new Bitmap(this.BGImageBitmap);
					}
				else
					{
					// create area brush
					Brush AreaBrush = (int) HatchStyleComboBox.SelectedItem < 0 ? (Brush) new SolidBrush(BrushColorButton.BackColor) :
						(Brush) new HatchBrush((HatchStyle) ((int) HatchStyleComboBox.SelectedItem), Color.Black, BrushColorButton.BackColor);

					// create picture object and and paint it with the brush
					OutputBitmap = new Bitmap(OutputImageWidth, OutputImageHeight);
					Graphics Graphics = Graphics.FromImage(OutputBitmap);
					Graphics.FillRectangle(AreaBrush, 0, 0, OutputImageWidth, OutputImageHeight);
					}

				if(ViewXRotation == 0)
					{
					OutputBitmap = CreateQRCodeImage(OutputBitmap, QRCodePosX, QRCodePosY, CameraRotation);
					}
				else
					{
					OutputBitmap = CreateQRCodeImage(OutputBitmap, QRCodePosX, QRCodePosY, CameraRotation, CameraDistance, ViewXRotation);
					}
				}

			// Error spots
			if(ErrorControl != ErrorSpotControl.None)
				{
				AddErrorSpots(OutputBitmap, ErrorControl, ErrorDiameter, ErrorSpots);
				}

			// get file name
			string FileName = QRCodeEncoderDemo.SaveFileName(FileFormat);
			if (FileName == null) return;

			// save image
			FileStream FS = new(FileName, FileMode.Create);
			OutputBitmap.Save(FS, FileFormat);
			FS.Close();

			// start image editor
			Process Proc = new()
				{
				StartInfo = new ProcessStartInfo(FileName) { UseShellExecute = true }
				};
			Proc.Start();
			return;
			}

		// create barcode image with background and rotation
		private Bitmap CreateQRCodeImage
				(
				Bitmap OutputImage,
				int ImageCenterPosX,
				int ImageCenterPosY,
				double Rotation
				)
			{
			// transformation matrix
			Matrix Matrix = new();
			Matrix.Translate(ImageCenterPosX, ImageCenterPosY);
			if(Rotation != 0) Matrix.Rotate((float) Rotation);

			// create graphics object
			Graphics Graphics = Graphics.FromImage(OutputImage);

			// attach transformation matrix
			Graphics.Transform = Matrix;

			// image origin
			int ImageDimension = QRCodeImageDimension;
			int XOffset = -ImageDimension / 2;
			int YOffset = XOffset;

			// set barcode area within background image to white
			Graphics.FillRectangle(Brushes.White, XOffset, YOffset, ImageDimension, ImageDimension);

			// adjust offset
			XOffset += QuietZone;
			int SaveXOffet = XOffset;
			YOffset += QuietZone;

			// barcode image width and height
			int MatrixDimension = QRCodeDimension;

			// convert result matrix to output matrix
			for(int Row = 0; Row < MatrixDimension; Row++)
				{
				for(int Col = 0; Col < MatrixDimension; Col++)
					{
					// bar is black
					if(QRCodeMatrix[Row, Col])
						{
						Graphics.FillRectangle(Brushes.Black, (float) XOffset, (float) YOffset, ModuleSize, ModuleSize);
						}

					// update horizontal offset
					XOffset += ModuleSize;
					}

				// update vertical offset
				XOffset = SaveXOffet;
				YOffset += ModuleSize;
				}
			return OutputImage;
			}

		// create perspective barcode image with background and rotation
		private Bitmap CreateQRCodeImage
				(
				Bitmap OutputImage,
				int ImageCenterPosX,
				int ImageCenterPosY,
				double Rotation,
				double CameraDistance,
				double ViewXRotation
				)
			{
			// create graphics object
			Graphics Graphics = Graphics.FromImage(OutputImage);

			// create perspective object
			Perspective Perspective = new(ImageCenterPosX, ImageCenterPosY, Rotation, CameraDistance, ViewXRotation);

			// image origin
			int ImageDimension = QRCodeImageDimension;
			int XOffset = -ImageDimension / 2;
			int YOffset = -XOffset;

			// over all image polygon
			PointF[] Polygon = new PointF[4];
			Perspective.GetPolygon(XOffset, YOffset, ImageDimension, ImageDimension, Polygon);

			double MaxX = Math.Max(Math.Max(Polygon[0].X, Polygon[1].X), Math.Max(Polygon[2].X, Polygon[3].X));
			double MinX = Math.Min(Math.Min(Polygon[0].X, Polygon[1].X), Math.Min(Polygon[2].X, Polygon[3].X));
			double MaxY = Math.Max(Math.Max(Polygon[0].Y, Polygon[1].Y), Math.Max(Polygon[2].Y, Polygon[3].Y));
			double MinY = Math.Min(Math.Min(Polygon[0].Y, Polygon[1].Y), Math.Min(Polygon[2].Y, Polygon[3].Y));

			double AvgX = 0.5 * (MaxX + MinX);
			double AvgY = 0.5 * (MaxY + MinY);

			double DeltaX = -(AvgX - 0.5 * OutputImage.Width);
			double DeltaY = -(AvgY - 0.5 * OutputImage.Height);

			Polygon[0].X += (float)DeltaX;
			Polygon[0].Y += (float)DeltaY;
			Polygon[1].X += (float)DeltaX;
			Polygon[1].Y += (float)DeltaY;
			Polygon[2].X += (float)DeltaX;
			Polygon[2].Y += (float)DeltaY;
			Polygon[3].X += (float)DeltaX;
			Polygon[3].Y += (float)DeltaY;

			// clear the area for barcode
			Graphics.FillPolygon(Brushes.White, Polygon);

			// adjust offset
			XOffset += QuietZone;
			int SaveXOffet = XOffset;
			YOffset += QuietZone;

			// convert result matrix to output matrix
			for(int Row = 0; Row < QRCodeDimension; Row++)
				{
				for(int Col = 0; Col < QRCodeDimension; Col++)
					{
					// bar is black
					if(QRCodeMatrix[Row, Col])
						{
						Perspective.GetPolygon(XOffset, YOffset, ModuleSize, ModuleSize, Polygon);
						Polygon[0].X += (float) DeltaX;
						Polygon[0].Y += (float) DeltaY;
						Polygon[1].X += (float) DeltaX;
						Polygon[1].Y += (float) DeltaY;
						Polygon[2].X += (float) DeltaX;
						Polygon[2].Y += (float) DeltaY;
						Polygon[3].X += (float) DeltaX;
						Polygon[3].Y += (float) DeltaY;
						Graphics.FillPolygon(Brushes.Black, Polygon);
						}

					// update horizontal offset
					XOffset += ModuleSize;
					}

				// update vertical offset
				XOffset = SaveXOffet;
				YOffset += ModuleSize;
				}
			return OutputImage;
			}

		// Add error spots for testing
		private static void AddErrorSpots
				(
				Bitmap ImageBitmap,
				ErrorSpotControl ErrorControl,
				double ErrorDiameter,
				double ErrorSpotsCount
				)
			{
			// random number generator
			Random RandNum = new();

			// create graphics object
			Graphics Graphics = Graphics.FromImage(ImageBitmap);

			double XRange = ImageBitmap.Width - ErrorDiameter - 4;
			double YRange = ImageBitmap.Height - ErrorDiameter - 4;
			Brush SpotBrush = ErrorControl == ErrorSpotControl.Black ? Brushes.Black : Brushes.White;

			for(int Index = 0; Index < ErrorSpotsCount; Index++)
				{
				double XPos = RandNum.NextDouble() * XRange;
				double YPos = RandNum.NextDouble() * YRange;
				if(ErrorControl == ErrorSpotControl.Alternate)
					SpotBrush = (Index & 1) == 0 ? Brushes.White : Brushes.Black;
				Graphics.FillEllipse(SpotBrush, (float) XPos, (float) YPos, (float) ErrorDiameter, (float) ErrorDiameter);
				}
			return;
			}

		private void OnCancelClick(object sender, EventArgs e)
			{
			DialogResult = DialogResult.Cancel;
			return;
			}
		}
	}
