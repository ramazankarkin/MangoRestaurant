using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";


        // Identity resource is a named group of claims that can be requested using a scope parameter.
        // Resources will be basically something that you want to protect with your identity server, either
        // identity data(id,name,email) of users or API itself.
        // when we access the identity token, all of these identity resources will be provided in some type of claim.
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };


        //public static IEnumerable<IdentityResource> IdentityResources()
        //{
        //    return new List<IdentityResource>
        //   {
        //        new IdentityResources.OpenId(),
        //        new IdentityResources.Email(),
        //        new IdentityResources.Profile()
        //   };

        //}

        // İki çeşit Identity server var. 
        //    - identity scope -> Will have an object of the profile itself.
        //                        Scope wil be used by client. There can be more than one client.
        //                     
        //    - identity resource

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("mango", "Mango Server"),
                new ApiScope(name:"read", displayName:"Read your data."),
                new ApiScope(name:"write", displayName:"Write your data."),
                new ApiScope(name:"delete", displayName:"Delete your data.")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> // request token from identity server. it can be either for authenticating a user
                             // or for accessing a resource. In this project client is mango.
                             // başka örnek olarak mobil, web, desktop app'ler örnek olarak verilebilir.
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes= {"read", "write", "profile"}
                },

                new Client
                {
                    ClientId = "mango",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:44386/signin-oidc"}, // signin- oidc for Openid connect
                    PostLogoutRedirectUris = {"https://localhost:44386/signout-callback-oidc"}, // çıkış yaptığımızda nereye yönlendirecek.
                    AllowedScopes=  new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mango" // this is API Scope for mango
                    }
                }
            };










    }
}
