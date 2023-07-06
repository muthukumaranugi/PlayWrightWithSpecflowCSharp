using CAP.SpecTests.Support;
using PlayWrightWithSpecflowCSharp.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAP.SpecTests.Models
{
    public class B2CUser
    {
        public string id { get; set; }
        public bool accountEnabled { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string displayName { get; set; }
        public string mail { get; set; }
        public List<Identity> identities { get; set; }
        public PasswordProfile passwordProfile { get; set; }

        public B2CUser(string firstName, string lastName)
        {
            id = string.Empty;
            accountEnabled = true;
            givenName = CommonFunctions.CheckRunSuffix(firstName);
            surname = lastName;
            displayName = $"{givenName} {surname}";
            mail = $"{firstName}.{surname}@test.com";
            identities = new List<Identity>
            {
                new Identity
                {
                    signInType = "emailAddress",
                    issuer = ConfigReader.azureADDomain,
                    issuerAssignedId = $"{givenName}.{surname}@test.com"
                }
             };
            passwordProfile = new PasswordProfile
            {
                forceChangePasswordNextSignIn = false,
                password = "test@test123"
            };
         }
    }

    public class Identity
    {
        public string signInType { get; set; }
        public string issuer { get; set; }
        public string issuerAssignedId { get; set; }
    }

    public class PasswordProfile
    {
        public bool forceChangePasswordNextSignIn { get; set; }
        public string password { get; set; }
    }
}
