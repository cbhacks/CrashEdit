using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldCamera
    {
        public static OldCamera Load(byte[] data)
        {
            if (data.Length < 50)
            {
                ErrorManager.SignalError("OldCamera: Data is too short");
            }
            if (data.Length < 52)
            {
                ErrorManager.SignalIgnorableError("OldCamera: Data is too short");
            }
            int slsteid = BitConv.FromInt32(data,0);
            int garbage = BitConv.FromInt32(data,4);
            int neighborcount = BitConv.FromInt32(data,8);
            if (neighborcount > 4 || neighborcount < 0)
            {
                ErrorManager.SignalError("OldCamera: Invalid neighbor camera count");
            }
            OldCameraNeighbor[] neighbors = new OldCameraNeighbor[4];
            for (int i = 0; i < 4; ++i)
            {
                neighbors[i] = new OldCameraNeighbor(
                    data[12+i*4+0],
                    data[12+i*4+1],
                    data[12+i*4+2],
                    data[12+i*4+3]);
            }
            byte entrypoint = data[28];
            byte exitpoint = data[29];
            short pointcount = BitConv.FromInt16(data,30);
            short mode = BitConv.FromInt16(data,32);
            short avgdist = BitConv.FromInt16(data,34);
            short zoom = BitConv.FromInt16(data,36);
            short unk1 = BitConv.FromInt16(data,38);
            short unk2 = BitConv.FromInt16(data,40);
            short unk3 = BitConv.FromInt16(data,42);
            short xdir = BitConv.FromInt16(data,44);
            short ydir = BitConv.FromInt16(data,46);
            short zdir = BitConv.FromInt16(data,48);
            OldCameraPosition[] position = new OldCameraPosition[pointcount];
            for (int i = 0; i < pointcount; i++)
            {
                short x = BitConv.FromInt16(data,50 + 12 * i);
                short y = BitConv.FromInt16(data,52 + 12 * i);
                short z = BitConv.FromInt16(data,54 + 12 * i);
                short xrot = BitConv.FromInt16(data,56 + 12 * i);
                short yrot = BitConv.FromInt16(data,58 + 12 * i);
                short zrot = BitConv.FromInt16(data,60 + 12 * i);
                position[i] = new OldCameraPosition(x,y,z,xrot,yrot,zrot);
            }
            short blank = data.Length < 52 + 12*pointcount ? (short)0 : BitConv.FromInt16(data,50 + 12*pointcount);
            return new OldCamera(slsteid,garbage,neighborcount,neighbors,entrypoint,exitpoint,mode,avgdist,zoom,unk1,unk2,unk3,xdir,ydir,zdir,position,blank);
        }

        private List<OldCameraPosition> position;

        public OldCamera(int slsteid,int garbage,int neighborcount,OldCameraNeighbor[] neighbors,byte entrypoint,byte exitpoint,short mode,short avgdist,short zoom,short unk1,short unk2,short unk3,short xdir,short ydir,short zdir,IEnumerable<OldCameraPosition> position,short blank)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            SLSTEID = slsteid;
            Garbage = garbage;
            NeighborCount = neighborcount;
            Neighbors = neighbors;
            EntryPoint = entrypoint;
            ExitPoint = exitpoint;
            Mode = mode;
            AvgDist = avgdist;
            Zoom = zoom;
            Unk1 = unk1;
            Unk2 = unk2;
            Unk3 = unk3;
            XDir = xdir;
            YDir = ydir;
            ZDir = zdir;
            this.position = new List<OldCameraPosition>(position);
            Blank = blank;
        }

        public int SLSTEID { get; set; }
        public int Garbage { get; set; }
        public int NeighborCount { get; set; }
        public OldCameraNeighbor[] Neighbors { get; set; }
        public byte EntryPoint { get; set; }
        public byte ExitPoint { get; set; }
        public short Mode { get; set; }
        public short AvgDist { get; set; }
        public short Zoom { get; set; }
        public short Unk1 { get; set; }
        public short Unk2 { get; set; }
        public short Unk3 { get; set; }
        public short XDir { get; set; }
        public short YDir { get; set; }
        public short ZDir { get; set; }
        public IList<OldCameraPosition> Positions => position;
        public short Blank { get; set; }

        public byte[] Save()
        {
            byte[] result = new byte [52 + position.Count * 12];
            BitConv.ToInt32(result,0,SLSTEID);
            BitConv.ToInt32(result,4,Garbage);
            BitConv.ToInt32(result,8,NeighborCount);
            for (int i = 0; i < 4; ++i)
            {
                result[12+i*4+0] = Neighbors[i].LinkType;
                result[12+i*4+1] = Neighbors[i].ZoneIndex;
                result[12+i*4+2] = Neighbors[i].CameraIndex;
                result[12+i*4+3] = Neighbors[i].Flag;
            }
            result[28] = EntryPoint;
            result[29] = ExitPoint;
            BitConv.ToInt16(result,30,(short)position.Count);
            BitConv.ToInt16(result,32,Mode);
            BitConv.ToInt16(result,34,AvgDist);
            BitConv.ToInt16(result,36,Zoom);
            BitConv.ToInt16(result,38,Unk1);
            BitConv.ToInt16(result,40,Unk2);
            BitConv.ToInt16(result,42,Unk3);
            BitConv.ToInt16(result,44,XDir);
            BitConv.ToInt16(result,46,YDir);
            BitConv.ToInt16(result,48,ZDir);
            for (int i = 0; i < position.Count; ++i)
            {
                BitConv.ToInt16(result,50 + i * 12,position[i].X);
                BitConv.ToInt16(result,52 + i * 12,position[i].Y);
                BitConv.ToInt16(result,54 + i * 12,position[i].Z);
                BitConv.ToInt16(result,56 + i * 12,position[i].XRot);
                BitConv.ToInt16(result,58 + i * 12,position[i].YRot);
                BitConv.ToInt16(result,60 + i * 12,position[i].ZRot);
            }
            BitConv.ToInt16(result,50 + position.Count*12,Blank);
            return result;
        }
    }
}
