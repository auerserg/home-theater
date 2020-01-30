﻿using System;
using System.Collections;
using System.Windows.Forms;

namespace HomeTheater.Helper
{
    internal class ListViewItemComparer : IComparer
    {
        private readonly int _column;
        private readonly bool _invert;

        public ListViewItemComparer()
        {
            _column = 0;
        }

        public ListViewItemComparer(int column, bool invertedOrder)
        {
            _column = column;
            _invert = invertedOrder;
        }

        public int Compare(object x, object y)
        {
            var sx = _invert ? ((ListViewItem) x).SubItems[_column].Text : ((ListViewItem) y).SubItems[_column].Text;
            var sy = _invert ? ((ListViewItem) y).SubItems[_column].Text : ((ListViewItem) x).SubItems[_column].Text;

            switch (_column)
            {
                case 6:
                case 7:
                case 8:
                case 15:
                case 16:
                    int ix, iy;
                    int.TryParse(sx, out ix);
                    int.TryParse(sy, out iy);
                    return iy.CompareTo(ix);
                case 13:
                case 14:
                    float fx, fy;
                    float.TryParse(sx, out fx);
                    float.TryParse(sy, out fy);
                    return fy.CompareTo(fx);
                case 21:
                    DateTime dx, dy;
                    DateTime.TryParse(sx, out dx);
                    DateTime.TryParse(sy, out dy);
                    return dx.CompareTo(dy);
                default:
                    return string.Compare(sy, sx);
            }
        }
    }
}