using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class EditorialItem
    {
        public string Description { get; private set; }
        public string Language { get; private set; }
        public LinkItem Supplier { get; private set; }
        public LinkItem Via { get; private set; }
        public string Attribution { get; private set; }

         /// <summary>
        /// Creates an EditorialItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An EditorialItem object</returns>
        internal static EditorialItem FromJToken(JToken item)
        {
            LinkItem useSupplier = null;
            LinkItem useVia = null;

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

            // Create the resulting EditorialItem object...
            return new EditorialItem()
            {
                Description = item.Value<string>("description"),
                Language = item.Value<string>("language"),

                Supplier = useSupplier,
                Via = useVia,
                Attribution = item.Value<string>("attribution"),
            };
        }
    }
}
