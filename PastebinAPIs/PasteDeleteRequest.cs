using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteDeleteRequest
    {
        public string UserKey { get; set; } = string.Empty;
        public string PasteKey { get; set; } = string.Empty;
    }

}
