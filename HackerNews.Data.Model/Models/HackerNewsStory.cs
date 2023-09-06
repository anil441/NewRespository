using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Data.Model.Models
{
    public class HackerNewsStory
    {
        public string Title { get; set; }

        public string Url { get; set; }

        [JsonProperty(PropertyName = "By")]
        public string PostedBy { get; set; }

        public string Time { get; set; }

        public string Score { get; set; }

        [JsonProperty(PropertyName = "Descendants")]
        public string CommentCount { get; set; }



    }
}
