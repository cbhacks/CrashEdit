using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoEntity
    {
        public static ProtoEntity Load(byte[] data)
        {
            if (data.Length < 28)
                ErrorManager.SignalError("ProtoEntity: Data is too short");
            int garbage = BitConv.FromInt32(data,0);
            short settinga = BitConv.FromInt16(data,4);
            short unknown = BitConv.FromInt16(data,6);
            short id = BitConv.FromInt16(data,8);
            short positioncount = BitConv.FromInt16(data,10);
            short startx = BitConv.FromInt16(data,12);
            short starty = BitConv.FromInt16(data,14);
            short startz = BitConv.FromInt16(data,16);
            if (data.Length < 28 + 4 * (positioncount - 1))
                ErrorManager.SignalError("ProtoEntity: Data is too short");
            ProtoEntityPosition[] index = new ProtoEntityPosition [positioncount];
            for (int i = 0;i < positioncount - 1;i++)
            {
                sbyte global = (sbyte)data[26 + 4 * i];
                sbyte x = (sbyte)data[27 + 4 * i];
                sbyte y = (sbyte)data[28 + 4 * i];
                sbyte z = (sbyte)data[29 + 4 * i];
                index[i] = new ProtoEntityPosition(global,x,y,z);
            }
            short settingb = BitConv.FromInt16(data,18);
            short settingc = BitConv.FromInt16(data,20);
            short settingd = BitConv.FromInt16(data,22);
            byte type = data[24];
            byte subtype = data[25];
            if (positioncount <= 0)
                ErrorManager.SignalError("ProtoEntity: Position count is negative or zero");
            short nullfield1 = BitConv.FromInt16(data,20 + positioncount * 4);
            return new ProtoEntity(garbage,settinga,unknown,id,positioncount,startx,starty,startz,settingb,settingc,settingd,type,subtype,index,nullfield1);
        }

        private List<ProtoEntityPosition> index = null;

        public ProtoEntity(int garbage,short unknown,short settinga,short id,short positioncount,short startx,short starty,short startz,short settingb,short settingc,short settingd,byte type,byte subtype,IEnumerable<ProtoEntityPosition> index,short nullfield1)
        {
            if (index == null)
                throw new ArgumentNullException("index");
            Garbage = garbage;
            Unknown = unknown;
            SettingA = settinga;
            this.index = new List<ProtoEntityPosition>(index);
            PositionCount = positioncount;
            ID = id;
            SettingB = settingb;
            SettingC = settingc;
            SettingD = settingd;
            Type = type;
            Subtype = subtype;
            Nullfield1 = nullfield1;
            StartX = startx;
            StartY = starty;
            StartZ = startz;
        }

        public short Unknown { get; set; }
        public short SettingA { get; set; }
        public int Garbage { get; }
        public short ID { get; set; }
        public short PositionCount { get; private set; }
        public short SettingB { get; set; }
        public short SettingC { get; set; }
        public short SettingD { get; set; }
        public byte Type { get; set; }
        public byte Subtype { get; set; }
        public IList<ProtoEntityPosition> Index => index;
        public short Nullfield1 { get; }
        public short StartX { get; set; }
        public short StartY { get; set; }
        public short StartZ { get; set; }

        public byte[] Save()
        {
            PositionCount = (short)Index.Count;
            byte[] result = new byte [28 + 4 * (PositionCount - 1)];
            BitConv.ToInt32(result,0,Garbage);
            BitConv.ToInt16(result,4,SettingA);
            BitConv.ToInt16(result,6,Unknown);
            BitConv.ToInt16(result,8,ID);
            BitConv.ToInt16(result,10,PositionCount);
            BitConv.ToInt16(result,12,StartX);
            BitConv.ToInt16(result,14,StartY);
            BitConv.ToInt16(result,16,StartZ);
            BitConv.ToInt16(result,18,SettingB);
            BitConv.ToInt16(result,20,SettingC);
            BitConv.ToInt16(result,22,SettingD);
            result[24] = Type;
            result[25] = Subtype;
            for (int i = 0; i < PositionCount - 1; i++)
            {
                result[26 + i * 4] = (byte)index[i].Global;
                result[27 + i * 4] = (byte)index[i].X;
                result[28 + i * 4] = (byte)index[i].Y;
                result[29 + i * 4] = (byte)index[i].Z;
            }
            return result;
        }
    }
}
