#pragma once
#include <windows.h>
#include <stdio.h>
#include <math.h>

class IIRFilter
{
private:
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

public:
	UINT32 MaxBankIndex = 0;

private:
	BANK* m_banks;
	double m_freqToOmega = 0.0;

public:
	IIRFilter(UINT32 sampleRate, UINT32 banks);
	~IIRFilter();

	void Lowpass(UINT32 bankNo, double freq, double q);
	void Bandpass(UINT32 bankNo, double freq, double oct);
	void Peaking(UINT32 bankNo, double freq, double oct, double gain);
	void Exec(UINT32& bankNo, INT16& input, double* output);
};
