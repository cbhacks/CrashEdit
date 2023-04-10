using System.IO;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SceneryEntry : Entry
    {
        private List<SceneryVertex> vertices;
        private List<SceneryTriangle> triangles;
        private List<SceneryQuad> quads;
        private List<ModelTexture> textures;
        private List<SceneryColor> colors;
        private List<ModelExtendedTexture> animatedtextures;

        public SceneryEntry(byte[] info,IEnumerable<SceneryVertex> vertices,IEnumerable<SceneryTriangle> triangles,IEnumerable<SceneryQuad> quads,IEnumerable<ModelTexture> textures,IEnumerable<SceneryColor> colors,IEnumerable<ModelExtendedTexture> animatedtextures,bool is_c3,int eid)
            : base(eid)
        {
            Info = info;
            this.vertices = new List<SceneryVertex>(vertices);
            this.triangles = new List<SceneryTriangle>(triangles);
            this.quads = new List<SceneryQuad>(quads);
            this.textures = new List<ModelTexture>(textures);
            this.colors = new List<SceneryColor>(colors);
            this.animatedtextures = new List<ModelExtendedTexture>(animatedtextures);
            IsC3 = is_c3;
        }

        public override int Type => 3;
        public byte[] Info { get; }
        public IList<SceneryVertex> Vertices => vertices;
        public IList<SceneryTriangle> Triangles => triangles;
        public IList<SceneryQuad> Quads => quads;
        public IList<ModelTexture> Textures => textures;
        public IList<SceneryColor> Colors => colors;
        public IList<ModelExtendedTexture> AnimatedTextures => animatedtextures;
        public bool IsC3 { get; }

        public int XOffset
        {
            get => BitConv.FromInt32(Info,0);
            set => BitConv.ToInt32(Info,0,value);
        }

        public int YOffset
        {
            get => BitConv.FromInt32(Info,4);
            set => BitConv.ToInt32(Info,4,value);
        }

        public int ZOffset
        {
            get => BitConv.FromInt32(Info,8);
            set => BitConv.ToInt32(Info,8,value);
        }

        public bool IsSky
        {
            get => BitConv.FromInt32(Info, 12) != 0;
            set => BitConv.ToInt32(Info, 12, value ? 1 : 0);
        }

        public int TPAGCount => BitConv.FromInt32(Info, 0x28);

        public int GetTPAG(int idx) => BitConv.FromInt32(Info, 0x2C + 4 * idx);

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [7][];
            items[0] = Info;
            items[1] = new byte [Aligner.Align(vertices.Count * 6,4)];
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].SaveXY().CopyTo(items[1],(vertices.Count - 1 - i) * 4);
                vertices[i].SaveZ().CopyTo(items[1],vertices.Count * 4 + i * 2);
            }
            items[2] = new byte [Aligner.Align(triangles.Count * 6,4)];
            for (int i = 0;i < triangles.Count;i++)
            {
                triangles[i].SaveA().CopyTo(items[2],(triangles.Count - 1 - i) * 4);
                triangles[i].SaveB().CopyTo(items[2],triangles.Count * 4 + i * 2);
            }
            items[3] = new byte [quads.Count * 8];
            for (int i = 0;i < quads.Count;i++)
            {
                quads[i].Save().CopyTo(items[3],i * 8);
            }
            items[4] = new byte[textures.Count * 12];
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i].Save().CopyTo(items[4], i * 12);
            }
            items[5] = new byte[colors.Count * 4];
            for (int i = 0; i < colors.Count; i++)
            {
                items[5][i * 4] = Colors[i].Red;
                items[5][i * 4 + 1] = Colors[i].Green;
                items[5][i * 4 + 2] = Colors[i].Blue;
                items[5][i * 4 + 3] = Colors[i].Extra;
            }
            items[6] = new byte[animatedtextures.Count * 4];
            for (int i = 0; i < animatedtextures.Count; i++)
            {
                animatedtextures[i].Save().CopyTo(items[6], i * 4);
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
