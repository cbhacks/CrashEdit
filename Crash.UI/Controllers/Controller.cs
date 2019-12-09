using System;
using System.Drawing;
using System.Windows.Forms;

namespace Crash.UI
{
    public abstract class Controller : IDisposable
    {
        public event EventHandler Invalidated;
        public event EvListEventHandler<Controller> DeepItemAdded;
        public event EvListEventHandler<Controller> DeepItemRemoved;

        public Controller()
        {
            Subcontrollers = new EvList<Controller>();
            Subcontrollers.ItemAdded += Subcontrollers_ItemAdded;
            Subcontrollers.ItemRemoved += Subcontrollers_ItemRemoved;
            Subcontrollers.Populate(Subcontrollers_ItemAdded);
        }

        public virtual string ImageKey => GetType().Name;

        public virtual Color ForeColor => Color.Empty;
        public virtual Color BackColor => Color.Empty;

        public EvList<Controller> Subcontrollers { get; }

        public virtual Control CreateControl()
        {
            Label label = new Label
            {
                AutoSize = false,
                Width = 200,
                Height = 200,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = Properties.Resources.Controller_NoOptionsAvailable
            };
            return label;
        }

        protected void Invalidate()
        {
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Dispose()
        {
            foreach (Controller subcontroller in Subcontrollers)
            {
                subcontroller.Dispose();
            }
        }

        public void DeepPopulate(EvListEventHandler<Controller> handler)
        {
            for (int i = 0;i < Subcontrollers.Count;i++)
            {
                EvListEventArgs<Controller> e = new EvListEventArgs<Controller>();
                e.Index = i;
                e.Item = Subcontrollers[i];
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
            DeepItemAdded?.Invoke(sender, e);
        }

        private void Subcontrollers_DeepItemRemoved(object sender,EvListEventArgs<Controller> e)
        {
            DeepItemRemoved?.Invoke(sender, e);
        }
    }
}
