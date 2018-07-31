using System;
using System.Collections.Generic;
using System.IO;

namespace MIDI {
	unsafe public class Instruments {
		public Dictionary<InstID, WaveInfo[]> List;

		public Instruments(string filePath) {
			var dls = new DLS.File(filePath);
			List = new Dictionary<InstID, WaveInfo[]>();

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

				Envelope envAmp = new Envelope();
				Envelope envCutoff = new Envelope();
				if (null != inst.ArtPool) {
					envAmp.LevelA = 1.0;
					envAmp.LevelH = 1.0;
					envAmp.LevelS = 1.0;
					envAmp.LevelR = 0.0;
					envCutoff.LevelA = 1.0;
					envCutoff.LevelH = 1.0;
					envCutoff.LevelS = 1.0;
					envCutoff.LevelR = 1.0;
					foreach (var art in inst.ArtPool.Art.List) {
						if (DLS.CONN_SRC_TYPE.NONE != art.Source)
							continue;
						switch (art.Destination) {
						case DLS.CONN_DST_TYPE.EG1_ATTACK_TIME:
							envAmp.LevelA = 0.0;
							envAmp.AttackTime = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_HOLD_TIME:
							envAmp.TimeH = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_DECAY_TIME:
							envAmp.DecayTime = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
							envAmp.LevelS = (art.Value == 0.0) ? 1.0 : (art.Value / 100.0);
							break;
						case DLS.CONN_DST_TYPE.EG1_RELEASE_TIME:
							envAmp.ReleaseTime = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG2_DECAY_TIME:
							envCutoff.LevelS = 0.5;
							envCutoff.DecayTime = art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG2_RELEASE_TIME:
							envCutoff.LevelR = 0.5;
							envCutoff.ReleaseTime = art.Value;
							break;
						}
					}
				}

				WaveInfo[] waveInfo = new WaveInfo[128];
				for (var noteNo = 0; noteNo < waveInfo.Length; ++noteNo) {
					foreach (var region in inst.RegionPool.List) {
						if (null != region.ArtPool) {
							envAmp.LevelA = 1.0;
							envAmp.LevelH = 1.0;
							envAmp.LevelS = 1.0;
							envAmp.LevelR = 0.0;
							envCutoff.LevelA = 1.0;
							envCutoff.LevelH = 1.0;
							envCutoff.LevelS = 1.0;
							envCutoff.LevelR = 1.0;
							foreach (var art in region.ArtPool.Art.List) {
								if (DLS.CONN_SRC_TYPE.NONE != art.Source)
									continue;
								switch (art.Destination) {
								case DLS.CONN_DST_TYPE.EG1_ATTACK_TIME:
									envAmp.LevelA = 0.0;
									envAmp.AttackTime = art.Value;
									break;
								case DLS.CONN_DST_TYPE.EG1_HOLD_TIME:
									envAmp.TimeH = art.Value;
									break;
								case DLS.CONN_DST_TYPE.EG1_DECAY_TIME:
									envAmp.DecayTime = art.Value;
									break;
								case DLS.CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
									envAmp.LevelS = (art.Value == 0.0) ? 1.0 : (art.Value / 100.0);
									break;
								case DLS.CONN_DST_TYPE.EG1_RELEASE_TIME:
									envAmp.ReleaseTime = art.Value;
									break;
								case DLS.CONN_DST_TYPE.EG2_DECAY_TIME:
									envCutoff.LevelS = 0.5;
									envCutoff.DecayTime = art.Value;
									break;
								case DLS.CONN_DST_TYPE.EG2_RELEASE_TIME:
									envCutoff.LevelR = 0.5;
									envCutoff.ReleaseTime = art.Value;
									break;
								}
							}
						}

						waveInfo[noteNo].EnvAmp = envAmp;
						waveInfo[noteNo].EnvCutoff = envCutoff;

						var waveIdx = (int)region.WaveLink.WaveIndex;
						if ((region.RegionHeader.KeyRangeLow <= noteNo) && (noteNo <= region.RegionHeader.KeyRangeHigh)) {
							if (0 < region.Sampler.List.Count) {
								waveInfo[noteNo].LoopBegin = region.Sampler[0].Begin;
								waveInfo[noteNo].LoopEnd = region.Sampler[0].Begin + region.Sampler[0].Length;
								waveInfo[noteNo].LoopEnable = true;
							}
							else {
								waveInfo[noteNo].LoopBegin = 0;
								waveInfo[noteNo].LoopEnd = (uint)waveList[waveIdx].Length;
								waveInfo[noteNo].LoopEnable = false;
							}

							waveInfo[noteNo].BaseNoteNo = (byte)region.Sampler.UnityNote;
							waveInfo[noteNo].Delta
								= Math.Pow(2.0, region.Sampler.FineTune / 1200.0)
								* dls.WavePool[waveIdx].Format.SamplesPerSec
								/ Const.SampleRate
							;
							waveInfo[noteNo].Gain = region.Sampler.Gain;
							waveInfo[noteNo].Buff = waveList[waveIdx];

							break;
						}
					}
				}

				List.Add(id, waveInfo);
			}
		}
	}
}