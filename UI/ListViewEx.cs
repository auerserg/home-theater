using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HomeTheater.UI
{
    /// <summary>
    ///     Zusammenfassung fьr ListViewEx.
    /// </summary>
#pragma warning disable CA1060 // Переместите вызовы PInvoke в класс собственных методов
    public class ListViewEx : ListView
#pragma warning restore CA1060 // Переместите вызовы PInvoke в класс собственных методов
    {
        private readonly ArrayList _embeddedControls = new ArrayList();

        [DefaultValue(View.LargeIcon)]
        public new View View
        {
            get => base.View;
            set
            {
                // Embedded controls are rendered only when we're in Details mode
                foreach (EmbeddedControl ec in _embeddedControls)
                    ec.Control.Visible = value == View.Details;

                base.View = value;
            }
        }

        /// <summary>
        ///     Retrieve the order in which columns appear
        /// </summary>
        /// <returns>Current display order of column indices</returns>
        protected int[] GetColumnOrder()
        {
            var lPar = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * Columns.Count);

            var res = SendMessage(Handle, LVM_GETCOLUMNORDERARRAY, new IntPtr(Columns.Count), lPar);
            if (res.ToInt32() == 0) // Something went wrong
            {
                Marshal.FreeHGlobal(lPar);
                return null;
            }

            var order = new int[Columns.Count];
            Marshal.Copy(lPar, order, 0, Columns.Count);

            Marshal.FreeHGlobal(lPar);

            return order;
        }

        /// <summary>
        ///     Retrieve the bounds of a ListViewSubItem
        /// </summary>
        /// <param name="Item">The Item containing the SubItem</param>
        /// <param name="SubItem">Index of the SubItem</param>
        /// <returns>Subitem's bounds</returns>
        protected Rectangle GetSubItemBounds(ListViewItem Item, int SubItem)
        {
            var subItemRect = Rectangle.Empty;

            if (Item == null)
#pragma warning disable CA1507 // Используйте nameof для выражения имен символов
                throw new ArgumentNullException("Item");
#pragma warning restore CA1507 // Используйте nameof для выражения имен символов

            var order = GetColumnOrder();
            if (order == null) // No Columns
                return subItemRect;

            if (SubItem >= order.Length)
                throw new IndexOutOfRangeException("SubItem " + SubItem + " out of range");

            // Retrieve the bounds of the entire ListViewItem (all subitems)
            var lviBounds = Item.GetBounds(ItemBoundsPortion.Entire);
            var subItemX = lviBounds.Left;

            // Calculate the X position of the SubItem.
            // Because the columns can be reordered we have to use Columns[order[i]] instead of Columns[i] !
            ColumnHeader col;
            int i;
            for (i = 0; i < order.Length; i++)
            {
                col = Columns[order[i]];
                if (col.Index == SubItem)
                    break;
                subItemX += col.Width;
            }

            subItemRect = new Rectangle(subItemX, lviBounds.Top, Columns[order[i]].Width, lviBounds.Height);

            return subItemRect;
        }

        /// <summary>
        ///     Add a control to the ListView
        /// </summary>
        /// <param name="c">Control to be added</param>
        /// <param name="col">Index of column</param>
        /// <param name="row">Index of row</param>
        public void AddEmbeddedControl(Control c, int col, int row)
        {
            AddEmbeddedControl(c, col, row, DockStyle.Fill);
        }

        /// <summary>
        ///     Add a control to the ListView
        /// </summary>
        /// <param name="c">Control to be added</param>
        /// <param name="col">Index of column</param>
        /// <param name="row">Index of row</param>
        /// <param name="dock">Location and resize behavior of embedded control</param>
        public void AddEmbeddedControl(Control c, int col, int row, DockStyle dock)
        {
#pragma warning disable CA2208 // Правильно создавайте экземпляры исключений аргументов
            if (c == null)
                throw new ArgumentNullException();
            if (col >= Columns.Count || row >= Items.Count)
                throw new ArgumentOutOfRangeException();
#pragma warning restore CA2208 // Правильно создавайте экземпляры исключений аргументов

            if (c.Disposing || c.IsDisposed)
            {
            }
            else
            {
                EmbeddedControl ec;
                ec.Control = c;
                ec.Column = col;
                ec.Row = row;
                ec.Dock = dock;
                ec.Item = Items[row];

                _embeddedControls.Add(ec);

                // Add a Click event handler to select the ListView row when an embedded control is clicked
                c.Click += _embeddedControl_Click;

                Controls.Add(c);
            }
        }

        /// <summary>
        ///     Remove a control from the ListView
        /// </summary>
        /// <param name="c">Control to be removed</param>
        public void RemoveEmbeddedControl(Control c)
        {
            if (c == null)
#pragma warning disable CA2208 // Правильно создавайте экземпляры исключений аргументов
                throw new ArgumentNullException();
#pragma warning restore CA2208 // Правильно создавайте экземпляры исключений аргументов

            for (var i = 0; i < _embeddedControls.Count; i++)
            {
                var ec = (EmbeddedControl) _embeddedControls[i];
                if (ec.Control == c)
                {
                    c.Click -= _embeddedControl_Click;
                    Controls.Remove(c);
                    _embeddedControls.RemoveAt(i);
                    return;
                }
            }

            throw new Exception("Control not found!");
        }

        /// <summary>
        ///     Retrieve the control embedded at a given location
        /// </summary>
        /// <param name="col">Index of Column</param>
        /// <param name="row">Index of Row</param>
        /// <returns>Control found at given location or null if none assigned.</returns>
        public Control GetEmbeddedControl(int col, int row)
        {
            foreach (EmbeddedControl ec in _embeddedControls)
                if (ec.Row == row && ec.Column == col)
                    return ec.Control;

            return null;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    if (View != View.Details)
                        break;

                    // Calculate the position of all embedded controls
                    foreach (EmbeddedControl ec in _embeddedControls)
                    {
                        var rc = GetSubItemBounds(ec.Item, ec.Column);

                        if (HeaderStyle != ColumnHeaderStyle.None &&
                            rc.Top < Font.Height) // Control overlaps ColumnHeader
                        {
                            ec.Control.Visible = false;
                            continue;
                        }

                        ec.Control.Visible = true;

                        switch (ec.Dock)
                        {
                            case DockStyle.Fill:
                                break;
                            case DockStyle.Top:
                                rc.Height = ec.Control.Height;
                                break;
                            case DockStyle.Left:
                                rc.Width = ec.Control.Width;
                                break;
                            case DockStyle.Bottom:
                                rc.Offset(0, rc.Height - ec.Control.Height);
                                rc.Height = ec.Control.Height;
                                break;
                            case DockStyle.Right:
                                rc.Offset(rc.Width - ec.Control.Width, 0);
                                rc.Width = ec.Control.Width;
                                break;
                            case DockStyle.None:
                                rc.Size = ec.Control.Size;
                                break;
                        }

                        // Set embedded control's bounds
                        ec.Control.Bounds = rc;
                    }

                    break;
            }

            base.WndProc(ref m);
        }

        private void _embeddedControl_Click(object sender, EventArgs e)
        {
            // When a control is clicked the ListViewItem holding it is selected
            foreach (EmbeddedControl ec in _embeddedControls)
                if (ec.Control == (Control) sender)
                {
                    SelectedItems.Clear();
                    ec.Item.Selected = true;
                }
        }

        /// <summary>
        ///     Structure to hold an embedded control's info
        /// </summary>
        private struct EmbeddedControl
        {
            public Control Control;
            public int Column;
            public int Row;
            public DockStyle Dock;
            public ListViewItem Item;
        }

        #region Interop-Defines

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wPar, IntPtr lPar);

        // ListView messages
        private const int LVM_FIRST = 0x1000;
        private const int LVM_GETCOLUMNORDERARRAY = LVM_FIRST + 59;

        // Windows Messages
        private const int WM_PAINT = 0x000F;

        #endregion
    }
}