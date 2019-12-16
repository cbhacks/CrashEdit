using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldEntity
    {
        public static OldEntity Load(byte[] data)
        {
            if (data.Length < 22)
                ErrorManager.SignalError("OldEntity: Data is too short");
            int garbage = BitConv.FromInt32(data,0);
            int flags = BitConv.FromInt32(data,4);
            short id = BitConv.FromInt16(data,8);
            short positioncount = BitConv.FromInt16(data,10);
            if (data.Length < 22 + 6 * positioncount)
                ErrorManager.SignalError("OldEntity: Data is too short");
            if (positioncount < 0)
                ErrorManager.SignalError("OldEntity: Position count is negative");
            short settinga = BitConv.FromInt16(data,12);
            short settingb = BitConv.FromInt16(data,14);
            short linkid = BitConv.FromInt16(data,16);
            byte type = data[18];
            byte subtype = data[19];
            EntityPosition[] positions = new EntityPosition [positioncount];
            for (int i = 0;i < positioncount;i++)
            {
                short x = BitConv.FromInt16(data,20 + 6*i);
                short y = BitConv.FromInt16(data,22 + 6*i);
                short z = BitConv.FromInt16(data,24 + 6*i);
                positions[i] = new EntityPosition(x,y,z);
            }
            short nullfield1 = BitConv.FromInt16(data,20 + positioncount * 6);
            return new OldEntity(garbage,flags,id,settinga,settingb,linkid,type,subtype,positions,nullfield1);
        }

        private List<EntityPosition> positions = null;

        public OldEntity(int garbage,int flags,short id,short settinga,short settingb,short linkid,byte type,byte subtype,IEnumerable<EntityPosition> positions,short nullfield1)
        {
            if (positions == null)
                throw new ArgumentNullException("index");
            Garbage = garbage;
            Flags = flags;
            this.positions = new List<EntityPosition>(positions);
            ID = id;
            SettingA = settinga;
            SettingB = settingb;
            SettingC = linkid;
            Type = type;
            Subtype = subtype;
            Nullfield1 = nullfield1;
        }

        public int Flags { get; set; }
        public int Garbage { get; set; }
        public short ID { get; set; }
        public short SettingA { get; set; }
        public short SettingB { get; set; }
        public short SettingC { get; set; }
        public byte Type { get; set; }
        public byte Subtype { get; set; }
        public IList<EntityPosition> Positions => positions;
        public short Nullfield1 { get; set; }

        public byte[] Save()
        {
            byte[] result = new byte [22 + (6 * positions.Count)];
            BitConv.ToInt32(result,0,Garbage);
            BitConv.ToInt32(result,4,Flags);
            BitConv.ToInt16(result,8,ID);
            BitConv.ToInt16(result,10,(short)positions.Count);
            BitConv.ToInt16(result,12,SettingA);
            BitConv.ToInt16(result,14,SettingB);
            BitConv.ToInt16(result,16,SettingC);
            result[18] = Type;
            result[19] = Subtype;
            for (int i = 0;i < positions.Count;++i)
            {
                BitConv.ToInt16(result,20 + i * 6,positions[i].X);
                BitConv.ToInt16(result,22 + i * 6,positions[i].Y);
                BitConv.ToInt16(result,24 + i * 6,positions[i].Z);
            }
            BitConv.ToInt16(result,20 + positions.Count * 6,Nullfield1);
            return result;
        }
    }
}
