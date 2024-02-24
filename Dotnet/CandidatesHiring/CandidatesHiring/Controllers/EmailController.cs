using CandidatesHiring.Model;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using CandidatesHiring.Operations;
using MailKit.Net.Smtp;
using System.Security.Authentication;

namespace CandidatesHiring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IOperation operation;

        public EmailController(IOperation _operation)
        {
            operation = _operation;
        }

        [HttpPost]
        public ActionResult sendEmail(string text, List<Profile> profiles)
        {
            this.operation.sendMail(text, profiles);
            return Ok("Email send!");
        }
    }
}
