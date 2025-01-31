﻿using Giselle.Commons;
using Giselle.Commons.Collections;
using Giselle.Commons.Net;
using Giselle.Commons.Net.Http;
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
        public string RawUri { get; }

        public string APIKey { get; set; } = null;

        public PastebinAPI()
        {
            this.Explorer = new WebExplorer();
            this.BaseUri = "https://pastebin.com/api/api_post.php";
            this.LoginUri = "https://pastebin.com/api/api_login.php";
            this.RawUri = "https://pastebin.com/api/api_raw.php";
        }

        public WebRequestParameter CreateWebRequest(string uri, QueryValues values)
        {
            var apiKeyName = "api_dev_key";
            var overrides = new QueryValues();
            overrides.AddRange(values);
            overrides.RemoveAll(apiKeyName);
            overrides.Add(apiKeyName, this.APIKey);

            var request = new WebRequestParameter
            {
                Method = "POST",
                Uri = uri,
                ContentType = "application/x-www-form-urlencoded",
                WriteParameter = overrides.ToString(false, false)
            };

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
            var queryValues = new QueryValues
            {
                { "api_user_name", request.Name },
                { "api_user_password", request.Password }
            };

            var webRequest = this.CreateWebRequest(this.LoginUri, queryValues);
            var userKey = this.ProcessWebRequest(webRequest);

            return userKey;
        }

        public string CreatePaste(PasteCreateRequest request)
        {
            var queryValues = new QueryValues
            {
                { "api_option", "paste" },
                { "api_user_key", request.UserKey },
                { "api_paste_private", ((byte)request.Private).ToString() },
                { "api_paste_name", HttpUtility.UrlEncode(request.Name) },
                { "api_paste_expire_date", request.ExpireDate?.Value },
                { "api_paste_format", request.Format },
                { "api_dev_key", this.APIKey },
                { "api_paste_code", HttpUtility.UrlEncode(request.Code) }
            };

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var url = this.ProcessWebRequest(webRequest);

            return url;
        }

        public PasteData[] ListPastes(PasteListRequest request)
        {
            var queryValues = new QueryValues
            {
                { "api_option", "list" },
                { "api_user_key", request.UserKey },
                { "api_results_limit", request.ResultsLimit }
            };

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
            var queryValues = new QueryValues
            {
                { "api_option", "delete" },
                { "api_user_key", request.UserKey },
                { "api_paste_key", request.PasteKey }
            };

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var result = this.ProcessWebRequest(webRequest);

            return result.Equals("Paste Removed", StringComparison.OrdinalIgnoreCase);
        }

        public PasteUser GetUser(string userKey)
        {
            var queryValues = new QueryValues
            {
                { "api_option", "userdetails" },
                { "api_user_key", userKey }
            };

            var webRequest = this.CreateWebRequest(this.BaseUri, queryValues);
            var html = this.ProcessWebRequest(webRequest);
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var userNode = document.DocumentNode.ChildNodes["user"];
            var user = new PasteUser(userNode);

            return user;
        }

        public string GetPasteRaw(PasteGetRawRequest request)
        {
            var queryValues = new QueryValues
            {
                { "api_option", "show_paste" },
                { "api_user_key", request.UserKey },
                { "api_paste_key", request.PasteKey }
            };

            var webRequest = this.CreateWebRequest(this.RawUri, queryValues);
            var raw = this.ProcessWebRequest(webRequest);

            return raw;
        }

    }

}
