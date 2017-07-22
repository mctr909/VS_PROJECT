#pragma once
#include <windows.h>
#include <stdio.h>
#include <math.h>

class IIRFilter
{
private:
#pragma pack(1)
	struct BANK
	{
		double a1;
		double a2;
		double b0;
		double b1;
		double b2;

		double aDelay1;
		double aDelay2;
		INT16 bDelay1;
		INT16 bDelay2;
	};
#pragma

public:
	int MaxBanks = 0;

private:
	BANK* m_banks;
	double m_freqToOmega = 0.0;

public:
	IIRFilter(int sampleRate, int banks);
	~IIRFilter();

	void Lowpass(int bankNo, double freq, double q);
	void Bandpass(int bankNo, double freq, double oct);
	void Peaking(int bankNo, double freq, double oct, double gain);
	void Exec(int& bankNo, INT16& input, double* output);
};
