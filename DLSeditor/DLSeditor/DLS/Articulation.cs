using System;
using System.IO;

namespace DLS
{
	unsafe public class LART
	{
		public ART1 Art;

		public LART()
		{
			Art = new ART1();
		}

		public LART(byte* buff, UInt32 termAddr)
		{
			Art = new ART1();

			while ((UInt32)buff < termAddr)
			{
				var chunkType = *(CHUNK_TYPE*)buff; buff += 4;
				var chunkSize = *(UInt32*)buff; buff += 4;

				switch (chunkType)
				{
					case CHUNK_TYPE.ART1:
					case CHUNK_TYPE.ART2:
						Art = new ART1(buff);
						break;
					default:
						throw new Exception();
				}

				buff += chunkSize;
			}
		}

		public byte[] Bytes
		{
			get
			{
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms))
				{
					var artb = Art.Bytes;
					bw.Write((UInt32)CHUNK_TYPE.ART1);
					bw.Write((UInt32)artb.Length);
					bw.Write(artb);
					return ms.ToArray();
				}
			}
		}
	}

	unsafe public class ART1 : LIST<CONN>
	{
		private UInt32 mChunkSize;
		private UInt32 mListCount;

		public ART1()
		{
			mChunkSize = 8;
			mListCount = 0;
		}

		public ART1(byte* buff)
		{
			mChunkSize = *(UInt32*)buff; buff += 4;
			mListCount = *(UInt32*)buff; buff += 4;
			for (var i = 0; i < mListCount; ++i)
			{
				Add(new CONN(buff));
				buff += (UInt32)sizeof(CONN);
			}
		}

		public byte[] Bytes
		{
			get
			{
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms))
				{
					bw.Write((UInt32)8);
					bw.Write(List.Count);
					foreach (var conn in List)
					{
						bw.Write(conn.Bytes);
					}
					return ms.ToArray();
				}
			}
		}
	}

	unsafe public struct CONN
	{
		private UInt16 mSource;
		private UInt16 mControl;
		private UInt16 mDestination;
		private UInt16 mTransform;
		private Int32 mScale;

		public CONN(byte* buff)
		{
			mSource      = *(UInt16*)buff; buff += 2;
			mControl     = *(UInt16*)buff; buff += 2;
			mDestination = *(UInt16*)buff; buff += 2;
			mTransform   = *(UInt16*)buff; buff += 2;
			mScale       = *(Int32*)buff;
		}

		#region プロパティ
		public CONN_SRC_TYPE Source
		{
			get { return (CONN_SRC_TYPE)mSource; }
			set { mSource = (UInt16)value; }
		}

		public CONN_SRC_TYPE Control
		{
			get { return (CONN_SRC_TYPE)mControl; }
			set { mControl = (UInt16)value; }
		}

		public CONN_DST_TYPE Destination
		{
			get { return (CONN_DST_TYPE)mDestination; }
			set { mDestination = (UInt16)value; }
		}

		public CONN_TRN_TYPE TRN_SRC
		{
			get { return (CONN_TRN_TYPE)((mTransform >> 10) & 0x000F); }
			set { mTransform |= (UInt16)(((UInt16)value & 0x000F) << 10); }
		}

		public CONN_TRN_TYPE TRN_CTRL
		{
			get { return (CONN_TRN_TYPE)((mTransform >> 4) & 0x000F); }
			set { mTransform |= (UInt16)(((UInt16)value & 0x000F) << 4); }
		}

		public CONN_TRN_TYPE TRN_DST
		{
			get { return (CONN_TRN_TYPE)(mTransform & 0x000F); }
			set { mTransform |= (UInt16)((UInt16)value & 0x000F); }
		}

		public double Value
		{
			get
			{
				switch (Destination)
				{
					case CONN_DST_TYPE.ATTENUATION:
					case CONN_DST_TYPE.FILTER_Q:
						return Math.Pow(10.0, mScale / (200 * 65536.0));
					case CONN_DST_TYPE.PAN:
						return (mScale / 655360.0) - 0.5;
					case CONN_DST_TYPE.LFO_START_DELAY:
					case CONN_DST_TYPE.VIB_START_DELAY:
					case CONN_DST_TYPE.EG1_ATTACK_TIME:
					case CONN_DST_TYPE.EG1_DECAY_TIME:
					case CONN_DST_TYPE.EG1_RELEASE_TIME:
					case CONN_DST_TYPE.EG1_DELAY_TIME:
					case CONN_DST_TYPE.EG1_HOLD_TIME:
					case CONN_DST_TYPE.EG1_SHUTDOWN_TIME:
					case CONN_DST_TYPE.EG2_ATTACK_TIME:
					case CONN_DST_TYPE.EG2_DECAY_TIME:
					case CONN_DST_TYPE.EG2_RELEASE_TIME:
					case CONN_DST_TYPE.EG2_DELAY_TIME:
					case CONN_DST_TYPE.EG2_HOLD_TIME:
						return Math.Pow(2.0, mScale / (1200 * 65536.0));
					case CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
					case CONN_DST_TYPE.EG2_SUSTAIN_LEVEL:
						return mScale / 655360.0;
					case CONN_DST_TYPE.PITCH:
					case CONN_DST_TYPE.LFO_FREQUENCY:
					case CONN_DST_TYPE.VIB_FREQUENCY:
					case CONN_DST_TYPE.FILTER_CUTOFF:
						return Math.Pow(2.0, (mScale / 65536.0 - 6900) / 1200.0) * 440;
					default:
						return 0.0;
				}
			}

			set
			{
				switch (Destination)
				{
					case CONN_DST_TYPE.ATTENUATION:
					case CONN_DST_TYPE.FILTER_Q:
						mScale = (Int32)(Math.Log10(value) * 200 * 65536);
						break;
					case CONN_DST_TYPE.PAN:
						mScale = (Int32)((value + 0.5) * 655360);
						break;
					case CONN_DST_TYPE.LFO_START_DELAY:
					case CONN_DST_TYPE.VIB_START_DELAY:
					case CONN_DST_TYPE.EG1_ATTACK_TIME:
					case CONN_DST_TYPE.EG1_DECAY_TIME:
					case CONN_DST_TYPE.EG1_RELEASE_TIME:
					case CONN_DST_TYPE.EG1_DELAY_TIME:
					case CONN_DST_TYPE.EG1_HOLD_TIME:
					case CONN_DST_TYPE.EG1_SHUTDOWN_TIME:
					case CONN_DST_TYPE.EG2_ATTACK_TIME:
					case CONN_DST_TYPE.EG2_DECAY_TIME:
					case CONN_DST_TYPE.EG2_RELEASE_TIME:
					case CONN_DST_TYPE.EG2_DELAY_TIME:
					case CONN_DST_TYPE.EG2_HOLD_TIME:
						mScale = (Int32)(Math.Log(value, 2.0) * 1200 * 65536);
						break;
					case CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
					case CONN_DST_TYPE.EG2_SUSTAIN_LEVEL:
						mScale = (Int32)(value * 655360);
						break;
					case CONN_DST_TYPE.PITCH:
					case CONN_DST_TYPE.LFO_FREQUENCY:
					case CONN_DST_TYPE.VIB_FREQUENCY:
					case CONN_DST_TYPE.FILTER_CUTOFF:
						mScale = (Int32)(((Math.Log(value / 440, 2.0) * 1200) + 6900) * 65536);
						break;
					default:
						mScale = 0;
						break;
				}
			}
		}

		public byte[] Bytes
		{
			get
			{
				var buff = new byte[12];
				byte* pBuff;
				fixed (byte* p = &buff[0]) pBuff = p;

				*(UInt16*)pBuff = mSource;		pBuff += 2;
				*(UInt16*)pBuff = mControl;		pBuff += 2;
				*(UInt16*)pBuff = mDestination;	pBuff += 2;
				*(UInt16*)pBuff = mTransform;	pBuff += 2;
				*(Int32*)pBuff = mScale;

				return buff;
			}
		}
		#endregion
	}
}
