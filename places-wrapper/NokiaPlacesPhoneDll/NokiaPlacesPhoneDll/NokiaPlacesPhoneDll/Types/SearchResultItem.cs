using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class SearchResultItem
    {
        public string Type { get; private set; }
        public string Id { get; private set; }
        public string Title { get; private set; }
        public int DistanceTo { get; private set; }
        public LocationItem Location{ get; internal set; }
        public Uri Icon { get; internal set; }
        public Uri Href { get; internal set; }
        public string Visinity { get; private set; }
        public CategoryItem Category { get; private set; }
        public RatingItem Rating { get; private set; }
        public bool Sponsored { get; private set; }

        /// <summary>
        /// Creates an SearchResultItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An SearchResultItem object</returns>
        internal static SearchResultItem FromJToken(JToken item)
        {
            int useDistance = -1;
            LocationItem useLocation = null;
            Uri useUrl = null;
            Uri useIconUrl = null;
            CategoryItem useCategory = null;
            RatingItem useRating = null;
            bool useSponsored = false;

            try{
                useLocation = LocationItem.FromJToken(item);
            }catch {
                useLocation = null;
            }

            try{
                useDistance = int.Parse(item.Value<string>("distance"));
            }catch{
                useDistance = -1;
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

            try{
                JToken jsonCat = item.Value<JToken>("category");
                if(jsonCat != null){
                    useCategory = CategoryItem.FromJToken(jsonCat);
                }
            }catch{
               useIconUrl = null;
            }

            try{
                useRating = RatingItem.FromJToken(item);
            }catch{
                useRating = null;
            }


            if (item.Value<string>("sponsored") != null){
                useSponsored = true;
            }else{
                useSponsored = false;
            }

            // Create the resulting PlaceItem object...
            return new SearchResultItem()
            {
                Id = item.Value<string>("id"),
                Title = item.Value<string>("title"),
                Location = useLocation,
                DistanceTo = useDistance,
                Href = useUrl,
                Icon = useIconUrl,
                Visinity = item.Value<string>("vicinity"),
                Type = item.Value<string>("type"),
                Rating = useRating,
                Category = useCategory,
                Sponsored = useSponsored
            };
        }
    }
}
