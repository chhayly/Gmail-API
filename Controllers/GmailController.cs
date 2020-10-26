using System.Collections.Generic;
using Gmail_Api.Factory;
using Gmail_Api.Interface;
using Gmail_Api.Model;
using Gmail_Api.Provider;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gmail_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GmailController : ControllerBase, IUserId
    {
        private GmailProvider _mGmail = null;

        private GmailProvider mGmail
        {
            get
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
        [Route("GetList")]
        public ActionResult GetList()
        {
            var service = mGmail.CreateGmailService();

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

            // List labels.
            IList<Label> labels = request.Execute().Labels;

            return Ok(labels);
        }

        [HttpPost]
        [Route("Sendmessage")]
        public ActionResult SendMessage([FromBody] mMessage mMessage)
        {
            ///TODO: add parameter 
            ///
            var service = mGmail.CreateGmailService();


            var message = new SendMail().CreateMessage(mMessage);

            message = service.Users.Messages.Send(message, UserId).Execute();


            return Ok(message);
        }
    }
}
