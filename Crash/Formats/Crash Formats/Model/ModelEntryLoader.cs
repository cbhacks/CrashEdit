using System;

namespace Crash
{
    [EntryType(2,GameVersion.Crash2)]
    [EntryType(2,GameVersion.Crash3)]
    public sealed class ModelEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("ModelEntry: Wrong number of items");
            }
            int structcount = BitConv.FromInt32(items[0],0x2C);
            if (items[1].Length != structcount * 4)
            {
                ErrorManager.SignalError("ModelEntry: Polygon count mismatch");
            }
            int colorcount = BitConv.FromInt32(items[0], 0x3C);
            if (items[2].Length != colorcount * 4)
            {
                ErrorManager.SignalError("ModelEntry: Color count mismatch");
            }
            int texturecount = BitConv.FromInt32(items[0], 0x34);
            if (items[3].Length != texturecount * 12)
            {
                ErrorManager.SignalError("ModelEntry: Texture count mismatch");
            }
            int animatedtexturecount = BitConv.FromInt32(items[0], 0x48);
            if (items[4].Length != animatedtexturecount * 4)
            {
                ErrorManager.SignalError("ModelEntry: Animated texture count mismatch");
            }
            uint[] structs = new uint [structcount];
            for (int i = 0;i < structcount;++i)
            {
                structs[i] = (uint)BitConv.FromInt32(items[1], i*4);
            }
            SceneryColor[] colors = new SceneryColor[colorcount];
            for (int i = 0; i < colorcount; i++)
            {
                byte r = items[2][0+4*i];
                byte g = items[2][1+4*i];
                byte b = items[2][2+4*i];
                byte e = items[2][3+4*i];
                colors[i] = new SceneryColor(r,g,b,e);
            }
            ModelTexture[] textures = new ModelTexture[texturecount];
            for (int i = 0; i < texturecount; i++)
            {
                byte[] texturedata = new byte[12];
                Array.Copy(items[3], i * 12, texturedata, 0, texturedata.Length);
                textures[i] = ModelTexture.Load(texturedata);
            }
            ModelExtendedTexture[] animatedtextures = new ModelExtendedTexture[animatedtexturecount];
            for (int i = 0; i < animatedtexturecount; i++)
            {
                byte[] animatedtexturedata = new byte[4];
                Array.Copy(items[4], i * 4, animatedtexturedata, 0, animatedtexturedata.Length);
                animatedtextures[i] = ModelExtendedTexture.Load(animatedtexturedata);
            }
            int positioncount = BitConv.FromInt32(items[0],0x38) - BitConv.FromInt32(items[0],0x4C);
            ModelPosition[] positions = new ModelPosition[positioncount];
            if (items.Length == 6)
            {
                if (items[5].Length != positioncount * 4)
                {
                    ErrorManager.SignalError("ModelEntry: Compression deltas count mismatch");
                }
                for (int i = 0; i < positioncount; i++)
                {
                    byte[] positiondata = new byte[4];
                    Array.Copy(items[5], i * 4, positiondata, 0, positiondata.Length);
                    positions[i] = ModelPosition.Load(positiondata);
                }
            }
            else
            {
                positioncount = 0;
                positions = null;
            }
            return new ModelEntry(items[0],structs,colors,textures,animatedtextures,positions,eid);
        }
    }
}
