using System;
using System.Collections.Generic;
using System.IO;

namespace MIDI
{
	unsafe public class InstTable
	{
		public Dictionary<InstID, WaveInfo[]> InstList;

		public InstTable(string filePath)
		{
			var dls = new DLS.File(filePath);
			InstList = new Dictionary<InstID, WaveInfo[]>();

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
				var waveInfo = new WaveInfo[128];
				var id = new InstID(
					inst.InstHeader.ProgramNo,
					inst.InstHeader.BankMSB,
					inst.InstHeader.BankLSB,
					inst.InstHeader.IsDrum
				);
				
				for (var i = 0; i < waveInfo.Length; ++i) {
					foreach (var region in inst.RegionPool.List) {
						var waveIdx = (int)region.WaveLink.WaveIndex;
						if ((region.RegionHeader.KeyRangeLow <= i) && (i <= region.RegionHeader.KeyRangeHigh)) {
							waveInfo[i].BaseNote = (byte)region.Sampler.UnityNote;
							waveInfo[i].Delta
								= Math.Pow(2.0, region.Sampler.FineTune / 1200.0)
								* dls.WavePool[waveIdx].Format.SamplesPerSec
								/ 44100.0
							;

							waveInfo[i].Buff = waveList[waveIdx];
							if (0 < region.Sampler.List.Count) {
								waveInfo[i].LoopBegin = region.Sampler[0].Begin;
								waveInfo[i].LoopEnd = region.Sampler[0].Begin + region.Sampler[0].Length;
							}
							else {
								waveInfo[i].LoopBegin = 0;
								waveInfo[i].LoopEnd = (uint)waveList[waveIdx].Length;
							}

							break;
						}
					}
				}
				InstList.Add(id, waveInfo);
			}
		}
	}
}