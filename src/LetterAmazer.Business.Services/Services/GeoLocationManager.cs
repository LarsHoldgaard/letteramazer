using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace LetterAmazer.Business.Services.Services
{
    public class GeoLocationManager
    {
        private const string MapsShortUrl = "http://maps.google.com/maps/api/geocode/json?sensor=false&address=";

        public Tuple<string, string> GetCoordiantes(string query)
        {
            try
            {

                var callUrl = MapsShortUrl + query;
                using (var client = new WebClient())
                {
                    JObject jsonRes = JObject.Parse(client.DownloadString(callUrl));
                    
                    var locatioNCords = jsonRes["results"].First["geometry"]["location"];
                    var latitude = locatioNCords["lat"].Value<string>();
                    var longitude = locatioNCords["lng"].Value<string>();

                    return new Tuple<string, string>(longitude, latitude);
                }


            }
            catch (Exception exp)
            {
                return new Tuple<string, string>("", "");
            }

        }

     
    }
}
