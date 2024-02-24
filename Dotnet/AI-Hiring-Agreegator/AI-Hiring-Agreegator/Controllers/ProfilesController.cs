using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AIHiringPortal.DatabaseService;
using AIHiringPortal.Model;
using System.Collections.Generic;

namespace AIHiringPortal.Controllers
{
    [Route("AIHiringPortal/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ISearchService searchService;

        public ProfilesController(SearchService ss)
        {
            searchService = ss;
        }

        [HttpPost]
        public async Task<IEnumerable<Profile>> GetProfies(object criterias)
        {
            return await searchService.GetfilteredprofileswithmatchingCriteria(criterias);
        }


        //[HttpPost("GetProfilesWithMatchingCriterias")]
        //public async Task<IEnumerable<Profile>> GetRecordsWithConditions([FromBody] object value)
        //{
        //    string? v = value as string;
        //    Console.WriteLine(v);
        //    //List<Profile> filterlist = (List<Profile>)await searchService.GetProfiles();
        //    return filterlist;
        //}
    }
}
