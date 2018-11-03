﻿using System;
using System.Runtime.InteropServices;

unsafe public class WaveOutLib {
	[StructLayout(LayoutKind.Sequential)]
	private struct WAVEFORMATEX {
		public ushort wFormatTag;
		public ushort nChannels;
		public uint nSamplesPerSec;
		public uint nAvgBytesPerSec;
		public ushort nBlockAlign;
		public ushort wBitsPerSample;
		public ushort cbSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct WAVEHDR {
		public IntPtr lpData;
		public uint dwBufferLength;
		public uint dwBytesRecorded;
		public uint dwUser;
		public uint dwFlags;
		public uint dwLoops;
		public IntPtr lpNext;
		public uint reserved;
	}

	private enum MMRESULT {
		MMSYSERR_NOERROR = 0,
		MMSYSERR_ERROR = (MMSYSERR_NOERROR + 1),
		MMSYSERR_BADDEVICEID = (MMSYSERR_NOERROR + 2),
		MMSYSERR_NOTENABLED = (MMSYSERR_NOERROR + 3),
		MMSYSERR_ALLOCATED = (MMSYSERR_NOERROR + 4),
		MMSYSERR_INVALHANDLE = (MMSYSERR_NOERROR + 5),
		MMSYSERR_NODRIVER = (MMSYSERR_NOERROR + 6),
		MMSYSERR_NOMEM = (MMSYSERR_NOERROR + 7),
		MMSYSERR_NOTSUPPORTED = (MMSYSERR_NOERROR + 8),
		MMSYSERR_BADERRNUM = (MMSYSERR_NOERROR + 9),
		MMSYSERR_INVALFLAG = (MMSYSERR_NOERROR + 10),
		MMSYSERR_INVALPARAM = (MMSYSERR_NOERROR + 11),
		MMSYSERR_HANDLEBUSY = (MMSYSERR_NOERROR + 12),
		MMSYSERR_INVALIDALIAS = (MMSYSERR_NOERROR + 13),
		MMSYSERR_BADDB = (MMSYSERR_NOERROR + 14),
		MMSYSERR_KEYNOTFOUND = (MMSYSERR_NOERROR + 15),
		MMSYSERR_READERROR = (MMSYSERR_NOERROR + 16),
		MMSYSERR_WRITEERROR = (MMSYSERR_NOERROR + 17),
		MMSYSERR_DELETEERROR = (MMSYSERR_NOERROR + 18),
		MMSYSERR_VALNOTFOUND = (MMSYSERR_NOERROR + 19),
		MMSYSERR_NODRIVERCB = (MMSYSERR_NOERROR + 20),
		MMSYSERR_MOREDATA = (MMSYSERR_NOERROR + 21),
		MMSYSERR_LASTERROR = (MMSYSERR_NOERROR + 21)
	}

	private enum WaveOutMessage {
		Close = 0x3BC,
		Done = 0x3BD,
		Open = 0x3BB
	}

	private const uint WAVE_MAPPER = unchecked((uint)(-1));

	private delegate void DCallback(IntPtr hdrvr, WaveOutMessage uMsg, int dwUser, IntPtr wavhdr, int dwParam2);

	[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern MMRESULT waveOutOpen(ref IntPtr hWaveOut, uint uDeviceID, ref WAVEFORMATEX lpFormat, DCallback dwCallback, IntPtr dwInstance, uint dwFlags);

	[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern MMRESULT waveOutClose(IntPtr hwo);

	[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern MMRESULT waveOutPrepareHeader(IntPtr hWaveOut, IntPtr lpWaveOutHdr, int uSize);

	[DllImport("winmm.dll")]
	private static extern MMRESULT waveOutUnprepareHeader(IntPtr hWaveOut, IntPtr lpWaveOutHdr, int uSize);

	[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern MMRESULT waveOutReset(IntPtr hwo);

	[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern MMRESULT waveOutWrite(IntPtr hwo, IntPtr pwh, uint cbwh);

	private IntPtr mWaveOutHandle;
	private IntPtr[] mWaveHeaderPtr;
	private WAVEHDR[] mWaveHeader;
	private DCallback mCallback;
	private WAVEFORMATEX mWaveFormatEx;

	protected short[] WaveBuffer;
	private int mBufferIndex;

	public int SampleRate { get; }
	public int Channels { get; }
	public int BufferSize { get; }

	public WaveOutLib(int sampleRate = 44100, int channels = 2, int bufferSize = 512, int bufferCount = 16) {
		SampleRate = sampleRate;
		Channels = channels;
		BufferSize = bufferSize;
		mBufferIndex = 0;

		mWaveOutHandle = IntPtr.Zero;
		mWaveHeaderPtr = new IntPtr[bufferCount];
		mWaveHeader = new WAVEHDR[bufferCount];
		WaveBuffer = new short[BufferSize];

		WaveOutOpen();
		PrepareHeader();
	}

	private void WaveOutOpen() {
		if (IntPtr.Zero != mWaveOutHandle) {
			MMRESULT mr = waveOutReset(mWaveOutHandle);
			if (MMRESULT.MMSYSERR_NOERROR != mr) {
				throw new Exception(mr.ToString());
			}
			mr = waveOutClose(mWaveOutHandle);
			if (MMRESULT.MMSYSERR_NOERROR != mr) {
				throw new Exception(mr.ToString());
			}
			mWaveOutHandle = IntPtr.Zero;
		}

		mWaveFormatEx = new WAVEFORMATEX();
		mWaveFormatEx.wFormatTag = 1;
		mWaveFormatEx.nChannels = (ushort)Channels;
		mWaveFormatEx.nSamplesPerSec = (uint)SampleRate;
		mWaveFormatEx.nAvgBytesPerSec = (uint)(SampleRate * Channels * 16 >> 3);
		mWaveFormatEx.nBlockAlign = (ushort)(Channels * 16 >> 3);
		mWaveFormatEx.wBitsPerSample = 16;
		mWaveFormatEx.cbSize = 0;

		mCallback = new DCallback(Callback);
		waveOutOpen(ref mWaveOutHandle, WAVE_MAPPER, ref mWaveFormatEx, mCallback, IntPtr.Zero, 0x00030000);
	}

	private void PrepareHeader() {
		for (int i = 0; i < mWaveHeader.Length; ++i) {
			mWaveHeaderPtr[i] = Marshal.AllocHGlobal(Marshal.SizeOf(mWaveHeader[i]));
			mWaveHeader[i].dwBufferLength = (uint)(WaveBuffer.Length * 16 >> 3);
			mWaveHeader[i].lpData = Marshal.AllocHGlobal((int)mWaveHeader[i].dwBufferLength);
			mWaveHeader[i].dwFlags = 0;
			Marshal.Copy(WaveBuffer, 0, mWaveHeader[i].lpData, WaveBuffer.Length);
			Marshal.StructureToPtr(mWaveHeader[i], mWaveHeaderPtr[i], true);

			waveOutPrepareHeader(mWaveOutHandle, mWaveHeaderPtr[i], Marshal.SizeOf(typeof(WAVEHDR)));
			waveOutWrite(mWaveOutHandle, mWaveHeaderPtr[i], (uint)Marshal.SizeOf(typeof(WAVEHDR)));
		}
	}

	private void Callback(IntPtr hdrvr, WaveOutMessage uMsg, int dwUser, IntPtr waveHdr, int dwParam2) {
		switch (uMsg) {
		case WaveOutMessage.Close:
			break;

		case WaveOutMessage.Done: {
				waveOutWrite(mWaveOutHandle, waveHdr, (uint)Marshal.SizeOf(typeof(WAVEHDR)));

				for (mBufferIndex = 0; mBufferIndex < mWaveHeader.Length; ++mBufferIndex) {
					if (mWaveHeaderPtr[mBufferIndex] == waveHdr) {
						SetData();
						mWaveHeader[mBufferIndex] = (WAVEHDR)Marshal.PtrToStructure(mWaveHeaderPtr[mBufferIndex], typeof(WAVEHDR));
						Marshal.Copy(WaveBuffer, 0, mWaveHeader[mBufferIndex].lpData, WaveBuffer.Length);
						Marshal.StructureToPtr(mWaveHeader[mBufferIndex], mWaveHeaderPtr[mBufferIndex], true);
					}
				}
			}
			break;

		case WaveOutMessage.Open:
			break;
		}
	}

	protected virtual void SetData() { }
}