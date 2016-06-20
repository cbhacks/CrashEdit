using Crash;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class HexBox : Control
    {
        private int offset;
        private int position;
        private int? input;
        private byte[] data;
        private int viewbit;
        private bool eidview;

        public HexBox()
        {
            offset = 0;
            position = 0;
            input = null;
            data = new byte [0];
            viewbit = 8;
            eidview = false;
            TabStop = true;
            SetStyle(ControlStyles.Selectable,true);
            DoubleBuffered = true;
        }

        public byte[] Data
        {
            //get { return data; }
            set { data = value; }
        }

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                if (position >= data.Length)
                {
                    position = data.Length - 1;
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

        public void InputNibble(int value)
        {
            if (input == null)
            {
                input = value;
            }
            else
            {
                if (position < data.Length)
                {
                    data[position] = (byte)((input << 4) | value);
                }
                input = null;
            }
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.Z:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:
                case Keys.Space:
                case Keys.N:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
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
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                    if (e.Control)
                    {
                        viewbit = e.KeyCode - Keys.D0;
                        Invalidate();
                    }
                    else
                    {
                        InputNibble(e.KeyCode - Keys.D0);
                    }
                    break;
                case Keys.D9:
                    InputNibble(e.KeyCode - Keys.D0);
                    break;
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                    if (e.Control)
                    {
                        viewbit = e.KeyCode - Keys.NumPad0;
                        Invalidate();
                    }
                    else
                    {
                        InputNibble(e.KeyCode - Keys.NumPad0);
                    }
                    break;
                case Keys.NumPad9:
                    InputNibble(e.KeyCode - Keys.NumPad0);
                    break;
                case Keys.A:
                    InputNibble(0xA);
                    break;
                case Keys.B:
                    InputNibble(0xB);
                    break;
                case Keys.C:
                    InputNibble(0xC);
                    break;
                case Keys.D:
                    InputNibble(0xD);
                    break;
                case Keys.E:
                    InputNibble(0xE);
                    break;
                case Keys.F:
                    InputNibble(0xF);
                    break;
                case Keys.N:
                    InputNibble(0x7);
                    InputNibble(0xF);
                    MoveRight();
                    InputNibble(0x3);
                    InputNibble(0x4);
                    MoveRight();
                    InputNibble(0x9);
                    InputNibble(0x6);
                    MoveRight();
                    InputNibble(0x6);
                    InputNibble(0x3);
                    MoveRight();
                    break;
                case Keys.Z:
                    eidview = !eidview;
                    Invalidate();
                    break;
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
                case Keys.Space:
                    if (e.Control)
                    {
                        if (MessageBox.Show("Are you sure?","Nullify",MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            for (int i = 0;i < data.Length;i++)
                            {
                                data[i] = 0;
                            }
                        }
                    }
                    else
                    {
                        data[position] = 0;
                    }
                    Invalidate();
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Brush borderbrush = Brushes.Black;
            Brush brush = Brushes.Navy;
            Brush backbrush = Brushes.White;
            Brush hibackbrush = Brushes.LightGreen;
            Brush bithibackbrush = Brushes.LightCoral;
            Brush selbrush = Brushes.White;
            Brush selbackbrush = Brushes.Navy;
            Brush deadselbackbrush = Brushes.DarkGray;
            Brush inputselbackbrush = Brushes.Red;
            Brush eidbackbrush = Brushes.Chocolate;
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
            int width = hstep * 16;
            int height = vstep * 16;
            int xsel = position % 16;
            int ysel = position / 16;
            e.Graphics.FillRectangle(borderbrush,0,0,width + 1,height + 1);
            for (int y = 0;y < 16;y++)
            {
                for (int x = 0;x < 16;x++)
                {
                    int i = x + (offset + y) * 16;
                    Font curfont;
                    Brush curbrush;
                    Brush curbackbrush;
                    StringFormat curformat;
                    Rectangle rect = new Rectangle();
                    rect.X = hstep * x + 1;
                    rect.Y = vstep * y + 1;
                    rect.Width = hstep - 1;
                    rect.Height = vstep - 1;
                    string text;
                    if (eidview && x % 4 == 0 && i + 3 < data.Length && (data[i] & 1) != 0 && (data[i + 3] & 128) == 0)
                    {
                        curfont = font;
                        curbrush = selbrush;
                        curbackbrush = eidbackbrush;
                        curformat = format;
                        rect.Width = hstep * 4 - 1;
                        int eid = BitConv.FromInt32(data,i);
                        e.Graphics.FillRectangle(curbackbrush,rect);
                        e.Graphics.DrawString(Entry.EIDToEName(eid),curfont,curbrush,rect,curformat);
                        x += 3;
                        continue;
                    }
                    if (x == xsel && y + offset == ysel && x + y * 16 < data.Length)
                    {
                        curfont = selfont;
                        curbrush = selbrush;
                        curformat = selformat;
                        if (input == null)
                        {
                            curbackbrush = Focused ? selbackbrush : deadselbackbrush;
                            text = data[x + (offset + y) * 16].ToString("X2");
                        }
                        else
                        {
                            curbackbrush = Focused ? inputselbackbrush : deadselbackbrush;
                            text = ((int)input).ToString("X");
                        }
                    }
                    else if (x + (offset + y) * 16 < data.Length)
                    {
                        curfont = font;
                        curbrush = brush;
                        if (viewbit != 8)
                        {
                            curbackbrush = ((data[x + (offset + y) * 16] & 1 << viewbit) != 0) ? bithibackbrush : backbrush;
                        }
                        else
                        {
                            curbackbrush = (data[x + (offset + y) * 16] != 0) ? hibackbrush : backbrush;
                        }
                        curformat = format;
                        text = data[x + (offset + y) * 16].ToString("X2");
                    }
                    else
                    {
                        curfont = null;
                        curbrush = null;
                        curbackbrush = voidbrush;
                        curformat = null;
                        text = "";
                    }
                    e.Graphics.FillRectangle(curbackbrush,rect);
                    if (x + (offset + y) * 16 < data.Length)
                    {
                        e.Graphics.DrawString(text,curfont,curbrush,rect,curformat);
                    }
                }
            }
        }
    }
}
