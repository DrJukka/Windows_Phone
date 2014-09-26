
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class UserItem
    {
        public string Name { get; private set; }
        public Uri Href { get; private set; }
        public string Type { get; private set; }
        public Uri Icon { get; private set; }
        

        /// <summary>
        /// Creates an UserItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An UserItem object</returns>
        internal static UserItem FromJToken(JToken item)
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

            // Create the resulting UserItem object...
            return new UserItem()
            {
                Name = item.Value<string>("name"),
                Href = useUrl,
                Type = item.Value<string>("type"),
                Icon = useIconUrl
            };
        }
    }
}

