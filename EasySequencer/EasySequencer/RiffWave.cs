using System;
using System.IO;
using System.Runtime.InteropServices;

unsafe class RiffWave {
	private static readonly byte[] SYMBOL_RIFF = { 0x52, 0x49, 0x46, 0x46 };    // "RIFF"
	private static readonly byte[] SYMBOL_WAVE = { 0x57, 0x41, 0x56, 0x45 };    // "WAVE"
	private static readonly byte[] SYMBOL_FMT_ = { 0x66, 0x6D, 0x74, 0x20 };    // "fmt "
	private static readonly byte[] SYMBOL_DATA = { 0x64, 0x61, 0x74, 0x61 };    // "data"
	private static readonly byte[] SYMBOL_0000 = { 0x00, 0x00, 0x00, 0x00 };    //

	[StructLayout(LayoutKind.Sequential)]
	private struct Riff {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Symbol;

		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Type;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct FMT_ {
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] Symbol;

		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 Size;

		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Id;

		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Channels;

		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 SampleRate;

		[MarshalAs(UnmanagedType.U4, SizeConst = 4)]
		public UInt32 BytesPerSec;

		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 BytesPerSample;

		[MarshalAs(UnmanagedType.U2, SizeConst = 2)]
		public UInt16 Bits;
	}

	private Riff mRiff;
	private FMT_ mFmt_;
	private FileStream mFs;
	private bool mIsWriteMode;
	private UInt32 mSamples;

	public RiffWave(string filePath, int sampleRate, int channels, int bits) {
		mFs = new FileStream(filePath, FileMode.Create);
		mIsWriteMode = true;
		mSamples = 0;

		mRiff = new Riff();
		mRiff.Symbol = SYMBOL_RIFF;
		mRiff.Size = 0;
		mRiff.Type = SYMBOL_WAVE;

		mFmt_ = new FMT_();
		mFmt_.Symbol = SYMBOL_FMT_;
		mFmt_.Size = 16;
		mFmt_.Id = 1;
		mFmt_.Channels = (UInt16)channels;
		mFmt_.SampleRate = (UInt32)sampleRate;
		mFmt_.BytesPerSec = (UInt32)(sampleRate * channels * bits / 8);
		mFmt_.BytesPerSample = (UInt16)(channels * bits / 8);
		mFmt_.Bits = (UInt16)bits;

		byte[] bytes;
		IntPtr ptr = IntPtr.Zero;

		// RIFF
		bytes = new byte[Marshal.SizeOf(mRiff)];
		ptr = Marshal.AllocHGlobal(bytes.Length);
		Marshal.StructureToPtr(mRiff, ptr, false);
		Marshal.Copy(ptr, bytes, 0, bytes.Length);
		Marshal.FreeHGlobal(ptr);
		mFs.Write(bytes, 0, bytes.Length);

		// fmt_
		bytes = new byte[Marshal.SizeOf(mFmt_)];
		ptr = Marshal.AllocHGlobal(bytes.Length);
		Marshal.StructureToPtr(mFmt_, ptr, false);
		Marshal.Copy(ptr, bytes, 0, bytes.Length);
		Marshal.FreeHGlobal(ptr);
		mFs.Write(bytes, 0, bytes.Length);

		// data
		mFs.Write(SYMBOL_DATA, 0, SYMBOL_DATA.Length);
		mFs.Write(SYMBOL_0000, 0, SYMBOL_0000.Length);
	}

	public void Write(ref short[] buff) {
		if(0 != buff.Length % mFmt_.Channels) {
			return;
		}

		fixed (short* ptr = &buff[0])
		{
			var bytes = new byte[2 * buff.Length];
			Marshal.Copy((IntPtr)ptr, bytes, 0, bytes.Length);
			mFs.Write(bytes, 0, bytes.Length);
			mSamples += (UInt32)bytes.Length / mFmt_.BytesPerSample;
		}
	}

	public void Close() {
		if (mIsWriteMode) {
			mRiff.Size = mFmt_.BytesPerSample * mSamples + 36;

			mFs.Position = 0;

			// RIFF
			var bytes = new byte[Marshal.SizeOf(mRiff)];
			var ptr = Marshal.AllocHGlobal(bytes.Length);
			Marshal.StructureToPtr(mRiff, ptr, false);
			Marshal.Copy(ptr, bytes, 0, bytes.Length);
			Marshal.FreeHGlobal(ptr);
			mFs.Write(bytes, 0, bytes.Length);

			// fmt_
			bytes = new byte[Marshal.SizeOf(mFmt_)];
			ptr = Marshal.AllocHGlobal(bytes.Length);
			Marshal.StructureToPtr(mFmt_, ptr, false);
			Marshal.Copy(ptr, bytes, 0, bytes.Length);
			Marshal.FreeHGlobal(ptr);
			mFs.Write(bytes, 0, bytes.Length);

			// data
			bytes = BitConverter.GetBytes(mFmt_.BytesPerSample * mSamples);
			mFs.Write(SYMBOL_DATA, 0, SYMBOL_DATA.Length);
			mFs.Write(bytes, 0, bytes.Length);
		}

		mFs.Close();
		mFs.Dispose();
	}
}
