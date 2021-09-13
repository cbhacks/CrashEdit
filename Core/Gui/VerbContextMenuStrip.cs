#nullable enable

using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;

namespace CrashEdit {

    public class VerbContextMenuStrip : ContextMenuStrip {

        public VerbContextMenuStrip(IVerbExecutor executor) {
            if (executor == null)
                throw new ArgumentNullException();

            Executor = executor;
            ImageList = Embeds.ImageList;
        }

        public IVerbExecutor Executor { get; }

        public Controller? Subject { get; set; }

        protected override void OnOpening(CancelEventArgs e) {
            if (Subject == null) {
                e.Cancel = true;
            } else {
                Items.Clear();

                var directVerbs = Verb.AllVerbs
                    .OfType<DirectVerb>()
                    .Where(x => x.ApplicableForSubject(Subject))
                    .ToList();
                foreach (var verb in directVerbs) {
                    var item = new ToolStripMenuItem();
                    item.Text = verb.Text;
                    item.ImageKey = verb.ImageKey;
                    item.Click += (sender, e) => {
                        var newVerb = (DirectVerb)verb.Clone();
                        newVerb.Subject = Subject;
                        Executor.ExecuteVerb(newVerb);
                    };
                    Items.Add(item);
                }

                if (Subject.Legacy != null) {
                    foreach (var legacyVerb in Subject.Legacy.LegacyVerbs) {
                        var item = new ToolStripMenuItem();
                        item.Text = legacyVerb.Text;
                        item.Click += (sender, e) => {
                            Executor.ExecuteVerb(legacyVerb);
                        };
                        Items.Add(item);
                    }
                }

                e.Cancel = (Items.Count == 0);
            }
            base.OnOpening(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e) {
            Items.Clear();
            base.OnClosed(e);
        }

    }

}
