using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public enum PastePrivate : byte
    {
        Public = 0,
        Unlisted = 1,
        Private = 2,
    }

}
