using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Fanword.Business.Builders.RssFeeds {
    public class RssFeedHtmlCleaner {
        /// <summary>
        /// Strip HTML from string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string StripHTML(string html) {

            if (string.IsNullOrEmpty(html)) return "";

            html = html.Replace("\t", "");
            html = html.Trim(new[] { '\n', '\r' });

            HtmlDocument doc = new HtmlDocument();
            StringWriter sw = new StringWriter();

            doc.LoadHtml(html);
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// Strips HTML from sub-nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outText"></param>
        private void ConvertContentTo(HtmlNode node, TextWriter outText) {

            foreach (HtmlNode subnode in node.ChildNodes) {

                ConvertTo(subnode, outText);
            }
        }

        /// <summary>
        /// Strips HTML from any node-type
        /// </summary>
        /// <param name="node"></param>
        /// <param name="outText"></param>
        public void ConvertTo(HtmlNode node, TextWriter outText) {

            string html;

            switch (node.NodeType) {

                case HtmlNodeType.Comment:
                    // Ignore comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // Remove Scripts & Styles
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // String
                    html = ((HtmlTextNode)node).Text;

                    // Special closing?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // Ignore empty whitespace
                    if (html.Trim().Length > 0) {

                        var deEntitized = HtmlEntity.DeEntitize(Regex.Replace(html, "&apos;", "'"));
                        outText.Write(Regex.Replace(deEntitized, @"&#39;", "'"));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name) {

                        case "p":
                            // Formatting
                            if (node.PreviousSibling != null)
                                outText.Write("\r\n\r\n");
                            break;
                    }

                    if (node.HasChildNodes) {

                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }
    }
}
