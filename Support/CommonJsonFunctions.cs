using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAP.SpecTests.Support
{
    public class CommonJsonFunctions
    {
        public static T? DeserializeJsonResponse<T>(dynamic JsonReponse)
        {
            try
            {
                Console.WriteLine($"Deserializing Json response to a \"{typeof(T)}\" type object");
                return (T?)Convert.ChangeType(JsonConvert.DeserializeObject<T?>(JsonReponse), typeof(T));
            }
            catch (Exception)
            {

                Console.WriteLine($"Exception thrown when Deserializing Json response to a \"{typeof(T)}\" type object");
                return default;
            }
        }
    }
}
