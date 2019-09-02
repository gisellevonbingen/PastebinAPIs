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

        public WebRequestParameter CreateWebRequest(string uri, QueryValues values)
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

        public string ProcessWebRequest(WebRequestParameter request)
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

        public string Login(PasteLoginRequest request)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_user_name", request.Name);
            queryValues.Add("api_user_password", request.Password);

            var webRequest = this.CreateWebRequest(this.LoginUri, queryValues);
            var userKey = this.ProcessWebRequest(webRequest);

            return userKey;
        }

        public string CreatePaste(PasteCreateRequest request)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_option", "paste");
            queryValues.Add("api_user_key", request.UserKey);
            queryValues.Add("api_paste_private", ((byte)request.Private).ToString());
            queryValues.Add("api_paste_name", HttpUtility.UrlEncode(request.Name));
            queryValues.Add("api_paste_expire_date", request.ExpireDate?.Value);
            queryValues.Add("api_paste_format", request.Format);
            queryValues.Add("api_dev_key", this.APIKey);
            queryValues.Add("api_paste_code", HttpUtility.UrlEncode(request.Code));

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var url = this.ProcessWebRequest(webRequest);

            return url;
        }

        public PasteData[] ListPastes(PasteListRequest request)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_option", "list");
            queryValues.Add("api_user_key", request.UserKey);
            queryValues.Add("api_results_limit", request.ResultsLimit);

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var html = this.ProcessWebRequest(webRequest);
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var pasteNodes = document.DocumentNode.ChildNodes.Where(n => n.Name.Equals("paste")).ToArray();
            var pasteArray = pasteNodes.Select(n => new PasteData(n)).ToArray();

            return pasteArray;
        }

        public bool DeletePaste(PasteDeleteRequest request)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_option", "delete");
            queryValues.Add("api_user_key", request.UserKey);
            queryValues.Add("api_paste_key", request.PasteKey);

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var result = this.ProcessWebRequest(webRequest);

            return result.Equals("Paste Removed", StringComparison.OrdinalIgnoreCase);
        }

        public PasteUser GetUser(string userKey)
        {
            var queryValues = new QueryValues();
            queryValues.Add("api_option", "userdetails");
            queryValues.Add("api_user_key", userKey);

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var html = this.ProcessWebRequest(webRequest);
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var userNode = document.DocumentNode.ChildNodes["user"];
            var user = new PasteUser(userNode);

            return user;
        }

    }

}
