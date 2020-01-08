using System;

namespace Crash
{
    public struct ProtoSceneryPolygon
    {
        public static ProtoSceneryPolygon Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 12)
                throw new ArgumentException("Value must be 12 bytes long.","data");
            int[] words = new int[3] { BitConv.FromInt32(data, 0),BitConv.FromInt32(data,4),BitConv.FromInt32(data,8) };
            short texture = (short)(words[0] & 0xFFF);
            int page = words[0] >> 12 & 0x7;
            int unknown = words[1] >> 4 & 0xFFFFFFF; // color? animated texture info?
            short vertexa = (short)(words[2] & 0xFFF);
            short vertexb = (short)(words[2] >> 12 & 0xFFF);
            short vertexc = (short)(((words[1] & 0xF) << 8) | (words[2] >> 24 & 0xFF));
            return new ProtoSceneryPolygon(texture,page,unknown,vertexa,vertexb,vertexc);
        }

        public ProtoSceneryPolygon(short texture,int page,int unknown,short vertexa,short vertexb,short vertexc)
        {
            Texture = texture;
            Page = page;
            Unknown = unknown;
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
        }

        public short VertexA { get; set; }
        public short VertexB { get; set; }
        public short VertexC { get; set; }
        public short Texture { get; set; }
        public int Page { get; set; }
        public int Unknown { get; set; }

        public byte[] Save()
        {
            byte[] data = new byte [12];
            int worda = Texture | (Page << 12);
            int wordb = (VertexC >> 8 & 0xF) | (Unknown << 4);
            int wordc = VertexA | (VertexB << 12) | ((VertexC & 0xFF) << 24);
            BitConv.ToInt32(data,0,worda);
            BitConv.ToInt32(data,4,wordb);
            BitConv.ToInt32(data,8,wordc);
            return data;
        }
    }
}
