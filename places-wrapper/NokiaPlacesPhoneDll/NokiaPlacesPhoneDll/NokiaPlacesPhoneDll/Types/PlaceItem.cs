
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class PlaceItem
    {
        //missing
        //references, related
        // having -- not documented

        public string Id { get; private set; }
        public string Name { get; private set; }
        public Uri View { get; internal set; }
        public LocationItem Location { get; internal set; }
        public ContactsItem Contacts { get; internal set; }
        public List<CategoryItem> Categories { get; private set; }
        public RatingItem Rating { get; private set; }
        public Uri Icon { get; internal set; }
        public Dictionary<string, string> AlternativeNames { get; private set; }
        public string Attribution { get; private set; }
        public LinkItem Supplier { get; private set; }
        public List<ImageItem> Images { get; private set; }
        public LinkItem CreateImage { get; private set; }
        public List<EditorialItem> Editorials { get; private set; }
        public List<ReviewItem> Reviews { get; private set; }
        public LinkItem CreateReview { get; private set; }
        public ExtendedItem Extended { get; private set; }
        public RelatedItem Related { get; private set; }
        
        /// <summary>
        /// Creates an PlaceItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An PlaceItem object</returns>
        internal static PlaceItem FromJToken(JToken item)
        {
            Uri usePlaceUrl = null;
            LocationItem useLocation = null;
            ContactsItem useContacts = null;
            List<CategoryItem> useCategories = null;
            RatingItem useRating = null;
            Uri useIconUrl = null;
            Dictionary<string, string> useAlternativeNames = null;
            LinkItem useSupplier = null;
            List<ImageItem> useImages = null;
            List<EditorialItem> useEditorials = null;
            List<ReviewItem> useReviews = null;
            LinkItem useCreateImage = null;
            LinkItem useCreateReview = null;
            ExtendedItem useExtended = null;
            RelatedItem useRelated = null;

            try{
                usePlaceUrl = new Uri(item.Value<string>("view"));
            }catch{
                usePlaceUrl = null;
            }

            try{
                JToken jsonRes = item.Value<JToken>("location");
                if (jsonRes != null){
                    useLocation = LocationItem.FromJToken(jsonRes);
                }
            }catch{
                useLocation = null;
            }

            try{
                JToken jsonRes = item.Value<JToken>("contacts");
                if (jsonRes != null){
                    useContacts = ContactsItem.FromJToken(jsonRes);
                }
            }catch{
                useContacts = null;
            }

            try{
                useCategories = new List<CategoryItem>();
                foreach (JToken cats in item.Value<JArray>("categories")){
                    CategoryItem result = CategoryItem.FromJToken(cats);
                    if (result != null){
                        useCategories.Add(result);
                    }
                }
            }catch{
                useCategories = null;
            }

            try{
                JToken jsonRes = item.Value<JToken>("ratings");
                if (jsonRes != null){
                    useRating = RatingItem.FromJToken(jsonRes);
                }
            }catch{
                useRating = null;
            }

            try{
                useIconUrl = new Uri(item.Value<string>("icon"));
            }catch{
                useIconUrl = null;
            }

            try{
                JArray altNamArr = item.Value<JArray>("alternativeNames");
                if (altNamArr != null){
                    useAlternativeNames = new Dictionary<string, string>();
                    foreach (JToken phItem in altNamArr){
                        useAlternativeNames.Add(phItem.Value<string>("language"), phItem.Value<string>("name"));
                    }
                }
            }catch{
                useAlternativeNames = null;
            }

            try{
                JToken jsonSup = item.Value<JToken>("supplier");
                if (jsonSup != null){
                    useSupplier = LinkItem.FromJToken(jsonSup);
                }
            }catch{
                useSupplier = null;
            }
                       
            try{
                JToken jsonImg = item.Value<JToken>("images");
                if (jsonImg != null){
                    JToken jsonLink = item.Value<JToken>("create");
                    if (jsonLink != null){
                        useCreateImage = LinkItem.FromJToken(jsonLink);
                    }
                    useImages = new List<ImageItem>();
                    foreach (JToken imgs in jsonImg.Value<JArray>("items")){
                        ImageItem result = ImageItem.FromJToken(imgs);
                        if (result != null){
                            useImages.Add(result);
                        }
                    }
                }
            }catch{
                useImages = null;
                useCreateImage = null;
            }

            try{
                JToken jsonEd = item.Value<JToken>("editorials");
                if (jsonEd != null){
                    useEditorials = new List<EditorialItem>();
                    foreach (JToken edss in jsonEd.Value<JArray>("items")){
                        EditorialItem result = EditorialItem.FromJToken(edss);
                        if (result != null){
                            useEditorials.Add(result);
                        }
                    }
                }
            }catch{
                useEditorials = null;
            }

            try{
                JToken jsonRew = item.Value<JToken>("reviews");
                if (jsonRew != null){
                    JToken jsonLink = item.Value<JToken>("create");
                    if (jsonLink != null){
                        useCreateReview = LinkItem.FromJToken(jsonLink);
                    }

                    useReviews = new List<ReviewItem>();
                    foreach (JToken rews in jsonRew.Value<JArray>("items")){
                        ReviewItem result = ReviewItem.FromJToken(rews);
                        if (result != null){
                            useReviews.Add(result);
                        }
                    }
                }
            }catch{
                useReviews = null;
                useCreateReview = null;
            }

            try{
                JToken jsonRes = item.Value<JToken>("extended");
                if (jsonRes != null){
                    useExtended = ExtendedItem.FromJToken(jsonRes);
                }
            }catch{
                useExtended = null;
            }

            try{
                JToken jsonRes = item.Value<JToken>("related");
                if (jsonRes != null){
                    useRelated = RelatedItem.FromJToken(jsonRes);
                }
            }catch{
                useRelated = null;
            }
            
            // Create the resulting PlaceItem object...
            return new PlaceItem()
            {
                Name = item.Value<string>("name"),
                Id = item.Value<string>("placeId"),
                View = usePlaceUrl,
                Location = useLocation,
                Contacts = useContacts,
                Categories = useCategories,
                Rating = useRating,
                Icon = useIconUrl,
                AlternativeNames = useAlternativeNames,
                Attribution = item.Value<string>("attribution"),
                Supplier = useSupplier,
                Images = useImages,
                Editorials = useEditorials,
                Reviews = useReviews,
                CreateImage = useCreateImage,
                CreateReview = useCreateReview,
                Extended = useExtended,
                Related = useRelated
            };
        }
    }
}
