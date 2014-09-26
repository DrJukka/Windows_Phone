using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class ImageItem
    {
        /*todo:
        - dimensions
        */

        public Uri Src { get; private set; }
        public string Id { get; private set; }
        public UserItem User { get; private set; }
        public LinkItem Supplier { get; private set; }
        public LinkItem Via { get; private set; }
        public string Attribution { get; private set; }
        
        /// <summary>
        /// Creates an ImageItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An ImageItem object</returns>
        internal static ImageItem FromJToken(JToken item)
        {
            Uri useSrc = null;
            UserItem useUser = null;
            LinkItem useSupplier = null;
            LinkItem useVia = null;

            try{
                useSrc = new Uri(item.Value<string>("src"));
            }catch{
                useSrc = null;
            }
            
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


            // Create the resulting ImageItem object...
            return new ImageItem()
            {
                Src = useSrc,
                Id = item.Value<string>("id"),
                User = useUser,
                Supplier = useSupplier,
                Via = useVia,
                Attribution = item.Value<string>("attribution"),
            };
        }
    }
}
