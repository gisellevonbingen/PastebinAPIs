using Giselle.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class InputResult
    {
        public string AsString { get; }

        public InputResult(string input)
        {
            this.AsString = input;
        }

        public int? AsInt => NumberUtils.ToIntNullable(this.AsString);
        public long? AsLong => NumberUtils.ToLongNullable(this.AsString);
        public bool? AsBool => NumberUtils.ToBoolNullable(this.AsString);

    }

}
