using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class ModelEntry : Entry
    {
        private List<int> positionindices;
        private List<int> colorindices;
        private List<byte> orientations;
        private List<SceneryColor> colors;
        private List<ModelTexture> textures;
        private List<ModelAnimatedTexture> animatedtextures;
        private List<ModelPosition> positions;

        public ModelEntry(byte[] info,uint[] polygons,IEnumerable<SceneryColor> colors,IEnumerable<ModelTexture> textures,IEnumerable<ModelAnimatedTexture> animatedtextures,IEnumerable<ModelPosition> positions,int eid,int size) : base(eid,size)
        {
            Info = info ?? throw new ArgumentNullException("info");
            PolyData = polygons ?? throw new ArgumentNullException("polygons");
            this.colors = new List<SceneryColor>(colors);
            this.textures = new List<ModelTexture>(textures);
            this.animatedtextures = new List<ModelAnimatedTexture>(animatedtextures);
            if (positions != null)
                this.positions = new List<ModelPosition>(positions);
            else
                this.positions = null;
            ConvertIndices();
        }

        private void ConvertIndices()
        {
            positionindices = new List<int>();
            Dictionary<byte, int> p = new Dictionary<byte, int>();
            int v = positions == null ? 0 : -BitConv.FromInt32(Info, 0x4C); // special vertex count, let's get rid of it for compressed models
            List<int> vtx = new List<int>();
            int lastvalidcc = -3; // dirty hack
            int lastccpos = -1;
            int lastaapos = -1;
            ModelColor lastfirstcolor = new ModelColor();
            ModelColor lastsecondcolor;
            for (int i = 0; i < PolyData.Length; ++i) // pre-pass (ugh)
            {
                ModelStruct s = ConvertPolyItem(PolyData[i]);
                if (s == null) // footer
                    break;
                else if (s is ModelColor c) // color
                    continue;
                else if (s is ModelTriangle t) // index
                {
                    if (t.Type == ModelTriangle.IndexType.Original)
                    {
                        if (t.PositionKey != ModelTriangle.NullPtr)
                        {
                            if (p.ContainsKey(t.PositionKey))
                                p[t.PositionKey] = v;
                            else
                                p.Add(t.PositionKey, v);
                        }
                        vtx.Add(v++);
                    }
                    else if (t.Type == ModelTriangle.IndexType.Duplicate)
                    {
                        vtx.Add(p[t.PositionKey]);
                    }
                    else
                        throw new Exception();
                }
                else
                    throw new Exception();
            }
            for (int i = 0, cur_v = 0; i < PolyData.Length;++i)
            {
                ModelStruct s = ConvertPolyItem(PolyData[i]);
                if (s == null) // footer
                    break;
                else if (s is ModelColor c) // color
                {
                    lastsecondcolor = lastfirstcolor;
                    lastfirstcolor = c;
                }
                else if (s is ModelTriangle t) // index
                {
                    if (t.Type == ModelTriangle.IndexType.Original)
                    {
                        switch (t.TriangleType)
                        {
                            case 0:
                                positionindices.Add(vtx[cur_v]);
                                positionindices.Add(vtx[cur_v-1]);
                                positionindices.Add(vtx[cur_v-2]);
                                lastaapos = cur_v;
                                break;
                            case 1:
                                positionindices.Add(vtx[cur_v]);
                                positionindices.Add(vtx[cur_v-1]);
                                positionindices.Add(lastccpos > lastaapos ? vtx[lastccpos] : vtx[lastaapos-2]);
                                break;
                            case 2:
                                if (i + 2 < PolyData.Length && lastvalidcc + 2 < i)
                                {
                                    lastvalidcc = i;
                                    positionindices.Add(vtx[cur_v]);
                                    positionindices.Add(vtx[cur_v+1]);
                                    positionindices.Add(vtx[cur_v+2]);
                                }
                                lastccpos = cur_v;
                                break;
                        }
                    }
                    else if (t.Type == ModelTriangle.IndexType.Duplicate)
                    {
                        switch (t.TriangleType)
                        {
                            case 0:
                                positionindices.Add(vtx[cur_v]);
                                positionindices.Add(vtx[cur_v-1]);
                                positionindices.Add(vtx[cur_v-2]);
                                lastaapos = cur_v;
                                break;
                            case 1:
                                positionindices.Add(vtx[cur_v]);
                                positionindices.Add(vtx[cur_v-1]);
                                positionindices.Add(lastccpos > lastaapos ? vtx[lastccpos] : vtx[lastaapos-2]);
                                break;
                            case 2:
                                if (i + 2 < PolyData.Length && lastvalidcc + 2 < i)
                                {
                                    lastvalidcc = i;
                                    positionindices.Add(vtx[cur_v]);
                                    positionindices.Add(vtx[cur_v+1]);
                                    positionindices.Add(vtx[cur_v+2]);
                                }
                                lastccpos = cur_v;
                                break;
                        }
                    }
                    switch (t.TriangleSubtype)
                    {
                        case 0: // front and back
                            orientations.Add(0);
                            break;
                        case 1: // clockwise
                            orientations.Add(0);
                            break;
                        case 2: // counter-clockwise
                            orientations.Add(0);
                            break;
                    }
                    ++cur_v;
                }
            }
        }

        private static ModelStruct ConvertPolyItem(uint item)
        {
            if (item == 0xFFFFFFFF)
            {
                return null;
            }
            else if ((item & 0xFFFF0000) != 0) // TODO check for a better mask
            {
                return ModelTriangle.Load(item);
            }
            else
            {
                return ModelColor.Load(item);
            }
        }

        public override int Type => 2;
        public byte[] Info { get; }
        public uint[] PolyData { get; }
        public IList<int> PositionIndices => positionindices;
        public IList<int> ColorIndices => colorindices;
        public IList<SceneryColor> Colors => colors;
        public IList<ModelTexture> Textures => textures;
        public IList<ModelAnimatedTexture> AnimatedTextures => animatedtextures;
        public IList<ModelPosition> Positions => positions;

        public override UnprocessedEntry Unprocess()
        {
            byte itemcount = 5;
            if (Positions != null)
                itemcount = 6;
            byte[][] items = new byte [itemcount][];
            items[0] = Info;
            items[1] = new byte [PolyData.Length * 4];
            for (int i = 0;i < PolyData.Length; i++)
            {
                BitConv.ToInt32(items[1],i*4,(int)PolyData[i]);
            }
            items[2] = new byte[colors.Count * 4];
            for (int i = 0;i < colors.Count;i++)
            {
                items[2][i * 4] = Colors[i].Red;
                items[2][i * 4 + 1] = Colors[i].Green;
                items[2][i * 4 + 2] = Colors[i].Blue;
                items[2][i * 4 + 3] = Colors[i].Extra;
            }
            items[3] = new byte[textures.Count * 12];
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i].Save().CopyTo(items[3], i * 12);
            }
            items[4] = new byte[animatedtextures.Count * 4];
            for (int i = 0; i < animatedtextures.Count; i++)
            {
                animatedtextures[i].Save().CopyTo(items[4], i * 4);
            }
            if (itemcount == 6)
            {
                items[5] = new byte[positions.Count * 4];
                for (int i = 0; i < positions.Count; i++)
                {
                    positions[i].Save().CopyTo(items[5], i * 4);
                }
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
