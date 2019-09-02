using Giselle.Commons;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteUser
    {
        public string Name { get; set; }
        public string FormatShort { get; set; }
        public string Expiration { get; set; }
        public string AvatarUrl { get; set; }
        public PastePrivate Private { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public PasteUserAccountType AccountType { get; set; }

        public PasteUser()
        {

        }

        public PasteUser(HtmlNode node)
        {
            this.Name = node.ChildNodes["user_name"].InnerHtml;
            this.FormatShort = node.ChildNodes["user_format_short"].InnerHtml;
            this.Expiration = node.ChildNodes["user_expiration"].InnerHtml;
            this.AvatarUrl = node.ChildNodes["user_avatar_url"].InnerHtml;
            this.Private = (PastePrivate)NumberUtils.ToByte(node.ChildNodes["user_private"].InnerHtml);
            this.Website = node.ChildNodes["user_website"].InnerHtml;
            this.Email = node.ChildNodes["user_email"].InnerHtml;
            this.Location = node.ChildNodes["user_location"].InnerHtml;
            this.AccountType = (PasteUserAccountType)NumberUtils.ToByte(node.ChildNodes["user_account_type"].InnerHtml);
        }

    }

}
