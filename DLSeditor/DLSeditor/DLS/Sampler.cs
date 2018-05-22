﻿using System;
using System.IO;

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
			mChunkSize   = *(UInt32*)buff; buff += 4;
			UnityNote    = *(UInt16*)buff; buff += 2;
			FineTune     = *(Int16*)buff;  buff += 2;
			mAttenuation = *(Int32*)buff;  buff += 4;
			mOptions     = *(UInt32*)buff; buff += 4;
			mLoops       = *(UInt32*)buff; buff += 4;
			for (var i = 0; i < mLoops; ++i)
			{
				Add(new LOOP(buff));
				buff += (UInt32)sizeof(LOOP);
			}
		}

		#region プロパティ
		public double Attenuation
		{
			get { return Math.Pow(10.0, mAttenuation / (200 * 65536.0)); }
			set { mAttenuation = (Int32)(Math.Log10(value) * 200 * 65536); }
		}

		public byte[] Bytes
		{
			get
			{
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms))
				{
					bw.Write(mChunkSize);
					bw.Write(UnityNote);
					bw.Write(FineTune);
					bw.Write(mAttenuation);
					bw.Write(mOptions);
					bw.Write((UInt32)List.Count);
					foreach(var loop in List)
					{
						bw.Write(loop.Bytes);
					}
					return ms.ToArray();
				}
			}
		}
		#endregion
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

		public byte[] Bytes
		{
			get
			{
				var buff = new byte[16];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt32*)pBuff = Size;		pBuff += 4;
				*(UInt32*)pBuff = Type;		pBuff += 4;
				*(UInt32*)pBuff = Begin;	pBuff += 4;
				*(UInt32*)pBuff = Length;

				return buff;
			}
		}
	}
}
