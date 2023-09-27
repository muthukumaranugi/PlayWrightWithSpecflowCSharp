using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CAP.SpecTests.Drivers
{
    public interface IAzureADApiDriver
    {
        public string GetResponseMessage();
        public HttpStatusCode GetResponseStatusCode();
        public void VerifyTheAPIResponseCode(int responseCode);
        public void AddOne<T>(T entity);
        public void GetOne<T>(string entity);
        public void DeleteOne<T>(string deleteEntity);
        public string GetResponseErrorMessage();
        public void CreateOneBookUser<T>(T entity);
        public void GetOneBookUser<T>(string id);
        public void DeleteOneBookUser<T>(string id);
    }
}
