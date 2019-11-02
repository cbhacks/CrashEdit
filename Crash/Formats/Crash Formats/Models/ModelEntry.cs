using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class ModelEntry : Entry
    {
        private List<ModelPolygon> polygons;
        private List<SceneryColor> colors;
        private List<ModelTexture> textures;
        private List<ModelAnimatedTexture> animatedtextures;
        private List<ModelPosition> positions;

        public ModelEntry(byte[] info,IEnumerable<ModelPolygon> polygons,IEnumerable<SceneryColor> colors,IEnumerable<ModelTexture> textures,IEnumerable<ModelAnimatedTexture> animatedtextures,IEnumerable<ModelPosition> positions,int eid,int size) : base(eid,size)
        {
            if (polygons == null)
                throw new ArgumentNullException("polygons");
            Info = info ?? throw new ArgumentNullException("info");
            this.polygons = new List<ModelPolygon>(polygons);
            this.colors = new List<SceneryColor>(colors);
            this.textures = new List<ModelTexture>(textures);
            this.animatedtextures = new List<ModelAnimatedTexture>(animatedtextures);
            if (positions != null)
                this.positions = new List<ModelPosition>(positions);
            else
                this.positions = null;
        }

        public override int Type => 2;
        public byte[] Info { get; }
        public IList<ModelPolygon> Polygons => polygons;
        public IList<SceneryColor> Colors => colors;
        public IList<ModelTexture> Textures => textures;
        public IList<ModelAnimatedTexture> AnimatedTextures => animatedtextures;
        public IList<ModelPosition> Positions => positions;

        public override UnprocessedEntry Unprocess()
        {
            //ErrorManager.SignalError("ModelEntry cannot be saved.");
            byte itemcount = 5;
            if (Positions != null)
                itemcount = 6;
            byte[][] items = new byte [itemcount][];
            items[0] = Info;
            items[1] = new byte [polygons.Count * 4];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 4);
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
