using CrashEdit.Crash;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public abstract class LegacyController : CrashEdit.LegacyController
    {
        public LegacyController(SubcontrollerGroup parentGroup, object resource) : base(parentGroup, resource) {}

        protected void AddMenu(string text,ControllerMenuDelegate proc)
        {
            LegacyVerbs.Add(new LegacyVerb(text, new Action(proc)));
        }

        protected void AddMenuSeparator()
        {
            // FIXME
        }

        public GameVersion GameVersion =>
            (Modern.Root.Resource as LevelWorkspace)?.GameVersion ?? GameVersion.None;

        public NSF GetNSF() {
            return (Modern.Root.Resource as LevelWorkspace)?.NSF;
        }

        public T GetEntry<T>(int eid) where T : class, IEntry {
            return (Modern.Root.Resource as LevelWorkspace)?.NSF?.GetEntry<T>(eid);
        }

        public IEnumerable<T> GetEntries<T>() where T : class, IEntry {
            return (Modern.Root.Resource as LevelWorkspace)?.NSF?.GetEntries<T>();
        }
    }
}
