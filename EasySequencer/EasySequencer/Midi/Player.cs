using System.Threading;
using System.Threading.Tasks;

namespace MIDI {
	public class Player {
		private MessageSender mSender;
		private Task mTask;
		private Event[] mEventList;

		private int mTicks;
		private int mMaxTime;
		private double mBPM;
		private double mCurrentTime;
		private bool mIsPlay;

		public int SoloChannel;
		public int Transpose;

		#region プロパティ
		public Channel[] Channel {
			get { return mSender.Channel; }
		}

		public int SeekTime {
			set {
				Stop();
				if (value < 0) {
					mCurrentTime = 0.0;
				}
				else if (mMaxTime < value) {
					mCurrentTime = mMaxTime;
				}
				else {
					mCurrentTime = value;
				}
				Play();
			}
		}

		public int MaxTime {
			get { return mMaxTime; }
		}

		public int CurrentTime {
			get { return (int)mCurrentTime; }
		}

		public bool IsPlay {
			get { return mIsPlay; }
		}

		public string TimeText {
			get {
				int ib = (int)(mCurrentTime / 960);
				int measure = ib / 4;
				int beat = ib % 4 + 1;
				int tick = (int)(mCurrentTime - 960 * ib);
				return string.Format(
					"{0}:{1}:{2}",
					measure.ToString("0000"),
					beat.ToString("00"),
					tick.ToString("000")
				);
			}
		}

		public string TempoText {
			get { return mBPM.ToString("000.00"); }
		}
		#endregion

		public Player(MessageSender hMessage) {
			mSender = hMessage;
			mEventList = null;
			mTask = null;
			mTicks = 960;
			mIsPlay = false;
			SoloChannel = -1;
		}

		public void SetEventList(Event[] eventList, int ticks) {
			mEventList = eventList;
			mTicks = ticks;
			mMaxTime = 0;

			foreach (var ev in eventList) {
				if (EVENT_TYPE.NOTE_OFF == ev.Message.Type || EVENT_TYPE.NOTE_ON == ev.Message.Type) {
					var time = 1000 * ev.Time / ticks;
					if (mMaxTime < time) {
						mMaxTime = (int)time;
					}
				}
			}

			mBPM = 120.0;
			mCurrentTime = 0.0;
		}

		public void Play() {
			if (null == mEventList) {
				return;
			}

			mIsPlay = true;
			mTask = Task.Factory.StartNew(() => Loop());
		}

		public void Stop() {
			if (null == mTask) {
				return;
			}

			mIsPlay = false;
			while (!mTask.IsCompleted) {
				Task.Delay(100);
			}
			mTask = null;

			for (byte ch = 0; ch < 16; ++ch) {
				for (byte noteNo = 0; noteNo < 128; ++noteNo) {
					mSender.Send(new Message(EVENT_TYPE.NOTE_OFF, ch, noteNo));
					Task.Delay(10);
				}
				{
					Message msg = new Message(CTRL_TYPE.ALL_RESET, ch);
					mSender.Send(msg);
				}
			}
		}

		private void Loop() {
			long current_mSec = 0;
			long previous_mSec = 0;

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			foreach (var ev in mEventList) {
				if (!mIsPlay) {
					break;
				}

				long eventTime = 1000 * ev.Time / mTicks;
				while (mCurrentTime < eventTime) {
					if (!mIsPlay) {
						break;
					}

					current_mSec = sw.ElapsedMilliseconds;
					mCurrentTime += mBPM * (current_mSec - previous_mSec) / 60.0;
					previous_mSec = current_mSec;
					Thread.Sleep(1);
				}

				var msg = ev.Message;
				var type = msg.Type;

				if (EVENT_TYPE.META == type) {
					if (META_TYPE.TEMPO == msg.Meta.Type) {
						mBPM = msg.Meta.BPM;
					}
				}

				if (EVENT_TYPE.NOTE_OFF == type || EVENT_TYPE.NOTE_ON == type) {
					if (mTicks < (mCurrentTime - eventTime)) {
						continue;
					}
					if (!mSender.Channel[msg.Channel].InstID.IsDrum) {
						if ((msg.Byte1 + Transpose) < 0 || 127 < (msg.Byte1 + Transpose)) {
							continue;
						}
						else {
							msg = new Message(msg.Type, msg.Channel, (byte)(msg.Byte1 + Transpose), msg.Byte2);
						}
					}
				}

				if (msg.Channel <= 16 && !mSender.Channel[msg.Channel].Enable || (0 <= SoloChannel && SoloChannel != msg.Channel)) {
					if (EVENT_TYPE.NOTE_ON == type && msg.Byte2 != 0) {
						continue;
					}
				}

				mSender.Send(msg);
			}

			mIsPlay = false;
		}

		public void Send(Message msg) {
			mSender.Send(msg);
		}

		public Channel Recv(byte channel) {
			return mSender.Channel[channel];
		}
	}
}