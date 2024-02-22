using CandidatesHiring.Model;
using CandidatesHiring.Operations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CandidatesHiring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {

        private readonly IOperation operation;

        public CandidateController(IOperation _operation)
        {
            operation = _operation;
        }
        //GET: api/<CandidateController>
        [HttpGet]
        public async Task<List<Profile>> GetProfiles() => (List<Profile>)await operation.GetProfiles();

       
    }
}
