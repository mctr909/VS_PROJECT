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

		double aDelay01;
		double aDelay02;
        double aDelay11;
        double aDelay12;
        double bDelay01;
        double bDelay02;
        double bDelay11;
        double bDelay12;
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
