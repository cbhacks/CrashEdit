using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crash.UI
{
    public abstract class Controller : IDisposable
    {
        private EvList<Controller> subcontrollers;

        public event EventHandler Invalidated;
        public event EvListEventHandler<Controller> DeepItemAdded;
        public event EvListEventHandler<Controller> DeepItemRemoved;

        public Controller()
        {
            this.subcontrollers = new EvList<Controller>();
            this.subcontrollers.ItemAdded += Subcontrollers_ItemAdded;
            this.subcontrollers.ItemRemoved += Subcontrollers_ItemRemoved;
            this.subcontrollers.Populate(Subcontrollers_ItemAdded);
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

        public virtual Control CreateControl()
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

        public void DeepPopulate(EvListEventHandler<Controller> handler)
        {
            for (int i = 0;i < subcontrollers.Count;i++)
            {
                EvListEventArgs<Controller> e = new EvListEventArgs<Controller>();
                e.Index = i;
                e.Item = subcontrollers[i];
                handler(this,e);
                e.Item.DeepPopulate(handler);
            }
        }

        private void Subcontrollers_ItemAdded(object sender,EvListEventArgs<Controller> e)
        {
            Subcontrollers_DeepItemAdded(this,e);
            e.Item.DeepItemAdded += Subcontrollers_DeepItemAdded;
            e.Item.DeepItemRemoved += Subcontrollers_DeepItemRemoved;
            e.Item.DeepPopulate(Subcontrollers_DeepItemAdded);
        }

        private void Subcontrollers_ItemRemoved(object sender,EvListEventArgs<Controller> e)
        {
            e.Item.Subcontrollers.Clear();
            Subcontrollers_DeepItemRemoved(this,e);
            e.Item.Dispose();
        }

        private void Subcontrollers_DeepItemAdded(object sender,EvListEventArgs<Controller> e)
        {
            if (DeepItemAdded != null)
            {
                DeepItemAdded(sender,e);
            }
        }

        private void Subcontrollers_DeepItemRemoved(object sender,EvListEventArgs<Controller> e)
        {
            if (DeepItemRemoved != null)
            {
                DeepItemRemoved(sender,e);
            }
        }
    }
}
