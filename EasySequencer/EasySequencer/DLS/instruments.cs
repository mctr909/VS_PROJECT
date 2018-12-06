using System;
using System.Collections.Generic;

namespace DLS {
	unsafe public class Instruments {
		public Dictionary<MIDI.INST_ID, MIDI.WaveInfo[]> List;

		public Instruments(IntPtr dlsPtr, uint dlsSize, int sampleRate) {
			var dls = new File(dlsPtr, dlsSize);
			var deltaTime = 1.0 / sampleRate;

			List = new Dictionary<MIDI.INST_ID, MIDI.WaveInfo[]>();

			foreach (var inst in dls.instruments.List) {
				var envAmp = new MIDI.Envelope();
				if (null != inst.articulations) {
					envAmp.attackTime = 1.0;
					envAmp.decayTime = 1.0;
					envAmp.releaceTime = 1.0;
					envAmp.sustainLevel = 1.0;
					envAmp.holdTime = 0.0;
					var holdTime = 1.0;
					foreach (var conn in inst.articulations.art.List) {
						if (SRC_TYPE.NONE != conn.source)
							continue;
						switch (conn.destination) {
						case DST_TYPE.EG1_ATTACK_TIME:
							envAmp.attackTime = ART.GetValue(conn);
							holdTime += ART.GetValue(conn);
							break;
						case DST_TYPE.EG1_DECAY_TIME:
							envAmp.decayTime = ART.GetValue(conn);
							break;
						case DST_TYPE.EG1_RELEASE_TIME:
							envAmp.releaceTime = ART.GetValue(conn);
							break;
						case DST_TYPE.EG1_SUSTAIN_LEVEL:
							envAmp.sustainLevel = (0.0 == ART.GetValue(conn)) ? 1.0 : (ART.GetValue(conn) * 0.01);
							break;
						case DST_TYPE.EG1_HOLD_TIME:
							holdTime += ART.GetValue(conn);
							break;
						}
					}
					envAmp.holdTime += holdTime;
				}

				var waveInfo = new MIDI.WaveInfo[128];
				for (var noteNo = 0; noteNo < waveInfo.Length; ++noteNo) {
					RGN_ region = null;
					foreach (var rgn in inst.regions.List) {
						if (rgn.pHeader->key.low <= noteNo && noteNo <= rgn.pHeader->key.high) {
							region = rgn;
							break;
						}
					}

					if (null == region) {
						continue;
					}

					if (null != region.articulations) {
						envAmp.attackTime = 1.0;
						envAmp.decayTime = 1.0;
						envAmp.releaceTime = 1.0;
						envAmp.sustainLevel = 1.0;
						envAmp.holdTime = 0.0;
						var holdTime = 1.0;
						foreach (var conn in region.articulations.art.List) {
							if (SRC_TYPE.NONE != conn.source)
								continue;
							switch (conn.destination) {
							case DST_TYPE.EG1_ATTACK_TIME:
								envAmp.attackTime = ART.GetValue(conn);
								holdTime += ART.GetValue(conn);
								break;
							case DST_TYPE.EG1_DECAY_TIME:
								envAmp.decayTime = ART.GetValue(conn);
								break;
							case DST_TYPE.EG1_RELEASE_TIME:
								envAmp.releaceTime = ART.GetValue(conn);
								break;
							case DST_TYPE.EG1_SUSTAIN_LEVEL:
								envAmp.sustainLevel = (0.0 == ART.GetValue(conn)) ? 1.0 : (ART.GetValue(conn) * 0.01);
								break;
							case DST_TYPE.EG1_HOLD_TIME:
								holdTime += ART.GetValue(conn);
								break;
							}
						}
						envAmp.holdTime += holdTime;
					}

					waveInfo[noteNo].envAmp = envAmp;
					var wave = dls.wavePool.List[(int)region.pWaveLink->tableIndex];

					if (0 < wave.pSampler->loopCount) {
						waveInfo[noteNo].loop.start = wave.pLoops[0].start;
						waveInfo[noteNo].loop.length = wave.pLoops[0].length;
						waveInfo[noteNo].loopEnable = true;
					}
					else {
						waveInfo[noteNo].loop.start = 0;
						waveInfo[noteNo].loop.length = wave.pLoops->length;
						waveInfo[noteNo].loopEnable = false;
					}

					if (null == region.pSampler) {
						waveInfo[noteNo].unityNote = (byte)wave.pSampler->unityNote;
						waveInfo[noteNo].delta
							= Math.Pow(2.0, wave.pSampler->fineTune / 1200.0)
							* wave.pFormat->sampleRate / sampleRate
						;
						waveInfo[noteNo].gain = wave.pSampler->Gain;
					}
					else {
						waveInfo[noteNo].unityNote = (byte)region.pSampler->unityNote;
						waveInfo[noteNo].delta
							= Math.Pow(2.0, region.pSampler->fineTune / 1200.0)
							* wave.pFormat->sampleRate / sampleRate
						;
						waveInfo[noteNo].gain = region.pSampler->Gain;
					}

					waveInfo[noteNo].pcmAddr = wave.pcmAddr - (uint)dlsPtr.ToInt32();
					waveInfo[noteNo].pcmLength = wave.dataSize / wave.pFormat->blockAlign;
				}

				var id = new MIDI.INST_ID();
				id.isDrum = inst.pHeader->locale.bankFlags;
				id.programNo = inst.pHeader->locale.programNo;
				id.bankMSB = inst.pHeader->locale.bankMSB;
				id.bankLSB = inst.pHeader->locale.bankLSB;

				List.Add(id, waveInfo);
			}
		}
	}
}