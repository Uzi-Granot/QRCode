/////////////////////////////////////////////////////////////////////
//
//	QR Code Decoder Library
//
//	Video camera sample grabber helper
//
//	Author: Uzi Granot
//
//	Current Version: 3.0.0
//	Date: March 1, 2022
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

using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace QRCodeDecoderLibrary
	{
	/// <summary>
	/// Helper for SampleGrabber. Used to make screenshots (snapshots).
	/// </summary>
	/// <remarks>This class is inherited from <see cref="ISampleGrabberCB"/> class.</remarks>
	/// 
	/// <author> free5lot (free5lot@yandex.ru) </author>
	/// <version> 2013.10.17 </version>
	internal sealed class SampleGrabberHelper : ISampleGrabberCB, IDisposable
	{
	/// <summary>
	/// Flag to wait for the async job to finish.
	/// </summary>
	private readonly ManualResetEvent m_PictureReady = null;

	/// <summary>
	/// Flag indicates we want to store a frame.
	/// </summary>
	private volatile bool m_bWantOneFrame = false;

	/// <summary>
	/// Buffer for bitmap data.  Always release by caller.
	/// </summary>
	private IntPtr m_ipBuffer = IntPtr.Zero;

	/// <summary>
	/// Video frame width. Calculated once in constructor for perf.
	/// </summary>
	private int m_videoWidth;

	/// <summary>
	/// Video frame height. Calculated once in constructor for perf.
	/// </summary>
	private int m_videoHeight;

	/// <summary>
	/// Video frame bits per pixel.
	/// </summary>
	private int m_videoBitCount;

	/// <summary>
	/// Size of frame in bytes.
	/// </summary>
	private int m_ImageSize;

	/// <summary>
	/// Pointer to COM-interface ISampleGrabber.
	/// </summary>
	private ISampleGrabber m_SampleGrabber = null;

	/// <summary>
	/// Flag means should helper store (buffer) samples of current frame or not.
	/// </summary>
	private readonly bool m_bBufferSamplesOfCurrentFrame = false;

	/// <summary>
	/// Default constructor for <see cref="SampleGrabberHelper"/> class.
	/// </summary>
	/// <param name="sampleGrabber">Pointer to COM-interface ISampleGrabber.</param>
	/// <param name="buffer_samples_of_current_frame">Flag means should helper store (buffer) samples of current frame or not.</param>
	public SampleGrabberHelper(ISampleGrabber sampleGrabber, bool buffer_samples_of_current_frame)
		{
		m_SampleGrabber = sampleGrabber;
		m_bBufferSamplesOfCurrentFrame = buffer_samples_of_current_frame;

		// tell the callback to ignore new images
		m_PictureReady = new ManualResetEvent(false);
		return;
		}

	/// <summary>
	/// Disposes object and snapshot.
	/// </summary>
	public void Dispose()
		{
		if(m_PictureReady != null) m_PictureReady.Close();
		m_SampleGrabber = null;
		return;
		}

	/// <summary>
	/// Configures mode (mediatype, format type and etc).
	/// </summary>
	public void ConfigureMode()
		{
		AMMediaType media = new();

		// Set the media type to Video/RBG24
		media.majorType = MediaType.Video;
		media.subType = MediaSubType.RGB24;
		media.formatType = FormatType.VideoInfo;
		int hr = m_SampleGrabber.SetMediaType(media);
		DsError.ThrowExceptionForHR(hr);

		DsUtils.FreeAMMediaType(media);

		// Configure the samplegrabber

		// To save current frame via SnapshotNextFrame
		// ISampleGrabber::SetCallback method
		// Note  [Deprecated. This API may be removed from future releases of Windows.]
		// http://msdn.microsoft.com/en-us/library/windows/desktop/dd376992%28v=vs.85%29.aspx
		hr = m_SampleGrabber.SetCallback(this, 1); // 1 == WhichMethodToCallback, call the ISampleGrabberCB::BufferCB method
		DsError.ThrowExceptionForHR(hr);

		// To save current frame via SnapshotCurrentFrame
		if(m_bBufferSamplesOfCurrentFrame)
			{
			//ISampleGrabber::SetBufferSamples method
			// Note  [Deprecated. This API may be removed from future releases of Windows.]
			// http://msdn.microsoft.com/en-us/windows/dd376991
			hr = m_SampleGrabber.SetBufferSamples(true);
			DsError.ThrowExceptionForHR(hr);
			}
		return;
		}

	/// <summary>
	/// Gets and saves mode (mediatype, format type and etc). 
	/// </summary>
	public void SaveMode()
		{
		// Get the media type from the SampleGrabber
		AMMediaType media = new();

		int hr = m_SampleGrabber.GetConnectedMediaType(media);
		DsError.ThrowExceptionForHR(hr);

		if((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
			{
			throw new NotSupportedException("Unknown Grabber Media Format");
			}

		// Grab the size info
		VideoInfoHeader videoInfoHeader = (VideoInfoHeader) Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
		m_videoWidth = videoInfoHeader.BmiHeader.Width;
		m_videoHeight = videoInfoHeader.BmiHeader.Height;
		m_videoBitCount = videoInfoHeader.BmiHeader.BitCount;
		m_ImageSize = videoInfoHeader.BmiHeader.ImageSize;

		DsUtils.FreeAMMediaType(media);
		return;
		}

	/// <summary>
	/// SampleCB callback (NOT USED). It should be implemented for ISampleGrabberCB
	/// </summary>
	int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
		{
		Marshal.ReleaseComObject(pSample);
		return 0;
		}

    [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
    public static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

	/// <summary>
	/// BufferCB callback 
	/// </summary>
	/// <remarks>COULD BE EXECUTED FROM FOREIGN THREAD.</remarks>
	int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
		{
		// Note that we depend on only being called once per call to Click.  Otherwise
		// a second call can overwrite the previous image.
		Debug.Assert(BufferLen == Math.Abs(m_videoBitCount / 8 * m_videoWidth) * m_videoHeight, "Incorrect buffer length");

		if(m_bWantOneFrame)
			{
			m_bWantOneFrame = false;
			Debug.Assert(m_ipBuffer != IntPtr.Zero, "Unitialized buffer");

			// Save the buffer
			CopyMemory(m_ipBuffer, pBuffer, BufferLen);

			// Picture is ready.
			m_PictureReady.Set();
			}

		return 0;
		}

	/// <summary>
	/// Makes a snapshot of next frame
	/// </summary>
	/// <returns>Bitmap with snapshot</returns>
	public Bitmap SnapshotNextFrame()
		{
		if(m_SampleGrabber == null) throw new ApplicationException("SampleGrabber was not initialized");

		// capture image
		IntPtr ip = GetNextFrame();

		if(ip == IntPtr.Zero) throw new ApplicationException("Can not snap next frame");

		PixelFormat pixelFormat = m_videoBitCount switch
			{
			24 => PixelFormat.Format24bppRgb,
			32 => PixelFormat.Format32bppRgb,
			48 => PixelFormat.Format48bppRgb,
			_ => throw new ApplicationException("Unsupported BitCount"),
			};

		Bitmap bitmap = new(m_videoWidth, m_videoHeight, (m_videoBitCount / 8) * m_videoWidth, pixelFormat, ip);

		Bitmap bitmap_clone = bitmap.Clone(new Rectangle(0, 0, m_videoWidth, m_videoHeight), PixelFormat.Format24bppRgb);
		bitmap_clone.RotateFlip(RotateFlipType.RotateNoneFlipY);

		// Release any previous buffer
		if(ip != IntPtr.Zero) Marshal.FreeCoTaskMem(ip);

		bitmap.Dispose();

		return bitmap_clone;
		}

	/// <summary>
	/// Makes a snapshot of current frame
	/// </summary>
	/// <returns>Bitmap with snapshot</returns>
	public Bitmap SnapshotCurrentFrame()
		{
		if(m_SampleGrabber == null) throw new ApplicationException("SampleGrabber was not initialized");

		if(!m_bBufferSamplesOfCurrentFrame) throw new ApplicationException("SampleGrabberHelper was created without buffering-mode (buffer of current frame)");

		// capture image
		IntPtr ip = GetCurrentFrame();

		PixelFormat pixelFormat = m_videoBitCount switch
			{
			24 => PixelFormat.Format24bppRgb,
			32 => PixelFormat.Format32bppRgb,
			48 => PixelFormat.Format48bppRgb,
			_ => throw new ApplicationException("Unsupported BitCount"),
			};
		Bitmap bitmap = new(m_videoWidth, m_videoHeight, (m_videoBitCount / 8) * m_videoWidth, pixelFormat, ip);

		Bitmap bitmap_clone = bitmap.Clone(new Rectangle(0, 0, m_videoWidth, m_videoHeight), PixelFormat.Format24bppRgb);
		bitmap_clone.RotateFlip(RotateFlipType.RotateNoneFlipY);


		// Release any previous buffer
		if(ip != IntPtr.Zero) Marshal.FreeCoTaskMem(ip);

		bitmap.Dispose();

		return bitmap_clone;
		}

	/// <summary>
	/// Get the image from the Still pin.  The returned image can turned into a bitmap with
	/// Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
	/// If the image is upside down, you can fix it with
	/// b.RotateFlip(RotateFlipType.RotateNoneFlipY);
	/// </summary>
	/// <returns>Returned pointer to be freed by caller with Marshal.FreeCoTaskMem</returns>
	private IntPtr GetNextFrame()
		{
		// get ready to wait for new image
		m_PictureReady.Reset();
		m_ipBuffer = Marshal.AllocCoTaskMem(Math.Abs(m_videoBitCount / 8 * m_videoWidth) * m_videoHeight);

		try
			{
			m_bWantOneFrame = true;

			// Start waiting
			if(!m_PictureReady.WaitOne(5000, false)) throw new ApplicationException("Timeout while waiting to get a snapshot");
			}
		catch
			{
			Marshal.FreeCoTaskMem(m_ipBuffer);
			m_ipBuffer = IntPtr.Zero;
			throw;
			}

		// Got one
		return m_ipBuffer;
		}

	/// <summary>
	/// Grab a snapshot of the most recent image played.
	/// Returns A pointer to the raw pixel data.
	/// Caller must release this memory with Marshal.FreeCoTaskMem when it is no longer needed.
	/// </summary>
	/// <returns>A pointer to the raw pixel data</returns>
	private IntPtr GetCurrentFrame()
		{
		if( ! m_bBufferSamplesOfCurrentFrame )
		throw new ApplicationException("SampleGrabberHelper was created without buffering-mode (buffer of current frame)");

		IntPtr ip = IntPtr.Zero;
		int iBuffSize = 0;

		// Read the buffer size
		int hr = m_SampleGrabber.GetCurrentBuffer(ref iBuffSize, ip);
		DsError.ThrowExceptionForHR(hr);

		Debug.Assert(iBuffSize == m_ImageSize, "Unexpected buffer size");

		// Allocate the buffer and read it
		ip = Marshal.AllocCoTaskMem(iBuffSize);

		hr = m_SampleGrabber.GetCurrentBuffer(ref iBuffSize, ip);
		DsError.ThrowExceptionForHR(hr);

		return ip;
		}
    }
}
