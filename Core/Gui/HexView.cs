#nullable enable

using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit {

    public sealed class HexView : UserControl {

        public HexView() {
            ResetLayout();
        }

        // The number of columns in the viewer.
        private int _columnCount = 0x10;
        public int ColumnCount {
            get => _columnCount;
            set {
                if (value == _columnCount)
                    return;
                if (value < 1)
                    throw new ArgumentException();

                _columnCount = value;
                ResetLayout();
                Invalidate();
            }
        }

        // The number of columns per "group". Each group of columns has an alternating set
        // of background colors in the viewer, to make it easier to identify columns and word
        // boundaries. If set to zero, this functionality is disabled.
        private int _columnsPerGroup = 4;
        public int ColumnsPerGroup {
            get => _columnsPerGroup;
            set {
                if (value == _columnsPerGroup)
                    return;
                if (value < 0)
                    throw new ArgumentException();

                _columnsPerGroup = value;
                Invalidate();
            }
        }

        // The number of rows in the viewer. This is computed based on the column count and
        // data length. A cell is always included for one past the last byte.
        public int RowCount => (Data.Length / ColumnCount) + 1;

        // The number of rows advanced when the user hits page-up or page-down.
        public int RowsPerPage { get; set; } = 0x10;

        // The number of columns to skip before the first byte shown to the user.
        private int _firstByteColumn = 0;
        public int FirstByteColumn {
            get => _firstByteColumn;
            set {
                if (value == _firstByteColumn)
                    return;
                if (value < 0)
                    throw new ArgumentException();

                _firstByteColumn = value;
                ResetLayout();
                Invalidate();
            }
        }

        // The address of the first byte. This is for display purposes only; the first byte
        // is always accessed at Data[0], the second at Data[1], then Data[2], etc.
        private long _firstByteAddress = 0;
        public long FirstByteAddress {
            get => _firstByteAddress;
            set {
                if (value == _firstByteAddress)
                    return;

                _firstByteAddress = value;
                ResetLayout();
                Invalidate();
            }
        }

        // The address of the first row to display to the user.
        public long FirstRowAddress => FirstByteAddress - FirstByteColumn;

        // The currently selected byte, with zero being the first byte. This is limited to the
        // range [0 ... Data.Length] inclusive.
        public int ByteCursor { get; private set; }

        // The column number of the currently selected byte, zero being the first column.
        public int ByteCursorColumn => (FirstByteColumn + ByteCursor) % ColumnCount;

        // The row number of the currently selected byte, zero being the first row.
        public int ByteCursorRow => (FirstByteColumn + ByteCursor) / ColumnCount;

        // The address of the currently selected byte.
        public long ByteCursorAddress => FirstByteAddress + ByteCursor;

        // The data on which the HexView operates. The memory must remain valid until replaced
        // or until the control is disposed.
        //
        // Replacing this value with a shorter memory view will also clamp ByteCursor to the new
        // range.
        //
        // Invalidate() must be called manually whenever the contents of the memory change.
        private ReadOnlyMemory<byte> _data;
        public ReadOnlyMemory<byte> Data {
            get => _data;
            set {
                _data = value;
                if (ByteCursor > _data.Length) {
                    ByteCursor = _data.Length;
                }
                ResetLayout();
                Invalidate();
            }
        }

        // Optional function for implementing data changes.
        //
        // If not null, editing functionality will be enabled. When the user inputs changes
        // to the data, the HexView will not apply these changes directly itself, but will call
        // this function to attempt to do so.
        //
        // Parameters:
        //
        //  * int destOffset: Starting offset in Data of the bytes to be replaced.
        //  * int destLength: Length of the bytes to be replaced.
        //  * byte[] source:  The new bytes which should replace the previous bytes.
        //
        // If AllowResize is true, destLength may be different from source.Length. This represents
        // a request to resize the data. Be aware especially of:
        //
        //  * deletion (source.Length == 0)
        //  * insertion (destLength == 0)
        //  * appending (destLength == 0 && destOffset == Data.Length)
        //
        // The function should return true if successful, and false otherwise. If the function
        // returns true, Invalidate() will be called automatically to update the control graphics,
        // but only for the regions covered by the *requested* change. If this does not match the
        // actual effected changes, Invalidate() should be called manually unless Data was
        // reassigned.
        //
        // If this property is null, editing is disabled entirely.
        private Func<int, int, byte[], bool>? _dataChangeHandler = null;
        public Func<int, int, byte[], bool>? DataChangeHandler {
            get => _dataChangeHandler;
            set {
                if (value == _dataChangeHandler)
                    return;

                ClearInput();
                _dataChangeHandler = value;
            }
        }

        // If editing is enabled, setting this true further allows editing features which require
        // data resizing, such as insertions and deletions.
        private bool _allowResize = false;
        public bool AllowResize {
            get => _allowResize;
            set {
                _allowResize = value;

                // Cancel any in-progress append inputs if resizing was just disabled.
                if (!value && _pendingInput != null && ByteCursor == Data.Length) {
                    ClearInput();
                }
            }
        }

        // Attempts to move the byte cursor, returning the distance actually traveled.
        // Positive values advance forward, but will stop on Data.Length if reached. Negative
        // values advance backward similarly, stopping on zero.
        //
        // This also scrolls the view in the control to fully display the new target.
        public int MoveBy(int delta) {
            int oldCursor = ByteCursor;
            MoveTo(ByteCursor + delta);
            return ByteCursor - oldCursor;
        }

        // Attempts to move the byte cursor, returning true if the destination was valid (in
        // bounds). If the destination is out of bounds, the cursor position is clamped to the
        // valid range and false is returned.
        //
        // This also scrolls the view in the control to fully display the new target.
        public bool MoveTo(int target) {
            int oldCursor = ByteCursor;

            bool inRange;
            if (target < 0) {
                ByteCursor = 0;
                inRange = false;
            } else if (target > Data.Length) {
                ByteCursor = Data.Length;
                inRange = false;
            } else {
                ByteCursor = target;
                inRange = true;
            }

            ClearInput();

            int oldCol = (oldCursor + FirstByteColumn) % ColumnCount;
            int oldRow = (oldCursor + FirstByteColumn) / ColumnCount;
            var oldRect = new Rectangle();
            oldRect.X = XStart + XStep * oldCol + AutoScrollPosition.X;
            oldRect.Y = YStart + YStep * oldRow + AutoScrollPosition.Y;
            oldRect.Width = XStep + _borderSize;
            oldRect.Height = YStep + _borderSize;
            Invalidate(oldRect);

            int newCol = (ByteCursor + FirstByteColumn) % ColumnCount;
            int newRow = (ByteCursor + FirstByteColumn) / ColumnCount;
            var newRect = new Rectangle();
            newRect.X = XStart + XStep * newCol + AutoScrollPosition.X;
            newRect.Y = YStart + YStep * newRow + AutoScrollPosition.Y;
            newRect.Width = XStep + _borderSize;
            newRect.Height = YStep + _borderSize;
            Invalidate(newRect);

            Point newScrollPos = AutoScrollPosition;
            if (newRect.Left < 0) {
                newScrollPos.X -= newRect.Left;
            } else if (newRect.Right > ClientSize.Width) {
                newScrollPos.X -= (newRect.Right - ClientSize.Width);
            }
            if (newRect.Top < 0) {
                newScrollPos.Y -= newRect.Top;
            } else if (newRect.Bottom > ClientSize.Height) {
                newScrollPos.Y -= (newRect.Bottom - ClientSize.Height);
            }
            if (newScrollPos != AutoScrollPosition) {
                // AutoScrollPosition is a poor API which requires you to set the inverse values
                // of what you expect to get back.
                AutoScrollPosition = new Point(
                    -newScrollPos.X,
                    -newScrollPos.Y);
            }

            return inRange;
        }

        // Enable special keys which would otherwise not be delivered to OnKeyDown.
        protected override bool IsInputKey(Keys keyData) =>
            keyData switch {
                Keys.Up => true,
                Keys.Down => true,
                Keys.Left => true,
                Keys.Right => true,
                _ => base.IsInputKey(keyData),
            };

        // Handle keyboard inputs.
        protected override void OnKeyDown(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Up:
                    // Move up by one cell.
                    MoveBy(-ColumnCount);
                    break;

                case Keys.Down:
                    // Move down by one cell.
                    MoveBy(ColumnCount);
                    break;

                case Keys.Left:
                    // Move backward by one cell.
                    MoveBy(-1);
                    break;

                case Keys.Right:
                    // Move forward by one cell.
                    MoveBy(1);
                    break;

                case Keys.PageUp:
                    // Move up by one "page".
                    MoveBy(-ColumnCount * RowsPerPage);
                    break;

                case Keys.PageDown:
                    // Move down by one "page".
                    MoveBy(ColumnCount * RowsPerPage);
                    break;

                case Keys.Home:
                    // Move to the start ...
                    if (e.Control) {
                        // ... of the entire data.
                        MoveTo(0);
                    } else {
                        // ... of the current row.
                        MoveBy(-ByteCursorColumn);
                    }
                    break;

                case Keys.End:
                    // Move to the end ...
                    if (e.Control) {
                        // ... of the entire data.
                        MoveTo(Data.Length);
                    } else {
                        // ... of the current row.
                        MoveBy(ColumnCount - ByteCursorColumn - 1);
                    }
                    break;

                case Keys k when (k >= Keys.D0 && k <= Keys.D9):
                    // Input hex digit 0-9.
                    InputNybble(k - Keys.D0);
                    break;

                case Keys k when (k >= Keys.NumPad0 && k <= Keys.NumPad9):
                    // Input hex digit 0-9 on numpad.
                    InputNybble(k - Keys.NumPad0);
                    break;

                case Keys k when (k >= Keys.A && k <= Keys.F):
                    // Input hex digit A-F.
                    InputNybble(k - Keys.A + 0xA);
                    break;

                case Keys.Back:
                    // Backspace input, if possible.
                    ClearInput();
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        // Border color drawn around cells.
        private static Brush _borderBrush = Brushes.Black;

        // Border color drawn around the selected cell.
        private static Brush _selectedBorderBrush = Brushes.Red;

        // Color for data being typed in.
        private static Brush _inputBrush = Brushes.Red;

        // Colors for normal cells.
        private static Brush _fgNormalBrush = Brushes.Navy;
        private static Brush _bgNormalBrush = Brushes.White;

        // Colors for normal cells, but for every-other column group.
        private static Brush _fgAlternateBrush = Brushes.Navy;
        private static Brush _bgAlternateBrush = Brushes.LightGray;

        // Color for zero-value cells.
        private static Brush _fgZeroBrush = Brushes.Navy;
        private static Brush _bgZeroBrush = Brushes.DarkGray;

        // Color for the selected cell. This overrides the other colors.
        private static Brush _fgSelectedBrush = Brushes.White;
        private static Brush _bgSelectedBrush = Brushes.Navy;

        // Size of borders between and around cells, in pixels.
        private static int _borderSize = 2;

        // Size of padding around the text, in pixels.
        private static int _padding = 4;

        // Font used for displaying numbers.
        private static Font _font = new Font(FontFamily.GenericMonospace, 8);

        // The space occupied by one character of text. This assumes a fixed-width font.
        private Size CharSize { get; set; }

        // The number of characters in an address.
        private int AddressCharCount { get; set; }

        // The distance between the left side of the control and the left side of the first column.
        private int XStart => _padding + CharSize.Width * AddressCharCount + _padding;

        // The distance between the left side of one column and the left side of the next.
        private int XStep => _borderSize + _padding + CharSize.Width * 2 + _padding;

        // The distance between the top of the control and the top of the first row.
        private int YStart => 0;

        // The distance between the top of one row and the top of the next.
        private int YStep => _borderSize + _padding + CharSize.Height + _padding;

        // Recomputes and applies the control's layout and desired display size.
        private void ResetLayout() {
            using (var g = CreateGraphics()) {
                CharSize = TextRenderer.MeasureText(g, "A", _font, Size.Empty, TextFormatFlags.NoPadding);
            }
            AddressCharCount = 5; // sensible minimum
            AddressCharCount = Math.Max(AddressCharCount, FirstByteAddress.ToString("x").Length);
            AddressCharCount = Math.Max(AddressCharCount, (FirstByteAddress + Data.Length).ToString("x").Length);
            AutoScrollMinSize = new Size(
                XStart + XStep * ColumnCount + _borderSize,
                YStart + YStep * RowCount + _borderSize
            );
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left) {
                if (e.X - AutoScrollPosition.X < XStart)
                    return;
                if (e.Y - AutoScrollPosition.Y < YStart)
                    return;

                int col = (e.X - AutoScrollPosition.X - XStart) / XStep;
                int row = (e.Y - AutoScrollPosition.Y - YStart) / YStep;

                if (col < 0 || col >= ColumnCount)
                    return;
                if (row < 0 || row >= RowCount)
                    return;

                int target = row * ColumnCount + col - FirstByteColumn;
                if (target < 0 || target > Data.Length)
                    return;

                MoveTo(target);
            }
        }

        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
            InvalidateCell(ByteCursorColumn, ByteCursorRow);
        }

        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            InvalidateCell(ByteCursorColumn, ByteCursorRow);
        }

        // Handles drawing the HexView.
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            var clipRect = e.ClipRectangle;
            clipRect.X -= AutoScrollPosition.X;
            clipRect.Y -= AutoScrollPosition.Y;
            e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            var data = Data.Span;

            var strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;

            // Determine which rows need to be drawn.
            int visRowFirst = (int)Math.Floor((clipRect.Top - YStart) / (float)YStep);
            int visRowLast = (int)Math.Ceiling((clipRect.Bottom - YStart + _borderSize) / (float)YStep);
            if (visRowFirst < 0) {
                visRowFirst = 0;
            }
            if (visRowLast >= RowCount) {
                visRowLast = RowCount - 1;
            }

            // Draw each visible row.
            for (int row = visRowFirst; row <= visRowLast; row++) {
                int rowY = YStart + YStep * row;
                int rowFirstByte = row * ColumnCount - FirstByteColumn;
                long rowAddress = FirstRowAddress + row * ColumnCount;

                // Draw the row address text.
                var rowAddrRect = new Rectangle();
                rowAddrRect.X = 0;
                rowAddrRect.Y = rowY;
                rowAddrRect.Width = XStart;
                rowAddrRect.Height = YStep + _borderSize;
                e.Graphics.DrawString(
                    rowAddress.ToString("X").PadLeft(AddressCharCount),
                    _font,
                    SystemBrushes.ControlText,
                    rowAddrRect,
                    strFormat);

                // Determine which columns need to be drawn.
                int colFirst;
                if (row == 0) {
                    colFirst = FirstByteColumn;
                } else {
                    colFirst = 0;
                }
                int colLast;
                if (row == RowCount - 1) {
                    colLast = data.Length - rowFirstByte; // include one-past-the-end
                } else {
                    colLast = ColumnCount - 1;
                }

                // Draw the top border for this row.
                e.Graphics.FillRectangle(
                    _borderBrush,
                    XStart + XStep * colFirst,
                    YStart + YStep * row,
                    XStep * (colLast - colFirst + 1) + _borderSize,
                    _borderSize);

                // Draw each cell.
                for (int col = colFirst; col <= colLast; col++) {
                    int colX = XStart + XStep * col;
                    int cellByte = rowFirstByte + col;

                    // Draw the left border.
                    e.Graphics.FillRectangle(
                        _borderBrush,
                        XStart + XStep * col,
                        YStart + YStep * row + _borderSize,
                        _borderSize,
                        YStep - _borderSize);

                    // If this is the last column, also draw the right border.
                    if (col == colLast) {
                        e.Graphics.FillRectangle(
                            _borderBrush,
                            XStart + XStep * (col + 1),
                            YStart + YStep * row + _borderSize,
                            _borderSize,
                            YStep - _borderSize);
                    }

                    // The final cell corresponds to the position one past the end of the
                    // data. Leave that one empty with no background or text.
                    if (cellByte == data.Length)
                        break;

                    Brush fgBrush;
                    Brush bgBrush;
                    if (cellByte == ByteCursor) {
                        fgBrush = _fgSelectedBrush;
                        bgBrush = _bgSelectedBrush;
                    } else if (data[cellByte] == 0) {
                        fgBrush = _fgZeroBrush;
                        bgBrush = _bgZeroBrush;
                    } else if (ColumnsPerGroup != 0 && col / ColumnsPerGroup % 2 == 1) {
                        fgBrush = _fgAlternateBrush;
                        bgBrush = _bgAlternateBrush;
                    } else {
                        fgBrush = _fgNormalBrush;
                        bgBrush = _bgNormalBrush;
                    }

                    var cellInnerRect = new Rectangle();
                    cellInnerRect.X = colX + _borderSize;
                    cellInnerRect.Y = rowY + _borderSize;
                    cellInnerRect.Width = _padding + CharSize.Width * 2 + _padding;
                    cellInnerRect.Height = _padding + CharSize.Height + _padding;

                    // Draw the background.
                    e.Graphics.FillRectangle(bgBrush, cellInnerRect);

                    // Draw the cell value.
                    var text = data[cellByte].ToString("X2");
                    if (_pendingInput != null && cellByte == ByteCursor) {
                        // Skip the first nybble if the user is typing a new byte value.
                        text = " " + text[1];
                    }
                    e.Graphics.DrawString(
                        text,
                        _font,
                        fgBrush,
                        cellInnerRect,
                        strFormat);
                }

                // Draw the bottom border for this row, unless the next row will draw it for us
                // as its top border.
                if (row == visRowLast || row == RowCount - 2) {
                    e.Graphics.FillRectangle(
                        _borderBrush,
                        XStart + XStep * colFirst,
                        YStart + YStep * (row + 1),
                        XStep * (colLast - colFirst + 1) + _borderSize,
                        _borderSize);
                }
            }

            // Draw a special border around the selected cell, if we have focus.
            if (Focused) {
                e.Graphics.FillRectangle(
                    _selectedBorderBrush,
                    XStart + XStep * ByteCursorColumn,
                    YStart + YStep * ByteCursorRow,
                    XStep + _borderSize,
                    _borderSize);
                e.Graphics.FillRectangle(
                    _selectedBorderBrush,
                    XStart + XStep * ByteCursorColumn,
                    YStart + YStep * (ByteCursorRow + 1),
                    XStep + _borderSize,
                    _borderSize);
                e.Graphics.FillRectangle(
                    _selectedBorderBrush,
                    XStart + XStep * ByteCursorColumn,
                    YStart + YStep * ByteCursorRow + _borderSize,
                    _borderSize,
                    YStep - _borderSize);
                e.Graphics.FillRectangle(
                    _selectedBorderBrush,
                    XStart + XStep * (ByteCursorColumn + 1),
                    YStart + YStep * ByteCursorRow + _borderSize,
                    _borderSize,
                    YStep - _borderSize);
            }

            // Draw the input nybble if input is in progress as well.
            if (_pendingInput != null) {
                var cellInnerRect = new Rectangle();
                cellInnerRect.X = XStart + XStep * ByteCursorColumn + _borderSize;
                cellInnerRect.Y = YStart + YStep * ByteCursorRow + _borderSize;
                cellInnerRect.Width = _padding + CharSize.Width * 2 + _padding;
                cellInnerRect.Height = _padding + CharSize.Height + _padding;

                e.Graphics.DrawString(
                    _pendingInput.Value.ToString("X") + " ",
                    _font,
                    _inputBrush,
                    cellInnerRect,
                    strFormat);
            }
        }

        public void InvalidateCell(int col, int row) {
            if (col < 0 || col >= ColumnCount)
                throw new ArgumentException();
            if (row < 0 || row >= RowCount)
                throw new ArgumentException();

            var rect = new Rectangle();
            rect.X = XStart + XStep * col + AutoScrollPosition.X;
            rect.Y = YStart + YStep * row + AutoScrollPosition.Y;
            rect.Width = XStep + _borderSize;
            rect.Height = YStep + _borderSize;
            Invalidate(rect);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _data = Memory<byte>.Empty;
            }

            base.Dispose(disposing);
        }

        private int? _pendingInput;

        public bool InputNybble(int value) {
            if (value < 0 || value > 0xF)
                throw new ArgumentException();

            // If edits are not allowed, fail now.
            if (DataChangeHandler == null)
                return false;

            // If resizing is not allowed, fail if trying to append.
            if (ByteCursor == Data.Length && !AllowResize)
                return false;

            if (_pendingInput == null) {
                // First half of the input (upper 4 bits).
                _pendingInput = value;
                InvalidateCell(ByteCursorColumn, ByteCursorRow);
                return true;
            } else {
                // Second half of the input (lower 4 bits).
                value |= _pendingInput.Value << 4;
                _pendingInput = null;

                if (ByteCursor == Data.Length) {
                    // Attempt to append.
                    bool ok = DataChangeHandler(ByteCursor, 0, new byte[] {(byte)value});
                    if (ok) {
                        MoveBy(1); // this also invalidates the cell
                    }
                    return ok;
                } else {
                    // Attempt to overwrite.
                    bool ok = DataChangeHandler(ByteCursor, 1, new byte[] {(byte)value});
                    if (ok) {
                        MoveBy(1); // this also invalidates the cell
                    }
                    return ok;
                }
            }
        }

        public void ClearInput() {
            if (_pendingInput != null) {
                _pendingInput = null;
                InvalidateCell(ByteCursorColumn, ByteCursorRow);
            }
        }

    }

}
