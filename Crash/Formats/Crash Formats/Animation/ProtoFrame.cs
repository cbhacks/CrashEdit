using System;
using System.IO;
using System.Collections.Generic;

namespace Crash
{
    public class ProtoFrame
    {
        public static ProtoFrame Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 44)
            {
                ErrorManager.SignalError("ProtoFrame: Data is too short");
            }
            int vertexcount = BitConv.FromInt32(data,0);
            if (vertexcount < 0 || vertexcount > Chunk.Length / 6)
            {
                ErrorManager.SignalError("ProtoFrame: Vertex count is invalid");
            }
            if (data.Length < 44 + vertexcount * 6 + 2)
            {
                ErrorManager.SignalError("ProtoFrame: Data is too short");
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
            OldFrameVertex[] vertices = new OldFrameVertex [vertexcount];
            for (int i = 0;i < vertexcount;i++)
            {
                //byte[] vertexdata = new byte [6];
                //Array.Copy(data,56 + i * 6,vertexdata,0,vertexdata.Length);
                //vertices[i] = OldFrameVertex.Load(vertexdata);
                vertices[i] = new OldFrameVertex(data[0], data[1], data[2], data[3], data[4], data[5]);
            }
            short unknown = BitConv.FromInt16(data,44 + vertexcount * 6);
            return new ProtoFrame(modeleid,xoffset,yoffset,zoffset,x1,y1,z1,x2,y2,z2,vertices,unknown);
        }

        private List<OldFrameVertex> vertices;

        public ProtoFrame(int modeleid,int xoffset,int yoffset,int zoffset,int x1,int y1,int z1,int x2,int y2,int z2,IEnumerable<OldFrameVertex> vertices,short unknown)
        {
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
            this.vertices = new List<OldFrameVertex>(vertices);
            Unknown = unknown;
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
        public short Unknown { get; set; }

        public IList<OldFrameVertex> Vertices => vertices;

        public byte[] Save()
        {
            byte[] data = new byte [44 + vertices.Count * 6 + 2];
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
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].Save().CopyTo(data,44 + i * 6);
            }
            BitConv.ToInt16(data,44 + vertices.Count * 6,Unknown);
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
                    obj.WriteLine();
                    obj.WriteLine("# Polygons");
                    foreach (OldModelPolygon polygon in model.Polygons)
                    {
                        obj.WriteLine("f {0} {1} {2}",polygon.VertexA / 6 + 1,polygon.VertexB / 6 + 1,polygon.VertexC / 6 + 1);
                    }
                }
                return stream.ToArray();
            }
        }
    }
}
