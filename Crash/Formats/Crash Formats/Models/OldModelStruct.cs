using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crash
{
    public interface OldModelStruct
    {
    }
    public struct OldModelColor : OldModelStruct
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public static OldModelColor Load(byte[] data)
        {
            byte r = data[2];
            byte g = data[1];
            byte b = data[0];
            return new OldModelColor(r, g, b);
        }

        public OldModelColor(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }
    }
}
