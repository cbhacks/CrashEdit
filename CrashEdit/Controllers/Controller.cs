using CrashEdit.Crash;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public abstract class LegacyController : CrashEdit.LegacyController
    {
        public LegacyController(LegacyController parent, object resource) : base(parent, resource)
        {
        }

        public LegacyController(SubcontrollerGroup parentGroup, object resource) : base(parentGroup, resource) {}

        public void AddNode(LegacyController controller)
        {
            if (controller.Parent != this) {
                throw new Exception();
            }
            LegacySubcontrollers.Add(controller);
        }

        public void InsertNode(int index,LegacyController controller)
        {
            if (controller.Parent != this) {
                throw new Exception();
            }
            LegacySubcontrollers.Insert(index, controller);
        }

        protected void AddMenu(string text,ControllerMenuDelegate proc)
        {
            LegacyVerbs.Add(new LegacyVerb(text, new Action(proc)));
        }

        protected void AddMenuSeparator()
        {
            // FIXME
        }

        public abstract void InvalidateNode();

        public virtual void InvalidateNodeImage() {}

        public void RemoveSelf()
        {
            Parent?.LegacySubcontrollers?.Remove(this);
        }
    }
}
