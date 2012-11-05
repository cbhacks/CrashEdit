using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class HexBox : Control
    {
        int offset;
        int position;
        private List<byte> data;

        public HexBox()
        {
            data = new List<byte>();
            data.Add(0);
            TabStop = true;
            SetStyle(ControlStyles.Selectable,true);
            DoubleBuffered = true;
        }

        public byte[] Data
        {
            get { return data.ToArray(); }
            set
            {
                data.Clear();
                data.AddRange(value);
                if (data.Count == 0)
                {
                    data.Add(0);
                }
            }
        }

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                if (position >= data.Count)
                {
                    position = data.Count - 1;
                }
                if (position < 0)
                {
                    position = 0;
                }
                while (offset < position / 16 - 15)
                {
                    offset++;
                }
                while (offset > position / 16)
                {
                    offset--;
                }
                Invalidate();
            }
        }

        public void MoveUp()
        {
            Position -= 16;
        }

        public void MoveUpPage()
        {
            Position -= 256;
        }

        public void MoveDown()
        {
            Position += 16;
        }

        public void MoveDownPage()
        {
            Position += 256;
        }

        public void MoveLeft()
        {
            Position--;
        }

        public void MoveRight()
        {
            Position++;
        }

        public void MoveHome()
        {
            Position = 0;
            offset = 0;
        }

        public void MoveHomeLine()
        {
            Position -= Position % 16;
        }

        public void MoveEnd()
        {
            Position = int.MaxValue;
            offset = Position / 16 - 15;
            if (offset < 0)
            {
                offset = 0;
            }
        }

        public void MoveEndLine()
        {
            Position = Position - Position % 16 + 15;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(System.EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    MoveUp();
                    break;
                case Keys.Down:
                    MoveDown();
                    break;
                case Keys.Left:
                    MoveLeft();
                    break;
                case Keys.Right:
                    MoveRight();
                    break;
                case Keys.PageUp:
                    MoveUpPage();
                    break;
                case Keys.PageDown:
                    MoveDownPage();
                    break;
                case Keys.Home:
                    if (e.Control)
                    {
                        MoveHome();
                    }
                    else
                    {
                        MoveHomeLine();
                    }
                    break;
                case Keys.End:
                    if (e.Control)
                    {
                        MoveEnd();
                    }
                    else
                    {
                        MoveEndLine();
                    }
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen borderpen = new Pen(Brushes.Black);
            Brush brush = Brushes.Navy;
            Brush backbrush = Brushes.White;
            Brush selbrush = Brushes.White;
            Brush selbackbrush = Brushes.Navy;
            Brush deadselbackbrush = Brushes.DarkGray;
            Brush voidbrush = Brushes.DarkMagenta;
            Font font = new Font(FontFamily.GenericMonospace,8);
            Font selfont = new Font(FontFamily.GenericMonospace,10);
            StringFormat format = new StringFormat();
            StringFormat selformat = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            selformat.Alignment = StringAlignment.Center;
            selformat.LineAlignment = StringAlignment.Center;
            int hstep = (Width - 1) / 16;
            int vstep = (Height - 1) / 16;
            int left = Location.X;
            int right = Location.X + hstep * 16;
            int top = Location.Y;
            int bottom = Location.Y + vstep * 16;
            int xsel = position % 16;
            int ysel = position / 16;
            for (int y = 0;y < 16;y++)
            {
                for (int x = 0;x < 16;x++)
                {
                    Font curfont;
                    Brush curbrush;
                    Brush curbackbrush;
                    StringFormat curformat;
                    Rectangle rect = new Rectangle();
                    rect.X = left + hstep * x + 1;
                    rect.Y = left + vstep * y + 1;
                    rect.Width = hstep - 1;
                    rect.Height = vstep - 1;
                    if (x == xsel && y + offset == ysel)
                    {
                        curfont = selfont;
                        curbrush = selbrush;
                        curbackbrush = Focused ? selbackbrush : deadselbackbrush;
                        curformat = selformat;
                    }
                    else if (x + (offset + y) * 16 < data.Count)
                    {
                        curfont = font;
                        curbrush = brush;
                        curbackbrush = backbrush;
                        curformat = format;
                    }
                    else
                    {
                        curfont = null;
                        curbrush = null;
                        curbackbrush = voidbrush;
                        curformat = null;
                    }
                    e.Graphics.FillRectangle(curbackbrush,rect);
                    if (x + (offset + y) * 16 < data.Count)
                    {
                        string text = data[x + (offset + y) * 16].ToString("X2");
                        e.Graphics.DrawString(text,curfont,curbrush,rect,curformat);
                    }
                }
            }
            for (int i = 0; i < 17; i++)
            {
                e.Graphics.DrawLine(borderpen,left + hstep * i,top,left + hstep * i,bottom);
                e.Graphics.DrawLine(borderpen,left,top + vstep * i,right,top + vstep * i);
            }
        }
    }
}
