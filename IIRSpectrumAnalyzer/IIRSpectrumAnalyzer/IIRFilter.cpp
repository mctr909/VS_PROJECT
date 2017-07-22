#pragma once
#include "IIRFilter.h"

IIRFilter::IIRFilter(int sampleRate, int banks)
{
	m_freqToOmega = 8.0 * atan(1.0) / sampleRate;
	m_banks = (BANK*)GlobalAlloc(GPTR, sizeof(BANK) * banks);
	MaxBanks = banks - 1;

	for (int i = 0; i < banks; ++i)
	{
		m_banks[i].a1 = 0.0;
		m_banks[i].a2 = 0.0;
		m_banks[i].b0 = 0.0;
		m_banks[i].b1 = 0.0;
		m_banks[i].b2 = 0.0;
		m_banks[i].aDelay1 = 0.0;
		m_banks[i].aDelay2 = 0.0;
		m_banks[i].bDelay1 = 0;
		m_banks[i].bDelay2 = 0;
	}
}

IIRFilter::~IIRFilter()
{
	GlobalFree(m_banks);
}

void
IIRFilter::Lowpass(int bankNo, double freq, double q)
{
	if (MaxBanks < bankNo) return;

	BANK* bank = m_banks + bankNo;
	double omega = freq * m_freqToOmega;
	double alpha = sin(omega) / (2.0 * q);

	double a0 = 1.0 + alpha;
	bank->a1 = -2.0 * cos(omega) / a0;
	bank->a2 = (1.0 - alpha) / a0;
	bank->b0 = (1.0 - cos(omega)) / (2.0 * a0);
	bank->b1 = (1.0 - cos(omega)) / a0;
	bank->b2 = (1.0 - cos(omega)) / (2.0 * a0);
}

void
IIRFilter::Bandpass(int bankNo, double freq, double oct)
{
	if (MaxBanks < bankNo) return;

	BANK* bank = m_banks + bankNo;
	double omega = freq * m_freqToOmega;
	double x = log(2.0) / 2.0 * oct * omega / sin(omega);
	double alpha = sin(omega) * (exp(x) - exp(-x)) / 2.0;

	double a0 = 1.0 + alpha;
	bank->a1 = -2.0 * cos(omega) / a0;
	bank->a2 = (1.0 - alpha) / a0;
	bank->b0 = alpha / a0;
	bank->b1 = 0.0;
	bank->b2 = -alpha / a0;
}

void
IIRFilter::Peaking(int bankNo, double freq, double oct, double gain)
{
	if (MaxBanks < bankNo) return;

	BANK* bank = m_banks + bankNo;
	double omega = freq * m_freqToOmega;
	double x = log(2.0) / 2.0 * oct * omega / sin(omega);
	double alpha = sin(omega) * (exp(x) - exp(-x)) / 2.0;
	double g = pow(10.0, (gain / 40.0));

	double a0 = 1.0 + alpha / g;
	bank->a1 = -2.0 * cos(omega) / a0;
	bank->a2 = (1.0 - alpha / g) / a0;
	bank->b0 = (1.0 + alpha * g) / a0;
	bank->b1 = -2.0 * cos(omega) / a0;
	bank->b2 = (1.0 - alpha * g) / a0;
}

void
IIRFilter::Exec(int& bankNo, INT16& input, double* output)
{
	BANK* bank = m_banks + bankNo;

	*output
		= bank->b0 * input
		+ bank->b1 * bank->bDelay1
		+ bank->b2 * bank->bDelay2
		- bank->a1 * bank->aDelay1
		- bank->a2 * bank->aDelay2
	;

	bank->aDelay2 = bank->aDelay1;
	bank->aDelay1 = *output;
	bank->bDelay2 = bank->bDelay1;
	bank->bDelay1 = input;
}
