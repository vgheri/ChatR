using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ChatR.Utilities
{
    public class YoutubeRichContentParser
    {
        // Regex taken from this SO answer: http://stackoverflow.com/a/5831191
        private static readonly Regex YoutubeRegex = new Regex(
            @"# Match non-linked youtube URL in the wild. (Rev:20111012)
            https?://         # Required scheme. Either http or https.
            (?:[0-9A-Z-]+\.)? # Optional subdomain.
            (?:               # Group host alternatives.
              youtu\.be/      # Either youtu.be,
            | youtube\.com    # or youtube.com followed by
              \S*             # Allow anything up to VIDEO_ID,
              [^\w\-\s]       # but char before ID is non-ID char.
            )                 # End host alternatives.
            ([\w\-]{11})      # $1: VIDEO_ID is exactly 11 chars.
            (?=[^\w\-]|$)     # Assert next char is non-ID or EOS.
            (?!               # Assert URL is not pre-linked.
              [?=&+%\w]*      # Allow URL (query) remainder.
              (?:             # Group pre-linked alternatives.
                [\'""][^<>]*> # Either inside a start tag,
              | </a>          # or inside <a> element text contents.
              )               # End recognized pre-linked alts.
            )                 # End negative lookahead assertion.
            [?=&+%\w-]*       # Consume any URL (query) remainder.",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        private static string GetRichContentFormat()
        {            
            var embed = @"<iframe width=""420"" height=""315"" src=""http://www.youtube.com/embed/{0}"" frameborder=""0"" allowfullscreen></iframe>";
            return embed;            
        }

        public static string GetEmbeddedYoutubeContent(string url)
        {
            Match match = YoutubeRegex.Match(url);
            if (match.Groups.Count < 2 || String.IsNullOrEmpty(match.Groups[1].Value))
            {
                return null;
            }

            string videoId = match.Groups[1].Value;
            return String.Format(CultureInfo.InvariantCulture, GetRichContentFormat(), videoId);
        }
    }

 
}