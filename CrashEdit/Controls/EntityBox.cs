using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class EntityBox : UserControl
    {
        private Entity entity;

        public EntityBox(Entity entity)
        {
            this.entity = entity;
        }
    }
}
