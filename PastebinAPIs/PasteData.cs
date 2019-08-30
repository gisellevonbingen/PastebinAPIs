using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteData
    {
        public string Code { get; set; } = string.Empty;
        public PastePrivate Private { get; set; } = PastePrivate.Public;
        public string Name { get; set; } = string.Empty;
        public PasteExpireDate ExpireDate { get; set; } = PasteExpireDate.Never;
        public string Format { get; set; } = "text";
        public string UserKey { get; set; } = string.Empty;

        public PasteData()
        {

        }

    }

}
