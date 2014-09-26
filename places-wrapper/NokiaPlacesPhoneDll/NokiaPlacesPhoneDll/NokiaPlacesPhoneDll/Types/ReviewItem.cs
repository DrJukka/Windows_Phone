using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class ReviewItem
    {
        public string Date { get; private set; }
        public string Title { get; private set; }
        public string Rating { get; private set; }
        public string Description { get; private set; }
        public string Language { get; private set; }
        public UserItem User { get; private set; }

        public LinkItem Supplier { get; private set; }
        public LinkItem Via { get; private set; }
        public string Attribution { get; private set; }

        /// <summary>
        /// Creates an ReviewItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An ReviewItem object</returns>
        internal static ReviewItem FromJToken(JToken item)
        {
            LinkItem useSupplier = null;
            LinkItem useVia = null;
            UserItem useUser = null;

            try{
                JToken jsonCat = item.Value<JToken>("user");
                if (jsonCat != null){
                    useUser = UserItem.FromJToken(jsonCat);
                }
            }catch{
                useUser = null;
            }

            try{
                JToken jsonCat = item.Value<JToken>("supplier");
                if (jsonCat != null){
                    useSupplier = LinkItem.FromJToken(jsonCat);
                }
            }catch{
                useSupplier = null;
            }

            try{
                JToken jsonCat = item.Value<JToken>("via");
                if (jsonCat != null){
                    useVia = LinkItem.FromJToken(jsonCat);
                }
            }catch{
                useVia = null;
            }

            // Create the resulting ReviewItem object...
            return new ReviewItem()
            {
                Date = item.Value<string>("date"),
                Title = item.Value<string>("title"),
                Rating = item.Value<string>("rating"),
                Description = item.Value<string>("description"),
                Language = item.Value<string>("language"),
                User = useUser,
                Supplier = useSupplier,
                Via = useVia,
                Attribution = item.Value<string>("attribution"),
            };
        }
    }
}
