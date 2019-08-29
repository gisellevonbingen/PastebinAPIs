using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class InputEditSnapshot
    {
        public InputEditType Type { get; set; }
        public string Text { get; set; }
        public int CursorLeft { get; set; }

        public InputEditSnapshot(InputEditType type, string text, int cursorLeft)
        {
            this.Type = type;
            this.Text = text;
            this.CursorLeft = cursorLeft;
        }

    }

}
