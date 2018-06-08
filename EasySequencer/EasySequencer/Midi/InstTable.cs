using System;
using System.Collections.Generic;
using System.IO;

namespace MIDI
{
	unsafe public class InstTable
	{
		public Dictionary<InstID, InstInfo> InstList;

		public InstTable(string filePath)
		{
			var dls = new DLS.File(filePath);
			InstList = new Dictionary<InstID, InstInfo>();

			var waveList = new Dictionary<int, short[]>();
			for (var i = 0; i < dls.WavePool.List.Count; ++i) {
				var temp = new MemoryStream(dls.WavePool[i].Samples);
				var br = new BinaryReader(temp);

				var tempBuff = new short[temp.Length >> 1];
				for (var j = 0; j < tempBuff.Length; ++j) {
					tempBuff[j] = br.ReadInt16();
				}
				waveList.Add(i, tempBuff);
			}

			foreach (var inst in dls.InstPool.List) {
				var id = new InstID(
					inst.InstHeader.ProgramNo,
					inst.InstHeader.BankMSB,
					inst.InstHeader.BankLSB,
					inst.InstHeader.IsDrum
				);

				var instInfo = new InstInfo();

				instInfo.EnvAmp.ALevel = 1.0;
				instInfo.EnvAmp.DLevel = 1.0;
				instInfo.EnvAmp.SLevel = 1.0;
				instInfo.EnvAmp.RLevel = 0.0;
				instInfo.EnvFilter.ALevel = 1.0;
				instInfo.EnvFilter.DLevel = 1.0;
				instInfo.EnvFilter.SLevel = 1.0;
				instInfo.EnvFilter.RLevel = 1.0;

				if (null != inst.ArtPool) {
					foreach (var art in inst.ArtPool.Art.List) {
						if (DLS.CONN_SRC_TYPE.NONE != art.Source) continue;
						switch (art.Destination) {
						case DLS.CONN_DST_TYPE.EG1_ATTACK_TIME:
							instInfo.EnvAmp.ALevel = 0.0;
							instInfo.EnvAmp.AttackTime = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_DECAY_TIME:
							instInfo.EnvAmp.DecayTime = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
							instInfo.EnvAmp.SLevel = (art.Value == 0.0) ? 1.0 : (art.Value / 100.0);
							break;
						case DLS.CONN_DST_TYPE.EG1_RELEASE_TIME:
							instInfo.EnvAmp.ReleaseTime = art.Value;
							break;
						}
					}
				}

				instInfo.WaveInfo = new WaveInfo[128];
				for (var i = 0; i < instInfo.WaveInfo.Length; ++i) {
					foreach (var region in inst.RegionPool.List) {
						var waveIdx = (int)region.WaveLink.WaveIndex;
						if ((region.RegionHeader.KeyRangeLow <= i) && (i <= region.RegionHeader.KeyRangeHigh)) {
							instInfo.WaveInfo[i].BaseNote = (byte)region.Sampler.UnityNote;
							instInfo.WaveInfo[i].Delta
								= Math.Pow(2.0, region.Sampler.FineTune / 1200.0)
								* dls.WavePool[waveIdx].Format.SamplesPerSec
								/ 44100.0
							;
							instInfo.WaveInfo[i].Gain = region.Sampler.Gain;

							instInfo.WaveInfo[i].Buff = waveList[waveIdx];
							if (0 < region.Sampler.List.Count) {
								instInfo.WaveInfo[i].LoopBegin = region.Sampler[0].Begin;
								instInfo.WaveInfo[i].LoopEnd = region.Sampler[0].Begin + region.Sampler[0].Length;
							}
							else {
								instInfo.WaveInfo[i].LoopBegin = 0;
								instInfo.WaveInfo[i].LoopEnd = (uint)waveList[waveIdx].Length;
							}
							break;
						}
					}
				}

				InstList.Add(id, instInfo);
			}
		}
	}
}