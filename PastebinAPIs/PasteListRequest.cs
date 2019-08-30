using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteListRequest
    {
        public int? ResultsLimit { get; set; } = null;
        public string UserKey { get; set; } = string.Empty;
    }

}
