using System;

namespace DLS
{
	unsafe public class WSMP : LIST<LOOP>
	{
		private UInt32 mChunkSize;
		public UInt16 UnityNote;
		public Int16 FineTune;
		private Int32 mAttenuation;
		private UInt32 mOptions;
		private UInt32 mLoops;

		public WSMP()
		{
			mChunkSize = 20;
			UnityNote = 64;
			FineTune = 0;
			mAttenuation = 0;
			mOptions = 0;
			mLoops = 0;
		}

		public WSMP(byte* buff)
		{
			mChunkSize = *(UInt32*)buff; buff += 4;
			UnityNote = *(UInt16*)buff; buff += 2;
			FineTune = *(Int16*)buff; buff += 2;
			mAttenuation = *(Int32*)buff; buff += 4;
			mOptions = *(UInt32*)buff; buff += 4;
			mLoops = *(UInt32*)buff; buff += 4;
			for (var i = 0; i < mLoops; ++i) {
				Add(new LOOP(buff));
				buff += (UInt32)sizeof(LOOP);
			}
		}

		public double Gain
		{
			get { return Math.Pow(10.0, mAttenuation / (200 * 65536.0)); }
			set { mAttenuation = (Int32)(Math.Log10(value) * 200 * 65536); }
		}
	}

	unsafe public struct LOOP
	{
		public UInt32 Size;
		public UInt32 Type;
		public UInt32 Begin;
		public UInt32 Length;

		public LOOP(byte* buff)
		{
			Size   = *(UInt32*)buff;	buff += 4;
			Type   = *(UInt32*)buff;	buff += 4;
			Begin  = *(UInt32*)buff;	buff += 4;
			Length = *(UInt32*)buff;
		}
	}
}
