using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class LinkItem
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public Uri Href { get; private set; }
        public string Method { get; private set; }
        public string Type { get; private set; }
        public Uri Icon { get; private set; }
        

        /// <summary>
        /// Creates an LinkItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An LinkItem object</returns>
        internal static LinkItem FromJToken(JToken item)
        {
            Uri useUrl = null;
            Uri useIconUrl = null;

            try{
                useUrl = new Uri(item.Value<string>("href"));
            }catch{
                useUrl = null;
            }

            try{
                useIconUrl = new Uri(item.Value<string>("icon"));
            }catch{
                useIconUrl = null;
            }

            // Create the resulting LinkItem object...
            return new LinkItem()
            {
                Id = item.Value<string>("id"),
                Title = item.Value<string>("title"),
                Href = useUrl,
                Method = item.Value<string>("method"),
                Type = item.Value<string>("type"),
                Icon = useIconUrl
            };
        }
    }
}
