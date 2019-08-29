using Giselle.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PastebinAPIResponse
    {
        public static string ErrorPrefix { get; } = "Bad API request, ";

        public static PastebinAPIResponse Parse(string s)
        {
            var response = new PastebinAPIResponse();

            if (s.StartsWith(ErrorPrefix) == true)
            {
                response.Error = StringUtils.RemovePrefix(s, ErrorPrefix);
            }
            else
            {
                response.Value = s;
            }

            return response;
        }

        public string Value { get; set; } = null;
        public string Error { get; set; } = null;

        public PastebinAPIResponse()
        {

        }

    }

}
