using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Security.Application;


namespace ChatR.Utilities
{
    public class TextParser
    {
        static Regex urlPattern = new Regex(@"(?:(?:https?|ftp)://|www\.)[^\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public static string TransformAndExtractUrls(string message, out HashSet<string> extractedUrls)
        {
            var urls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            message = urlPattern.Replace(message, m =>
            {
                string url = m.Value;
                if (!url.Contains("://"))
                {
                    url = "http://" + url;
                }

                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    return m.Value;
                }

                urls.Add(HttpUtility.HtmlDecode(url));

                string youtubeEmbed = YoutubeRichContentParser.GetEmbeddedYoutubeContent(url);
                if (!string.IsNullOrWhiteSpace(youtubeEmbed))
                {
                    return youtubeEmbed;
                }
                else
                {
                    return String.Format(CultureInfo.InvariantCulture,
                                         "<a rel=\"nofollow external\" target=\"_blank\" href=\"{0}\" title=\"{1}\">{1}</a>",
                                         Encoder.HtmlAttributeEncode(url),
                                         m.Value);
                }
            });

            extractedUrls = urls;
            return message;
        }

        

    }
}