using Giselle.Commons;
using Giselle.Commons.Web;
using HtmlAgilityPack;
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

        public WebRequestParameter CreateRequest(string uri, QueryValues values)
        {
            var apiKeyName = "api_dev_key";
            var overrides = new QueryValues(values);
            overrides.RemoveAll(apiKeyName);
            overrides.Add(apiKeyName, this.APIKey);

            var request = new WebRequestParameter();
            request.Method = "POST";
            request.Uri = uri;
            request.ContentType = "application/x-www-form-urlencoded";
            request.WriteParameter = overrides.ToString(false, false);

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
            var queryValues = new QueryValues();
            queryValues.Add("api_user_name", name);
            queryValues.Add("api_user_password", password);

            var request = this.CreateRequest(this.LoginUri, queryValues);
            var userKey = this.Request(request);

            return userKey;
        }

        public string CreatePaste(PasteCreateRequest data)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_option", "paste");
            queryValues.Add("api_user_key", data.UserKey);
            queryValues.Add("api_paste_private", ((byte)data.Private).ToString());
            queryValues.Add("api_paste_name", HttpUtility.UrlEncode(data.Name));
            queryValues.Add("api_paste_expire_date", data.ExpireDate?.Value);
            queryValues.Add("api_paste_format", data.Format);
            queryValues.Add("api_dev_key", this.APIKey);
            queryValues.Add("api_paste_code", HttpUtility.UrlEncode(data.Code));

            var request = this.CreateRequest(this.BaseUri, queryValues);
            var url = this.Request(request);

            return url;
        }

        public PasteData[] ListPastes(PasteListRequest data)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_option", "list");
            queryValues.Add("api_user_key", data.UserKey);
            queryValues.Add("api_results_limit", data.ResultsLimit);

            var request = this.CreateRequest(this.BaseUri, queryValues);
            var html = this.Request(request);
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var pasteNodes = document.DocumentNode.ChildNodes.Where(n => n.Name.Equals("paste")).ToArray();
            var pasteArray = pasteNodes.Select(n => new PasteData(n)).ToArray();

            return pasteArray;
        }

    }

}
