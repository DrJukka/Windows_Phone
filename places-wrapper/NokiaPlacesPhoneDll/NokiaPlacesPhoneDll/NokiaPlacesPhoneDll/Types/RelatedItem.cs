using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class RelatedItem
    {
        public LinkItem Recommended { get; private set; }

        /// <summary>
        /// Creates an RelatedItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An RelatedItem object</returns>
        internal static RelatedItem FromJToken(JToken item)
        {
            LinkItem useRecommended = null;

            try
            {
                JToken jsonCat = item.Value<JToken>("recommended");
                if (jsonCat != null)
                {
                    useRecommended = LinkItem.FromJToken(jsonCat);
                }
            }
            catch
            {
                useRecommended = null;
            }
            // Create the resulting RelatedItem object...
            return new RelatedItem()
            {
                Recommended = useRecommended
            };
        }
    }
}
