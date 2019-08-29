using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class UserFile : UserAbstract
    {
        private string[] Lines;
        private int Position;

        public UserFile(string path)
        {
            this.Lines = File.ReadAllLines(path);
            this.Position = -1;
        }

        public string NextLine()
        {
            var pos = this.Position + 1;
            this.Position = pos;

            return this.Lines[pos];
        }

        protected override string OnReadInput()
        {
            return this.NextLine();
        }

        public override void SendMessage()
        {

        }

        public override void SendMessage(string message)
        {

        }

    }

}
