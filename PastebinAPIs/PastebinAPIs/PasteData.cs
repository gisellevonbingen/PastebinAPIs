using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteData
    {
        public string Code { get; set; } = null;
        public PastePrivate Private { get; set; } = PastePrivate.Public;
        public string Name { get; set; } = null;
        public PasteExpireDate ExpireDate { get; set; } = null;
        public string Format { get; set; } = null;
        public string UserKey { get; set; } = null;

        public PasteData()
        {

        }

    }

}
