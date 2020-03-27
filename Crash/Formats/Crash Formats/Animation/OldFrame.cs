using System;
using System.IO;
using System.Collections.Generic;

namespace Crash
{
    public class OldFrame
    {
        public static OldFrame Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 56)
            {
                ErrorManager.SignalError("OldFrame: Data is too short");
            }
            int vertexcount = BitConv.FromInt32(data,0);
            if (vertexcount < 0 || vertexcount > Chunk.Length / 6)
            {
                ErrorManager.SignalError("OldFrame: Vertex count is invalid");
            }
            bool proto = false;
            int targetsize = 56 + vertexcount * 6 + 2;
            Aligner.Align(ref targetsize, 4);
            if (data.Length < targetsize)
            {
                proto = true;
                if (data.Length < 44 + vertexcount * 6 + 2)
                {
                    ErrorManager.SignalError("OldFrame: Data is too short");
                }
                if (data.Length > 44 + vertexcount * 6 + 2)
                {
                    ErrorManager.SignalError("OldFrame: Data is too large");
                }
            }
            if (data.Length > targetsize)
            {
                ErrorManager.SignalError("OldFrame: Data is too large");
            }
            int modeleid = BitConv.FromInt32(data,4);
            int xoffset = BitConv.FromInt32(data,8);
            int yoffset = BitConv.FromInt32(data,12);
            int zoffset = BitConv.FromInt32(data,16);
            int x1 = BitConv.FromInt32(data,20);
            int y1 = BitConv.FromInt32(data,24);
            int z1 = BitConv.FromInt32(data,28);
            int x2 = BitConv.FromInt32(data,32);
            int y2 = BitConv.FromInt32(data,36);
            int z2 = BitConv.FromInt32(data,40);
            if (!proto) {
                int xglobal = BitConv.FromInt32(data,44);
                int yglobal = BitConv.FromInt32(data,48);
                int zglobal = BitConv.FromInt32(data,52);
                OldFrameVertex[] vertices = new OldFrameVertex [vertexcount];
                for (int i = 0;i < vertexcount;i++)
                {
                    vertices[i] = new OldFrameVertex(data[56+i*6+0],data[56+i*6+1],data[56+i*6+2],(sbyte)data[56+i*6+3],(sbyte)data[56+i*6+4],(sbyte)data[56+i*6+5]);
                }
                short unknown = BitConv.FromInt16(data,56 + vertexcount * 6);
                short? unknown2 = null;
                if (data.Length >= 56 + vertexcount * 6 + 4)
                {
                    unknown2 = BitConv.FromInt16(data,58 + vertexcount * 6);
                }
                return new OldFrame(modeleid,xoffset,yoffset,zoffset,x1,y1,z1,x2,y2,z2,xglobal,yglobal,zglobal,vertices,unknown,unknown2,proto);
            }
            else
            {
                OldFrameVertex[] vertices = new OldFrameVertex [vertexcount];
                for (int i = 0;i < vertexcount;i++)
                {
                    vertices[i] = new OldFrameVertex(data[44+i*6+0],data[44+i*6+1],data[44+i*6+2],(sbyte)data[44+i*6+3],(sbyte)data[44+i*6+4],(sbyte)data[44+i*6+5]);
                }
                short unknown = BitConv.FromInt16(data,44 + vertexcount * 6);
                short? unknown2 = null;
                if (data.Length >= 44 + vertexcount * 6 + 4)
                {
                    unknown2 = BitConv.FromInt16(data,46 + vertexcount * 6);
                }
                return new OldFrame(modeleid,xoffset,yoffset,zoffset,x1,y1,z1,x2,y2,z2,0,0,0,vertices,unknown,unknown2,proto);
            }
        }

        private List<OldFrameVertex> vertices;

        public OldFrame(int modeleid,int xoffset,int yoffset,int zoffset,int x1,int y1,int z1,int x2,int y2,int z2,int xglobal,int yglobal,int zglobal,IEnumerable<OldFrameVertex> vertices,short unknown,short? unknown2,bool proto)
        {
            this.vertices = new List<OldFrameVertex>(vertices);
            ModelEID = modeleid;
            XOffset = xoffset;
            YOffset = yoffset;
            ZOffset = zoffset;
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
            XGlobal = xglobal;
            YGlobal = yglobal;
            ZGlobal = zglobal;
            Unknown = unknown;
            Unknown2 = unknown2;
            Proto = proto;
        }

        public int ModelEID { get; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int ZOffset { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int Z1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int Z2 { get; set; }
        public int XGlobal { get; set; }
        public int YGlobal { get; set; }
        public int ZGlobal { get; set; }
        public bool Proto { get; set; }

        public IList<OldFrameVertex> Vertices => vertices;

        public short Unknown { get; set; }
        public short? Unknown2 { get; set; }

        public byte[] Save()
        {
            byte[] data = new byte [44 + vertices.Count * 6 + 2 + (Unknown2.HasValue? 2:0) + (!Proto ? 12 : 0)];
            BitConv.ToInt32(data,0,vertices.Count);
            BitConv.ToInt32(data,4,ModelEID);
            BitConv.ToInt32(data,8,XOffset);
            BitConv.ToInt32(data,12,YOffset);
            BitConv.ToInt32(data,16,ZOffset);
            BitConv.ToInt32(data,20,X1);
            BitConv.ToInt32(data,24,Y1);
            BitConv.ToInt32(data,28,Z1);
            BitConv.ToInt32(data,32,X2);
            BitConv.ToInt32(data,36,Y2);
            BitConv.ToInt32(data,40,Z2);
            if (!Proto)
            {
                BitConv.ToInt32(data,44,XGlobal);
                BitConv.ToInt32(data,48,YGlobal);
                BitConv.ToInt32(data,52,ZGlobal);
                for (int i = 0;i < vertices.Count;i++)
                {
                    vertices[i].Save().CopyTo(data,56 + i * 6);
                }
                BitConv.ToInt16(data,56 + vertices.Count * 6,Unknown);
                if (Unknown2.HasValue)
                {
                    BitConv.ToInt16(data,58 + vertices.Count * 6,Unknown2.Value);
                }
            }
            else
            {
                for (int i = 0;i < vertices.Count;i++)
                {
                    vertices[i].Save().CopyTo(data,44 + i * 6);
                }
                BitConv.ToInt16(data,44 + vertices.Count * 6,Unknown);
                if (Unknown2.HasValue)
                {
                    BitConv.ToInt16(data,46 + vertices.Count * 6,Unknown2.Value);
                }
            }
            return data;
        }

        public byte[] ToOBJ(OldModelEntry model)
        {
            long xorigin = 0;
            long yorigin = 0;
            long zorigin = 0;
            //foreach (OldFrameVertex vertex in vertices)
            //{
            //    xorigin += vertex.X;
            //    yorigin += vertex.Y;
            //    zorigin += vertex.Z;
            //}
            //xorigin /= vertices.Count;
            //yorigin /= vertices.Count;
            //zorigin /= vertices.Count;
            xorigin -= XOffset;
            yorigin -= YOffset;
            zorigin -= ZOffset;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    obj.WriteLine("# Vertices");
                    foreach (OldFrameVertex vertex in vertices)
                    {
                        obj.WriteLine("v {0} {1} {2}",vertex.X - xorigin,vertex.Y - yorigin,vertex.Z - zorigin);
                    }
                    foreach (OldFrameVertex vertex in vertices)
                    {
                        obj.WriteLine("vn {0} {1} {2}",vertex.NormalX/127.0,vertex.NormalY/127.0,vertex.NormalZ/127.0);
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Polygons");
                    foreach (OldModelPolygon polygon in model.Polygons)
                    {
                        obj.WriteLine("f {0}//{0} {1}//{1} {2}//{2}", polygon.VertexA / 6 + 1,polygon.VertexB / 6 + 1,polygon.VertexC / 6 + 1);
                    }
                }
                return stream.ToArray();
            }
        }
    }
}
