using System;

namespace Crash
{
    public struct OldSceneryPolygon
    {
        public static OldSceneryPolygon Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.","data");
            int worda = BitConv.FromInt32(data,0);
            int wordb = BitConv.FromInt32(data,4);
            int vertexa = (worda >> 20) & 0xFFF;
            int vertexb = (wordb >> 8) & 0xFFF;
            int vertexc = (wordb >> 20) & 0xFFF;
            int modelstruct = (worda >> 8) & 0xFFF;
            byte page = (byte)(worda >> 5 & 0b111);
            byte anim0 = (byte)(worda & 0x1F);
            byte unknown = (byte)(wordb & 0xFF);
            return new OldSceneryPolygon(vertexa,vertexb,vertexc,modelstruct,page,anim0,unknown);
        }

        public OldSceneryPolygon(int vertexa,int vertexb,int vertexc,int modelstruct,byte page,byte anim0,byte unknown)
        {
            if (vertexa < 0 || vertexa > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexa");
            if (vertexb < 0 || vertexb > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexb");
            if (vertexc < 0 || vertexc > 0xFFF)
                throw new ArgumentOutOfRangeException("vertexc");
            if (modelstruct < 0 || modelstruct > 0xFFF)
                throw new ArgumentOutOfRangeException("unknown1");
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
            ModelStruct = modelstruct;
            Page = page;
            Anim0 = anim0;
            Unknown = unknown;
        }

        public int VertexA { get; set; }
        public int VertexB { get; set; }
        public int VertexC { get; set; }
        public int ModelStruct { get; }
        public byte Page { get; }
        public byte Anim0 { get; }
        public byte Unknown { get; }

        public byte[] Save()
        {
            int worda = 0;
            int wordb = 0;
            worda |= VertexA << 20;
            wordb |= VertexB << 8;
            wordb |= VertexC << 20;
            worda |= ModelStruct << 8;
            worda |= Page << 5;
            worda |= Anim0;
            wordb |= Unknown;
            byte[] data = new byte [8];
            BitConv.ToInt32(data,0,worda);
            BitConv.ToInt32(data,4,wordb);
            return data;
        }
    }
}
