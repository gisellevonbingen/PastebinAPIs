using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class QueryResult<T>
    {
        public int Index { get; }
        public T Value { get; }
        public bool Breaked { get; }

        public QueryResult(int index, T value, bool breaked)
        {
            this.Index = index;
            this.Value = value;
            this.Breaked = breaked;
        }

    }

}
