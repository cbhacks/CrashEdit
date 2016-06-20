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

        private int slsteid;
        private int garbage;
        private int? neighborcount;
        private byte? relative1;
        private byte? parentzone1;
        private byte? pathitem1;
        private byte? relativeflag1;
        private byte? relative2;
        private byte? parentzone2;
        private byte? pathitem2;
        private byte? relativeflag2;
        private byte? relative3;
        private byte? parentzone3;
        private byte? pathitem3;
        private byte? relativeflag3;
        private byte? relative4;
        private byte? parentzone4;
        private byte? pathitem4;
        private byte? relativeflag4;
        private byte? entrypoint;
        private byte? exitpoint;
        private short pointcount;
        private short? mode;
        private short? avgdist;
        private short? zoom;
        private short unk1;
        private short unk2;
        private short unk3;
        private short? xdir;
        private short? ydir;
        private short? zdir;
        private List<OldCameraPosition> position;

        public OldCamera(int slsteid,int garbage,int? neighborcount,byte? relative1,byte? parentzone1,byte? pathitem1,byte? relativeflag1,byte? relative2,byte? parentzone2,byte? pathitem2,byte? relativeflag2,byte? relative3,byte? parentzone3,byte? pathitem3,byte? relativeflag3,byte? relative4,byte? parentzone4,byte? pathitem4,byte? relativeflag4,byte? entrypoint,byte? exitpoint,short pointcount,short? mode,short avgdist,short? zoom,short unk1,short unk2,short unk3,short? xdir,short? ydir,short? zdir,IEnumerable<OldCameraPosition> position)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            this.slsteid = slsteid;
            this.garbage = garbage;
            this.neighborcount = neighborcount;
            this.relative1 = relative1;
            this.parentzone1 = parentzone1;
            this.pathitem1 = pathitem1;
            this.relativeflag1 = relativeflag1;
            this.relative2 = relative2;
            this.parentzone2 = parentzone2;
            this.pathitem2 = pathitem2;
            this.relativeflag2 = relativeflag2;
            this.relative3 = relative3;
            this.parentzone3 = parentzone3;
            this.pathitem3 = pathitem3;
            this.relativeflag3 = relativeflag3;
            this.relative4 = relative4;
            this.parentzone4 = parentzone4;
            this.pathitem4 = pathitem4;
            this.relativeflag4 = relativeflag4;
            this.entrypoint = entrypoint;
            this.exitpoint = exitpoint;
            this.pointcount = pointcount;
            this.mode = mode;
            this.avgdist = avgdist;
            this.zoom = zoom;
            this.unk1 = unk1;
            this.unk2 = unk2;
            this.unk3 = unk3;
            this.xdir = xdir;
            this.ydir = ydir;
            this.zdir = zdir;
            this.position = new List<OldCameraPosition>(position);
        }

        public short Unk1
        {
            get { return unk1; }
            set { unk1 = value; }
        }

        public short Unk2
        {
            get { return unk2; }
            set { unk2 = value; }
        }

        public short Unk3
        {
            get { return unk3; }
            set { unk3 = value; }
        }

        public int SLSTEID
        {
            get { return slsteid; }
            set { slsteid = value; }
        }

        public int Garbage
        {
            get { return garbage; }
            set { garbage = value; }
        }

        public int? NeighborCount
        {
            get { return neighborcount; }
            set { neighborcount = value; }
        }

        public byte? Relative1
        {
            get { return relative1; }
            set { relative1 = value; }
        }

        public byte? ParentZone1
        {
            get { return parentzone1; }
            set { parentzone1 = value; }
        }

        public byte? PathItem1
        {
            get { return pathitem1; }
            set { pathitem1 = value; }
        }

        public byte? RelativeFlag1
        {
            get { return relativeflag1; }
            set { relativeflag1 = value; }
        }

        public byte? Relative2
        {
            get { return relative2; }
            set { relative2 = value; }
        }

        public byte? ParentZone2
        {
            get { return parentzone2; }
            set { parentzone2 = value; }
        }

        public byte? PathItem2
        {
            get { return pathitem2; }
            set { pathitem2 = value; }
        }

        public byte? RelativeFlag2
        {
            get { return relativeflag2; }
            set { relativeflag2 = value; }
        }

        public byte? Relative3
        {
            get { return relative3; }
            set { relative3 = value; }
        }

        public byte? ParentZone3
        {
            get { return parentzone3; }
            set { parentzone3 = value; }
        }

        public byte? PathItem3
        {
            get { return pathitem3; }
            set { pathitem3 = value; }
        }

        public byte? RelativeFlag3
        {
            get { return relativeflag3; }
            set { relativeflag3 = value; }
        }

        public byte? Relative4
        {
            get { return relative4; }
            set { relative4 = value; }
        }

        public byte? ParentZone4
        {
            get { return parentzone4; }
            set { parentzone4 = value; }
        }

        public byte? PathItem4
        {
            get { return pathitem4; }
            set { pathitem4 = value; }
        }

        public byte? RelativeFlag4
        {
            get { return relativeflag4; }
            set { relativeflag4 = value; }
        }

        public byte? EntryPoint
        {
            get { return entrypoint; }
            set { entrypoint = value; }
        }

        public byte? ExitPoint
        {
            get { return exitpoint; }
            set { exitpoint = value; }
        }

        public short PointCount
        {
            get { return pointcount; }
            set { pointcount = value; }
        }

        public short? Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public short? AvgDist
        {
            get { return avgdist; }
            set { avgdist = value; }
        }

        public short? Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        public short? XDir
        {
            get { return xdir; }
            set { xdir = value; }
        }

        public short? YDir
        {
            get { return ydir; }
            set { ydir = value; }
        }

        public short? ZDir
        {
            get { return zdir; }
            set { zdir = value; }
        }

        public IList<OldCameraPosition> Positions
        {
            get { return position; }
        }

        public byte[] Save()
        {
            byte[] result = new byte [50 + (12 * pointcount)];
            BitConv.ToInt32(result,0,slsteid);
            BitConv.ToInt32(result,4,garbage);
            BitConv.ToInt32(result,8,NeighborCount.Value);
            result[12] = Relative1.Value;
            result[13] = ParentZone1.Value;
            result[14] = PathItem1.Value;
            result[15] = RelativeFlag1.Value;
            result[16] = Relative2.Value;
            result[17] = ParentZone2.Value;
            result[18] = PathItem2.Value;
            result[19] = RelativeFlag2.Value;
            result[20] = Relative3.Value;
            result[21] = ParentZone3.Value;
            result[22] = PathItem3.Value;
            result[23] = RelativeFlag3.Value;
            result[24] = Relative4.Value;
            result[25] = ParentZone4.Value;
            result[26] = PathItem4.Value;
            result[27] = RelativeFlag4.Value;
            result[28] = EntryPoint.Value;
            result[29] = ExitPoint.Value;
            BitConv.ToInt16(result,30,pointcount);
            BitConv.ToInt16(result,32,Mode.Value);
            BitConv.ToInt16(result,34,AvgDist.Value);
            BitConv.ToInt16(result,36,Zoom.Value);
            BitConv.ToInt16(result,38,unk1);
            BitConv.ToInt16(result,40,unk2);
            BitConv.ToInt16(result,42,unk3);
            BitConv.ToInt16(result,44,XDir.Value);
            BitConv.ToInt16(result,46,YDir.Value);
            BitConv.ToInt16(result,48,ZDir.Value);
            for (int i = 0; i < pointcount; i++)
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
