using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{

    public class VerbContextMenuStrip : ContextMenuStrip
    {

        public VerbContextMenuStrip(IVerbExecutor executor)
        {
            ArgumentNullException.ThrowIfNull(executor);

            Executor = executor;
            ImageList = Embeds.ImageList;
            Renderer = new CustomRenderer();
        }

        // Custom renderer to draw heading labels.
        private sealed class CustomRenderer : ToolStripProfessionalRenderer
        {
            public static object HeadingTag = new object();

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (e.Item.Tag == HeadingTag)
                {
                    var rect = new Rectangle();
                    rect.Width = e.Item.Width;
                    rect.Height = e.Item.Height;

                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    using (var brush = new SolidBrush(e.Item.BackColor))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }

                    using (var brush = new SolidBrush(e.Item.ForeColor))
                    {
                        e.Graphics.DrawString(e.Item.Text, e.Item.Font, brush, rect, format);
                    }
                }
                else
                {
                    base.OnRenderItemText(e);
                }
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Tag == HeadingTag)
                {
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
        }

        public IVerbExecutor Executor { get; }

        public Controller? Subject { get; set; }

        private void AddHeading(string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            Items.Add(new ToolStripMenuItem
            {
                Text = text,
                Enabled = false,
                Tag = CustomRenderer.HeadingTag, // use custom renderer to draw this
            });
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (Subject == null)
            {
                e.Cancel = true;
            }
            else
            {
                Items.Clear();

                var directVerbs = Verb.AllVerbs
                    .OfType<DirectVerb>()
                    .Where(x => x.ApplicableForSubject(Subject))
                    .ToList();
                foreach (var verb in directVerbs)
                {
                    if (Items.Count == 0)
                    {
                        AddHeading(Subject.Text);
                    }

                    var item = new ToolStripMenuItem();
                    item.Text = verb.Text;
                    item.ImageKey = verb.ImageKey;
                    item.Click += (sender, e) =>
                    {
                        var newVerb = (DirectVerb)verb.Clone();
                        newVerb.Subject = Subject;
                        Executor.ExecuteVerb(newVerb);
                    };
                    Items.Add(item);
                }

                if (Subject.Legacy != null)
                {
                    foreach (var legacyVerb in Subject.Legacy.LegacyVerbs)
                    {
                        if (Items.Count == 0)
                        {
                            AddHeading(Subject.Text);
                        }

                        var item = new ToolStripMenuItem();
                        item.Text = legacyVerb.Text;
                        item.Click += (sender, e) =>
                        {
                            Executor.ExecuteVerb(legacyVerb);
                        };
                        Items.Add(item);
                    }
                }

                foreach (var group in Subject.SubcontrollerGroups)
                {
                    var groupVerbs = Verb.AllVerbs
                        .OfType<GroupVerb>()
                        .Where(x => x.ApplicableForGroup(group))
                        .ToList();
                    if (groupVerbs.Count == 0)
                        continue;

                    AddHeading(group.Text);

                    foreach (var verb in groupVerbs)
                    {
                        var item = new ToolStripMenuItem();
                        item.Text = verb.Text;
                        item.ImageKey = verb.ImageKey;
                        item.Click += (sender, e) =>
                        {
                            var newVerb = (GroupVerb)verb.Clone();
                            newVerb.Group = group;
                            Executor.ExecuteVerb(newVerb);
                        };
                        Items.Add(item);
                    }
                }

                e.Cancel = (Items.Count == 0);
            }
            base.OnOpening(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            Items.Clear();
            base.OnClosed(e);
        }

    }

}
