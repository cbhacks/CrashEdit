using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrashEdit
{
    public static class ControlExt
    {
        public static IEnumerable<Control> GetAll(this Control control,Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => ctrl.GetAll(type)).Concat(controls).Where(c => c.GetType() == type);
        }
    }
}
