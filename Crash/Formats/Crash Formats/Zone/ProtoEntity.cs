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
            short flags = BitConv.FromInt16(data,4);
            byte spawn = data[6];
            byte unk = data[7];
            short id = BitConv.FromInt16(data,8);
            if (spawn != 3)
            {
                ErrorManager.SignalIgnorableError(string.Format("ProtoEntity: Entity {0} is unspawnable.", id));
            }
            short positioncount = BitConv.FromInt16(data,10);
            if (data.Length < 28 + 4 * (positioncount - 1))
                ErrorManager.SignalError("ProtoEntity: Data is too short");
            if (positioncount <= 0)
                ErrorManager.SignalError("ProtoEntity: Position count is negative or zero");
            short startx = BitConv.FromInt16(data,12);
            short starty = BitConv.FromInt16(data,14);
            short startz = BitConv.FromInt16(data,16);
            short vecx = BitConv.FromInt16(data,18);
            short vecy = BitConv.FromInt16(data,20);
            short vecz = BitConv.FromInt16(data,22);
            byte type = data[24];
            byte subtype = data[25];
            ProtoEntityPosition[] deltas = new ProtoEntityPosition [positioncount - 1];
            for (int i = 0;i < deltas.Length; ++i)
            {
                deltas[i] = new ProtoEntityPosition((sbyte)data[26+4*i],(sbyte)data[27+4*i],(sbyte)data[28+4*i]);
            }
            short nullfield1 = BitConv.FromInt16(data,26+deltas.Length*4);
            return new ProtoEntity(flags,spawn,unk,id,startx,starty,startz,vecx,vecy,vecz,type,subtype,deltas,nullfield1);
        }

        private List<ProtoEntityPosition> deltas = null;

        public ProtoEntity(short flags,byte spawn,byte unk,short id,short startx,short starty,short startz,short vecx,short vecy,short vecz,byte type,byte subtype,IEnumerable<ProtoEntityPosition> deltas,short nullfield1)
        {
            if (deltas == null)
                throw new ArgumentNullException("index");
            Flags = flags;
            Spawn = spawn;
            Unk = unk;
            this.deltas = new List<ProtoEntityPosition>(deltas);
            ID = id;
            VecX = vecx;
            VecY = vecy;
            VecZ = vecz;
            Type = type;
            Subtype = subtype;
            Nullfield1 = nullfield1;
            StartX = startx;
            StartY = starty;
            StartZ = startz;
        }

        public short Flags { get; set; }
        public byte Spawn { get; set; }
        public byte Unk { get; set; }
        public short ID { get; set; }
        public short VecX { get; set; }
        public short VecY { get; set; }
        public short VecZ { get; set; }
        public byte Type { get; set; }
        public byte Subtype { get; set; }
        public IList<ProtoEntityPosition> Deltas => deltas;
        public short Nullfield1 { get; }
        public short StartX { get; set; }
        public short StartY { get; set; }
        public short StartZ { get; set; }

        public byte[] Save()
        {
            int deltacount = deltas.Count;
            byte[] result = new byte [28 + 4 * deltacount];
            BitConv.ToInt32(result,0,0);
            BitConv.ToInt16(result,4,Flags);
            result[6] = Spawn;
            result[7] = Unk;
            BitConv.ToInt16(result,8,ID);
            BitConv.ToInt16(result,10,(short)(deltacount+1));
            BitConv.ToInt16(result,12,StartX);
            BitConv.ToInt16(result,14,StartY);
            BitConv.ToInt16(result,16,StartZ);
            BitConv.ToInt16(result,18,VecX);
            BitConv.ToInt16(result,20,VecY);
            BitConv.ToInt16(result,22,VecZ);
            result[24] = Type;
            result[25] = Subtype;
            for (int i = 0; i < deltacount; i++)
            {
                result[26+i*4] = (byte)deltas[i].X;
                result[27+i*4] = (byte)deltas[i].Y;
                result[28+i*4] = (byte)deltas[i].Z;
            }
            BitConv.ToInt16(result,26+deltacount*4,Nullfield1);
            return result;
        }
    }
}
