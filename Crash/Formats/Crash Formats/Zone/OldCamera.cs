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
            int slsteid = BitConv.FromInt32(data,0);
            int garbage = BitConv.FromInt32(data,4);
            int neighborcount = BitConv.FromInt32(data,8);
            // Start Neighbor stuff
            byte relative1 = data[12];
            byte parentzone1 = data[13];
            byte pathitem1 = data[14];
            byte relativeflag1 = data[15];
            byte relative2 = data[16];
            byte parentzone2 = data[17];
            byte pathitem2 = data[18];
            byte relativeflag2 = data[19];
            byte relative3 = data[20];
            byte parentzone3 = data[21];
            byte pathitem3 = data[22];
            byte relativeflag3 = data[23];
            byte relative4 = data[24];
            byte parentzone4 = data[25];
            byte pathitem4 = data[26];
            byte relativeflag4 = data[27];
            // End Neighbor stuff
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
            return new OldCamera(slsteid,garbage,neighborcount,relative1,parentzone1,pathitem1,relativeflag1,relative2,parentzone2,pathitem2,relativeflag2,relative3,parentzone3,pathitem3,relativeflag3,relative4,parentzone4,pathitem4,relativeflag4,entrypoint,exitpoint,pointcount,mode,avgdist,zoom,unk1,unk2,unk3,xdir,ydir,zdir,position);
        }

        private List<OldCameraPosition> position;

        public OldCamera(int slsteid,int garbage,int neighborcount,byte relative1,byte parentzone1,byte pathitem1,byte relativeflag1,byte relative2,byte parentzone2,byte pathitem2,byte relativeflag2,byte relative3,byte parentzone3,byte pathitem3,byte relativeflag3,byte relative4,byte parentzone4,byte pathitem4,byte relativeflag4,byte entrypoint,byte exitpoint,short pointcount,short mode,short avgdist,short zoom,short unk1,short unk2,short unk3,short xdir,short ydir,short zdir,IEnumerable<OldCameraPosition> position)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            SLSTEID = slsteid;
            Garbage = garbage;
            NeighborCount = neighborcount;
            Relative1 = relative1;
            ParentZone1 = parentzone1;
            PathItem1 = pathitem1;
            RelativeFlag1 = relativeflag1;
            Relative2 = relative2;
            ParentZone2 = parentzone2;
            PathItem2 = pathitem2;
            RelativeFlag2 = relativeflag2;
            Relative3 = relative3;
            ParentZone3 = parentzone3;
            PathItem3 = pathitem3;
            RelativeFlag3 = relativeflag3;
            Relative4 = relative4;
            ParentZone4 = parentzone4;
            PathItem4 = pathitem4;
            RelativeFlag4 = relativeflag4;
            EntryPoint = entrypoint;
            ExitPoint = exitpoint;
            PointCount = pointcount;
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
        }

        public short Unk1 { get; set; }
        public short Unk2 { get; set; }
        public short Unk3 { get; set; }
        public int SLSTEID { get; set; }
        public int Garbage { get; set; }
        public int NeighborCount { get; set; }
        public byte Relative1 { get; set; }
        public byte ParentZone1 { get; set; }
        public byte PathItem1 { get; set; }
        public byte RelativeFlag1 { get; set; }
        public byte Relative2 { get; set; }
        public byte ParentZone2 { get; set; }
        public byte PathItem2 { get; set; }
        public byte RelativeFlag2 { get; set; }
        public byte Relative3 { get; set; }
        public byte ParentZone3 { get; set; }
        public byte PathItem3 { get; set; }
        public byte RelativeFlag3 { get; set; }
        public byte Relative4 { get; set; }
        public byte ParentZone4 { get; set; }
        public byte PathItem4 { get; set; }
        public byte RelativeFlag4 { get; set; }
        public byte EntryPoint { get; set; }
        public byte ExitPoint { get; set; }
        public short PointCount { get; set; }
        public short Mode { get; set; }
        public short AvgDist { get; set; }
        public short Zoom { get; set; }
        public short XDir { get; set; }
        public short YDir { get; set; }
        public short ZDir { get; set; }
        public IList<OldCameraPosition> Positions => position;

        public byte[] Save()
        {
            byte[] result = new byte [50 + (12 * PointCount)];
            BitConv.ToInt32(result,0,SLSTEID);
            BitConv.ToInt32(result,4,Garbage);
            BitConv.ToInt32(result,8,NeighborCount);
            result[12] = Relative1;
            result[13] = ParentZone1;
            result[14] = PathItem1;
            result[15] = RelativeFlag1;
            result[16] = Relative2;
            result[17] = ParentZone2;
            result[18] = PathItem2;
            result[19] = RelativeFlag2;
            result[20] = Relative3;
            result[21] = ParentZone3;
            result[22] = PathItem3;
            result[23] = RelativeFlag3;
            result[24] = Relative4;
            result[25] = ParentZone4;
            result[26] = PathItem4;
            result[27] = RelativeFlag4;
            result[28] = EntryPoint;
            result[29] = ExitPoint;
            BitConv.ToInt16(result,30,PointCount);
            BitConv.ToInt16(result,32,Mode);
            BitConv.ToInt16(result,34,AvgDist);
            BitConv.ToInt16(result,36,Zoom);
            BitConv.ToInt16(result,38,Unk1);
            BitConv.ToInt16(result,40,Unk2);
            BitConv.ToInt16(result,42,Unk3);
            BitConv.ToInt16(result,44,XDir);
            BitConv.ToInt16(result,46,YDir);
            BitConv.ToInt16(result,48,ZDir);
            for (int i = 0; i < PointCount; i++)
            {
                BitConv.ToInt16(result,50 + i * 12,position[i].X);
                BitConv.ToInt16(result,52 + i * 12,position[i].Y);
                BitConv.ToInt16(result,54 + i * 12,position[i].Z);
                BitConv.ToInt16(result,56 + i * 12,position[i].XRot);
                BitConv.ToInt16(result,58 + i * 12,position[i].YRot);
                BitConv.ToInt16(result,60 + i * 12,position[i].ZRot);
            }
            return result;
        }
    }
}
