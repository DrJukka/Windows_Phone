
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class ExtendedItem
    {
        public Dictionary<string, string> Payment { get; private set; }
        public Dictionary<string, string> OpeningHours { get; private set; }
        public Dictionary<string, string> AnnualClosings { get; private set; }
        public Dictionary<string, string> NearestLandmark { get; private set; }
        public Dictionary<string, string> LanguagesSpoken { get; private set; }
        public Dictionary<string, string> AvailableParking { get; private set; }
        public Dictionary<string, string> Smoking { get; private set; }
        public Dictionary<string, string> DisabledAccess { get; private set; }
        
        /// <summary>
       /// Creates an ExtendedItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
       /// <returns>An ExtendedItem object</returns>
       internal static ExtendedItem FromJToken(JToken item)
        {
            Dictionary<string, string> usePayment = null;
            Dictionary<string, string> useOpeningHours = null;
            Dictionary<string, string> useAnnualClosings = null;
            Dictionary<string, string> useNearestLandmark = null;
            Dictionary<string, string> useLanguagesSpoken = null;
            Dictionary<string, string> useAvailableParking = null;
            Dictionary<string, string> useSmoking = null;
            Dictionary<string, string> useDisabledAccess = null;



            try{
                JArray PaymentArr = item.Value<JArray>("payment");
                if (PaymentArr != null){
                    usePayment = new Dictionary<string, string>();
                    foreach (JToken phItem in PaymentArr){
                        usePayment.Add(phItem.Value<string>("label"), phItem.Value<string>("text"));
                    }
                }
            }catch{
                usePayment = null;
            }

            try{
                JArray phoneArr = item.Value<JArray>("openingHours");
                if (phoneArr != null){
                    useOpeningHours = new Dictionary<string, string>();
                    foreach (JToken phItem in phoneArr){
                        useOpeningHours.Add(phItem.Value<string>("label"), phItem.Value<string>("text"));
                    }
                }
            }catch{
                useOpeningHours = null;
            }

            try{
                JArray AnnualClosingsArr = item.Value<JArray>("annualClosings");
                if (AnnualClosingsArr != null){
                    useAnnualClosings = new Dictionary<string, string>();
                    foreach (JToken emItem in AnnualClosingsArr){
                        useAnnualClosings.Add(emItem.Value<string>("label"), emItem.Value<string>("text"));
                    }
                }
            }catch{
                useAnnualClosings = null;
            }

            try{
                JArray websArr = item.Value<JArray>("nearestLandmark");
                if (websArr != null){
                    useNearestLandmark = new Dictionary<string, string>();
                    foreach (JToken wsItem in websArr){
                        useNearestLandmark.Add(wsItem.Value<string>("label"), wsItem.Value<string>("text"));
                    }
                }
            }catch{
                useNearestLandmark = null;
            }

            try{
                JArray websArr = item.Value<JArray>("languagesSpoken");
                if (websArr != null){
                    useLanguagesSpoken = new Dictionary<string, string>();
                    foreach (JToken wsItem in websArr){
                        useLanguagesSpoken.Add(wsItem.Value<string>("label"), wsItem.Value<string>("text"));
                    }
                }
            }catch{
                useLanguagesSpoken = null;
            }

            try{
                JArray websArr = item.Value<JArray>("availableParking");
                if (websArr != null){
                    useAvailableParking = new Dictionary<string, string>();
                    foreach (JToken wsItem in websArr){
                        useAvailableParking.Add(wsItem.Value<string>("label"), wsItem.Value<string>("text"));
                    }
                }
            }catch{
                useAvailableParking = null;
            }
           
           try{
               JArray websArr = item.Value<JArray>("smoking");
               if (websArr != null){
                   useSmoking = new Dictionary<string, string>();
                   foreach (JToken wsItem in websArr){
                       useSmoking.Add(wsItem.Value<string>("label"), wsItem.Value<string>("text"));
                   }
               }
           }catch{
               useSmoking = null;
           }
           
           try{
               JArray websArr = item.Value<JArray>("disabledAccess");
               if (websArr != null){
                   useDisabledAccess = new Dictionary<string, string>();
                   foreach (JToken wsItem in websArr){
                       useDisabledAccess.Add(wsItem.Value<string>("label"), wsItem.Value<string>("text"));
                   }
               }
           }catch{
               useDisabledAccess = null;
           }

            // Create the resulting ExtendedItem object...
            return new ExtendedItem()
            {
                Payment = usePayment,
                OpeningHours = useOpeningHours,
                AnnualClosings = useAnnualClosings,
                NearestLandmark = useNearestLandmark,
                LanguagesSpoken = useLanguagesSpoken,
                AvailableParking = useAvailableParking,
                Smoking = useSmoking,
                DisabledAccess = useDisabledAccess
            };
        }
    }
}
