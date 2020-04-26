using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace EM.Membership.Isp
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("projects-api", "Projects API")
            };
        }

        public static IEnumerable<Client> GetClients(IEnumerable<string> rootUrisSpaClients = null, IEnumerable<string> rootUrisMvcClients = null)
        {
            rootUrisSpaClients ??= new List<string>() { "http://localhost:4200/" };
            rootUrisMvcClients ??= new List<string>() { "http://localhost:4201" };

            List<string> spaRedirectUris = new List<string>();
            List<string> spaPostLogoutRedirectUris = new List<string>();
            List<string> spaAllowedCorsOrigins = new List<string>();
            List<string> mvcRedirectUris = new List<string>();
            List<string> mvcPostLogoutRedirectUris = new List<string>();

            foreach(string b in rootUrisSpaClients)
            {
                string uriBase = b;
                if (uriBase.EndsWith("/")) uriBase = uriBase.Substring(0, uriBase.LastIndexOf("/"));
                spaRedirectUris.Add(uriBase + "/signin-callback");
                spaRedirectUris.Add(uriBase + "/silent-callback.html");
                spaPostLogoutRedirectUris.Add(uriBase + "/signin-callback");
                spaPostLogoutRedirectUris.Add(uriBase + "/silent-callback.html");
                spaAllowedCorsOrigins.Add(uriBase);
            }
            foreach(string b in rootUrisMvcClients)
            {
                string uriBase = b;
                if (uriBase.EndsWith("/")) uriBase = uriBase.Substring(0, uriBase.LastIndexOf("/"));
                mvcRedirectUris.Add(uriBase + "/signin-oidc");
                mvcPostLogoutRedirectUris.Add(uriBase + "/signout-callback-oidc");
            }

            return new List<Client>
            {
                new Client
                {
                    ClientId = "spa-client",
                    ClientName = "Projects SPA",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    RequireConsent = false,

                    RedirectUris =           spaRedirectUris,
                    PostLogoutRedirectUris = spaPostLogoutRedirectUris,
                    AllowedCorsOrigins =     spaAllowedCorsOrigins,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "projects-api"
                    },
                    AccessTokenLifetime = 1800 // // The token timeout needs to be shorter than the cookie timeout, so virtual sliding timeout works. 
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = mvcRedirectUris,
                    PostLogoutRedirectUris = mvcPostLogoutRedirectUris,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AllowOfflineAccess = true
                }
            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }

}
