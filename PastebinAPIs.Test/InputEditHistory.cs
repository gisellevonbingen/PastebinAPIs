using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class InputEditHistory : History<InputEditSnapshot>
    {
        public InputEditHistory()
            : base(-1)
        {

        }

        public void RemoveAfter(int cursor)
        {
            var list = this.List;
            var startIndex = cursor - this.CursorOffset;
            var endIndex = list.Count;
            var removeCount = endIndex - startIndex;

            list.RemoveRange(startIndex, removeCount);
        }

        public override void Record(InputEditSnapshot value)
        {
            var cursor = this.Cursor;
            this.RemoveAfter(cursor);

            var prev = this.GetPrevIfTypeEquals(value.Type, cursor);

            if (prev != null)
            {
                prev.Text = value.Text;
                prev.CursorLeft = value.CursorLeft;
            }
            else
            {
                base.Record(value);
            }

        }

        private InputEditSnapshot GetPrevIfTypeEquals(InputEditType type, int cursor)
        {
            if (cursor > -1)
            {
                var cursored = this.Feach(cursor);

                if (cursored.Type == type)
                {
                    return cursored;
                }

            }

            return null;
        }

    }

}
