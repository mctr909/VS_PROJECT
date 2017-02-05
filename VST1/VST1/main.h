// ============================================================================================
// �C���N���[�h�t�@�C��
// ============================================================================================
#include "audioeffectx.h"

// ============================================================================================
// �萔
// ============================================================================================
#define SAMPLE_RATE		44100
#define MASTER_PITCH	8.1757989
#define OSC_MAXNUM		32
#define MIDIMSG_MAXNUM	4410

// ============================================================================================
// �\����
// ============================================================================================
typedef struct MidiMessage
{
	VstInt32 deltaFrames;  //MIDI���b�Z�[�W����������^�C�~���O
	unsigned char message; //MIDI���b�Z�[�W�ԍ�
	unsigned char channel; //MIDI�`�����l��
	unsigned char data1;   //MIDI�f�[�^1
	unsigned char data2;   //MIDI�f�[�^2
};

typedef struct ADSR
{
	float attackTime;
	float decayTime;
	float sustainTime;
	float releaseTime;

	float attackLevel;
	float decayLevel;
	float sustainLevel;
};

typedef struct Channel
{
	unsigned char volume;
	unsigned char expression;
	unsigned char pan;

	unsigned char program;
	unsigned char bankMsb;
	unsigned char bankLsb;

	signed char	  rpnMsb;
	signed char	  rpnLsb;
	unsigned char bendWidth;

	float		  pitch;

	ADSR*         adsrAMP;
	ADSR*         adsrEQ;
};

typedef struct Osc
{
	unsigned char channel;
	signed char   noteNo;
	unsigned char release;

	float counter;
	float tempCounter;
	float time;

	float level;
	float amplitude;
	float frequency;
	float nois;
};

// ============================================================================================
// VST�̊�{�ƂȂ�N���X
// ============================================================================================
class VST1 : public AudioEffectX
{
private:
	void clearChannel(Channel *channel);
	void clearOsc(Osc *osc);
	void releaseOsc(Osc *osc);
	void readMidiMsg(MidiMessage *midiMsg, Channel *channel, Osc *osc);

	float sqr50(Osc *osc);
	float sqr25(Osc *osc);
	float sqr12(Osc *osc);
	float tri(Osc *osc);
	float nois(Osc *osc);

protected:
	int          m_MidiMsgNum;					// �󂯎����MIDI���b�Z�[�W�̐�
	MidiMessage  m_MidiMsgList[MIDIMSG_MAXNUM];	// �󂯎����MIDI���b�Z�[�W��ۊǂ���o�b�t�@
	Channel      channel[16];					// MIDI�`�����l���̃o�b�t�@
	Osc			 osc[OSC_MAXNUM];				// ���U��̃o�b�t�@

public:
	VST1(audioMasterCallback audioMaster);

	// MIDI���b�Z�[�W���z�X�g�A�v���P�[�V��������󂯎�邽�߂̃����o�[�֐�
	VstInt32 processEvents(VstEvents* events);

	// �����M�����������郁���o�[�֐�
	virtual void processReplacing(float** inputs, float** outputs, VstInt32 sampleFrames);
};