/////////////////////////////////////////////////////////////////////
//
//	QR Code Library
//
//	QR Code result class.
//
//	Author: Uzi Granot
//
//	Current Version: 3.1.0
//	Date: March 7, 2022
//
//	Original Version: 1.0
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
//	The video decoder is using some of the source modules of
//	Camera_Net project published at CodeProject.com:
//	https://www.codeproject.com/Articles/671407/Camera_Net-Library
//	and at GitHub: https://github.com/free5lot/Camera_Net.
//	This project is based on DirectShowLib.
//	http://sourceforge.net/projects/directshownet/
//	This project includes a modified subset of the source modules.
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
//	For version history please refer to QRDecoder.cs
/////////////////////////////////////////////////////////////////////

namespace QRCodeDecoderLibrary
	{
	public class QRCodeResult
		{
		/// <summary>
		/// QR Code Data array
		/// </summary>
		public byte[] DataArray;

		/// <summary>
		/// ECI Assignment Value
		/// </summary>
		public int ECIAssignValue;

		/// <summary>
		/// QR Code matrix version
		/// </summary>
		public int QRCodeVersion;

		/// <summary>
		/// QR Code matrix dimension in bits
		/// </summary>
		public int QRCodeDimension;

		/// <summary>
		/// QR Code error correction code (L, M, Q, H)
		/// </summary>
		public ErrorCorrection ErrorCorrection;

		public QRCodeResult
				(
				byte[] DataArray
				)
			{
			this.DataArray = DataArray;
			return;
			}
		}
	}
