/////////////////////////////////////////////////////////////////////
//
//	QR Code Encoder Library
//
//	QR Save image.
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
//	For full version history please look at QREncoder.cs
/////////////////////////////////////////////////////////////////////

using System.Drawing.Imaging;

namespace QRCodeEncoderLibrary
	{
	/// <summary>
	/// Save QR Code image as Bitmap class
	/// </summary>
	public class QRSaveBitmapImage
		{
		/// <summary>
		/// Module size (Default: 2)
		/// </summary>
		public int ModuleSize
			{
			get
				{
				return _ModuleSize;
				}
			set
				{
				if(value < 1 || value > 100)
					throw new ArgumentException("Module size error. Default is 2.");
				_ModuleSize = value;
				return;
				}
			}
		private int _ModuleSize = 2;

		/// <summary>
		/// Quiet zone around the barcode in pixels (Default: 8)
		/// It should be 4 times the module size.
		/// However the calling application can set it 0 to 400
		/// </summary>
		public int QuietZone
			{
			get
				{
				return _QuietZone;
				}
			set
				{
				if(value < 0 || value > 400)
					throw new ArgumentException("Quiet zone must be 0 to 400. Default is 8.");
				_QuietZone = value;
				return;
				}
			}
		private int _QuietZone = 8;

		/// <summary>
		/// White brush (default white)
		/// </summary>
		public Brush WhiteBrush
			{
			get
				{
				return _WhiteBrush;
				}
			set
				{
				_WhiteBrush = value;
				}
			}
		private Brush _WhiteBrush = Brushes.White;
		
		/// <summary>
		/// Black brush (default black)
		/// </summary>
		public Brush BlackBrush
			{
			get
				{
				return _BlackBrush;
				}
			set
				{
				_BlackBrush = value;
				}
			}
		private Brush _BlackBrush = Brushes.Black;

		/// <summary>
		/// QR code matrix (no quiet zone)
		/// Black module = true, White module = false
		/// </summary>
		private readonly bool[,] QRCodeMatrix;

		/// <summary>
		/// QRCode dimension
		/// </summary>
		private readonly int QRCodeDimension;

		/// <summary>
		/// Gets QR Code image dimension
		/// </summary>
		private int QRCodeImageDimension;

		/// <summary>
		/// Save QR Code Bitmap image constructor
		/// </summary>
		/// <param name="QRCodeMatrix">QR code matrix</param>
		public QRSaveBitmapImage
				(
				bool[,] QRCodeMatrix
				)
			{
			// test argument
			if(QRCodeMatrix == null)
				throw new ArgumentException("QRSaveBitmapImage: QRCodeMatrix is null");

			// test matrix dimensions
			int Width = QRCodeMatrix.GetLength(0);
			int Height = QRCodeMatrix.GetLength(1);
			if(Width != Height)
				throw new ArgumentException("QRSaveBitmapImage: QRCodeMatrix width and height are not equal");
			if(Width < 21 || Width > 177 || ((Width - 21) % 4) != 0)
				throw new ArgumentException("QRSaveBitmapImage: Invalid QRCodeMatrix dimension");

			// save argument
			this.QRCodeMatrix = QRCodeMatrix;
			QRCodeDimension = Width;
			return;
			}

		/// <summary>
		/// Create QR Code Bitmap image from boolean black and white matrix
		/// </summary>
		/// <returns>QRCode image</returns>
		public Bitmap CreateQRCodeBitmap()
			{
			// image dimension
			QRCodeImageDimension = ModuleSize * QRCodeDimension + 2 * QuietZone;

			// create picture object and make it white
			Bitmap Image = new(QRCodeImageDimension, QRCodeImageDimension);
			Graphics Graphics = Graphics.FromImage(Image);
			Graphics.FillRectangle(_WhiteBrush, 0, 0, QRCodeImageDimension, QRCodeImageDimension);

			// x and y image pointers
			int XOffset = QuietZone;
			int YOffset = QuietZone;

			// convert result matrix to output matrix
			for(int Row = 0; Row < QRCodeDimension; Row++)
				{
				for(int Col = 0; Col < QRCodeDimension; Col++)
					{
					// bar is black
					if(QRCodeMatrix[Row, Col]) Graphics.FillRectangle(_BlackBrush, XOffset, YOffset, ModuleSize, ModuleSize);
					XOffset += ModuleSize;
					}
				XOffset = QuietZone;
				YOffset += ModuleSize;
				}

			// return image
			return Image;
			}

		/// <summary>
		/// Save QRCode image to image file
		/// </summary>
		/// <param name="FileName">Image file name</param>
		public void SaveQRCodeToImageFile
				(
				string FileName,
				ImageFormat Format
				)
			{
			// exceptions
			if (FileName == null)
				throw new ArgumentException("SaveQRCodeToPngFile: FileName is null");

			// create Bitmap
			Bitmap ImageBitmap = CreateQRCodeBitmap();

			// save bitmap
			ImageBitmap.Save(FileName, Format);
			return;
			}

		/// <summary>
		/// Save QRCode image to image file
		/// </summary>
		/// <param name="FileName">Image file name</param>
		public void SaveQRCodeToImageFile
				(
				Stream OutputStream,
				ImageFormat Format
				)
			{
			// exceptions
			if(OutputStream == null)
				throw new ArgumentException("SaveQRCodeToImageFile: Output stream is null");

			// create Bitmap
			Bitmap ImageBitmap = CreateQRCodeBitmap();

			// write to stream 
			ImageBitmap.Save(OutputStream, Format);

			// flush all buffers
			OutputStream.Flush();
			return;
			}
		}
	}
