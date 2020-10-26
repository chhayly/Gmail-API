using Gmail_Api.Helper;
using Gmail_Api.Model;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gmail_Api.Provider
{
    public class GmailProvider : mGmail
    {
        public GmailProvider(string UserId) : base(UserId) { }

        string[] Scopes = { GmailService.Scope.MailGoogleCom };
        public GmailService CreateGmailService()
        {
            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetUserCredential(),
                ApplicationName = "ACE",
            });

            return service;

        }
        public void SetScopes(string[] Scopes)
        {
            this.Scopes = Scopes;
        }
        public string[] GetScopes() => this.Scopes;


        private UserCredential GetUserCredential()
        {
            var flow = GetCodeFlow();

            var t = new TokenResponse();

            t.AccessToken = token.access_token;
            t.ExpiresInSeconds = token.expires_in;
            t.Scope = token.scope;
            t.RefreshToken = token.refresh_token;
            t.TokenType = token.token_type;

            RefreshTokenIfInvalid(flow,ref t);

            return new UserCredential(flow, "me", t);
        }

        internal IAuthorizationCodeFlow GetCodeFlow()
        {
            IAuthorizationCodeFlow flow;
            string credPath = "token.json";
            flow =
        new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = credential.client_id,
                ClientSecret = credential.client_secret
            },
            Scopes = Scopes,
            DataStore = new FileDataStore(credPath, true)
        });
            return flow;
        }

        internal void RefreshTokenIfInvalid(IAuthorizationCodeFlow flow , ref TokenResponse token) 
        {
            var TokenIsExpired = flow.ShouldForceTokenRetrieval();
            if (TokenIsExpired)
            {
                token = flow.RefreshTokenAsync("me", token.RefreshToken, CancellationToken.None).Result;

                SaveToken(token);
            }
        }

        internal void SaveToken(TokenResponse token) 
        {
            string content = JsonConvert.SerializeObject(token);
            string path = $"tokens\\token_{this.GetUserId()}.json";
            FileIOHelper.WriteStringToJson(content, path); 
        }
    }
}
