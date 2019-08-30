using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public struct QueryValue
    {
        public static string ValueDelimiter { get; } = "=";

        public string Key { get; set; }
        public string Value { get; set; }

        public QueryValue(string key, string value) : this()
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            return $"{this.Key}{ValueDelimiter}{this.Value}";
        }

    }

}
