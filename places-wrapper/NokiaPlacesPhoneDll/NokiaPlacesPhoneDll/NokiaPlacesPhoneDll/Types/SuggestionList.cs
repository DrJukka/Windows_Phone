using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;

namespace Nokia.Places.Phone.Types
{
    public class SuggestionList
    {
        public List<string> Suggestions { get; private set; }

        /// <summary>
        /// Creates an SuggestionList from a JSON Object
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>An SuggestionList object</returns>
        internal static SuggestionList FromJToken(JToken item)
        {
            List<string> useSuggestions = null;
            try
            {
                JArray result = item.Value<JArray>("suggestions");

                useSuggestions = new List<string>();
                for(int i=0; i < result.Count; i++)
                {
                    useSuggestions.Add(result[i].ToString());
                }
            }
            catch
            {
                useSuggestions = null;
            }

            // Create the resulting SuggestionList object...
            return new SuggestionList()
            {
                Suggestions = useSuggestions
            };
        }
    }
}
