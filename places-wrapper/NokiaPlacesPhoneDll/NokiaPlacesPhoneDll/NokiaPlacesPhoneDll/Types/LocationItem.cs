using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;

namespace Nokia.Places.Phone.Types
{
    public class LocationItem
    {
        public GeoCoordinate GeoCoordinate { get; private set; }
        public AddressItem Address { get; private set; }
        public LocationRectangle BoundingArea { get; private set; }
        public string LocationId { get; private set; }
       

        /// <summary>
        /// Creates an LocationItem from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An LocationItem object</returns>
        internal static LocationItem FromJToken(JToken item)
        {
            GeoCoordinate useCoord = null;
            AddressItem useAddress = null;
            LocationRectangle useBoundingArea = null;
            try{
                JToken jsonRes = item.Value<JToken>("position");
                if (jsonRes != null){
                    useCoord = new GeoCoordinate();
                    useCoord.Latitude = (double)jsonRes.First;
                    useCoord.Longitude = (double)jsonRes.Last;
                }
            }catch{
                useCoord = null;
            }

            try{
                JToken jsonCat = item.Value<JToken>("address");
                if(jsonCat != null){
                    useAddress = AddressItem.FromJToken(jsonCat);
                }
            }catch{
                useAddress = null;
            }

            try{
                JToken jsonRes = item.Value<JToken>("bbox");
                if (jsonRes != null){
                    //West longitude, South latitude, East longitude, North latitude]
                    double West= (double)jsonRes.First;
                    double South= (double)jsonRes.Next;
                    double East= (double)jsonRes.Next;
                    double North = (double)jsonRes.Next;
                    //north, west, south and east 
                    useBoundingArea = new LocationRectangle(North, West, South, East);
                }
            }catch{
                useBoundingArea = null;
            }

            // Create the resulting LocationItem object...
            return new LocationItem()
            {
                GeoCoordinate = useCoord,
                Address = useAddress,
                BoundingArea = useBoundingArea,
                LocationId = item.Value<string>("locationId")
            };
        }
    }
}
