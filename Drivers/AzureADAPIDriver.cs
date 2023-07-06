using CAP.SpecTests.Support;
using NUnit.Framework;
using PlayWrightWithSpecflowCSharp.Support;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CAP.SpecTests.Drivers
{
    public class AzureADApiDriver : IAzureADApiDriver
    {
        private RestClient client;
        private RestResponse LastResponse;
        
        public AzureADApiDriver()
        {
            client = new RestClient(ConfigReader.azureADBaseUrl);
            LastResponse = new RestResponse();
        }

        public string GetResponseMessage()
        {
            return LastResponse.Content!;
        }

        public HttpStatusCode GetResponseStatusCode()
        {
            return LastResponse.StatusCode;
        }

        public string GetResponseErrorMessage()
        {
            return LastResponse.ErrorMessage;
        }
        public void VerifyTheAPIResponseCode(int responseCode)
        {
            var apiRespCode = GetResponseStatusCode();
            var intendedRespCode = responseCode == 200 ? HttpStatusCode.OK
                : responseCode == 400 ? HttpStatusCode.BadRequest
                : responseCode == 401 ? HttpStatusCode.Unauthorized
                : responseCode == 404 ? HttpStatusCode.NotFound
                : responseCode == 409 ? HttpStatusCode.Conflict
                : responseCode == 204 ? HttpStatusCode.NoContent
                : responseCode == 201 ? HttpStatusCode.Created
                : HttpStatusCode.NotImplemented;

            Assert.That(apiRespCode, Is.EqualTo(intendedRespCode), $"Incorrect response returned: {apiRespCode}. Expected: {intendedRespCode}");
            Console.WriteLine($"The api response code is as expected: {apiRespCode}");
        }

        public void GetOne<T>(string entity)
        {
            string ModelName = typeof(T).Name;
            var endpoint = ModelName == "B2CUser" ? "users"
                : $"{ModelName}";

            endpoint = endpoint + "/" + entity;
            var request = new RestRequest(endpoint, Method.Get);
            string accessToken = GetAccessTokenHelper.GetAzureActiveDirToken();
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            var resp = client.Execute(request);
            Console.WriteLine($"The api response for getting {ModelName} is: {resp.Content}");
            LastResponse = resp;
        }

        public void AddOne<T>(T entity)
        {
            string ModelName = typeof(T).Name;
            var endpoint = ModelName == "B2CUser" ? "users"
                : $"{ModelName}";

            var request = new RestRequest(endpoint, Method.Post).AddBody(entity);
            string accessToken = GetAccessTokenHelper.GetAzureActiveDirToken();
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            var resp = client.Execute(request);
            Console.WriteLine($"The api response for adding {ModelName} is: {resp.Content}");
            LastResponse = resp;
        }

        public void DeleteOne<T>(string deleteEntity)
        {
            string ModelName = typeof(T).Name;
            var endpoint = ModelName == "B2CUser" ? "users"
                : $"{ModelName}";
            endpoint = endpoint + "/" + deleteEntity;
            var request = new RestRequest(endpoint, Method.Delete);
            string accessToken = GetAccessTokenHelper.GetAzureActiveDirToken();
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddHeader("Content-Type", "application/json");
            var resp = client.Execute(request);
            Console.WriteLine($"The api request for deleting '{deleteEntity}' from '{ModelName}' is completed");
            LastResponse = resp;
        }

    }
}
