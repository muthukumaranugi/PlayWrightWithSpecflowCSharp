using Newtonsoft.Json.Linq;
using PlayWrightWithSpecflowCSharp.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CAP.SpecTests.Support
{
    public class GetAccessTokenHelper
    {
        IPage page;

        public GetAccessTokenHelper(IPage page)
        {
            this.page = page;
        }

        public async Task<string> GetAccessToken()
        {
            //Fetch local storage of the application from the browser
            IBrowserContext browserCurrentContext = page.Context;
            string appLocalStorage = await browserCurrentContext.StorageStateAsync();
            JsonDocument jsonStorageState = JsonDocument.Parse(appLocalStorage);
            JsonElement rootElement = jsonStorageState.RootElement;
            JsonElement localStorage = rootElement.GetProperty("origins")[0].GetProperty("localStorage");

            //Extracting access token from the local storage
            string bearerAccessToken = string.Empty;
            foreach (var data in localStorage.EnumerateArray())
            {
                if (data.GetProperty("name").GetString().Contains("accesstoken"))
                {
                    JsonElement accessTokenProperty = data.GetProperty("value");
                    JsonDocument accessTokenPropertyJson = JsonDocument.Parse(accessTokenProperty.ToString());
                    JsonElement accessTokenJsonRoot = accessTokenPropertyJson.RootElement;
                    bearerAccessToken = accessTokenJsonRoot.GetProperty("secret").GetString();
                    break;
                }
            }
            return bearerAccessToken;
        }

        public static string GetAzureActiveDirToken()
        {
            dynamic azureActiveDirAPIInfo = ConfigReader.azureADAuthInfo;
            string tokenURL = azureActiveDirAPIInfo["authURL"];
            string appID = azureActiveDirAPIInfo["client_id"];
            string clientSecret = azureActiveDirAPIInfo["client_secret"];
            string grantType = azureActiveDirAPIInfo["grant_type"];
            string scope = azureActiveDirAPIInfo["scope"];

            string result = string.Empty;
            string token = string.Empty;

            try
            {
                var client = new HttpClient();
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", appID),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", grantType),
                    new KeyValuePair<string, string>("scope", scope)
                };

                var content = new FormUrlEncodedContent(pairs);

                var response = client.PostAsync(tokenURL, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    JObject resultResponse = JObject.Parse(result);
                    token = resultResponse.GetValue("access_token").ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return token;
        }

    }
}
