#include "main.h"
#include <math.h>
#include <stdlib.h>

// ============================================================================================
// このVSTのを生成するための関数
// ============================================================================================
AudioEffect* createEffectInstance(audioMasterCallback audioMaster)
{
	//newでこのVSTを生成したポインタを返す
	return new VST1(audioMaster);
}

// ============================================================================================
// コンストラクタ(VSTの初期化)
// ============================================================================================
VST1::VST1(audioMasterCallback audioMaster)
	: AudioEffectX(audioMaster, 1, 0)
{
	//以下の関数を呼び出して入力数、出力数等の情報を設定する。
	//必ず呼び出さなければならない。
	setNumInputs(2);		//入力数。モノラル入力=1、ステレオ入力=2
	setNumOutputs(2);		//出力数。モノラル出力=1、ステレオ出力=2
	setUniqueID('SMPL');	//ユニークIDの設定
							//公開する場合は以下URLで発行されたユニークIDを入力する。
							//http://ygrabit.steinberg.de/~ygrabit/public_html/index.html

	//このVSTがSynthかどうかのフラグを設定。Synthの場合…true、Effectorの場合…false
	isSynth(true);

	//このVSTが音声処理可能かどうかのフラグを設定。音声処理を行わないVSTはないので必ずこの関数を呼び出す。
	canProcessReplacing();

	//上記の関数を呼び出した後に初期化を行う
	m_MidiMsgNum = 0;
	memset(m_MidiMsgList, 0, sizeof(MidiMessage) * MIDIMSG_MAXNUM);

	memset(channel, 0, sizeof(Channel) * OSC_MAXNUM);
	for (int ch = 0; ch < 16; ch++) {
		clearChannel(&channel[ch]);
	}

	memset(osc, 0, sizeof(Osc) * OSC_MAXNUM);
	for (int oscCnt = 0; oscCnt < OSC_MAXNUM; oscCnt++) {
		clearOsc(&osc[oscCnt]);
	}
}

// ============================================================================================
// チャンネルのクリア
// ============================================================================================
void VST1::clearChannel(Channel *channel)
{
	channel->volume = 127;
	channel->expression = 127;
	channel->pan = 64;
	channel->program = 0;
	channel->pitch = 1.0F;
	channel->bendWidth = 2;
	channel->rpnMsb = -1;
	channel->rpnLsb = -1;
}

// ============================================================================================
// 発振器のクリア
// ============================================================================================
void VST1::clearOsc(Osc *osc)
{
	osc->channel = 0;
	osc->noteNo = -1;
	osc->velocity = 0;

	osc->amp = 0.0f;
	osc->env = 0.0f;
	osc->freq = 0.0f;
	osc->counter = 0.0f;
}

// ============================================================================================
// 発振器
// ============================================================================================
float VST1::sqr50(Osc *osc)
{
	return osc->counter < SAMPLE_RATE / 2 ? 1 : -1;
}
float VST1::sqr25(Osc *osc)
{
	return osc->counter < SAMPLE_RATE / 4 ? 1 : -1;
}
float VST1::sqr12(Osc *osc)
{
	return osc->counter < SAMPLE_RATE / 8 ? 1 : -1;
}
float VST1::tri(Osc *osc)
{
	float a = 32.0 * osc->counter / SAMPLE_RATE;
	if (a < 8.0)   return (int)(a - 0.5) / 4.0;
	else if (a < 24.0)	return (int)(16.0 - a - 0.5) / 4.0;
	else				return (int)(a - 32.0 - 0.5) / 4.0;
}
float VST1::nois(Osc *osc)
{
	return 2048 * (1.0 / 1024.0 - 128 * rand() / 2147483647.0);
}

// ============================================================================================
// MIDIメッセージの読み取り
// ============================================================================================
void VST1::readMidiMsg(MidiMessage *midiMsg, Channel *channel, Osc *osc)
{
	int oscCnt = 0;

	//*** NoteOff ***//
	if (midiMsg->message == 0x80) {
		for (oscCnt = 0; oscCnt < OSC_MAXNUM; ++oscCnt) {
			if (osc[oscCnt].channel == midiMsg->channel
				&& osc[oscCnt].noteNo == midiMsg->data1) {
				osc[oscCnt].env = 1.0f - 100.0f / SAMPLE_RATE;
			}
		}
	}

	//*** NoteOn ***//
	if (midiMsg->message == 0x90) {
		for (oscCnt = 0; oscCnt < OSC_MAXNUM; ++oscCnt) {
			if (osc[oscCnt].noteNo < 0) {
				osc[oscCnt].channel = midiMsg->channel;
				osc[oscCnt].noteNo = midiMsg->data1;
				osc[oscCnt].velocity = midiMsg->data2;

				osc[oscCnt].amp = osc[oscCnt].velocity / 127.0f;
				osc[oscCnt].env = 1.0f - 0.25f / SAMPLE_RATE;
				osc[oscCnt].freq = MASTER_PITCH * pow(2.0, osc[oscCnt].noteNo / 12.0);
				osc[oscCnt].counter = 0.0f;

				break;
			}
		}
	}

	//*** C.C. ***//
	if (midiMsg->message == 0xB0)
	{
		// DataEntry
		if (midiMsg->data1 == 0x6)
		{
			// RPN BendWidth
			if (channel[midiMsg->channel].rpnMsb == 0 && channel[midiMsg->channel].rpnLsb == 0)
			{
				channel[midiMsg->channel].bendWidth = midiMsg->data2;
			}

			// RPN Reset
			if (channel[midiMsg->channel].rpnMsb >= 0 || channel[midiMsg->channel].rpnLsb >= 0)
			{
				channel[midiMsg->channel].rpnMsb = -1;
				channel[midiMsg->channel].rpnLsb = -1;
			}
		}
		// Volume
		if (midiMsg->data1 == 0x7)
		{
			channel[midiMsg->channel].volume = midiMsg->data2;
		}
		// Pan
		if (midiMsg->data1 == 0xA)
		{
			channel[midiMsg->channel].pan = midiMsg->data2;
		}
		// Expression
		if (midiMsg->data1 == 0xB)
		{
			channel[midiMsg->channel].expression = midiMsg->data2;
		}

		// RPN MSB
		if (midiMsg->data1 == 0x65)
		{
			channel[midiMsg->channel].rpnMsb = midiMsg->data2;
		}
		// RPN LSB
		if (midiMsg->data1 == 0x64)
		{
			channel[midiMsg->channel].rpnLsb = midiMsg->data2;
		}
	}

	//*** Program ***//
	if (midiMsg->message == 0xC0)
	{
		channel[midiMsg->channel].program = midiMsg->data1;
	}

	//*** Pitch ***//
	if (midiMsg->message == 0xE0)
	{
		channel[midiMsg->channel].pitch = midiMsg->data1 | midiMsg->data2 << 7;
		channel[midiMsg->channel].pitch -= 8192;
		channel[midiMsg->channel].pitch = pow(2.0, channel[midiMsg->channel].bendWidth * channel[midiMsg->channel].pitch / 98304.0);
	}
}

// ============================================================================================
// MIDIメッセージを処理するメンバー関数
// processReplacing()の前に必ず1度だけ呼び出される。
// ============================================================================================
VstInt32 VST1::processEvents(VstEvents* events)
{
	// MIDIのリストを初期化
	m_MidiMsgNum = 0;
	memset(m_MidiMsgList, 0, sizeof(MidiMessage) * MIDIMSG_MAXNUM);

	// VSTイベントの回数だけループをまわす。
	int loops = (events->numEvents);
	for (int i = 0; i < loops; i++)
	{
		// 与えられたイベントがMIDIならばmidimsgbufにストックする
		if ((events->events[i])->type == kVstMidiType)
		{
			VstMidiEvent *midievent = (VstMidiEvent*)(events->events[i]);

			m_MidiMsgList[m_MidiMsgNum].deltaFrames = midievent->deltaFrames;
			m_MidiMsgList[m_MidiMsgNum].message = midievent->midiData[0] & 0xF0;	// MIDIメッセージ
			m_MidiMsgList[m_MidiMsgNum].channel = midievent->midiData[0] & 0x0F;	// MIDIチャンネル
			m_MidiMsgList[m_MidiMsgNum].data1 = midievent->midiData[1];				// MIDIデータ1
			m_MidiMsgList[m_MidiMsgNum].data2 = midievent->midiData[2];				// MIDIデータ2
			m_MidiMsgNum++;

			// MIDIメッセージのバッファがいっぱいの場合はループを打ち切る。
			if (i >= MIDIMSG_MAXNUM)
			{
				break;
			}
		}
	}

	return 1;
}

// ============================================================================================
// 音声信号を処理するメンバー関数
// ============================================================================================
void VST1::processReplacing(float** inputs, float** outputs, VstInt32 sampleFrames)
{
	//入力、出力は2次元配列で渡される。
	//入力は-1.0f～1.0fの間で渡される。
	//出力は-1.0f～1.0fの間で書き込む必要がある。
	//sampleFramesが処理するバッファのサイズ
	float* outL = outputs[0];	//出力 左用
	float* outR = outputs[1];	//出力 右用

	int midiMsgCur = 0;			// midieventlistの読み込み位置
	int oscCnt = 0;
	float wave = 0.0f;

	for (int i = 0; i < sampleFrames; ++i)
	{
		//ここで音声処理を行う。

		// MIDIメッセージがあるか確認
		if (m_MidiMsgNum > 0)
		{
			// MIDIメッセージを処理するタイミングかどうかを確認する。
			if (m_MidiMsgList[midiMsgCur].deltaFrames <= i)
			{
				// MIDIメッセージの読み取り
				readMidiMsg(&m_MidiMsgList[midiMsgCur], channel, osc);

				// midimsgbufからMIDIメッセージを読み出したので
				// 読み込み位置を進め、MIDIメッセージの数を減らす
				m_MidiMsgNum--;
				midiMsgCur++;
			}
		}

		//出力バッファへ書き込む。
		outL[i] = 0.0f;
		outR[i] = 0.0f;
		for (oscCnt = 0; oscCnt < OSC_MAXNUM; ++oscCnt)
		{

			if (osc[oscCnt].channel == 0)
			{
				wave = sqr12(&osc[oscCnt]);
			}
			else if (osc[oscCnt].channel == 1)
			{
				wave = sqr25(&osc[oscCnt]);
			}
			else if (osc[oscCnt].channel == 2)
			{
				wave = sqr50(&osc[oscCnt]);
			}
			else if (osc[oscCnt].channel == 9)
			{
				wave = nois(&osc[oscCnt]);
			}
			else
			{
				wave = tri(&osc[oscCnt]);
			}

			wave *= 0.25
				* channel[osc[oscCnt].channel].volume
				* channel[osc[oscCnt].channel].expression
				* osc[oscCnt].amp
				/ (127 * 127);

			outL[i] += wave * (1.0 - channel[osc[oscCnt].channel].pan / 128.0);
			outR[i] += wave * (channel[osc[oscCnt].channel].pan / 128.0);

			osc[oscCnt].counter += osc[oscCnt].freq * channel[osc[oscCnt].channel].pitch;
			if (osc[oscCnt].counter >= SAMPLE_RATE) osc[oscCnt].counter -= SAMPLE_RATE;

			osc[oscCnt].amp *= osc[oscCnt].env;
			if (osc[oscCnt].amp < 1.0 / 1024.0) {
				clearOsc(&osc[oscCnt]);
			}
		}
	}
}
