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

using System.IO.Compression;

namespace QRCodeEncoderLibrary
	{
	/// <summary>
	/// Save QR Code as PNG image class
	/// </summary>
	public class QRSavePngImage
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

		// PNG file signature and end marker
		private static readonly byte[] PngFileSignature = new byte[] {137, (byte) 'P', (byte) 'N', (byte) 'G', (byte) '\r', (byte) '\n', 26, (byte) '\n'};
		private static readonly byte[] PngIendChunk = new byte[] {0, 0, 0, 0, (byte) 'I', (byte) 'E', (byte) 'N', (byte) 'D', 0xae, 0x42, 0x60, 0x82};

		/// <summary>
		/// Save QR Code as PNG image constructor
		/// </summary>
		public QRSavePngImage
				(
				bool[,] QRCodeMatrix
				)
			{
			// test argument
			if(QRCodeMatrix == null)
				throw new ArgumentException("QRSavePngImage: QRCodeMatrix is null");

			// test matrix dimensions
			int Width = QRCodeMatrix.GetLength(0);
			int Height = QRCodeMatrix.GetLength(1);
			if(Width != Height)
				throw new ArgumentException("QRSavePngImage: QRCodeMatrix width is not equals height");
			if(Width < 21 || Width > 177 || ((Width - 21) % 4) != 0)
				throw new ArgumentException("QRSavePngImage: Invalid QRCodeMatrix dimension");

			// save argument
			this.QRCodeMatrix = QRCodeMatrix;
			QRCodeDimension = Width;
			return;
			}

		/// <summary>
		/// Save QRCode image to PNG file
		/// </summary>
		/// <param name="FileName">PNG file name</param>
		public void SaveQRCodeToPngFile
				(
				string FileName
				)
			{
			// exceptions
			if(FileName == null)
				throw new ArgumentException("SaveQRCodeToPngFile: FileName is null");

			if(!FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException("SaveQRCodeToPngFile: FileName extension must be .png");

			// file name to stream
			using Stream OutputStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);

			// save file
			SaveQRCodeToPngFile(OutputStream);
			return;
			}

		/// <summary>
		/// Save QRCode image to PNG stream
		/// </summary>
		/// <param name="OutputStream">PNG output stream</param>
		public void SaveQRCodeToPngFile
				(
				Stream OutputStream
				)
			{
			// convert code to PNG file
			byte[] PngFile = QRCodeToPngFormat();

			// stream to binary writer
			BinaryWriter Writer = new(OutputStream);

			// write png to file 
			Writer.Write(PngFile, 0, PngFile.Length);

			// flush all buffers
			Writer.Flush();
			return;
			}

		/// <summary>
		/// QRCode image to PNG file format
		/// </summary>
		public byte[] QRCodeToPngFormat()
			{
			// image dimension
			QRCodeImageDimension = 2 * _QuietZone + QRCodeDimension * _ModuleSize;

			// header
			byte[] Header = BuildPngHeader();

			// barcode data
			byte[] InputBuf = QRCodeMatrixToPng();

			// compress barcode data
			byte[] OutputBuf = PngImageData(InputBuf);

			// output buffer
			byte[] PngFile = new byte[PngFileSignature.Length + Header.Length + OutputBuf.Length + PngIendChunk.Length];
			int Ptr = 0;

			// write signature
			Array.Copy(PngFileSignature, 0, PngFile, Ptr, PngFileSignature.Length);
			Ptr += PngFileSignature.Length;

			// write header
			Array.Copy(Header, 0, PngFile, Ptr, Header.Length);
			Ptr += Header.Length;

			// write image data
			Array.Copy(OutputBuf, 0, PngFile, Ptr, OutputBuf.Length);
			Ptr+= OutputBuf.Length;

			// write end of file
			Array.Copy(PngIendChunk, 0, PngFile, Ptr, PngIendChunk.Length);

			// exit with png file in byte array
			return PngFile;
			}

		/// <summary>
		/// Build PNG file header
		/// </summary>
		/// <returns>Header as byte array</returns>
		private byte[] BuildPngHeader()
			{ 
			// header
			byte[] Header = new byte[25];
					
			// header length
			Header[0] = 0;
			Header[1] = 0;
			Header[2] = 0;
			Header[3] = 13;

			// header label
			Header[4] = (byte) 'I';
			Header[5] = (byte) 'H';
			Header[6] = (byte) 'D';
			Header[7] = (byte) 'R';

			// image width
			Header[8] = (byte) (QRCodeImageDimension >> 24);
			Header[9] = (byte) (QRCodeImageDimension >> 16);
			Header[10] = (byte) (QRCodeImageDimension >> 8);
			Header[11] = (byte) QRCodeImageDimension;

			// image height
			Header[12] = Header[8];
			Header[13] = Header[9];
			Header[14] = Header[10];
			Header[15] = Header[11];

			// bit depth (1)
			Header[16] = 1;

			// color type (grey)
			Header[17] = 0;

			// Compression (deflate)
			Header[18] = 0;

			// filtering (up)
			Header[19] = 0; // 2;

			// interlace (none)
			Header[20] = 0;

			// crc
			uint Crc = CRC32Checksum(Header, 4, 17);
			Header[21] = (byte) (Crc >> 24);
			Header[22] = (byte) (Crc >> 16);
			Header[23] = (byte) (Crc >> 8);
			Header[24] = (byte) Crc;

			// return header
			return Header;
			}

		/// <summary>
		/// Convert barcode matrix to PNG image format
		/// </summary>
		/// <returns>PDG data array</returns>
		private byte[] QRCodeMatrixToPng()
			{
			// image width and height
			int ImageDimension = this.QRCodeImageDimension;

			// width in bytes including filter leading byte
			int PngWidth = (ImageDimension + 7) / 8 + 1;

			// PNG image array
			// array is all zeros in other words it is black image
			int PngLength = PngWidth * ImageDimension;
			byte[] PngImage = new byte[PngLength];

			// first row is a quiet zone and it is all white (filter is 0 none)
			int PngPtr;
			for(PngPtr = 1; PngPtr < PngWidth; PngPtr++) PngImage[PngPtr] = 255;

			// additional quiet zone rows are the same as first line (filter is 2 up)
			int PngEnd = QuietZone * PngWidth;
			for(; PngPtr < PngEnd; PngPtr += PngWidth) PngImage[PngPtr] = 2;

			// convert result matrix to output matrix
			for(int MatrixRow = 0; MatrixRow < QRCodeDimension; MatrixRow++)
				{
				// make next row all white (filter is 0 none)
				PngEnd = PngPtr + PngWidth;
				for(int PngCol = PngPtr + 1; PngCol < PngEnd; PngCol++) PngImage[PngCol] = 255;

				// add black to next row
				for(int MatrixCol = 0; MatrixCol < QRCodeDimension; MatrixCol++)
					{
					// bar is white
					if(!QRCodeMatrix[MatrixRow, MatrixCol]) continue;

					int PixelCol = ModuleSize * MatrixCol + QuietZone;
					int PixelEnd = PixelCol + ModuleSize;
					for(; PixelCol < PixelEnd; PixelCol++)
						{ 
						PngImage[PngPtr + (1 + PixelCol / 8)] &= (byte) ~(1 << (7 - (PixelCol & 7)));
						}
					}

				// additional rows are the same as the one above (filter is 2 up)
				PngEnd = PngPtr + ModuleSize * PngWidth;
				for(PngPtr += PngWidth; PngPtr < PngEnd; PngPtr += PngWidth) PngImage[PngPtr] = 2;
				}

			// bottom quiet zone and it is all white (filter is 0 none)
			PngEnd = PngPtr + PngWidth;
			for(PngPtr++; PngPtr < PngEnd; PngPtr++) PngImage[PngPtr] = 255;

			// additional quiet zone rows are the same as first line (filter is 2 up)
			for(; PngPtr < PngLength; PngPtr += PngWidth) PngImage[PngPtr] = 2;

			// exit with Png image in byte array
			return PngImage;
			}

		/// <summary>
		/// Compress PNG image data
		/// </summary>
		/// <param name="InputBuf">PNG data</param>
		/// <returns>Compressed PNG data as byte array</returns>
		private static byte[] PngImageData
				(
				byte[] InputBuf
				)
			{
			// output buffer is:
			// Png IDAT length 4 bytes
			// Png chunk type IDAT 4 bytes
			// Png chunk data made of:
			//		header 2 bytes
			//		compressed data DataLen bytes
			//		adler32 input buffer checksum 4 bytes
			// Png CRC 4 bytes
			// Total output buffer length is 18 + DataLen

			// compress image
			byte[] OutputBuf = ZLibCompress(InputBuf);

			// png chunk data length
			int PngDataLen = OutputBuf.Length - 12;
			OutputBuf[0] = (byte) (PngDataLen >> 24);
			OutputBuf[1] = (byte) (PngDataLen >> 16);
			OutputBuf[2] = (byte) (PngDataLen >> 8);
			OutputBuf[3] = (byte) PngDataLen;

			// add IDAT
			OutputBuf[4] = (byte) 'I';
			OutputBuf[5] = (byte) 'D';
			OutputBuf[6] = (byte) 'A';
			OutputBuf[7] = (byte) 'T';

			// adler32 checksum
			uint ReadAdler32 = Adler32Checksum(InputBuf, 0, InputBuf.Length);

			// ZLib checksum is Adler32 write it big endian order, high byte first
			int AdlerPtr = OutputBuf.Length - 8;
			OutputBuf[AdlerPtr++] = (byte) (ReadAdler32 >> 24);
			OutputBuf[AdlerPtr++] = (byte) (ReadAdler32 >> 16);
			OutputBuf[AdlerPtr++] = (byte) (ReadAdler32 >> 8);
			OutputBuf[AdlerPtr] = (byte) ReadAdler32;

			// crc
			uint Crc = CRC32Checksum(OutputBuf, 4, OutputBuf.Length - 8);
			int CrcPtr = OutputBuf.Length - 4;
			OutputBuf[CrcPtr++] = (byte) (Crc >> 24);
			OutputBuf[CrcPtr++] = (byte) (Crc >> 16);
			OutputBuf[CrcPtr++] = (byte) (Crc >> 8);
			OutputBuf[CrcPtr++] = (byte) Crc;

			// successful exit
			return OutputBuf;
			}

		// CRC32 Table
		private static readonly uint[] CRC32Table =
			{
			0x00000000, 0x77073096, 0xee0e612c, 0x990951ba, 0x076dc419,
			0x706af48f, 0xe963a535, 0x9e6495a3, 0x0edb8832, 0x79dcb8a4,
			0xe0d5e91e, 0x97d2d988, 0x09b64c2b, 0x7eb17cbd, 0xe7b82d07,
			0x90bf1d91, 0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de,
			0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7, 0x136c9856,
			0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9,
			0xfa0f3d63, 0x8d080df5, 0x3b6e20c8, 0x4c69105e, 0xd56041e4,
			0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,
			0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3,
			0x45df5c75, 0xdcd60dcf, 0xabd13d59, 0x26d930ac, 0x51de003a,
			0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599,
			0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
			0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d, 0x76dc4190,
			0x01db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x06b6b51f,
			0x9fbfe4a5, 0xe8b8d433, 0x7807c9a2, 0x0f00f934, 0x9609a88e,
			0xe10e9818, 0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
			0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed,
			0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950,
			0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3,
			0xfbd44c65, 0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2,
			0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a,
			0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5,
			0xaa0a4c5f, 0xdd0d7cc9, 0x5005713c, 0x270241aa, 0xbe0b1010,
			0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
			0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17,
			0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad, 0xedb88320, 0x9abfb3b6,
			0x03b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x04db2615,
			0x73dc1683, 0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8,
			0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1, 0xf00f9344,
			0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb,
			0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a,
			0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
			0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1,
			0xa6bc5767, 0x3fb506dd, 0x48b2364b, 0xd80d2bda, 0xaf0a1b4c,
			0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef,
			0x4669be79, 0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236,
			0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe,
			0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31,
			0x2cd99e8b, 0x5bdeae1d, 0x9b64c2b0, 0xec63f226, 0x756aa39c,
			0x026d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713,
			0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38, 0x92d28e9b,
			0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21, 0x86d3d2d4, 0xf1d4e242,
			0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1,
			0x18b74777, 0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c,
			0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45, 0xa00ae278,
			0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7,
			0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc, 0x40df0b66,
			0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
			0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605,
			0xcdd70693, 0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8,
			0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b,
			0x2d02ef8d
			};

		/// <summary>
		/// Accumulate CRC 32
		/// </summary>
		/// <param name="Buffer">Byte array buffer</param>
		/// <param name="Pos">Buffer position</param>
		/// <param name="Len">Buffer length</param>
		/// <returns>CRC32</returns>
		internal static uint CRC32Checksum
				(
				byte[] Buffer,
				int Pos,
				int Len
				)
			{
			uint CRC = 0xffffffff;
			for(; Len > 0; Len--)
				CRC = CRC32Table[(CRC ^ Buffer[Pos++]) & 0xff] ^ (CRC >> 8);
			return (~CRC);
			}

		/// <summary>
		/// Accumulate Adler Checksum
		/// </summary>
		/// <param name="Buffer">Byte array buffer</param>
		/// <param name="Pos">Buffer position</param>
		/// <param name="Len">Buffer length</param>
		/// <returns>Adler32 checksum</returns>
		internal static uint Adler32Checksum
				(
				byte[] Buffer,
				int Pos,
				int Len
				)
			{
			const uint Adler32Base = 65521;

			// split current Adler chksum into two 
			uint AdlerLow = 1; // AdlerValue & 0xFFFF;
			uint AdlerHigh = 0; // AdlerValue >> 16;

			// loop for segments of 5552 bytes or less
			while(Len > 0)
				{
				// We can defer the modulo operation:
				// Under worst case the starting value of the two halves is 65520 = (AdlerBase - 1)
				// each new byte is maximum 255
				// The low half grows AdlerLow(n) = AdlerBase - 1 + n * 255
				// The high half grows AdlerHigh(n) = (n + 1)*(AdlerBase - 1) + n * (n + 1) * 255 / 2
				// The maximum n before overflow of 32 bit unsigned integer is 5552
				// it is the solution of the following quadratic equation
				// 255 * n * n + (2 * (AdlerBase - 1) + 255) * n + 2 * (AdlerBase - 1 - uint.MaxValue) = 0
				int n = Len < 5552 ? Len : 5552;
				Len -= n;
				while(--n >= 0)
					{
					AdlerLow += (uint) Buffer[Pos++];
					AdlerHigh += AdlerLow;
					}
				AdlerLow %= Adler32Base;
				AdlerHigh %= Adler32Base;
				}
			return ((AdlerHigh << 16) | AdlerLow);
			}

		internal static byte[] ZLibCompress
				(
				byte[] InputBuf
				)
			{
			// input length
			int InputLen = InputBuf.Length;

			// create output memory stream to receive the compressed buffer
			MemoryStream OutputStream = new();

			// deflate compression object
			DeflateStream Deflate = new(OutputStream, CompressionMode.Compress, true);

			// load input buffer into the compression class
			Deflate.Write(InputBuf, 0, InputLen);

			// compress, flush and close
			Deflate.Close();

			// compressed file length
			int OutputLen = (int) OutputStream.Length;

			// create empty output buffer
			byte[] OutputBuf = new Byte[OutputLen + 18];

			// Header is made out of 16 bits [iiiicccclldxxxxx]
			// iiii is compression information. It is WindowBit - 8 in this case 7. iiii = 0111
			// cccc is compression method. Deflate (8 dec) or Store (0 dec)
			// The first byte is 0x78 for deflate and 0x70 for store
			// ll is compression level 2
			// d is preset dictionary. The preset dictionary is not supported by this program. d is always 0
			// xxx is 5 bit check sum (31 - header % 31)
			// write two bytes in most significant byte first
			OutputBuf[8] = 0x78;
			OutputBuf[9] = 0x9c;

			// copy the compressed result
			OutputStream.Seek(0, SeekOrigin.Begin);
			OutputStream.Read(OutputBuf, 10, OutputLen);
			OutputStream.Close();

			// successful exit
			return OutputBuf;
			}
		}
	}
