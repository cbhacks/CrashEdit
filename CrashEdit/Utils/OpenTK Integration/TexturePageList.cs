using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrashEdit.CE
{
    public sealed class TexturePageList : Dictionary<int, short>
    {
        public void AddTexturePage(int eid)
        {
            if (!ContainsKey(eid))
                this[eid] = (short)Count;
        }
    }
}
