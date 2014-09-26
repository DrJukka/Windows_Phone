using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class RatingItem
    {
        public string Average { get; private set; }
        public int Count { get; private set; }
        
        /// <summary>
        /// Creates an RatingItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An RatingItem object</returns>
        internal static RatingItem FromJToken(JToken item)
        {
            string useAvarage = "";
            int usecCount = -1;
            try{
                usecCount = int.Parse(item.Value<string>("count"));
            }catch{
                usecCount = -1;
            }

            useAvarage = item.Value<string>("average");
            if (useAvarage == null || (useAvarage.Length == 0)){
                useAvarage = item.Value<string>("averageRating");
            }

            // Create the resulting RatingItem object...
            return new RatingItem()
            {
                Average = useAvarage,
                Count = usecCount
            };
        }
    }
}
