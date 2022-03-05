/////////////////////////////////////////////////////////////////////
//
//	QR Code Decoder Library
//
//	Small subset of the DirectShowLib	
//
//	Author: Uzi Granot
//
//	Current Version: 3.0.0
//	Date: March 1, 2022
//
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

using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;

namespace QRCodeDecoderLibrary
	{
	/// <summary>
	/// From AMPROPERTY_PIN
	/// </summary>
	public enum AMPropertyPin
		{
		Category,
		Medium
		}

	/// <summary>
	/// From _AM_RENSDEREXFLAGS
	/// </summary>
	[Flags]
	public enum AMRenderExFlags
		{
		None = 0,
		RenderToExistingRenderers = 1
		}

	/// <summary>
	/// From CDEF_CLASS_* defines
	/// </summary>
	[Flags]
	public enum CDef
		{
		None = 0,
		ClassDefault = 0x0001,
		BypassClassManager = 0x0002,
		ClassLegacy = 0x0004,
		MeritAboveDoNotUse = 0x0008,
		DevmonCMGRDevice = 0x0010,
		DevmonDMO = 0x0020,
		DevmonPNPDevice = 0x0040,
		DevmonFilter = 0x0080,
		DevmonSelectiveMask = 0x00f0
		}

	/// <summary>
	/// From FILTER_STATE
	/// </summary>
	public enum FilterState
		{
		Stopped,
		Paused,
		Running
		}

	/// <summary>
	/// From KSPROPERTY_SUPPORT_* defines
	/// </summary>
	public enum KSPropertySupport
		{
		Get = 1,
		Set = 2
		}

	/// <summary>
	/// Not from DirectShow
	/// </summary>
	public enum PinConnectedStatus
		{
		Unconnected,
		Connected
		}

	/// <summary>
	/// From PIN_DIRECTION
	/// </summary>
	public enum PinDirection
		{
		Input,
		Output
		}

	/// <summary>
	/// From VMR9AspectRatioMode
	/// </summary>
	public enum VMR9AspectRatioMode
		{
		None,
		LetterBox,
		}

	/// <summary>
	/// From VMR9Mode
	/// </summary>
	[Flags]
	public enum VMR9Mode
		{
		None = 0,
		Windowed = 0x00000001,
		Windowless = 0x00000002,
		Renderless = 0x00000004,
		}

	/// <summary>
	/// From VMR9RenderPrefs
	/// </summary>
	[Flags]
	public enum VMR9RenderPrefs
		{
		None = 0,
		DoNotRenderBorder = 0x00000001, // app paints color keys
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a868a9-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGraphBuilder : IFilterGraph
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("36b73882-c2c8-11cf-8b46-00805f6cef60"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFilterGraph2 : IGraphBuilder
		{
		[PreserveSig]
		int AddFilter(
			[In] IBaseFilter pFilter,
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName
			);

		[PreserveSig]
		int RemoveFilter([In] IBaseFilter pFilter);

		[PreserveSig]
		int EnumFilters([Out] out IEnumFilters ppEnum);

		[PreserveSig]
		int FindFilterByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName,
			[Out] out IBaseFilter ppFilter
			);

		[PreserveSig]
		int ConnectDirect(
			[In] IPin ppinOut,
			[In] IPin ppinIn,
			[In, MarshalAs(UnmanagedType.LPStruct)]
			AMMediaType pmt
			);

		[PreserveSig]
		int Reconnect([In] IPin ppin);

		[PreserveSig]
		int Disconnect([In] IPin ppin);

		[PreserveSig]
		int SetDefaultSyncSource();

		[PreserveSig]
		int Connect(
			[In] IPin ppinOut,
			[In] IPin ppinIn
			);

		[PreserveSig]
		int Render([In] IPin ppinOut);

		[PreserveSig]
		int RenderFile(
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFile,
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrPlayList
			);

		[PreserveSig]
		int AddSourceFilter(
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFileName,
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
			[Out] out IBaseFilter ppFilter
			);

		[PreserveSig]
		int SetLogFile(IntPtr hFile); // DWORD_PTR

		[PreserveSig]
		int Abort();

		[PreserveSig]
		int ShouldOperationContinue();

		[PreserveSig]
		int AddSourceFilterForMoniker(
			[In] IMoniker pMoniker,
			[In] IBindCtx pCtx,
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpcwstrFilterName,
			[Out] out IBaseFilter ppFilter
			);

		[PreserveSig]
		int ReconnectEx(
			[In] IPin ppin,
			[In] AMMediaType pmt
			);

		[PreserveSig]
		int RenderEx(
			[In] IPin pPinOut,
			[In] AMRenderExFlags dwFlags,
			[In] IntPtr pvContext // DWORD *
			);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("31EFAC30-515C-11d0-A9AA-00AA0061BE93"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IKsPropertySet
		{
		[PreserveSig]
		int Set(
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid guidPropSet,
			[In] int dwPropID,
			[In] IntPtr pInstanceData,
			[In] int cbInstanceData,
			[In] IntPtr pPropData,
			[In] int cbPropData
			);

		[PreserveSig]
		int Get(
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid guidPropSet,
			[In] int dwPropID,
			[In] IntPtr pInstanceData,
			[In] int cbInstanceData,
			[In, Out] IntPtr pPropData,
			[In] int cbPropData,
			[Out] out int pcbReturned
			);

		[PreserveSig]
		int QuerySupported(
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid guidPropSet,
			[In] int dwPropID,
			[Out] out KSPropertySupport pTypeSupport
			);
		}


	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("C6E13340-30AC-11d0-A18C-00A0C9118956"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMStreamConfig
		{
		[PreserveSig]
		int SetFormat([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		[PreserveSig]
		int GetFormat([Out] out AMMediaType pmt);

		[PreserveSig]
		int GetNumberOfCapabilities(out int piCount, out int piSize);

		[PreserveSig]
		int GetStreamCaps(
			[In] int iIndex,
			[Out] out AMMediaType ppmt,
			[In] IntPtr pSCC
			);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a868b1-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaControl
		{
		[PreserveSig]
		int Run();

		[PreserveSig]
		int Pause();

		[PreserveSig]
		int Stop();

		[PreserveSig]
		int GetState([In] int msTimeout, [Out] out FilterState pfs);

		[PreserveSig]
		int RenderFile([In, MarshalAs(UnmanagedType.BStr)] string strFilename);

		[PreserveSig,
		Obsolete("Automation interface, for pre-.NET VB.  Use IGraphBuilder::AddSourceFilter instead", false)]
		int AddSourceFilter(
			[In, MarshalAs(UnmanagedType.BStr)] string strFilename,
			[Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk
			);

		[PreserveSig,
		Obsolete("Automation interface, for pre-.NET VB.  Use IFilterGraph::EnumFilters instead", false)]
		int Get_FilterCollection([Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig,
		Obsolete("Automation interface, for pre-.NET VB.  Use IFilterMapper2::EnumMatchingFilters instead", false)]
		int Get_RegFilterCollection([Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int StopWhenReady();
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a86891-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPin
		{
		[PreserveSig]
		int Connect(
			[In] IPin pReceivePin,
			[In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
			);

		[PreserveSig]
		int ReceiveConnection(
			[In] IPin pReceivePin,
			[In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
			);

		[PreserveSig]
		int Disconnect();

		[PreserveSig]
		int ConnectedTo(
			[Out] out IPin ppPin);

		/// <summary>
		/// Release returned parameter with DsUtils.FreeAMMediaType
		/// </summary>
		[PreserveSig]
		int ConnectionMediaType(
			[Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		/// <summary>
		/// Release returned parameter with DsUtils.FreePinInfo
		/// </summary>
		[PreserveSig]
		int QueryPinInfo([Out] out PinInfo pInfo);

		[PreserveSig]
		int QueryDirection(out PinDirection pPinDir);

		[PreserveSig]
		int QueryId([Out, MarshalAs(UnmanagedType.LPWStr)] out string Id);

		[PreserveSig]
		int QueryAccept([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		[PreserveSig]
		int EnumMediaTypes([Out] out IEnumMediaTypes ppEnum);

		[PreserveSig]
		int QueryInternalConnections(
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IPin[] ppPins,
			[In, Out] ref int nPin
			);

		[PreserveSig]
		int EndOfStream();

		[PreserveSig]
		int BeginFlush();

		[PreserveSig]
		int EndFlush();

		[PreserveSig]
		int NewSegment(
			[In] long tStart,
			[In] long tStop,
			[In] double dRate
			);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a8689a-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaSample
		{
		[PreserveSig]
		int GetPointer([Out] out IntPtr ppBuffer); // BYTE **

		[PreserveSig]
		int GetSize();

		[PreserveSig]
		int GetTime(
			[Out] out long pTimeStart,
			[Out] out long pTimeEnd
			);

		[PreserveSig]
		int SetTime(
			[In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
			[In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
			);

		[PreserveSig]
		int IsSyncPoint();

		[PreserveSig]
		int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);

		[PreserveSig]
		int IsPreroll();

		[PreserveSig]
		int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);

		[PreserveSig]
		int GetActualDataLength();

		[PreserveSig]
		int SetActualDataLength([In] int len);

		/// <summary>
		/// Returned object must be released with DsUtils.FreeAMMediaType()
		/// </summary>
		[PreserveSig]
		int GetMediaType([Out, MarshalAs(UnmanagedType.LPStruct)] out AMMediaType ppMediaType);

		[PreserveSig]
		int SetMediaType([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pMediaType);

		[PreserveSig]
		int IsDiscontinuity();

		[PreserveSig]
		int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);

		[PreserveSig]
		int GetMediaTime(
			[Out] out long pTimeStart,
			[Out] out long pTimeEnd
			);

		[PreserveSig]
		int SetMediaTime(
			[In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeStart,
			[In, MarshalAs(UnmanagedType.LPStruct)] DsLong pTimeEnd
			);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a86899-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaFilter : IPersist
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a86895-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBaseFilter : IMediaFilter
		{
		[PreserveSig]
		int GetClassID(
			[Out] out Guid pClassID);

		[PreserveSig]
		int Stop();

		[PreserveSig]
		int Pause();

		[PreserveSig]
		int Run(long tStart);

		[PreserveSig]
		int GetState([In] int dwMilliSecsTimeout, [Out] out FilterState filtState);

		[PreserveSig]
		int SetSyncSource([In] IReferenceClock pClock);

		[PreserveSig]
		int GetSyncSource([Out] out IReferenceClock pClock);

		[PreserveSig]
		int EnumPins([Out] out IEnumPins ppEnum);

		[PreserveSig]
		int FindPin(
			[In, MarshalAs(UnmanagedType.LPWStr)] string Id,
			[Out] out IPin ppPin
			);

		[PreserveSig]
		int QueryFilterInfo([Out] out FilterInfo pInfo);

		[PreserveSig]
		int JoinFilterGraph(
			[In] IFilterGraph pGraph,
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName
			);

		[PreserveSig]
		int QueryVendorInfo([Out, MarshalAs(UnmanagedType.LPWStr)] out string pVendorInfo);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a8689f-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFilterGraph
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a86893-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumFilters
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a86892-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumPins
		{
		[PreserveSig]
		int Next(
			[In] int cPins,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IPin[] ppPins,
			[In] IntPtr pcFetched
			);

		[PreserveSig]
		int Skip([In] int cPins);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out] out IEnumPins ppEnum);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("56a86897-0ad4-11ce-b03a-0020af0ba770"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IReferenceClock
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("89c31040-846b-11ce-97d3-00aa0055595a"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumMediaTypes
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("29840822-5B84-11D0-BD3B-00A0C911CE86"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICreateDevEnum
		{
		[PreserveSig]
		int CreateClassEnumerator(
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid pType,
			[Out] out IEnumMoniker ppEnumMoniker,
			[In] CDef dwFlags);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
   Guid("0000010c-0000-0000-C000-000000000046"),
   InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersist
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("55272A00-42CB-11CE-8135-00AA004BB851"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyBag
		{
		[PreserveSig]
		int Read(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[Out, MarshalAs(UnmanagedType.Struct)] out object pVar,
			[In] IErrorLog pErrorLog
			);

		[PreserveSig]
		int Write(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[In, MarshalAs(UnmanagedType.Struct)] ref object pVar
			);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("3127CA40-446E-11CE-8135-00AA004BB851"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IErrorLog
		{
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("6B652FFF-11FE-4fce-92AD-0266B5D7C78F"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISampleGrabber
		{
		[PreserveSig]
		int SetOneShot(
			[In, MarshalAs(UnmanagedType.Bool)] bool OneShot);

		[PreserveSig]
		int SetMediaType(
			[In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		[PreserveSig]
		int GetConnectedMediaType(
			[Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		[PreserveSig]
		int SetBufferSamples(
			[In, MarshalAs(UnmanagedType.Bool)] bool BufferThem);

		[PreserveSig]
		int GetCurrentBuffer(ref int pBufferSize, IntPtr pBuffer);

		[PreserveSig]
		int GetCurrentSample(out IMediaSample ppSample);

		[PreserveSig]
		int SetCallback(ISampleGrabberCB pCallback, int WhichMethodToCallback);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("0579154A-2B53-4994-B0D0-E773148EFF85"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISampleGrabberCB
		{
		/// <summary>
		/// When called, callee must release pSample
		/// </summary>
		[PreserveSig]
		int SampleCB(double SampleTime, IMediaSample pSample);

		[PreserveSig]
		int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("5a804648-4f66-4867-9c43-4f5c822cf1b8"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRFilterConfig9
		{
		[PreserveSig]
		int SetImageCompositor([In] IVMRImageCompositor9 lpVMRImgCompositor);

		[PreserveSig]
		int SetNumberOfStreams([In] int dwMaxStreams);

		[PreserveSig]
		int GetNumberOfStreams([Out] out int pdwMaxStreams);

		[PreserveSig]
		int SetRenderingPrefs([In] VMR9RenderPrefs dwRenderFlags);

		[PreserveSig]
		int GetRenderingPrefs([Out] out VMR9RenderPrefs pdwRenderFlags);

		[PreserveSig]
		int SetRenderingMode([In] VMR9Mode Mode);

		[PreserveSig]
		int GetRenderingMode([Out] out VMR9Mode Mode);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("8f537d09-f85e-4414-b23b-502e54c79927"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRWindowlessControl9
		{
		int GetNativeVideoSize(
			[Out] out int lpWidth,
			[Out] out int lpHeight,
			[Out] out int lpARWidth,
			[Out] out int lpARHeight
			);

		int GetMinIdealVideoSize(
			[Out] out int lpWidth,
			[Out] out int lpHeight
			);

		int GetMaxIdealVideoSize(
			[Out] out int lpWidth,
			[Out] out int lpHeight
			);

		int SetVideoPosition(
			[In] DsRect lpSRCRect,
			[In] DsRect lpDSTRect
			);

		int GetVideoPosition(
			[Out] DsRect lpSRCRect,
			[Out] DsRect lpDSTRect
			);

		int GetAspectRatioMode([Out] out VMR9AspectRatioMode lpAspectRatioMode);

		int SetAspectRatioMode([In] VMR9AspectRatioMode AspectRatioMode);

		int SetVideoClippingWindow([In] IntPtr hwnd); // HWND

		int RepaintVideo(
			[In] IntPtr hwnd, // HWND
			[In] IntPtr hdc // HDC
			);

		int DisplayModeChanged();

		int GetCurrentImage([Out] out IntPtr lpDib); // BYTE**

		int SetBorderColor([In] int Clr);

		int GetBorderColor([Out] out int lpClr);
		}

	[ComImport, System.Security.SuppressUnmanagedCodeSecurity,
	Guid("4a5c89eb-df51-4654-ac2a-e48e02bbabf6"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRImageCompositor9
		{
		}

	/// <summary>
	/// From PIN_INFO
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PinInfo
		{
		[MarshalAs(UnmanagedType.Interface)] public IBaseFilter filter;
		public PinDirection dir;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string name;
		}

	/// <summary>
	/// From FILTER_INFO
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct FilterInfo
		{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string achName;
		[MarshalAs(UnmanagedType.Interface)] public IFilterGraph pGraph;
		}

	/// <summary>
	/// CLSID_FilterGraph
	/// </summary>
	[ComImport, Guid("e436ebb3-524f-11ce-9f53-0020af0ba770")]
	public class FilterGraph
		{
		}

	/// <summary>
	/// CLSID_VideoMixingRenderer9
	/// </summary>
	[ComImport, Guid("51b4abf3-748f-4e3b-a276-c828330e926a")]
	public class VideoMixingRenderer9
		{
		}

	/// <summary>
	/// CLSID_SmartTee
	/// </summary>
	[ComImport, Guid("CC58E280-8AA1-11d1-B3F1-00AA003761C5")]
	public class SmartTee
		{
		}

	/// <summary>
	/// CLSID_SampleGrabber
	/// </summary>
	[ComImport, Guid("C1F400A0-3F08-11d3-9F0B-006008039E37")]
	public class SampleGrabber
		{
		}

	/// <summary>
	/// CLSID_SystemDeviceEnum
	/// </summary>
	[ComImport, Guid("62BE5D10-60EB-11d0-BD3B-00A0C911CE86")]
	public class CreateDevEnum
		{
		}

	static public class FilterCategory
		{
		/// <summary> CLSID_VideoInputDeviceCategory, video capture category </summary>
		public static readonly Guid VideoInputDevice = new(0x860BB310, 0x5D01, 0x11d0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);
		}

	static public class MediaType
		{
		/// <summary> MEDIATYPE_Video 'vids' </summary>
		public static readonly Guid Video = new(0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
		}

	static public class MediaSubType
		{
		/// <summary> MEDIASUBTYPE_RGB16_D3D_DX7_RT </summary>
		public static readonly Guid RGB16_D3D_DX7_RT = new(0x36315237, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_RGB16_D3D_DX9_RT </summary>
		public static readonly Guid RGB16_D3D_DX9_RT = new(0x36315239, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_RGB24 </summary>
		public static readonly Guid RGB24 = new(0xe436eb7d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB32 </summary>
		public static readonly Guid RGB32 = new(0xe436eb7e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_ARGB32 </summary>
		public static readonly Guid ARGB32 = new(0x773c9ac0, 0x3274, 0x11d0, 0xb7, 0x24, 0x00, 0xaa, 0x00, 0x6c, 0x1a, 0x01);

		/// <summary> MEDIASUBTYPE_YUY2 </summary>
		public static readonly Guid YUY2 = new(0x32595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
		}

	static public class FormatType
		{
		/// <summary> FORMAT_VideoInfo </summary>
		public static readonly Guid VideoInfo = new(0x05589f80, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
		}

	static public class PropSetID
		{
		/// <summary> AMPROPSETID_Pin</summary>
		public static readonly Guid Pin = new(0x9b00f101, 0x1567, 0x11d1, 0xb3, 0xf1, 0x00, 0xaa, 0x00, 0x37, 0x61, 0xc5);
		}

	/// <summary>
	/// From AM_MEDIA_TYPE - When you are done with an instance of this class,
	/// it should be released with FreeAMMediaType() to avoid leaking
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class AMMediaType
		{
		public Guid majorType;
		public Guid subType;
		[MarshalAs(UnmanagedType.Bool)] public bool fixedSizeSamples;
		[MarshalAs(UnmanagedType.Bool)] public bool temporalCompression;
		public int sampleSize;
		public Guid formatType;
		public IntPtr unkPtr; // IUnknown Pointer
		public int formatSize;
		public IntPtr formatPtr; // Pointer to a buff determined by formatType
		}
	/// <summary>
	/// From BITMAPINFOHEADER
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public class BitmapInfoHeader
		{
		public int Size;
		public int Width;
		public int Height;
		public short Planes;
		public short BitCount;
		public int Compression;
		public int ImageSize;
		public int XPelsPerMeter;
		public int YPelsPerMeter;
		public int ClrUsed;
		public int ClrImportant;
		}

	/// <summary>
	/// From VIDEOINFOHEADER
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class VideoInfoHeader
		{
		public DsRect SrcRect;
		public DsRect TargetRect;
		public int BitRate;
		public int BitErrorRate;
		public long AvgTimePerFrame;
		public BitmapInfoHeader BmiHeader;
		}

	/// <summary>
	/// DirectShowLib.DsLong is a wrapper class around a <see cref="System.Int64"/> value type.
	/// </summary>
	/// <remarks>
	/// This class is necessary to enable null paramters passing.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public class DsLong
		{
		private readonly long Value;

		/// <summary>
		/// Constructor
		/// Initialize a new instance of DirectShowLib.DsLong with the Value parameter
		/// </summary>
		/// <param name="Value">Value to assign to this new instance</param>
		public DsLong(long Value)
			{
			this.Value = Value;
			}

		/// <summary>
		/// Get a string representation of this DirectShowLib.DsLong Instance.
		/// </summary>
		/// <returns>A string representing this instance</returns>
		public override string ToString()
			{
			return this.Value.ToString();
			}

		public override int GetHashCode()
			{
			return this.Value.GetHashCode();
			}

		/// <summary>
		/// Define implicit cast between DirectShowLib.DsLong and System.Int64 for languages supporting this feature.
		/// VB.Net doesn't support implicit cast. <see cref="DirectShowLibrary.DsLong.ToInt64"/> for similar functionality.
		/// <code>
		///   // Define a new DsLong instance
		///   DsLong dsL = new DsLong(9876543210);
		///   // Do implicit cast between DsLong and Int64
		///   long l = dsL;
		///
		///   Console.WriteLine(l.ToString());
		/// </code>
		/// </summary>
		/// <param name="g">DirectShowLib.DsLong to be cast</param>
		/// <returns>A casted System.Int64</returns>
		public static implicit operator long(DsLong l)
			{
			return l.Value;
			}

		/// <summary>
		/// Define implicit cast between System.Int64 and DirectShowLib.DsLong for languages supporting this feature.
		/// VB.Net doesn't support implicit cast. <see cref="DirectShowLibrary.DsGuid.FromInt64"/> for similar functionality.
		/// <code>
		///   // Define a new Int64 instance
		///   long l = 9876543210;
		///   // Do implicit cast between Int64 and DsLong
		///   DsLong dsl = l;
		///
		///   Console.WriteLine(dsl.ToString());
		/// </code>
		/// </summary>
		/// <param name="g">System.Int64 to be cast</param>
		/// <returns>A casted DirectShowLib.DsLong</returns>
		public static implicit operator DsLong(long l)
			{
			return new DsLong(l);
			}

		/// <summary>
		/// Get the System.Int64 equivalent to this DirectShowLib.DsLong instance.
		/// </summary>
		/// <returns>A System.Int64</returns>
		public long ToInt64()
			{
			return this.Value;
			}

		/// <summary>
		/// Get a new DirectShowLib.DsLong instance for a given System.Int64
		/// </summary>
		/// <param name="g">The System.Int64 to wrap into a DirectShowLib.DsLong</param>
		/// <returns>A new instance of DirectShowLib.DsLong</returns>
		public static DsLong FromInt64(long l)
			{
			return new DsLong(l);
			}
		}


	/// <summary>
	/// DirectShowLib.DsRect is a managed representation of the Win32 RECT structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class DsRect
		{
		public int left;
		public int top;
		public int right;
		public int bottom;

		/// <summary>
		/// A parametred constructor. Initialize fields with a given <see cref="System.Drawing.Rectangle"/>.
		/// </summary>
		/// <param name="rectangle">A <see cref="System.Drawing.Rectangle"/></param>
		/// <remarks>
		/// Warning, DsRect define a rectangle by defining two of his corners and <see cref="System.Drawing.Rectangle"/> define a rectangle with his upper/left corner, his width and his height.
		/// </remarks>
		public DsRect(Rectangle rectangle)
			{
			this.left = rectangle.Left;
			this.top = rectangle.Top;
			this.right = rectangle.Right;
			this.bottom = rectangle.Bottom;
			}

		/// <summary>
		/// Define implicit cast between DirectShowLib.DsRect and System.Drawing.Rectangle for languages supporting this feature.
		/// VB.Net doesn't support implicit cast. <see cref="DirectShowLibrary.DsRect.ToRectangle"/> for similar functionality.
		/// <code>
		///   // Define a new Rectangle instance
		///   Rectangle r = new Rectangle(0, 0, 100, 100);
		///   // Do implicit cast between Rectangle and DsRect
		///   DsRect dsR = r;
		///
		///   Console.WriteLine(dsR.ToString());
		/// </code>
		/// </summary>
		/// <param name="r">a DsRect to be cast</param>
		/// <returns>A casted System.Drawing.Rectangle</returns>
		public static implicit operator Rectangle(DsRect r)
			{
			return r.ToRectangle();
			}

		/// <summary>
		/// Define implicit cast between System.Drawing.Rectangle and DirectShowLib.DsRect for languages supporting this feature.
		/// VB.Net doesn't support implicit cast. <see cref="DirectShowLibrary.DsRect.FromRectangle"/> for similar functionality.
		/// <code>
		///   // Define a new DsRect instance
		///   DsRect dsR = new DsRect(0, 0, 100, 100);
		///   // Do implicit cast between DsRect and Rectangle
		///   Rectangle r = dsR;
		///
		///   Console.WriteLine(r.ToString());
		/// </code>
		/// </summary>
		/// <param name="r">A System.Drawing.Rectangle to be cast</param>
		/// <returns>A casted DsRect</returns>
		public static implicit operator DsRect(Rectangle r)
			{
			return new DsRect(r);
			}

		/// <summary>
		/// Get the System.Drawing.Rectangle equivalent to this DirectShowLib.DsRect instance.
		/// </summary>
		/// <returns>A System.Drawing.Rectangle</returns>
		public Rectangle ToRectangle()
			{
			return new Rectangle(this.left, this.top, (this.right - this.left), (this.bottom - this.top));
			}

		/// <summary>
		/// Get a new DirectShowLib.DsRect instance for a given <see cref="System.Drawing.Rectangle"/>
		/// </summary>
		/// <param name="r">The <see cref="System.Drawing.Rectangle"/> used to initialize this new DirectShowLib.DsGuid</param>
		/// <returns>A new instance of DirectShowLib.DsGuid</returns>
		public static DsRect FromRectangle(Rectangle r)
			{
			return new DsRect(r);
			}
		}

	[StructLayout(LayoutKind.Sequential)]
	public struct NormalizedRect
		{
		public float left;
		public float top;
		public float right;
		public float bottom;

		public NormalizedRect(RectangleF r)
			{
			this.left = r.Left;
			this.top = r.Top;
			this.right = r.Right;
			this.bottom = r.Bottom;
			}

		public static implicit operator RectangleF(NormalizedRect r)
			{
			return r.ToRectangleF();
			}

		public static implicit operator NormalizedRect(Rectangle r)
			{
			return new NormalizedRect(r);
			}

		public static bool operator ==(NormalizedRect r1, NormalizedRect r2)
			{
			return ((r1.left == r2.left) && (r1.top == r2.top) && (r1.right == r2.right) && (r1.bottom == r2.bottom));
			}

		public static bool operator !=(NormalizedRect r1, NormalizedRect r2)
			{
			return ((r1.left != r2.left) || (r1.top != r2.top) || (r1.right != r2.right) || (r1.bottom != r2.bottom));
			}

		public override bool Equals(object obj)
			{
			if(obj is not NormalizedRect) return false;

			NormalizedRect other = (NormalizedRect) obj;
			return (this == other);
			}

		public RectangleF ToRectangleF()
			{
			return new RectangleF(this.left, this.top, (this.right - this.left), (this.bottom - this.top));
			}

		public override int GetHashCode()
			{
            return left.GetHashCode() |
                top.GetHashCode() |
                right.GetHashCode() |
                bottom.GetHashCode();
			}
		}

	static public class DsResults
		{
		public const int E_BufferNotSet = unchecked((int) 0x8004020C);
		public const int E_NotConnected = unchecked((int) 0x80040209);
		}

	static public class DsError
		{
		[DllImport("quartz.dll", CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "AMGetErrorTextW"),
		SuppressUnmanagedCodeSecurity]
		private static extern int AMGetErrorText(int hr, StringBuilder buf, int max);

		/// <summary>
		/// If hr has a "failed" status code (E_*), throw an exception.  Note that status
		/// messages (S_*) are not considered failure codes.  If DirectShow error text
		/// is available, it is used to build the exception, otherwise a generic com error
		/// is thrown.
		/// </summary>
		/// <param name="hr">The HRESULT to check</param>
		public static void ThrowExceptionForHR(int hr)
			{
			// If a severe error has occurred
			if(hr < 0)
				{
				string s = GetErrorText(hr);

				// If a string is returned, build a com error from it
				if(s != null)
					{
					throw new COMException(s, hr);
					}
				else
					{
					// No string, just use standard com error
					Marshal.ThrowExceptionForHR(hr);
					}
				}
			}

		/// <summary>
		/// Returns a string describing a DS error.  Works for both error codes
		/// (values < 0) and Status codes (values >= 0)
		/// </summary>
		/// <param name="hr">HRESULT for which to get description</param>
		/// <returns>The string, or null if no error text can be found</returns>
		public static string GetErrorText(int hr)
			{
			const int MAX_ERROR_TEXT_LEN = 160;

			// Make a buffer to hold the string
			StringBuilder buf = new(MAX_ERROR_TEXT_LEN, MAX_ERROR_TEXT_LEN);

			// If a string is returned, build a com error from it
			if(AMGetErrorText(hr, buf, MAX_ERROR_TEXT_LEN) > 0) return buf.ToString();
			return null;
			}
		}

	static public class DsUtils
		{
		/// <summary>
		/// Returns the PinCategory of the specified pin.  Usually a member of PinCategory.  Not all pins have a category.
		/// </summary>
		/// <param name="pPin"></param>
		/// <returns>Guid indicating pin category or Guid.Empty on no category.  Usually a member of PinCategory</returns>
		public static Guid GetPinCategory(IPin pPin)
			{
			Guid guidRet = Guid.Empty;

			// Memory to hold the returned guid
			int iSize = Marshal.SizeOf(typeof(Guid));
			IntPtr ipOut = Marshal.AllocCoTaskMem(iSize);

			try
				{
				Guid g = PropSetID.Pin;

				// Get an IKsPropertySet from the pin
				if (pPin is IKsPropertySet pKs)
					{
					// Query for the Category
					int hr = pKs.Get(g, (int) AMPropertyPin.Category, IntPtr.Zero, 0, ipOut, iSize, out int cbBytes);
					DsError.ThrowExceptionForHR(hr);

					// Marshal it to the return variable
					guidRet = (Guid)Marshal.PtrToStructure(ipOut, typeof(Guid));
					}
				}

			finally
				{
				Marshal.FreeCoTaskMem(ipOut);
				}

			return guidRet;
			}

		/// <summary>
		///  Free the nested structures and release any
		///  COM objects within an AMMediaType struct.
		/// </summary>
		public static void FreeAMMediaType(AMMediaType mediaType)
			{
			if(mediaType != null)
				{
				if(mediaType.formatSize != 0)
					{
					Marshal.FreeCoTaskMem(mediaType.formatPtr);
					mediaType.formatSize = 0;
					mediaType.formatPtr = IntPtr.Zero;
					}
				if(mediaType.unkPtr != IntPtr.Zero)
					{
					Marshal.Release(mediaType.unkPtr);
					mediaType.unkPtr = IntPtr.Zero;
					}
				}
			}

		/// <summary>
		///  Free the nested interfaces within a PinInfo struct.
		/// </summary>
		public static void FreePinInfo(PinInfo pinInfo)
			{
			if(pinInfo.filter != null)
				{
				Marshal.ReleaseComObject(pinInfo.filter);
				pinInfo.filter = null;
				}
			}

		}

	public class DsDevice : IDisposable
		{
		private IMoniker m_Moniker;
		private string m_Name;

		public DsDevice(IMoniker Moniker)
			{
			m_Moniker = Moniker;
			m_Name = null;
			}

		public IMoniker Moniker
			{
			get
				{
				return m_Moniker;
				}
			}

		public string Name
			{
			get
				{
				if(m_Name == null)
					{
					m_Name = GetPropBagValue("FriendlyName");
					}
				return m_Name;
				}
			}

		/// <summary>
		/// Returns an array of DsDevices of type devcat.
		/// </summary>
		/// <param name="cat">Any one of FilterCategory</param>
		public static DsDevice[] GetDevicesOfCat(Guid FilterCategory)
			{
			// Use arrayList to build the retun list since it is easily resizable
			DsDevice[] devret;
			ArrayList devs = new();
			ICreateDevEnum enumDev = (ICreateDevEnum) new CreateDevEnum();
			int hr = enumDev.CreateClassEnumerator(FilterCategory, out IEnumMoniker enumMon, 0);
			DsError.ThrowExceptionForHR(hr);

			// CreateClassEnumerator returns null for enumMon if there are no entries
			if(hr != 1)
				{
				try
					{
					try
						{
						IMoniker[] mon = new IMoniker[1];
						while((enumMon.Next(1, mon, IntPtr.Zero) == 0))
							{
							try
								{
								// The devs array now owns this object.  Don't
								// release it if we are going to be successfully
								// returning the devret array
								devs.Add(new DsDevice(mon[0]));
								}
							catch
								{
								Marshal.ReleaseComObject(mon[0]);
								throw;
								}
							}
						}
					finally
						{
						Marshal.ReleaseComObject(enumMon);
						}

					// Copy the ArrayList to the DsDevice[]
					devret = new DsDevice[devs.Count];
					devs.CopyTo(devret);
					}
				catch
					{
					foreach(DsDevice d in devs)
						{
						d.Dispose();
						}
					throw;
					}
				}
			else
				{
				devret = Array.Empty<DsDevice>();
				}

			return devret;
			}

		/// <summary>
		/// Get a specific PropertyBag value from a moniker
		/// </summary>
		/// <param name="sPropName">The name of the value to retrieve</param>
		/// <returns>String or null on error</returns>
		public string GetPropBagValue(string sPropName)
			{
			IPropertyBag bag;
			string ret = null;
			object bagObj = null;

			try
				{
				Guid bagId = typeof(IPropertyBag).GUID;
				m_Moniker.BindToStorage(null, null, ref bagId, out bagObj);

				bag = (IPropertyBag)bagObj;

				int hr = bag.Read(sPropName, out object val, null);
				DsError.ThrowExceptionForHR(hr);

				ret = val as string;
				}
			catch
				{
				ret = null;
				}
			finally
				{
				if (bagObj != null) Marshal.ReleaseComObject(bagObj);
				}

			return ret;
			}

		public void Dispose()
			{
			if(Moniker != null)
				{
				Marshal.ReleaseComObject(Moniker);
				m_Moniker = null;
				}
			m_Name = null;
			GC.SuppressFinalize(this);
			return;
			}
		}

	static public class DsFindPin
		{
		/// <summary>
		/// Scans a filter's pins looking for a pin in the specified direction
		/// </summary>
		/// <param name="vSource">The filter to scan</param>
		/// <param name="vDir">The direction to find</param>
		/// <param name="iIndex">Zero based index (ie 2 will return the third pin in the specified direction)</param>
		/// <returns>The matching pin, or null if not found</returns>
		public static IPin ByDirection(IBaseFilter vSource, PinDirection vDir, int iIndex)
			{
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

			if(vSource == null) return null;

			// Get the pin enumerator
			int hr = vSource.EnumPins(out IEnumPins ppEnum);
			DsError.ThrowExceptionForHR(hr);

			try
				{
				// Walk the pins looking for a match
				while(ppEnum.Next(1, pPins, IntPtr.Zero) == 0)
					{
					// Read the direction
					hr = pPins[0].QueryDirection(out PinDirection ppindir);
					DsError.ThrowExceptionForHR(hr);

					// Is it the right direction?
					if(ppindir == vDir)
						{
						// Is is the right index?
						if(iIndex == 0)
							{
							pRet = pPins[0];
							break;
							}
						iIndex--;
						}
					Marshal.ReleaseComObject(pPins[0]);
					}
				}
			finally
				{
				Marshal.ReleaseComObject(ppEnum);
				}

			return pRet;
			}

		/// <summary>
		/// Scans a filter's pins looking for a pin with the specified name
		/// </summary>
		/// <param name="vSource">The filter to scan</param>
		/// <param name="vPinName">The pin name to find</param>
		/// <returns>The matching pin, or null if not found</returns>
		public static IPin ByName(IBaseFilter vSource, string vPinName)
			{
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

			if(vSource == null)
				{
				return null;
				}

			// Get the pin enumerator
			int hr = vSource.EnumPins(out IEnumPins ppEnum);
			DsError.ThrowExceptionForHR(hr);

			try
				{
				// Walk the pins looking for a match
				while(ppEnum.Next(1, pPins, IntPtr.Zero) == 0)
					{
					// Read the info
					hr = pPins[0].QueryPinInfo(out PinInfo ppinfo);
					DsError.ThrowExceptionForHR(hr);

					// Is it the right name?
					if(ppinfo.name == vPinName)
						{
						DsUtils.FreePinInfo(ppinfo);
						pRet = pPins[0];
						break;
						}
					Marshal.ReleaseComObject(pPins[0]);
					DsUtils.FreePinInfo(ppinfo);
					}
				}
			finally
				{
				Marshal.ReleaseComObject(ppEnum);
				}

			return pRet;
			}

		/// <summary>
		/// Scans a filter's pins looking for a pin with the specified connection status
		/// </summary>
		/// <param name="vSource">The filter to scan</param>
		/// <param name="vStat">The status to find (connected/unconnected)</param>
		/// <param name="iIndex">Zero based index (ie 2 will return the third pin with the specified status)</param>
		/// <returns>The matching pin, or null if not found</returns>
		public static IPin ByConnectionStatus(IBaseFilter vSource, PinConnectedStatus vStat, int iIndex)
			{
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

			if (vSource == null) return null;

			// Get the pin enumerator
			int hr = vSource.EnumPins(out IEnumPins ppEnum);
			DsError.ThrowExceptionForHR(hr);

			try
				{
				// Walk the pins looking for a match
				while(ppEnum.Next(1, pPins, IntPtr.Zero) == 0)
					{
					// Read the connected status
					hr = pPins[0].ConnectedTo(out IPin pOutPin);

					// Check for VFW_E_NOT_CONNECTED.  Anything else is bad.
					if(hr != DsResults.E_NotConnected)
						{
						DsError.ThrowExceptionForHR(hr);

						// The ConnectedTo call succeeded, release the interface
						Marshal.ReleaseComObject(pOutPin);
						}

					// Is it the right status?
					if(
						(hr == 0 && vStat == PinConnectedStatus.Connected) ||
						(hr == DsResults.E_NotConnected && vStat == PinConnectedStatus.Unconnected)
						)
						{
						// Is is the right index?
						if(iIndex == 0)
							{
							pRet = pPins[0];
							break;
							}
						iIndex--;
						}
					Marshal.ReleaseComObject(pPins[0]);
					}
				}
			finally
				{
				Marshal.ReleaseComObject(ppEnum);
				}

			return pRet;
			}
		}

	// This abstract class contains definitions for use in implementing a custom marshaler.
	//
	// MarshalManagedToNative() gets called before the COM method, and MarshalNativeToManaged() gets
	// called after.  This allows for allocating a correctly sized memory block for the COM call,
	// then to break up the memory block and build an object that c# can digest.

	abstract internal class DsMarshaler : ICustomMarshaler
		{
		// The cookie isn't currently being used.
		protected string m_cookie;

		// The managed object passed in to MarshalManagedToNative, and modified in MarshalNativeToManaged
		protected object m_obj;

		// The constructor.  This is called from GetInstance (below)
		public DsMarshaler(string cookie)
			{
			// If we get a cookie, save it.
			m_cookie = cookie;
			}

		// Called just before invoking the COM method.  The returned IntPtr is what goes on the stack
		// for the COM call.  The input arg is the parameter that was passed to the method.
		virtual public IntPtr MarshalManagedToNative(object managedObj)
			{
			// Save off the passed-in value.  Safe since we just checked the type.
			m_obj = managedObj;

			// Create an appropriately sized buffer, blank it, and send it to the marshaler to
			// make the COM call with.
			int iSize = GetNativeDataSize() + 3;
			IntPtr p = Marshal.AllocCoTaskMem(iSize);

			for(int x = 0; x < iSize / 4; x++)
				{
				Marshal.WriteInt32(p, x * 4, 0);
				}

			return p;
			}

		// Called just after invoking the COM method.  The IntPtr is the same one that just got returned
		// from MarshalManagedToNative.  The return value is unused.
		virtual public object MarshalNativeToManaged(IntPtr pNativeData)
			{
			return m_obj;
			}

		// Release the (now unused) buffer
		virtual public void CleanUpNativeData(IntPtr pNativeData)
			{
			if(pNativeData != IntPtr.Zero) Marshal.FreeCoTaskMem(pNativeData);
			}

		// Release the (now unused) managed object
		virtual public void CleanUpManagedData(object managedObj)
			{
			m_obj = null;
			}

		// This routine is (apparently) never called by the marshaler.  However it can be useful.
		abstract public int GetNativeDataSize();

		// GetInstance is called by the marshaler in preparation to doing custom marshaling.  The (optional)
		// cookie is the value specified in MarshalCookie="asdf", or "" is none is specified.

		// It is commented out in this abstract class, but MUST be implemented in derived classes
		//public static ICustomMarshaler GetInstance(string cookie)
		}

	// c# does not correctly marshal arrays of pointers.
	//
	internal class EMTMarshaler : DsMarshaler
		{
		public EMTMarshaler(string cookie) : base(cookie)
			{
			}

		// Called just after invoking the COM method.  The IntPtr is the same one that just got returned
		// from MarshalManagedToNative.  The return value is unused.
		override public object MarshalNativeToManaged(IntPtr pNativeData)
			{
			AMMediaType[] emt = m_obj as AMMediaType[];

			for(int x = 0; x < emt.Length; x++)
				{
				// Copy in the value, and advance the pointer
				IntPtr p = Marshal.ReadIntPtr(pNativeData, x * IntPtr.Size);
				if(p != IntPtr.Zero)
					{
					emt[x] = (AMMediaType) Marshal.PtrToStructure(p, typeof(AMMediaType));
					}
				else
					{
					emt[x] = null;
					}
				}

			return null;
			}

		// The number of bytes to marshal out
		override public int GetNativeDataSize()
			{
			// Get the array size
			// Multiply that times the size of a pointer
			return ((Array) m_obj).Length * IntPtr.Size;
			}
		}
	}
