using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class CategoryItem
    {
        // add within ?

        public string Id { get; private set; }
        public string Title { get; private set; }
        public Uri Href { get; private set; }

        public string Type { get; private set; }
        public Uri Icon { get; private set; }

        public List<string> Within { get; private set; }

        /// <summary>
        /// Creates an CategoryItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An CategoryItem object</returns>
        internal static CategoryItem FromJToken(JToken item)
        {
            Uri useUrl = null;
            Uri useIconUrl = null;
            List<string> useWithin = null;

            try{
                JArray result = item.Value<JArray>("within");
                useWithin = new List<string>();
                for (int i = 0; i < result.Count; i++){
                    useWithin.Add(result[i].ToString());
                }
            }catch{
                useWithin = null;
            }

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

            // Create the resulting CategoryItem object...
            return new CategoryItem()
            {
                Id = item.Value<string>("id"),
                Title = item.Value<string>("title"),
                Href = useUrl,
                Type = item.Value<string>("type"),
                Icon = useIconUrl,
                Within = useWithin
            };
        }
    }
}
