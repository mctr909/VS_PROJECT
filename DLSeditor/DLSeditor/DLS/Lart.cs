using System;

namespace DLS
{
	unsafe public struct CONN
	{
		public CONN_SRC_TYPE Source;
		public CONN_SRC_TYPE Control;
		public CONN_DST_TYPE Destination;
		public CONN_TRN_TYPE Transform;
		private Int32 Scale;

		public CONN(byte* ptr)
		{
			Source = *(CONN_SRC_TYPE*)ptr;
			ptr += sizeof(CONN_SRC_TYPE);
			Control = *(CONN_SRC_TYPE*)ptr;
			ptr += sizeof(CONN_SRC_TYPE);
			Destination = *(CONN_DST_TYPE*)ptr;
			ptr += sizeof(CONN_DST_TYPE);
			Transform = *(CONN_TRN_TYPE*)ptr;
			ptr += sizeof(CONN_TRN_TYPE);
			Scale = *(Int32*)ptr;
		}

		public double Value
		{
			get {
				switch (Destination) {
				case CONN_DST_TYPE.ATTENUATION:
				case CONN_DST_TYPE.FILTER_Q:
					return Math.Pow(10.0, Scale / (200 * 65536.0));
				case CONN_DST_TYPE.PAN:
					return (Scale / 655360.0) - 0.5;
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
					return Math.Pow(2.0, Scale / (1200 * 65536.0));
				case CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
				case CONN_DST_TYPE.EG2_SUSTAIN_LEVEL:
					return Scale / 655360.0;
				case CONN_DST_TYPE.PITCH:
				case CONN_DST_TYPE.LFO_FREQUENCY:
				case CONN_DST_TYPE.VIB_FREQUENCY:
				case CONN_DST_TYPE.FILTER_CUTOFF:
					return Math.Pow(2.0, (Scale / 65536.0 - 6900) / 1200.0) * 440;
				default:
					return 0.0;
				}
			}

			set {
				switch (Destination) {
				case CONN_DST_TYPE.ATTENUATION:
				case CONN_DST_TYPE.FILTER_Q:
					Scale = (Int32)(Math.Log10(value) * 200 * 65536);
					break;
				case CONN_DST_TYPE.PAN:
					Scale = (Int32)((value + 0.5) * 655360);
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
					Scale = (Int32)(Math.Log(value, 2.0) * 1200 * 65536);
					break;
				case CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
				case CONN_DST_TYPE.EG2_SUSTAIN_LEVEL:
					Scale = (Int32)(value * 655360);
					break;
				case CONN_DST_TYPE.PITCH:
				case CONN_DST_TYPE.LFO_FREQUENCY:
				case CONN_DST_TYPE.VIB_FREQUENCY:
				case CONN_DST_TYPE.FILTER_CUTOFF:
					Scale = (Int32)(((Math.Log(value / 440, 2.0) * 1200) + 6900) * 65536);
					break;
				default:
					Scale = 0;
					break;
				}
			}
		}
	}

	unsafe public struct ART
	{
		public UInt32 Size;
		public UInt32 ConnectionCount;
		public CONN[] Connections;

		public ART(byte* ptr)
		{
			Size = *(UInt32*)ptr;
			ptr += 4;
			ConnectionCount = *(UInt32*)ptr;
			ptr += 4;

			Connections = new CONN[ConnectionCount];
			for (var i = 0; i < ConnectionCount; ++i) {
				Connections[i] = new CONN(ptr);
				ptr += sizeof(CONN);
			}
		}
	}

	unsafe public class LART : List<ART>
	{
		public LART() {}

		public LART(byte* ptr, UInt32 endAddr)
		{
			while ((UInt32)ptr < endAddr) {
				var chunkType = *(ChunkID*)ptr;
				ptr += 4;
				var chunkSize = *(UInt32*)ptr;
				ptr += 4;

				switch (chunkType) {
				case ChunkID.ART1:
				case ChunkID.ART2:
					Add(new ART(ptr));
					break;
				default:
					throw new Exception();
				}

				ptr += chunkSize;
			}
		}
	}
}
