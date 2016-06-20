using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldEntity
    {
        public static OldEntity Load(byte[] data)
        {
            if (data.Length < 28)
                ErrorManager.SignalError("OldEntity: Data is too short\n\nReason: Data is less than 28 bytes long");
            int garbage = BitConv.FromInt32(data,0);
            short unknown1 = BitConv.FromInt16(data,4);
            short unknown2 = BitConv.FromInt16(data,6);
            OldEntityID? id = new OldEntityID(BitConv.FromInt16(data,8));
            short positioncount = BitConv.FromInt16(data,10);
            if (data.Length < 22 + 6 * positioncount)
                ErrorManager.SignalError("OldEntity: Data is too short\n\nReason: Data is less than 22 + 6 * positioncount bytes long");
            short settinga = BitConv.FromInt16(data,12);
            short settingb = BitConv.FromInt16(data,14);
            short linkid = BitConv.FromInt16(data,16);
            byte type = data[18];
            byte subtype = data[19];
            EntityPosition[] index = new EntityPosition [positioncount];
            for (int i = 0;i < positioncount;i++)
            {
                short x = BitConv.FromInt16(data,20 + 6 * i);
                short y = BitConv.FromInt16(data,22 + 6 * i);
                short z = BitConv.FromInt16(data,24 + 6 * i);
                index[i] = new EntityPosition(x,y,z);
            }
            if (positioncount <= 0)
                ErrorManager.SignalError("OldEntity: Position count is negative or equal to zero");
            short nullfield1 = BitConv.FromInt16(data,20 + positioncount * 6);
            return new OldEntity(garbage,unknown1,unknown2,id,positioncount,settinga,settingb,linkid,type,subtype,index,nullfield1);
        }

        private int garbage;
        private short unknown2;
        private short? unknown1;
        private OldEntityID? id;
        private short positioncount;
        private short? settinga;
        private short? settingb;
        private short? linkid;
        private byte? type;
        private byte? subtype;
        private List<EntityPosition> index = null;
        private short nullfield1;

        public OldEntity(int garbage,short unknown1,short unknown2,OldEntityID? id,short positioncount,short settinga,short settingb,short linkid,byte type,byte subtype,IEnumerable<EntityPosition> index,short nullfield1)
        {
            if (index == null)
                throw new ArgumentNullException("index");
            this.garbage = garbage;
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
            this.index = new List<EntityPosition>(index);
            this.positioncount = positioncount;
            this.id = id;
            this.settinga = settinga;
            this.settingb = settingb;
            this.linkid = linkid;
            this.type = type;
            this.subtype = subtype;
            this.nullfield1 = nullfield1;
        }

        public short Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        public short? Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        public int Garbage
        {
            get { return garbage; }
            set { garbage = value; }
        }

        public short? ID
        {
            get { return id.HasValue ? (short?)id.Value.ID : null; }
            set { id = new OldEntityID(value.Value); }
        }

        public short PositionCount
        {
            get { return positioncount; }
        }

        public short? SettingA
        {
            get { return settinga; }
            set { settinga = value; }
        }

        public short? SettingB
        {
            get { return settingb; }
            set { settingb = value; }
        }

        public short? LinkID
        {
            get { return linkid; }
            set { linkid = value; }
        }

        public byte? Type
        {
            get { return type; }
            set { type = value; }
        }

        public byte? Subtype
        {
            get { return subtype; }
            set { subtype = value; }
        }

        public IList<EntityPosition> Index
        {
            get { return index; }
        }

        public short Nullfield1
        {
            get { return nullfield1; }
            set { nullfield1 = value; }
        }

        public byte[] Save()
        {
            positioncount = (short)Index.Count;
            byte[] result = new byte [22 + (6 * positioncount)];
            BitConv.ToInt32(result,0,garbage);
            BitConv.ToInt16(result,4,Unknown1.Value);
            BitConv.ToInt16(result,6,unknown2);
            BitConv.ToInt16(result,8,ID.Value);
            BitConv.ToInt16(result,10,positioncount);
            BitConv.ToInt16(result,12,SettingA.Value);
            BitConv.ToInt16(result,14,SettingB.Value);
            BitConv.ToInt16(result,16,LinkID.Value);
            result[18] = Type.Value;
            result[19] = Subtype.Value;
            for (int i = 0;i < positioncount;i++)
            {
                BitConv.ToInt16(result,20 + i * 6,index[i].X);
                BitConv.ToInt16(result,22 + i * 6,index[i].Y);
                BitConv.ToInt16(result,24 + i * 6,index[i].Z);
            }
            return result;
        }
    }
}
