using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using RestSharp;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Gmail_Api.Model;
using Gmail_Api.Interface;
using Gmail_Api.Factory;
using Gmail_Api.Provider;
using Gmail_Api.Helper;

namespace Gmail_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OauthController : ControllerBase, IUserId
    {
        private GmailProvider _mGmail = null;

        private GmailProvider mGmail { get 
            {
                if (_mGmail == null) 
                {
                    _mGmail = GmailFactory.GetGmail(UserId);
                }
                return _mGmail;
            }
        }

        public string UserId
        {
            get
            {
                if (!HttpContext.Request.Query.ContainsKey("UserId"))
                {
                    return HttpContext.Request.Cookies["UserId"].ToString();
                }
                return HttpContext.Request.Query["UserId"].ToString();
            }
        }


        [HttpGet]
        [Route("CallBack")]
        public IActionResult CallBack()
        {

            var code = HttpContext.Request.Query["code"];

            var client = new RestClient("https://oauth2.googleapis.com/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("code", code);
            request.AddParameter("client_id", mGmail.credential.client_id);
            request.AddParameter("client_secret", mGmail.credential.client_secret);
            request.AddParameter("redirect_uri", mGmail.credential.redirect_uris.Where(s => s.Contains(HttpContext.Request.Host.ToString())).FirstOrDefault());
            request.AddParameter("grant_type", "authorization_code");
            IRestResponse response = client.Execute(request);

            mToken t = JsonConvert.DeserializeObject<mToken>(response.Content);

            FileIOHelper.WriteStringToJson(response.Content,$"tokens\\token_{mGmail.GetUserId()}.json");

            //Clear Cookies
            HttpContext.Response.Cookies.Delete("UserId");

            return Ok(response.Content);
        }

 
        
        [HttpGet]
        [Route("GetToken")]
        public ActionResult GetToken()
        {
            HttpContext.Response.Cookies.Delete("UserId");

            HttpContext.Response.Cookies.Append("UserId", mGmail.GetUserId());

            IAuthorizationCodeFlow flow = mGmail.GetCodeFlow();

            var token = flow.LoadTokenAsync("me", CancellationToken.None).Result;

            mGmail.RefreshTokenIfInvalid(flow, ref token);
            //var TokenIsExpired = flow.ShouldForceTokenRetrieval();

            //if (TokenIsExpired) {
                
            //}
           
            if (token != null && mGmail.GetScopes().Any<string>(s=> token.Scope.Contains(s))) 
            {
                mGmail.SaveToken(token);         
                return Ok(token);
            }

            AuthorizationCodeRequestUrl url = flow.CreateAuthorizationCodeRequest(mGmail.credential.redirect_uris.Where(s => s.Contains(HttpContext.Request.Host.ToString())).FirstOrDefault());
            var uurl = url.Build().ToString();
            
            return Redirect(uurl);
        }

        [HttpGet]
        [Route("RefreshToken")]
        public ActionResult RefreshToken()
        {
            IAuthorizationCodeFlow flow = mGmail.GetCodeFlow();

            var t = flow.RefreshTokenAsync("me", mGmail.token.refresh_token ,CancellationToken.None).Result;

            if (t == null)
            {
                return GetToken();
            }
            mGmail.RefreshTokenIfInvalid(flow, ref t);

            return Ok(mGmail.token);
        }

       
    }

}