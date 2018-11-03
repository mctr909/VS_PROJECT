namespace MIDI {
	class WaveOut : WaveOutLib {
		private MessageSender mSender;

		public WaveOut(MessageSender sender) : base(Const.SampleRate, 2, 256, 16) {
			mSender = sender;
			Open();
			Play();
		}

		protected override void SetWave() {
			mSender.SetWave(ref WaveBuffer);
		}
	}
}
