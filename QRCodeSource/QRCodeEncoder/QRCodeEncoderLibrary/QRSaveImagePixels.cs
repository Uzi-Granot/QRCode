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
//	For full version history please look at QREncoder.cs
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
/////////////////////////////////////////////////////////////////////

namespace QRCodeEncoderLibrary
	{
	/// <summary>
	/// Convert QR code matrix to boolean image class
	/// </summary>
	public class QRSaveImagePixels
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
					throw new ArgumentException("QRSaveImagePixels: Module size error. Default is 2.");
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
					throw new ArgumentException("QRSaveImagePixels: Quiet zone must be 0 to 400. Default is 8.");
				_QuietZone = value;
				return;
				}
			}
		private int _QuietZone = 8;

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
		/// Convert QR code matrix to boolean image constructor
		/// </summary>
		public QRSaveImagePixels
				(
				bool[,] QRCodeMatrix
				)
			{
			// test argument
			if(QRCodeMatrix == null)
				throw new ArgumentException("QRSaveImagePixels: QRCodeMatrix is null");

			// test matrix dimensions
			int Width = QRCodeMatrix.GetLength(0);
			int Height = QRCodeMatrix.GetLength(1);
			if(Width != Height)
				throw new ArgumentException("QRSaveImagePixels: QRCodeMatrix width is not equals height");
			if(Width < 21 || Width > 177 || ((Width - 21) % 4) != 0)
				throw new ArgumentException("QRSaveImagePixels: Invalid QRCodeMatrix dimension");

			// save argument
			this.QRCodeMatrix = QRCodeMatrix;
			QRCodeDimension = Width;
			return;
			}

		/// <summary>
		/// convert black and white matrix to black and white image
		/// </summary>
		/// <returns>Black and white image in pixels</returns>
		public bool[,] ConvertQRCodeMatrixToPixels()
			{
			int QRCodeImageDimension = _ModuleSize * QRCodeDimension + 2 * _QuietZone;

			// output matrix size in pixels all matrix elements are white (false)
			bool[,] BWImage = new bool[QRCodeImageDimension, QRCodeImageDimension];

			// quiet zone offset
			int XOffset = _QuietZone;
			int YOffset = _QuietZone;

			// convert result matrix to output matrix
			for(int Row = 0; Row < QRCodeDimension; Row++)
				{
				for(int Col = 0; Col < QRCodeDimension; Col++)
					{
					// bar is black
					if(QRCodeMatrix[Row, Col])
						{
						for(int Y = 0; Y < ModuleSize; Y++)
							{
							for(int X = 0; X < ModuleSize; X++)
								BWImage[YOffset + Y, XOffset + X] = true;
							}
						}
					XOffset += ModuleSize;
					}
				XOffset = _QuietZone;
				YOffset += ModuleSize;
				}
			return BWImage;
			}
		}
	}
