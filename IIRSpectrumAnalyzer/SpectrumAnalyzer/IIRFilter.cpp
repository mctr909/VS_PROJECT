#pragma once
#include "IIRFilter.h"

IIRFilter::IIRFilter(UINT32 sampleRate, UINT32 banks)
{
	m_freqToOmega = 8.0 * atan(1.0) / sampleRate;
	m_banks = (BANK*)malloc(sizeof(BANK) * banks);
	memset(m_banks, 0, sizeof(BANK) * banks);
	MaxBankIndex = banks - 1;
}

IIRFilter::~IIRFilter()
{
	free(m_banks);
}

void
IIRFilter::Lowpass(UINT32 bankNo, double freq, double q)
{
	if (MaxBankIndex < bankNo) return;

	double omega = freq * m_freqToOmega;
	double alpha = sin(omega) / (2.0 * q);
	double a0 = 1.0 + alpha;

	BANK* bank = m_banks + bankNo;
	bank->a1 = -2.0 * cos(omega) / a0;
	bank->a2 = (1.0 - alpha) / a0;
	bank->b0 = (1.0 - cos(omega)) / (2.0 * a0);
	bank->b1 = (1.0 - cos(omega)) / a0;
	bank->b2 = (1.0 - cos(omega)) / (2.0 * a0);
}

void
IIRFilter::Bandpass(UINT32 bankNo, double freq, double oct)
{
	if (MaxBankIndex < bankNo) return;

	double omega = freq * m_freqToOmega;
	double x = log(2.0) / 2.0 * oct * omega / sin(omega);
	double alpha = sin(omega) * (exp(x) - exp(-x)) / 2.0;
	double a0 = 1.0 + alpha;

	BANK* bank = m_banks + bankNo;
	bank->a1 = -2.0 * cos(omega) / a0;
	bank->a2 = (1.0 - alpha) / a0;
	bank->b0 = alpha / a0;
	bank->b1 = 0.0;
	bank->b2 = -alpha / a0;
}

void
IIRFilter::Peaking(UINT32 bankNo, double freq, double oct, double gain)
{
	if (MaxBankIndex < bankNo) return;

	double omega = freq * m_freqToOmega;
	double x = log(2.0) / 2.0 * oct * omega / sin(omega);
	double alpha = sin(omega) * (exp(x) - exp(-x)) / 2.0;
	double g = pow(10.0, (gain / 40.0));
	double a0 = 1.0 + alpha / g;

	BANK* bank = m_banks + bankNo;
	bank->a1 = -2.0 * cos(omega) / a0;
	bank->a2 = (1.0 - alpha / g) / a0;
	bank->b0 = (1.0 + alpha * g) / a0;
	bank->b1 = -2.0 * cos(omega) / a0;
	bank->b2 = (1.0 - alpha * g) / a0;
}

void
IIRFilter::Exec(UINT32& bankNo, INT16& input, double* output)
{
	BANK* bank = m_banks + bankNo;
    double temp;

	*output
		= bank->b0 * input
		+ bank->b1 * bank->bDelay01
		+ bank->b2 * bank->bDelay02
		- bank->a1 * bank->aDelay01
		- bank->a2 * bank->aDelay02
	;

	bank->aDelay02 = bank->aDelay01;
	bank->aDelay01 = *output;
	bank->bDelay02 = bank->bDelay01;
	bank->bDelay01 = input;

    temp = *output;
    *output
        = bank->b0 * temp
        + bank->b1 * bank->bDelay11
        + bank->b2 * bank->bDelay12
        - bank->a1 * bank->aDelay11
        - bank->a2 * bank->aDelay12
    ;

    bank->aDelay12 = bank->aDelay11;
    bank->aDelay11 = *output;
    bank->bDelay12 = bank->bDelay11;
    bank->bDelay11 = temp;
}