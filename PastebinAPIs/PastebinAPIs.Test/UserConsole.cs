using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class UserConsole : UserAbstract
    {
        private Encoding _Encoding;
        private string _InputPrefix;
        private int _CursorLeft;

        public object SyncRoot { get; }
        public StringBuilder InputBuffer { get; }
        public InputEditHistory InputEditHistory { get; }
        public History<string> InputEnterHistory { get; }
        public Encoding Encoding { get { return this._Encoding; } set { this.UpdateEncoding(value); } }
        public string InputPrefix { get { return this._InputPrefix; } set { this.UpdateInputPrefix(value); } }
        public int CursorLeft { get { return this._CursorLeft; } set { this.UpdateCursor(value); } }

        public UserConsole()
        {
            this._Encoding = Console.InputEncoding;
            this._InputPrefix = "> ";
            this._CursorLeft = 0;

            this.SyncRoot = new object();
            this.InputBuffer = new StringBuilder();
            this.InputEditHistory = new InputEditHistory();
            this.InputEnterHistory = new History<string>(0);

            this.RefreshLine();
        }

        protected virtual void UpdateEncoding(Encoding value)
        {
            lock (this.SyncRoot)
            {
                this._Encoding = value;
                Console.InputEncoding = value;
                Console.OutputEncoding = value;
            }

        }

        protected virtual void UpdateInputPrefix(string value)
        {
            lock (this.SyncRoot)
            {
                this._InputPrefix = value;
                this.RefreshLine();
            }

        }

        public void UpdateCursor()
        {
            this.UpdateCursor(this.CursorLeft);
        }

        public virtual void UpdateCursor(int newCursorLeft)
        {
            lock (this.SyncRoot)
            {
                var prefixLength = this.InputPrefix.Length;
                var inputBuffer = this.InputBuffer.ToString();
                newCursorLeft = Math.Max(0, Math.Min(newCursorLeft, inputBuffer.Length));

                var bytesCount = this.Encoding.GetByteCount(inputBuffer.ToCharArray(), 0, newCursorLeft);

                this._CursorLeft = newCursorLeft;
                Console.CursorLeft = prefixLength + bytesCount;
            }

        }

        protected void WriteLine(string str)
        {
            lock (this.SyncRoot)
            {
                this.ClearLine();

                Console.CursorLeft = 0;
                Console.WriteLine(str);

                this.WriteInputBuffer();
            }

        }

        protected void RefreshLine(InputEditType? editType = null, bool resetEnterCursor = true)
        {
            lock (this.SyncRoot)
            {
                this.ClearLine();
                this.WriteInputBuffer();

                if (editType.HasValue == true)
                {
                    var editHistory = this.InputEditHistory;
                    editHistory.Record(new InputEditSnapshot(editType.Value, this.InputBuffer.ToString(), this.CursorLeft));

                }

                if (resetEnterCursor == true)
                {
                    var inputEnterHistory = this.InputEnterHistory;
                    inputEnterHistory.Cursor = inputEnterHistory.GetMaxCursor();
                }

            }

        }

        protected void WriteInputBuffer()
        {
            lock (this.SyncRoot)
            {
                Console.CursorLeft = 0;
                Console.Write(this.InputPrefix + this.InputBuffer);

                this.UpdateCursor();
            }

        }

        public void ClearLine()
        {
            lock (this.SyncRoot)
            {
                var top = Console.CursorTop;

                Console.CursorLeft = 0;
                var bufferWidth = Console.BufferWidth;
                Console.Write(new string(' ', bufferWidth - 1));
                Console.CursorLeft = 0;
                Console.CursorTop = top;

                this.UpdateCursor();
            }

        }

        public int GetPrevWordIndex(int cursor)
        {
            lock (this.SyncRoot)
            {
                var input = this.InputBuffer.ToString();

                if (cursor > 1)
                {
                    var index = input.LastIndexOf(' ', cursor - 2);

                    if (index != -1)
                    {
                        return index + 1;
                    }

                }

                return 0;
            }

        }

        public int GetNextWordIndex(int cursor)
        {
            lock (this.SyncRoot)
            {
                var input = this.InputBuffer.ToString();

                if (cursor < input.Length)
                {
                    var index = input.IndexOf(' ', cursor + 1);

                    if (index != -1)
                    {
                        return index + 1;
                    }

                }

                return input.Length;
            }

        }

        protected void WriteLine()
        {
            this.WriteLine(string.Empty);
        }

        public override void SendMessage()
        {
            this.WriteLine();
        }

        protected override string OnReadInput()
        {
            var editHistory = this.InputEditHistory;
            var buffer = this.InputBuffer;
            editHistory.Clear();
            buffer.Clear();

            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;
                var control = keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control);

                lock (this.SyncRoot)
                {
                    if (key == ConsoleKey.Enter)
                    {
                        var input = buffer.ToString();
                        editHistory.Clear();
                        buffer.Clear();

                        this.WriteLine(this.InputPrefix + input);
                        this.WriteLine();
                        this.CursorLeft = 0;

                        this.InputEnterHistory.Record(input);

                        return input;
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        this.MoveCursorLeft(control);
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        this.MoveCursorRight(control);
                    }
                    else if (key == ConsoleKey.Home)
                    {
                        this.MoveCursorHead();
                    }
                    else if (key == ConsoleKey.End)
                    {
                        this.MoveCursorTail();
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        this.PrevInput();
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        this.NextInput();
                    }
                    else if (key == ConsoleKey.Backspace)
                    {
                        this.Backspace(control);
                    }
                    else if (key == ConsoleKey.Delete)
                    {
                        this.Delete(control);
                    }
                    else if (key == ConsoleKey.Z && control == true)
                    {
                        this.PrevEdit();
                    }
                    else if (key == ConsoleKey.Y && control == true)
                    {
                        this.NextEdit();
                    }
                    else
                    {
                        buffer.Insert(this.CursorLeft, keyInfo.KeyChar);
                        this.CursorLeft++;
                        this.RefreshLine(InputEditType.Append);
                    }

                }

            }

        }

        private void MoveCursorLeft(bool word)
        {
            lock (this.SyncRoot)
            {
                var cursor = this.CursorLeft;

                if (word == true)
                {
                    this.CursorLeft = this.GetPrevWordIndex(cursor);
                }
                else
                {
                    this.CursorLeft = cursor - 1;
                }

            }

        }

        private void MoveCursorRight(bool word)
        {
            lock (this.SyncRoot)
            {
                var cursor = this.CursorLeft;

                if (word == true)
                {
                    this.CursorLeft = this.GetNextWordIndex(cursor);
                }
                else
                {
                    this.CursorLeft = cursor + 1;
                }

            }

        }

        public void MoveCursorHead()
        {
            lock (this.SyncRoot)
            {
                this.CursorLeft = 0;
            }

        }

        public void MoveCursorTail()
        {
            lock (this.SyncRoot)
            {
                this.CursorLeft = this.InputBuffer.Length;
            }

        }

        public void Backspace(bool word)
        {
            lock (this.SyncRoot)
            {
                var cursor = this.CursorLeft;
                var buffer = this.InputBuffer;

                if (cursor > 0)
                {
                    if (word == true)
                    {
                        var wordIndex = this.GetPrevWordIndex(cursor);
                        var count = cursor - wordIndex;
                        buffer.Remove(wordIndex, count);
                        this.CursorLeft -= count;
                        this.RefreshLine(InputEditType.Backspace);
                    }
                    else
                    {
                        buffer.Remove(cursor - 1, 1);
                        this.CursorLeft--;
                        this.RefreshLine(InputEditType.Backspace);
                    }

                }

            }

        }

        public void Delete(bool word)
        {
            lock (this.SyncRoot)
            {
                var cursor = this.CursorLeft;
                var buffer = this.InputBuffer;

                if (cursor < buffer.Length)
                {
                    if (word == true)
                    {
                        var wordIndex = this.GetNextWordIndex(cursor);
                        var count = wordIndex - cursor;
                        buffer.Remove(cursor, count);
                        this.RefreshLine(InputEditType.Delete);
                    }
                    else
                    {
                        buffer.Remove(cursor, 1);
                        this.RefreshLine(InputEditType.Delete);
                    }

                }

            }

        }

        public void PrevInput()
        {
            lock (this.SyncRoot)
            {
                var enterHistory = this.InputEnterHistory;
                var prev = enterHistory.Prev();

                if (prev != null)
                {
                    var buffer = this.InputBuffer;
                    buffer.Clear();
                    buffer.Append(prev);
                    this.CursorLeft = prev.Length;
                    this.RefreshLine(resetEnterCursor: false);
                }

            }

        }

        public void NextInput()
        {
            lock (this.SyncRoot)
            {
                var enterHistory = this.InputEnterHistory;
                var next = enterHistory.Next();

                var buffer = this.InputBuffer;
                buffer.Clear();

                if (next != null)
                {
                    buffer.Append(next);
                    this.CursorLeft = next.Length;
                }
                else
                {
                    var lastEdit = this.InputEditHistory.Feach();
                    buffer.Append(lastEdit?.Text);
                    this.CursorLeft = lastEdit?.CursorLeft ?? 0;
                }

                this.RefreshLine(resetEnterCursor: false);
            }

        }

        public void PrevEdit()
        {
            lock (this.SyncRoot)
            {
                var editHistory = this.InputEditHistory;
                var prev = editHistory.Prev();

                var buffer = this.InputBuffer;
                buffer.Clear();

                if (prev != null)
                {
                    buffer.Append(prev.Text);
                    this.CursorLeft = prev.CursorLeft;
                }
                else
                {
                    this.CursorLeft = 0;
                }

                this.RefreshLine();
            }

        }

        public void NextEdit()
        {
            lock (this.SyncRoot)
            {
                var editHistory = this.InputEditHistory;
                var next = editHistory.Next();

                if (next != null)
                {
                    var buffer = this.InputBuffer;
                    buffer.Clear();
                    buffer.Append(next.Text);
                    this.CursorLeft = next.CursorLeft;
                    this.RefreshLine();
                }

            }

        }

        public override void SendMessage(string message)
        {
            this.WriteLine(message);
        }

    }

}
