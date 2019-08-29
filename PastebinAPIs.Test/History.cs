using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class History<T>
    {
        private int _Cursor;

        protected List<T> List { get; }
        public int CursorOffset { get; }
        public int Cursor { get { return this._Cursor; } set { this.UpdateCursor(value); } }
        public int Count { get { return this.List.Count; } }

        public History(int cursorOffset)
        {
            this._Cursor = 0;

            this.CursorOffset = cursorOffset;
            this.List = new List<T>();

            this.Clear();
        }

        protected virtual void UpdateCursor(int value)
        {
            this._Cursor = Math.Min(this.GetMaxCursor(), Math.Max(this.GetMinCursor(), value));
        }

        public int GetMinCursor()
        {
            return this.CursorOffset;
        }

        public int GetMaxCursor()
        {
            var list = this.List;
            var cursorOffset = this.CursorOffset;

            return list.Count + cursorOffset;
        }

        public T Feach()
        {
            return this.Feach(this.Cursor);
        }

        public T Feach(int cursor)
        {
            var list = this.List;

            if (0 <= cursor && cursor < list.Count)
            {
                return list[cursor];
            }
            else
            {
                return default;
            }

        }

        public T Prev()
        {
            this.Cursor--;
            return this.Feach();
        }

        public T Next()
        {
            this.Cursor++;
            return this.Feach();
        }

        public void Clear()
        {
            this.List.Clear();
            this.Cursor = this.CursorOffset;
        }

        public virtual void Record(T value)
        {
            var list = this.List;
            list.Add(value);
            this.Cursor = list.Count + this.CursorOffset;
        }

    }

}
