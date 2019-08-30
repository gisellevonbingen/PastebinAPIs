using Giselle.Commons;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    public class PasteData
    {
        public string Key { get; set; } = null;
        public long Date { get; set; } = 0;
        public string Title { get; set; } = null;
        public long Size { get; set; } = 0;
        public long ExpireDate { get; set; } = 0;
        public PastePrivate Private { get; set; } = PastePrivate.Public;
        public string FormatLong { get; set; } = null;
        public string FormatShort { get; set; } = null;
        public string Url { get; set; } = null;
        public long Hits { get; set; } = 0;

        public PasteData()
        {

        }

        public PasteData(HtmlNode node)
        {
            this.Key = node.ChildNodes["paste_key"].InnerHtml;
            this.Date = NumberUtils.ToLong(node.ChildNodes["paste_date"].InnerHtml);
            this.Title = node.ChildNodes["paste_title"].InnerHtml;
            this.Size = NumberUtils.ToLong(node.ChildNodes["paste_size"].InnerHtml);
            this.ExpireDate = NumberUtils.ToLong(node.ChildNodes["paste_expire_date"].InnerHtml);
            this.Private = (PastePrivate)NumberUtils.ToByte(node.ChildNodes["paste_private"].InnerHtml);
            this.FormatLong = node.ChildNodes["paste_format_long"].InnerHtml;
            this.FormatShort = node.ChildNodes["paste_format_short"].InnerHtml;
            this.Url = node.ChildNodes["paste_url"].InnerHtml;
            this.Hits = NumberUtils.ToLong(node.ChildNodes["paste_hits"].InnerHtml);
        }

    }

}
