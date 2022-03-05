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

using System.Drawing.Imaging;

namespace QRCodeEncoder
	{
	public enum SaveType
		{
		Png,
		Bitmap,
		Special
		}

	public partial class SaveImage : Form
		{
		public int ModuleSize;
		public int QuietZone;
		public ImageFormat FileFormat;
		public SaveType SaveIndex;

		/// <summary>
		/// Image file format
		/// </summary>
		public static readonly ImageFormat[] FileFormatArray = { ImageFormat.Png, ImageFormat.Jpeg, ImageFormat.Bmp, ImageFormat.Gif };

		public SaveImage()
			{
			InitializeComponent();
			return;
			}

		private void OnLoad(object sender, EventArgs e)
			{
			// initialize module size and quiet zone text boxes
			ModuleSizeTextBox.Text = ModuleSize.ToString();
			QuietZoneTextBox.Text = QuietZone.ToString();

			// load image file type combo box
			int SelectedIndex = 0;
			for(int Index = 0; Index < FileFormatArray.Length; Index++)
				{
				ImageFormatComboBox.Items.Add(FileFormatArray[Index]);
				if(FileFormatArray[Index] == FileFormat) SelectedIndex = Index;
				}
			ImageFormatComboBox.SelectedIndex = SelectedIndex;
			return;
			}


		private void OnSavePng(object sender, EventArgs e)
			{
			if (!TestFields()) return;
			SaveIndex = SaveType.Png;
			DialogResult = DialogResult.OK;
			return;
			}

		private void OnSaveBitmap(object sender, EventArgs e)
			{
			if (!TestFields()) return;
			SaveIndex = SaveType.Bitmap;
			DialogResult = DialogResult.OK;
			return;
			}

		private void OnSaveSpecial(object sender, EventArgs e)
			{
			if (!TestFields()) return;
			SaveIndex = SaveType.Special;
			DialogResult = DialogResult.OK;
			return;
			}

		private void OnCancel(object sender, EventArgs e)
			{
			DialogResult = DialogResult.Cancel;
			return;
			}

		private bool TestFields()
			{ 
			// get module size
			string ModuleStr = ModuleSizeTextBox.Text.Trim();
				if(!int.TryParse(ModuleStr, out ModuleSize) || ModuleSize< 1 || ModuleSize> 100)
					{
					MessageBox.Show("Module size error.");
					return false;
					}

			// get quiet zone
			string QuietStr = QuietZoneTextBox.Text.Trim();
				if(!int.TryParse(QuietStr, out QuietZone) || QuietZone< 1 || QuietZone> 100)
					{
					MessageBox.Show("Quiet zone error.");
					return false;
					}

			FileFormat = (ImageFormat) ImageFormatComboBox.SelectedItem;
			return true;
			}

		private void OnImageFileFormat
				(
				object sender,
				ListControlConvertEventArgs e
				)
			{
			ImageFormat Format = (ImageFormat)e.ListItem;
			e.Value = string.Format("{0} (*.{1})", Format.ToString(), Format.ToString().ToLower());
			return;
			}
		}
	}
