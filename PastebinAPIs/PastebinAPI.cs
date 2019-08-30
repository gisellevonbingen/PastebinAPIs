using Giselle.Commons;
using Giselle.Commons.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PastebinAPIs
{
    public class PastebinAPI
    {
        public WebExplorer Explorer { get; }
        public string BaseUri { get; }
        public string LoginUri { get; }

        public string APIKey { get; set; } = null;

        public PastebinAPI()
        {
            this.Explorer = new WebExplorer();
            this.BaseUri = "https://pastebin.com/api/api_post.php";
            this.LoginUri = "https://pastebin.com/api/api_login.php";
        }

        public WebRequestParameter CreateRequest(string uri, Dictionary<string, string> parameters)
        {
            var overrideParameters = new Dictionary<string, string>(parameters);
            overrideParameters["api_dev_key"] = this.APIKey;

            var request = new WebRequestParameter();
            request.Method = "POST";
            request.Uri = uri;
            request.ContentType = "application/x-www-form-urlencoded";
            request.WriteParameter = EnumerableUtils.Join(overrideParameters, "&", pair => $"{pair.Key}={pair.Value}");

            return request;
        }

        public string Request(WebRequestParameter request)
        {
            using (var response = this.Explorer.Request(request))
            {
                var str = response.ReadAsString();
                var pResponse = PastebinAPIResponse.Parse(str);
                var error = pResponse.Error;

                if (string.IsNullOrWhiteSpace(error) == false)
                {
                    throw new PastebinException(error);
                }

                return pResponse.Value;
            }

        }

        public string Login(string name, string password)
        {
            var map = new Dictionary<string, string>();
            map["api_user_name"] = name;
            map["api_user_password"] = password;

            var request = this.CreateRequest(this.LoginUri, map);
            var userKey = this.Request(request);

            return userKey;
        }

        public string CreateNewPaste(PasteData data)
        {
            var map = new Dictionary<string, string>();
            map["api_option"] = "paste";
            map["api_user_key"] = data.UserKey;
            map["api_paste_private"] = ((byte)data.Private).ToString();
            map["api_paste_name"] = HttpUtility.UrlEncode(data.Name);
            map["api_paste_expire_date"] = data.ExpireDate?.Value;
            map["api_paste_format"] = data.Format;
            map["api_dev_key"] = this.APIKey;
            map["api_paste_code"] = HttpUtility.UrlEncode(data.Code);

            var request = this.CreateRequest(this.BaseUri, map);
            var uri = this.Request(request);

            return uri;
        }

    }

}
