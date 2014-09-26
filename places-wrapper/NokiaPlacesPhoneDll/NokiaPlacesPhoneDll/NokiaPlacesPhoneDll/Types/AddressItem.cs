using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class AddressItem
    {
        public string Text { get; private set; }
        public string House { get; private set; }
        public string Street { get; private set; }
        public string District { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string County { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string CountryCode { get; private set; }

        /// <summary>
        /// Creates an AddressItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An AddressItem object</returns>
        internal static AddressItem FromJToken(JToken item)
        {
            // Create the resulting AddressItem object...
            return new AddressItem()
            {
                Text = item.Value<string>("text"),
                House = item.Value<string>("house"),
                Street = item.Value<string>("street"),
                District = item.Value<string>("district"),
                PostalCode = item.Value<string>("postalCode"),
                City = item.Value<string>("city"),
                County = item.Value<string>("county"),
                State = item.Value<string>("state"),
                Country = item.Value<string>("country"),
                CountryCode = item.Value<string>("countryCode"),
            };
        }
    }
}
