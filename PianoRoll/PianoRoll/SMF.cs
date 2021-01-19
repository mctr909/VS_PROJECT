using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMF {
    public enum E_STATUS : byte {
        NOTE_OFF = 0x80,
        NOTE_ON = 0x90,
        CTRL = 0xB0,
        INST = 0xC0,
        PITCH = 0xE0,
        SYS_BEGIN = 0xF0,
        SYS_END = 0xF7
    }

    public enum E_CTRL : byte {
        BANK_LSB = 0,
        BANK_MSB = 32,

        VIB_DEP = 1,
        VIB_RATE = 76,

        VOL = 7,
        PAN = 10,
        EXP = 11,

        DAMPER = 64,

        RESONANCE = 71,
        RELEASE = 72,
        ATTACK = 73,
        CUTOFF = 74,

        REV = 91,
        CHO = 93,
        DEL_DEP = 94,
        DEL_TIME = 12,

        RPN_LSB = 100,
        RPN_MSB = 101,
        DATA = 6,

        ALL_RESET = 121
    }

    public struct Event {
        public int tick;
        public byte track;
        public byte status;
        public byte data1;
        public byte data2;
        public Event(int tick, params int[] data) {
            this.tick = tick;
            track = 0;
            status = 0;
            data1 = 0;
            data2 = 0;
            switch (data.Length) {
            case 3:
                track = (byte)data[0];
                status = (byte)data[1];
                data1 = (byte)data[2];
                break;
            case 4:
                track = (byte)data[0];
                status = (byte)data[1];
                data1 = (byte)data[2];
                data2 = (byte)data[3];
                break;
            }
        }

        public E_STATUS Type {
            get {
                if (status < 0xF0) {
                    return (E_STATUS)(status & 0xF0);
                } else {
                    return (E_STATUS)status;
                }
            }
        }
    }

    class SMF {
    }
}
