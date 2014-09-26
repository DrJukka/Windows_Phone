using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class ContactsItem
    {
       public Dictionary<string, string> Phone { get; private set; }
       public Dictionary<string, string> Fax { get; private set; }
       public Dictionary<string, string> Email { get; private set; }
       public Dictionary<string, string> Website { get; private set; }

        /// <summary>
       /// Creates an ContactsItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
       /// <returns>An ContactsItem object</returns>
       internal static ContactsItem FromJToken(JToken item)
        {
            Dictionary<string, string> usePhone = null;
            Dictionary<string, string> useFax = null;
            Dictionary<string, string> useEmail = null;
            Dictionary<string, string> useWebsite = null;

            try{
                JArray phoneArr = item.Value<JArray>("phone");
                if (phoneArr != null){
                    usePhone = new Dictionary<string, string>();
                    foreach (JToken phItem in phoneArr){
                        usePhone.Add(phItem.Value<string>("label"), phItem.Value<string>("value"));
                    }
                }
            }catch{
                usePhone = null;
            }

            try{
                JArray phoneArr = item.Value<JArray>("fax");
                if (phoneArr != null){
                    useFax = new Dictionary<string, string>();
                    foreach (JToken phItem in phoneArr){
                        useFax.Add(phItem.Value<string>("label"), phItem.Value<string>("value"));
                    }
                }
            }catch{
                useFax = null;
            }

            try{
                JArray emailArr = item.Value<JArray>("email");
                if (emailArr != null){
                    useEmail = new Dictionary<string, string>();
                    foreach (JToken emItem in emailArr){
                        useEmail.Add(emItem.Value<string>("label"), emItem.Value<string>("value"));
                    }
                }
            }catch{
                useEmail = null;
            }

            try{
                JArray websArr = item.Value<JArray>("website");
                if (websArr != null){
                    useWebsite = new Dictionary<string, string>();
                    foreach (JToken wsItem in websArr){
                        useWebsite.Add(wsItem.Value<string>("label"), wsItem.Value<string>("value"));
                    }
                }
            }catch{
                useWebsite = null;
            }

            // Create the resulting ContactsItem object...
            return new ContactsItem()
            {
                Phone = usePhone,
                Fax = useFax,
                Email = useEmail,
                Website = useWebsite
            };
        }
    }
}
