using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crash.UI
{
    public abstract class Controller : IDisposable
    {
        private EvList<Controller> subcontrollers;

        public event EventHandler Invalidated;

        public Controller()
        {
            this.subcontrollers = new EvList<Controller>();
        }

        public virtual string ImageKey
        {
            get { return GetType().Name; }
        }

        public virtual Color ForeColor
        {
            get { return Color.Empty; }
        }

        public virtual Color BackColor
        {
            get { return Color.Empty; }
        }

        public EvList<Controller> Subcontrollers
        {
            get { return subcontrollers; }
        }

        protected virtual Control CreateControl()
        {
            Label label = new Label();
            label.AutoSize = false;
            label.Width = 200;
            label.Height = 200;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = Properties.Resources.Controller_NoOptionsAvailable;
            return label;
        }

        protected void Invalidate()
        {
            if (Invalidated != null)
            {
                Invalidated(this,EventArgs.Empty);
            }
        }

        public virtual void Dispose()
        {
            foreach (Controller subcontroller in subcontrollers)
            {
                subcontroller.Dispose();
            }
        }
    }
}
