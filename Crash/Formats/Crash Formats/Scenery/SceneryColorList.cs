using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SceneryColorList
    {
        public static SceneryColorList Load(byte[] data)
        {
            if (data.Length < 4)
                ErrorManager.SignalError("SceneryColorList: Length is wrong");
            SceneryColor[] colors = new SceneryColor [data.Length / 4];
            for (int i = 0;i < data.Length / 4;i++)
            {
                byte red = data[0 + i * 4];
                byte green = data[1 + i * 4];
                byte blue = data[2 + i * 4];
                byte extra = data[3 + i * 4];
                colors[i] = new SceneryColor(red,green,blue,extra);
            }
            return new SceneryColorList(colors);
        }

        private List<SceneryColor> colors = null;

        public SceneryColorList(IEnumerable<SceneryColor> colors)
        {
            if (colors == null)
                throw new ArgumentNullException("colors");
            this.colors = new List<SceneryColor>(colors);
        }
        
        public IList<SceneryColor> Colors
        {
            get { return colors; }
        }
        
        public byte[] Save()
        {
            byte[] result = new byte [colors.Count * 4];
            for (int i = 0;i < colors.Count;i++)
            {
                result[0 + i * 4] = colors[i].Red;
                result[1 + i * 4] = colors[i].Green;
                result[2 + i * 4] = colors[i].Blue;
                result[3 + i * 4] = colors[i].Extra;
            }
            return result;
        }
    }
}
