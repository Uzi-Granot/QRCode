/////////////////////////////////////////////////////////////////////
//
//	QR Code Encoder Library
//
//	Perspective transformation class
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

namespace QRCodeEncoder
	{
	/// <summary>
	///	Create a QRCode barcode image for testing. The image is transformed
	///	using perspective algorithm.
	/// </summary>
	internal class Perspective
	{
	private readonly double CenterX;
	private readonly double CenterY;
	private readonly double CosRot;
	private readonly double SinRot;
	private readonly double CamDist;
	private readonly double CosX;
	private readonly double SinX;
	private readonly double CamVectY;
	private readonly double CamVectZ;
	private readonly double CamPosY;
	private readonly double CamPosZ;

	internal Perspective
			(
			double CenterX,
			double CenterY,
			double ImageRot,
			double CamDist,
			double RotX
			)
		{
		// center position
		this.CenterX = CenterX;
		this.CenterY = CenterY;

		// image rotation
		double RotRad = Math.PI * ImageRot / 180.0;
		CosRot = Math.Cos(RotRad);
		SinRot = Math.Sin(RotRad);
 
		// camera distance from Pdf417 barcode
		this.CamDist = CamDist;

		// x and z axis rotation constants
		double RotXRad = Math.PI * RotX / 180.0;
		CosX = Math.Cos(RotXRad);
		SinX = Math.Sin(RotXRad);

		// camera vector relative to barcode image
		CamVectY = SinX;
		CamVectZ = CosX;

		// camera position relative to barcode image
		CamPosY =  CamDist * CamVectY;
		CamPosZ =  CamDist * CamVectZ;

		// exit
		return;
		}

	// screen equation
	// CamVectX * X + CamVectY * Y + CamVectZ * Z = 0

	// line equations between barcode point to camera position
	// X = PosX + (CamPosX - PosX) * T
	// Y = PosY + (CamPosY - PosZ) * T
	// Z = PosZ + (CamPosZ - PosZ) * T

	// line intersection with screen
	// CamVectX * (PosX + (CamPosX - PosX) * T) +
	//		CamVectY * (PosY + (CamPosY - PosY) * T) +
	//			CamVectZ * (PosZ + (CamPosZ - PosZ) * T) = 0
	//
	// (CamVectX * (CamPosX - PosX) + CamVectY * (CamPosY - PosY) + CamVectZ * (CamPosZ - PosZ)) * T =
	//		- CamVectX * PosX - CamVectY * PosY - CamVectZ * PosZ;
	//
	//	T = -(CamVectX * PosX + CamVectY * PosY + CamVectZ * PosZ) /
	//		(CamVectX * (CamPosX - PosX) + CamVectY * (CamPosY - PosY) + CamVectZ * (CamPosY - PosZ));
	//	Q = CamVectX * PosX + CamVectY * PosY + CamVectZ * PosZ
	//	T = Q / (Q - CamDist)

	internal PointF ScreenPosition
			(
			double BarcodePosX,
			double BarcodePosY
			)
		{
		// rotation
		double PosX = CosRot * BarcodePosX - SinRot * BarcodePosY;
		double PosY = SinRot * BarcodePosX + CosRot * BarcodePosY;

		// temp values for intersection calclulation
		double CamToBarcode = CamVectY * PosY;
		double T = CamToBarcode / (CamToBarcode - CamDist);

		// screen position relative to screen center
		double ScrnPosX = CenterX + PosX * (1 - T);
		double TempPosY = PosY + (CamPosY - PosY) * T;
		double TempPosZ = CamPosZ * T; // - ScrnCenterZ;

		// rotate around x axis
		double ScrnPosY = CenterY + TempPosY * CosX - TempPosZ * SinX;

		// program test
		#if DEBUG
		double ScrnPosZ = TempPosY * SinX + TempPosZ * CosX;
		if(Math.Abs(ScrnPosZ) > 0.0001) throw new ApplicationException("Screen Z position must be zero");
		#endif

		return new PointF((float) ScrnPosX, (float) ScrnPosY);
		}

	internal void GetPolygon
			(
			double PosX,
			double PosY,
			double Width,
			double Height,
			PointF[] Polygon
			)
		{
		Polygon[0] = ScreenPosition(PosX, PosY);
		Polygon[1] = ScreenPosition(PosX + Width, PosY);
		Polygon[2] = ScreenPosition(PosX + Width, PosY + Height);
		Polygon[3] = ScreenPosition(PosX, PosY + Height);
		return;
		}
	}
}
