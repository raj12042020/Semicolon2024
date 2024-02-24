using AIHiringPortal.DatabaseService;
using AIHiringPortal.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIHiringPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ISearchService searchService;

        public EmailController(ISearchService _searchService)
        {
            searchService = _searchService;
        }

        [HttpPost]
        public ActionResult sendEmail(string text, List<Profile> profiles)
        {
            this.searchService.sendMail(text, profiles);
            return Ok("Email send!");
        }
    }
}
