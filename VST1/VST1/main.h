// ============================================================================================
// インクルードファイル
// ============================================================================================
#include "audioeffectx.h"

// ============================================================================================
// 定数
// ============================================================================================
#define SAMPLE_RATE		44100
#define MASTER_PITCH	8.1757989
#define OSC_MAXNUM		128
#define MIDIMSG_MAXNUM	256
#define PI2				6.283185307

// ============================================================================================
// 構造体
// ============================================================================================
struct MidiMessage
{
	VstInt32 deltaFrames;  //MIDIメッセージを処理するタイミング
	unsigned char message; //MIDIメッセージ番号
	unsigned char channel; //MIDIチャンネル
	unsigned char data1;   //MIDIデータ1
	unsigned char data2;   //MIDIデータ2
};
struct Channel
{
	unsigned char volume;
	unsigned char expression;
	unsigned char pan;
	unsigned char program;
	unsigned char bendWidth;

	signed char	rpnMsb;
	signed char	rpnLsb;
	
	float		pitch;
};
struct Osc
{
	char channel;
	char noteNo;
	unsigned char velocity;

	float		env;
	float		amp;
	float		freq;
	float		counter;
};

// ============================================================================================
// VSTの基本となるクラス
// ============================================================================================
class VST1 : public AudioEffectX
{
private:
	void clearChannel(Channel *channel);
	void clearOsc(Osc *osc);
	void readMidiMsg(MidiMessage *midiMsg, Channel *channel, Osc *osc);

	float sqr50(Osc *osc);
	float sqr25(Osc *osc);
	float sqr12(Osc *osc);
	float tri(Osc *osc);
	float nois(Osc *osc);

protected:
	int          m_MidiMsgNum;					// 受け取ったMIDIメッセージの数
	MidiMessage  m_MidiMsgList[MIDIMSG_MAXNUM];	// 受け取ったMIDIメッセージを保管するバッファ
	Channel      channel[16];					// MIDIチャンネルのバッファ
	Osc			 osc[OSC_MAXNUM];				// 発振器のバッファ

public:
	VST1(audioMasterCallback audioMaster);

	// MIDIメッセージをホストアプリケーションから受け取るためのメンバー関数
	VstInt32 processEvents(VstEvents* events);

	// 音声信号を処理するメンバー関数
	virtual void processReplacing(float** inputs, float** outputs, VstInt32 sampleFrames);
};